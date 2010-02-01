// PackSize.cpp
//
// Copyright 2008	DecisionPower, Inc.
//
// Social Network Model for Microsegment
// Created by Steve Noble
// routines were stolen/copied from Purchasing Code
#include "PackSize.h"
#include "randlib.h"

vector<PackSize*> PackSize::Packs;

PackSize* PackSize::GetPack(int pack_id)
{
	for(int ii = 0; ii < Packs.size(); ++ii)
	{
		PackSize* pack = Packs[ii];

		if (pack->id == pack_id)
		{
			return pack;
		}
	}

	// need to create a new pack size
	PackSize* newPack = new PackSize(pack_id);

	Packs.push_back(newPack);

	return newPack;
}

PackSize::~PackSize()
{
	// remove this from Packs

	vector<PackSize*>::iterator iter = Packs.begin();
	for(; iter != Packs.end(); ++iter)
	{
		if ( id == (*iter)->id )
		{
			Packs.erase(iter);
			break;
		}
	}
}

PackSize::PackSize(int pack_id)
{
	// dist = new vector<double>();
	id = pack_id;
}

void PackSize::Add(int size, double val)
{
	int curSize = dist.size();

	if (curSize < size + 1)
	{
		// add enough to equal size

		for(int ii = 0; ii < size + 1 - curSize; ++ii)
		{
			dist.push_back(0.0);
		}
	}

	dist[size] = val;
}

int PackSize::GetSize()
{
	double rand = LocalRandUniform();

	double cum = 0;

	for(int ii =0; ii < dist.size(); ++ii)
	{
		cum += dist[ii];

		if ( rand < cum)
		{
			return ii;
		}
	}

	// no size

	return 0;
}
