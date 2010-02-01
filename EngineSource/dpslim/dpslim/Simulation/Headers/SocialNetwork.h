#pragma once
#define _CRT_SECURE_NO_WARNINGS
// SocialNetwork.h
//
// Copyright 2005	DecisionPower
//
// Social Network Model for Microsegment


#include "stdafx.h"

// forward declares
class NetworkParamsRecordset;
class Consumer;
class CMicroSegment;

// base class for different types of networks
class SocialNetwork
{
private:

	CMicroSegment* fromSegment;
	CMicroSegment* toSegment;

	// negwork parameters
	long networkType;
	int useLocal;
	double  numContacts;
	double awarenessWeight;
	double probTalkPostUse;
	double probTalkPreUse;
	double persuasionPreUse;
	double persuasionPostUse;

	// unused for now
	double percentTalking;
	double negPersuasionReject;
	double negPersuaionPreUse;
	double negPersuaionPostUse;

	void TalkAboutProduct(int, vector<int>& prods, int postUse);

	// talk talk talk
	void TellEmergentFriends(int,vector<int>& prods, int postUse);
	void TellAll(int,  vector<int>& prods, int postUse);

	void TellRandomFriends(int,  vector<int>& prods, int postUse);

	void TellFriendWhatIBought( int,  Consumer* otherGuy, vector<int>& prods, int postUse);

	long getNumContacts(int);

	long* numContactList;

public:

	// Constructors & destructors
	SocialNetwork();
	SocialNetwork(CMicroSegment*, CMicroSegment*, NetworkParamsRecordset*);

	~SocialNetwork();

	// methods used by microsegment

	virtual void  MakeRecommendations(int);
};

