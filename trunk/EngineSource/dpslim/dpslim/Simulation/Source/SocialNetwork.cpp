// SocialNetwork.cpp
//
// Copyright 2005-200	DecisionPower, Inc.
//
// Social Network Model for Microsegment
// Created by Steve Noble
// routines were stolen/copied from Purchasing Code
#include "SocialNetwork.h"

#include "DBNetworkParams.h"
#include "Consumer.h"
#include "ProductTree.h"
#include "PopObject.h"
#include "Product.h"

SocialNetwork::SocialNetwork(CMicroSegment* fromSeg, CMicroSegment* toSeg, NetworkParamsRecordset* param)
{
	// NOTE: toSegment may be 0
	// in this case messages sent to all
	fromSegment = fromSeg;
	toSegment = toSeg;

	if (param->type == 0)
		networkType = kSocNetTalkAnytime;
	else
		networkType = kSocNetTalkOnPurchase;

	
	useLocal = param->use_local;
	numContacts =  param->num_contacts ;
	// do not exceed global maximum
	if (numContacts > kMaxNumFriends)
		numContacts = kMaxNumFriends;

	numContactList = 0;

	awarenessWeight = param->awareness_weight;

	probTalkPreUse = param->prob_of_talking_pre_use;
	probTalkPostUse = param->prob_of_talking_post_use;
	
	persuasionPreUse = param->persuasion_pre_use;
	persuasionPostUse = param->persuasion_post_use;

	// unused for now
	percentTalking = param->percent_talking;
	negPersuasionReject = param->neg_persuasion_reject;
	negPersuaionPreUse = param->neg_persuasion_pre_use;
	negPersuaionPostUse = param->neg_persuasion_post_use;
}

SocialNetwork::~SocialNetwork()
{
	if (numContactList)
	{
		delete[] numContactList;
	}
}

// find things to talk about
void SocialNetwork::MakeRecommendations(int guyNum)
{
	Consumer* aGuy = fromSegment->Population()->iConsumerList[guyNum];

	if (networkType == kSocNetTalkOnPurchase)
	{
		if (!aGuy->iBoughtThisTick)
		{
			return;
		}
	}

	// what to talk about
	vector<int> prods;

	int postUse = false;

	if (aGuy->iProductLastBought != kNoProductBought)
	{
		prods.push_back(aGuy->iProductLastBought);
		postUse = true;
	}

	int badProd = -1;
	int goodProd = -1;


	// find product I am most persuaded by...
	// or least persuaded by
	double maxPersuasion = 1;
	double minPersuasion = -1;

	for(int prodNum = 0; prodNum < (int)fromSegment->iProducts.size(); ++prodNum)
	{
		if (prodNum == aGuy->iProductLastBought)
		{
			continue;
		}

		// find product with highest persuasion and talk about that
		if (aGuy->Aware(prodNum))
		{
			// negative
			if (aGuy->productPersuasion[prodNum] < minPersuasion)
			{
				badProd = prodNum;
				minPersuasion = aGuy->productPersuasion[prodNum];
			}
			else if (aGuy->productPersuasion[prodNum] > maxPersuasion)
			{
				goodProd = prodNum;
				maxPersuasion = aGuy->productPersuasion[prodNum];
			}
		}
	}

	if (badProd >= 0)
	{
		prods.push_back(badProd);

		if(aGuy->EverTriedProduct(badProd, true))
		{
			postUse = true;
		}

	}

	if (goodProd >= 0)
	{
		prods.push_back(goodProd);
		if(aGuy->EverTriedProduct(goodProd, true))
		{
			postUse = true;
		}
	}


	TalkAboutProduct(guyNum, prods, postUse);

}

// TODO: this needs to be integrated into the "when does a consumer talk" logic
// so consumers talk but are not blabbermouths and only doc about persuasive things
// this will speed up performance as well
void SocialNetwork::TalkAboutProduct(int guyNum, vector<int>& prods, int postUse)
{
	Consumer* aGuy = fromSegment->Population()->iConsumerList[guyNum];

	double	probTalking;

	
	// pre-use or post use?

	// see if consumer will talk about it to close friends
	if (postUse)
	{
		probTalking = probTalkPostUse;
	}
	else
	{
		probTalking = probTalkPreUse;
	}

	// pick & test random number
	if ((probTalking > 0.0) && (WillDo0to1(probTalking)))
	{
		TellEmergentFriends(guyNum, prods, postUse);			
	}
	
}

// ------------------------------------------------------------------------------
// true indicates find friends like aGuy
// 1. If my communication is to people like aGuy-
//	- if use local, do as is done now
//	- if use local is not checked
//		- if there are multiple segments, pick randomly within my segment
//		- if the is only one segment, pick someone similar to aGuy, the way it is currently done
// 
// 2. If communication is to people unlike aGuy
//	- if there are multiple segments, pick a different segment randomly, and pick a person within that segment
//	- if the is only one segment, pick someone different from aGuy, the way it is done now.
// ------------------------------------------------------------------------------
void SocialNetwork::TellEmergentFriends(int guyNum, vector<int>& prods, int postUse)
{
	// this guy wants to tell the world
	if (toSegment == 0)
		return TellAll(guyNum, prods, postUse);
	
	TellRandomFriends(guyNum, prods, postUse);
}



// this is different then telling each, as we pick a segment to tell at random
void SocialNetwork::TellAll(int guyNum, vector<int>& prods, int postUse)
{
	static CMicroSegment* inSegment[kMaxNumFriends];
	static int friendsFound[kMaxNumFriends];
	int ii;

	// original code used kMaxNumFriends
	// but this makes more sense
	long numToTell = getNumContacts(guyNum);
	for(ii = 0; ii < numToTell; ++ii)
	{
		inSegment[ii] = 0;
		friendsFound[ii] = -1;
	}

	// make a list of random, unique guys
	// I have no way to guarantee that these guys don't appear
	// in the list of guys like me, because we talked to them at
	// a different time

	int				numOtherSegs = (int)fromSegment->iSegmentPointers.size();
	int				otherSeg;
	CMicroSegment*	friendSeg;
	int				oneSeg = ((int)fromSegment->iSegmentPointers.size() == 0);
	long			friendsTold = 0;
	int				friendID;

	while (friendsTold < numToTell)
	{
		// pick one to tell
		// if we have only one segment, llok in that one
		if (oneSeg)
		{
			friendID = LocalRand((long)fromSegment->Population()->iConsumerList.size());
			friendSeg = fromSegment;
		}
		// otherwise, pick another segment at random
		else
		{
			otherSeg = (int)LocalRand(numOtherSegs);
			friendSeg = fromSegment->iSegmentPointers[otherSeg];
			friendID = LocalRand((long)friendSeg->Population()->iConsumerList.size());
		}

		// make sure we don't tell the same one twice
		int unique = true;

		// make sure we don't tell ourselves
		if (oneSeg)
		{
			Consumer* otherGuy = friendSeg->Population()->iConsumerList[friendID];
			if (friendID == guyNum)
				unique = false;
		}

		for (ii=0; unique && ii< friendsTold; ii++)
		{
			if ((friendsFound[ii] == friendID) && (inSegment[ii] == friendSeg))
			{
				unique = false;
			}
		}

		if (!unique)
			continue;

		// temporarily set toSegment to friendSegment
		// we could call TellEmergentFriends this point
		// later, when we have more confidence

		// tell a friend!
		friendsFound[friendsTold] = friendID;
		inSegment[friendsTold] = friendSeg;

		Consumer* otherGuy = friendSeg->Population()->iConsumerList[friendID];
		TellFriendWhatIBought(guyNum, otherGuy, prods, postUse);

		friendsTold++;
	}
}	// end unlike me


// ------------------------------------------------------------------------------
// Tell a random friend in this segment
// ------------------------------------------------------------------------------
void SocialNetwork::TellRandomFriends(int guyNum, vector<int>& prods, int postUse)
{
	long		friendsTold = 0;
	long		i;
	long		friendNum;
	long		friendID;
	int		unique;

	static int friendsFound[kMaxNumFriends];

	int numToTell = getNumContacts(guyNum);

	// initialize who we told
	for (friendNum = 0; friendNum < numToTell; friendNum++)
		friendsFound[friendNum] = -1;


	while (friendsTold < numToTell)
	{
		friendID = LocalRand( (long)toSegment->Population()->iConsumerList.size());
		Consumer* otherGuy = toSegment->Population()->iConsumerList[friendID];

		// make sure we don't tell ourselves
		if (toSegment == fromSegment && guyNum == friendID)
			continue;

		// make sure we don't tell the same one twice
		unique = true;
		for (i=0; unique && i<friendsTold; i++)
		{
			if (friendsFound[i] == friendID)
			{
				unique = false;
			}
		}

		if (!unique)
			continue;

		// tell a friend!
		friendsFound[friendsTold] = friendID;
	
		TellFriendWhatIBought( guyNum, otherGuy, prods, postUse);
		friendsTold++;
	}
}


// This is the main routine for telling someone in your network
void SocialNetwork::TellFriendWhatIBought(int guyNum, Consumer* otherGuy, vector<int>& prods, int postUse)
{
	double	persuasion;

	if (postUse)
		persuasion = persuasionPostUse;
	else
		persuasion = persuasionPreUse;

	
	Consumer* aGuy = fromSegment->Population()->iConsumerList[guyNum];


	for(size_t ii = 0; ii < prods.size(); ++ii)
	{
		Index pn = prods[ii];

		if (aGuy->productPersuasion[pn] < 0)
		{
			otherGuy->ProcessAdAwarenessAndPersuasion(this->awarenessWeight, pn, 0, -persuasion);
		}
		else
		{
			otherGuy->ProcessAdAwarenessAndPersuasion(this->awarenessWeight, pn, 0, persuasion);
		}
	}
}


// check if we have the right number of contacts
// then uses the GetIntegerFromStatFloat static method in the Microsegment 
long SocialNetwork::getNumContacts(int GuyNum)
{
	// psuedo persistance

	if (!numContactList)
	{
		numContactList = new long[fromSegment->Population()->iConsumerList.size()];
	
		// do not exceed 10% of selection population
		long	maxPossibleFriends = 0;

		if (toSegment != 0)
		{
			maxPossibleFriends = (long)toSegment->Population()->iConsumerList.size();
		}
		else
		{
			// sum over all other segments
			int numOtherSegs = (int)fromSegment->iSegmentPointers.size();

			if (numOtherSegs > 0)
			{
				CMicroSegment* otherSeg;
				maxPossibleFriends = 0;
				for( int ii = 0; ii < numOtherSegs; ++ii)
				{
					otherSeg = fromSegment->iSegmentPointers[ii];

					maxPossibleFriends += (long)otherSeg->Population()->iConsumerList.size();
				}
			}
			else
			{
				maxPossibleFriends = (long)fromSegment->Population()->iConsumerList.size();
			}
		}


		if (numContacts > maxPossibleFriends/10)
			numContacts = maxPossibleFriends/10;


		// for each consumer construct  then number
		for(size_t guy = 0; guy < fromSegment->Population()->iConsumerList.size(); ++guy)
		{
			double randContacts = kMaxNumFriends;
			double rand = RandomUniform();

			// if rand is close to zero then just use max
			if ( rand > exp(-kMaxNumFriends/numContacts))
			{
				randContacts = -numContacts * log( rand);
			}

			numContactList[guy] =  GetIntegerFromStatFloat(randContacts);
		}
	}

	
	// for now contacts are exponentially distributed.
	// so large number of contacts is rare
	// most contacts are sparse

	return numContactList[GuyNum];
}