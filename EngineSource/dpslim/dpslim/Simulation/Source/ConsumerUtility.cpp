// Consumer.cpp
//
// Copyright 1998	Salamander Interactive
//
// Consumer Agent Object
// Created by Ken Karakotsios 10/26/98
#include "Consumer.h"

#include "RepurchaseModel.h"
#include "ProductTree.h"
#include "DBModel.h"
#include "Product.h"
#include "Attributes.h"
#include "MarketUtility.h"
#include "math.h"

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
int	Consumer::SeeIfConsumerWillHaveTriedProduct(int prodNum, 
			int multiOK)
{
	unsigned int*	numTriesAddr;
	unsigned int	numTries;
	//unsigned int	prodBits = GetProdTriesMask(prodNum);
	unsigned int	prodBits = kProdTriesMask;
	int	clearProdBits = ~prodBits;

	numTriesAddr = &(productsBought[prodNum]);
	numTries = *numTriesAddr & prodBits;
	if ((numTries == 0) || multiOK)
	{
		numTries = 3;
		// clear out old value
		*numTriesAddr &= clearProdBits;
		// put in new value
		*numTriesAddr |=  numTries;
		return true;
	}
	return false;
}

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	Consumer::DistributionDrivenAwareness(Pcn pcn)
{
	// check if the channel matches
	if (pcn.cn != iPreferredChannel)
		return;
					
	ProcessAdAwarenessAndPersuasion(segment->iProductsAvailable[pcn]->iDistributionAwareness, pcn.pn, 0, 
		segment->iProductsAvailable[pcn]->iDistributionPersuasion);
}


// ------------------------------------------------------------------------------
// See if this purchase is a result of a consumer preference switch.
// returns true if it was a switch
// ------------------------------------------------------------------------------
int	Consumer::CheckForSwitches(int productToBuy)
{
	if (iProductLastBought != kNoProductBought)
	{

		ProductTree::List* buying_anc = ProductTree::theTree->Ancestors(productToBuy);
		ProductTree::List* bought_anc = ProductTree::theTree->Ancestors(iProductLastBought);
		ProductTree::Iter iter;
		for(iter = buying_anc->begin(); iter != buying_anc->end(); ++iter)
		{
			int anc_pn = *iter;
			if(!ProductTree::theTree->isAncestor(anc_pn, iProductLastBought))
			{
				segment->iProducts[anc_pn]->iAddsThisTick += 1;
			}
		}
		for(iter = bought_anc->begin(); iter != bought_anc->end(); ++iter)
		{
			int anc_pn = *iter;
			if(!ProductTree::theTree->isAncestor(anc_pn, productToBuy))
			{
				segment->iProducts[anc_pn]->iDropsThisTick += 1;
			}
		}
	}

	if ((iProductLastBought !=  productToBuy) && (iProductLastBought != kNoProductBought))
	{
		return true;
	}
	else if (iProductLastBought == kNoProductBought)
	{
		return true;
	}
	else
	{
		return false;
	}
}


// ------------------------------------------------------------------------------
// scaled utility(p) = utility(p) / (product(all utilities))^(1/number of products)
// ------------------------------------------------------------------------------
void Consumer::ScaleScoresLogit()
{
	//int	i;
	Pcn	pcn;
	double	product = 1.0;
	double	denominator;
	int	numActiveProdsAvailable;
	double	scaleFactor = 1.0;

	product = FormLogProduct(&numActiveProdsAvailable);

	if (numActiveProdsAvailable > 0)
	{
		denominator = product;
	}
	else
	{
		denominator = 0.0;
	}


	double	tempFloat;

	vector< Pcn >::const_iterator iter;
	for(iter = consideration_set.begin(); iter != consideration_set.end(); ++iter)
	{
		pcn = *iter;
		tempFloat = this->segment->iProductsAvailable[pcn]->overallScore;
		if (this->segment->iProductsAvailable[pcn]->overallScore > 0.0)
		{
			tempFloat = log(tempFloat);
			tempFloat = tempFloat - denominator; // x/y = e^(log(x) - log(y))
			tempFloat = exp(tempFloat);
			this->segment->iProductsAvailable[pcn]->overallScore = tempFloat;
		}
	}
}

// ------------------------------------------------------------------------------
// for ith product of n products, prob of choice = e^(u[i])/sum(e^(u[n]))
// NOTES:
// removed warning messages as I Added some better math for normalizing
// may cause more products to signal an "overflow" when no overflow should be seen.
// ------------------------------------------------------------------------------
double Consumer::GetLogitProbabilityFromUtility()
{
	double		sum;
	double		newVal;
	Pcn		pcn;
	double	maxUtil;
	int	firstPass = true;
	//double	medianUtil;
	double	util;

	// get the scale factor
	vector< Pcn >::const_iterator iter;
	for (iter = consideration_set.begin(); iter != consideration_set.end(); ++iter)
	{
		pcn = *iter; 
		if (firstPass)
		{
			maxUtil = this->segment->iProductsAvailable[pcn]->overallScore;
			firstPass = false;
		}
		else
		{
			if (this->segment->iProductsAvailable[pcn]->overallScore > maxUtil)
				maxUtil = this->segment->iProductsAvailable[pcn]->overallScore;
		}
	}

	
	sum = 0.0;
	for (iter = consideration_set.begin(); iter != consideration_set.end(); ++iter)
	{	
		pcn = *iter;
		util = this->segment->iProductsAvailable[pcn]->overallScore;
		util -= maxUtil;

		// using 20 as cutoff means that newVal will be greater then
		// 2.0611536224385578279659403801558e-9
		// the normalized probability will be even lower
		// if you had a million products to choose from and they were all equally likely then
		// the scores would all be greater then e-15
		// SSN 7/20/2006
		if (util < -20)
		{
			newVal = 0.0;
		}
		else
		{
			newVal = exp(util);
		}

		this->segment->iProductsAvailable[pcn]->overallScore = newVal;

		sum += newVal;		
	}

	for (iter = consideration_set.begin(); iter != consideration_set.end(); ++iter)
	{
		pcn = *iter;
		this->segment->iProductsAvailable[pcn]->overallScore /= sum;
	}
	
	// make message weighted scores cumulative
	sum = 0.0;
	for (iter = consideration_set.begin(); iter != consideration_set.end(); ++iter)
	{
		pcn = *iter;
		sum += this->segment->iProductsAvailable[pcn]->overallScore;
		this->segment->iProductsAvailable[pcn]->overallScore = sum;
	}

	return sum;
}

// update only this product
int 	Consumer::UpdateConsumerProductPurchase(Index pn)
{
	unsigned int	prodBits = kProdTriesMask;
	int	clearProdBits = ~prodBits;
	int	numTriesBeforeThisOne;
	unsigned int	numTries;
	
	// need to count tries
	numTries = productsBought[pn] & prodBits;
	numTriesBeforeThisOne = numTries;
	if (numTries < 3)
	{
		numTries++;
		// clear out old value
		productsBought[pn] &= clearProdBits;
		// put in new value
		productsBought[pn] |=  numTries;
	}

	if(segment->iNumDays >= segment->iModelInfo->reset_panel_data_day)
	{
		productTries[pn]++;
	}

	return productTries[pn];
}
// keeps track of product state for consumer
// updates global stats as well
// this updates ancestors as follows: when you try a child of an ancestor then it is as if you have tried the ancestor
// will update siblings with if the shareProductUseWithSibs is true
int	Consumer::ConsumerIsTryingProduct(Index pn, int isSample)
{
	int rval = 0;

	ProductTree::List* ancestors = ProductTree::theTree->Ancestors(pn);
	for(ProductTree::Iter iter = ancestors->begin(); iter != ancestors->end(); ++iter)
	{
		Index anc_pn = *iter;
		int numTriesBeforeThisOne = UpdateConsumerProductPurchase(anc_pn);

		if (anc_pn == pn)
		{
			rval = numTriesBeforeThisOne;
		}

		if (numTriesBeforeThisOne == 0)
		{
			// we should not get here on inital purchases, because, initial purchases are assigned to  
			// consumers that already have penetration, which means they have a numTriesBeforeThisOne != 0
			segment->iProducts[anc_pn]->iNumEverTriedProduct += 1;
		}
		if (!isSample && numTriesBeforeThisOne == 1)
		{
			segment->iProducts[anc_pn]->iRepeatPurchasersCumulative += 1;
		}
		if(numTriesBeforeThisOne > 0)
		{
			segment->iProducts[anc_pn]->iTotalRepeatPurchaseOccasionsCumulative += 1;
		}
	}


	// This is used to implement the new forget function
	if(!(productsBought[pn] & kTriedSib))
	{
		ProductTree::List* sibs = ProductTree::theTree->Siblings(pn);
		if (sibs)
		{
			for( ProductTree::Iter iter = sibs->begin(); iter != sibs->end(); ++iter)
			{
				int sibPn = *iter;
				productsBought[sibPn] |= kTriedSib;	
			}
		}
	}
	

	return rval;
}

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
int	Consumer::IsChannelOK(Pcn pcn)
{
	if (iPreferredChannel == kNoChannel)
		return true;

	//if (iPreferredChannel == GetChannelNumFromProdChannelNum(prodChanNum))
	if (iPreferredChannel == pcn.second)
		return true;
	else
		return false;
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	Consumer::ProcessAdAwarenessAndPersuasion(double awareness, Index pn, int *impressionsDelivered, double persuasion)
{
	int	isAware = false;

	
	if (WillDo0to1(awareness))
	{
		MakeConsumerAware(pn, kDoSibs);
		isAware = true;
	}
	else
	{
		isAware = true;
	}


	

	// if receiver is aware
	// if (ConsumerIsAware(groupNum, indexInGroup,  prodNum, NULL)) optimized with isAware
	if (isAware)
	{
		// Add persuasion units
		Persuade(pn, persuasion, kDoSibs);	// is Aware
	}
	if (impressionsDelivered != 0)
	{
		(*impressionsDelivered) += 1;
	}
}



// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void Consumer::MakeAware(Index pn, double persuasion)
{
	MakeConsumerAware(pn, kDoSibs);

	Persuade(pn, persuasion, kDoSibs);	// is Aware KMK 9/15/04 no need to track units Added
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	Consumer::MakeConsumerAware(Index pn, int doSibs)
{
	if(!ProductTree::theTree->IsLeaf(pn))
	{
		ProductTree::LeafNodeList leafs(pn);

		if(leafs.size() != 0)
		{
			for( ProductTree::Iter iter = leafs.begin(); iter != leafs.end(); ++iter)
			{
				int leafPn = *iter;

				if (segment->iProducts[leafPn]->iProdActiveInSomeChannel)
				{
					// make consumer aware
					MakeConsumerAware(leafPn, false);
				}
			}
		}
	}
	else if(segment->iShareAwarenessWithSibs && doSibs)
	{
		int parent_pn = ProductTree::theTree->Parent(pn);
		if (parent_pn >= 0)
		{
			MakeConsumerAware(parent_pn, false);
		}
		else
		{
			MakeConsumerAware(pn, false);
		}
	}
	else if(!Aware(pn))
	{
		//PRODUCT[prodNum].iNumAwareSofar += POPULATION->iPopScaleFactor;
		segment->iProducts[pn]->iNumAwareSofar += 1;
		productAwareness[pn]  = this->segment->iNumDays;
		for(da_iter iter = dynamic_attributes[pn].begin(); iter != dynamic_attributes[pn].end(); ++iter)
		{
			iter->second->MakeAware();
		}
	}
	else if(dynamic_attributes[pn].size() > 0)
	{
		for(da_iter iter = dynamic_attributes[pn].begin(); iter != dynamic_attributes[pn].end(); ++iter)
		{
			iter->second->MakeAware();
		}
	}
}

double Consumer::AwareProb(Index pn)
{
	 double		locAwarenessDecayPreUse = ((double)segment->iAwarenessDecayRatePreUse) * 0.01;
	 double		locAwarenessDecayPostUse = ((double)segment->iAwarenessDecayRatePostUse) * 0.01;
	 int count;
	 count = productsBought[pn] & kProdTriesMask;
	 if((productsBought[pn] & kTriedSib) || count > 0)
	 {
		return locAwarenessDecayPostUse;
	 }
	 else
	 {
		return locAwarenessDecayPreUse;
	 }
}

double Consumer::PersuasionLoss(Index pn)
{
	double		locPersuasionDecayRatePreUse = segment->iPersuasionDecayRatePreUse * 0.01;
	double		locPersuasionDecayRatePostUse = segment->iPersuasionDecayRatePostUse * 0.01;
	int count;
	count = productsBought[pn] & kProdTriesMask;
	if(count > 0)
	{
		return locPersuasionDecayRatePostUse;
	}
	else
	{
		return locPersuasionDecayRatePreUse;
	}
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void Consumer::Persuade(Index pn, double persuasion, int doSibs)
{
	int	numPersuasionUnitsToAdd = 0;

	if (persuasion > 0.0)
	{
		while (persuasion >= 1.0)
		{
			numPersuasionUnitsToAdd+= 1;
			persuasion -= 1.0;
		}
		if (persuasion > 0.0)
		{
			if (WillDo0to1(persuasion))
				numPersuasionUnitsToAdd += 1;
		}
	}
	else if (persuasion < 0.0)
	{
		while (persuasion <= -1.0)
		{
			numPersuasionUnitsToAdd-= 1;
			persuasion += 1.0;
		}
		if (persuasion < 0.0)
		{
			if (WillDo0to1(0.0 - persuasion))
				numPersuasionUnitsToAdd -= 1;
		}
	}
	if (numPersuasionUnitsToAdd != 0)
	{
		// KMK 8/27/04 always aware when this is called MakeConsumerAware(groupNum, indexInGroup, prodNum, NULL, kDoSibs);	// KMK 2/5/04
		if(!ProductTree::theTree->IsLeaf(pn))
		{
			//ProductTree::List* leafs = ProductTree::theTree->ChildNodes(prodNum);
			ProductTree::LeafNodeList leafs(pn);


			for( ProductTree::Iter iter = leafs.begin(); iter != leafs.end(); ++iter)
			{
				int leafPn = *iter;

				if (segment->iProducts[leafPn]->iProdActiveInSomeChannel)
				{
					// note a product we ar enot aware of does not get pesuasion
					if (Aware(leafPn))
					{
						AddPersuasion(leafPn, numPersuasionUnitsToAdd);
					}
				}
			}
		}
		else if (doSibs && segment->iShareAwarenessWithSibs)
		{

			int parent_pn = ProductTree::theTree->Parent(pn);
			if (parent_pn >= 0)
			{
				Persuade(parent_pn, numPersuasionUnitsToAdd, false);
			}
			else
			{
				Persuade(pn, numPersuasionUnitsToAdd, false);
			}
		}
		else if (Aware(pn))
		{
			AddPersuasion(pn, numPersuasionUnitsToAdd);
		}
	}	// end check for persuasion units to Add
}


// ------------------------------------------------------------------------------
//	ISNTODO
// ------------------------------------------------------------------------------
void Consumer::MakeConsumerTrySample(Index pcn,
			double sizeOfSample, double samplePersuasion)
{

}

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
int	Consumer::RejectedAllAvailableProducts()
{
	Index	pn;
	Pcn		pcn;

	for (Pcn_iter iter = segment->iProductsAvailable.begin(); iter != segment->iProductsAvailable.end(); ++iter)
	{
		pcn = segment->GetPcn(iter);
		if ((*iter)->iActive)
		{
			pn = pcn.pn;
			if (!ConsumerTriedAndRejectedProduct(pn))
				return false;
		}
	}
	return true;
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
int	Consumer::CountProductTrials(Index pn)
{
	return productsBought[pn] & kProdTriesMask;
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void Consumer::AddPersuasion(Index pn, int	numPersuasionUnitsToAdd)
{
	productPersuasion[pn] += numPersuasionUnitsToAdd;
	segment->iTotalMessages += abs(numPersuasionUnitsToAdd);
	
	segment->iProducts[pn]->iTotalPersuasion += numPersuasionUnitsToAdd;	// KMK 2/4/04	

	iRecommend = true;
}

// ------------------------------------------------------------------------------
// We will take the sum of the logs, rather than the product
// product is over all considered product channels
// ------------------------------------------------------------------------------
double Consumer::FormLogProduct(int *numActiveProdsAvailable)
{
	double	product = 0.0;

	*numActiveProdsAvailable = 0;
	vector< Pcn >::const_iterator iter;
	for (iter = consideration_set.begin(); iter != consideration_set.end(); ++iter)
	{
		Pcn pcn = *iter;
		if (this->segment->iProductsAvailable[pcn]->overallScore > 0.0)
		{
			product += log(this->segment->iProductsAvailable[pcn]->overallScore);
			(*numActiveProdsAvailable)++;
		
			if (product >= 1e30)
			{
				return -1.0;
			}
		}
		else
		{
			this->segment->iProductsAvailable[pcn]->overallScore = 0.0;
		}
	}

	return product;
}