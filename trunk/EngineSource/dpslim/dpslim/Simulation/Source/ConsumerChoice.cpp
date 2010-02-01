// Consumer.cpp
//
// Copyright 1998	Salamander Interactive
//
// Consumer Agent Object
// Created by Ken Karakotsios 10/26/98

#include "Consumer.h"
#include "RepurchaseModel.h"
#include "ProductTree.h"
#include "MarketUtility.h"

////////////////////////////////////////////////////////////////////////////////
//
// Products have been marked as Considered, but certain events occur  int the store
// In this order
// awareness from the product being in the store (distribution)
// Special case of a price display causing a display awareness
// Display awareness being generated
////////////////////////////////////////////////////////////////////////////////
void Consumer::ProcessDisplays(Pcn pcn, int& numDisplayHits)
{	
	Index pn = pcn.pn;
	MarketUtility* market_utility;

	// the following may  make the consumer aware and change the persuasion as well
	DistributionDrivenAwareness(pcn);

	//if using display price, then there is a 100% chance of encountering a display, so the display percentages are scaled
	//the result is not exactly 100% if there are multiple displays, but it should be close enough :)
	double NIMO_display_price_scale_factor = 1.0;
	double prob_display_price = segment->iProductsAvailable[pcn]->iProbPrice[kPriceTypeDisplayAbsolute] + segment->iProductsAvailable[pcn]->iProbPrice[kPriceTypeDisplayPercent];
	if(prob_display_price > 0.0)
	{
		NIMO_display_price_scale_factor = 0.0;
		if(segment->iProductsAvailable[pcn]->iUsingPriceType == kPriceTypeDisplayAbsolute || segment->iProductsAvailable[pcn]->iUsingPriceType == kPriceTypeDisplayPercent)
		{
			NIMO_display_price_scale_factor = 0.0;
			for (MU_iter iter = segment->iProductsAvailable[pcn]->iMarketUtility.begin(); iter != segment->iProductsAvailable[pcn]->iMarketUtility.end(); ++iter)
			{
				market_utility = iter->second;
				if(market_utility->IsDisplay())
				{
					NIMO_display_price_scale_factor += segment->ScaledOddsOfDisplay(pcn, market_utility);
				}
			}
		}
		else
		{
			for (MU_iter iter = segment->iProductsAvailable[pcn]->iMarketUtility.begin(); iter != segment->iProductsAvailable[pcn]->iMarketUtility.end(); ++iter)
			{
				market_utility = iter->second;
				if(market_utility->IsDisplay())
				{
					double prob_display = segment->ScaledOddsOfDisplay(pcn, market_utility);
					NIMO_display_price_scale_factor += prob_display*(1-prob_display_price)/(prob_display - prob_display_price);
				}
			}
		}
	}

	for (MU_iter iter = segment->iProductsAvailable[pcn]->iMarketUtility.begin(); iter != segment->iProductsAvailable[pcn]->iMarketUtility.end(); ++iter)
	{
		market_utility = iter->second;

		// check display hits for this display
		// if to many time stop processing
		if (segment->iMaxDisplayHits > 0 && numDisplayHits > segment->iMaxDisplayHits)
			continue;

		// do we see the display
		double probOfDisplayEncounter = segment->ScaledOddsOfDisplay(pcn, market_utility)/NIMO_display_price_scale_factor;

		// now check for display event
		if (WillDo0to1(probOfDisplayEncounter))
		{

			numDisplayHits++;

			ProcessAdAwarenessAndPersuasion(market_utility->GetAwareness(),  pn, NULL, market_utility->GetPersuasion());

			segment->iProductsAvailable[pcn]->msgWeightedScores += market_utility->GetUtility() + segment->iDisplayUtility;
			
		}
	}
}