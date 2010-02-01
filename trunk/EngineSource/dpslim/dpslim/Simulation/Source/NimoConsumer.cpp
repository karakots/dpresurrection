// Consumer.cpp
//
// Copyright 1998	Salamander Interactive
//
// Consumer Agent Object
// Created by Ken Karakotsios 10/26/98


#ifndef __MICROSEG__
#include "CMicroSegment.h"
#endif

#include <stdlib.h>
#include <math.h>

#include "Consumer.h"
#include "RepurchaseModel.h"


// ------------------------------------------------------------------------------
// When seeding intial purchases, if TBB then we need to get products in stock
// ------------------------------------------------------------------------------

int  Consumer::GoNIMOShopping()
{
	int		lastProdChanBought;
	int		prodNum;
	int		newProdNum;
	int		productChanToBuy;
	int		recommendedProdNum = -1;
	int		recommendTrialCount;
	int		minRecommendTrialCount = -1;
	int		numUses;
	int		prodToBuy;
	int		numSKUsPurchased;

	prodToBuy = GoShopping(iProductLastBought, false, &numUses, &numSKUsPurchased);

	// if all the tasks have been not been covered, we should schedule another shopping trip
	YallComeBackNow();

	return prodToBuy;
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	Consumer::CalcNIMONumPurchased(int prodNum, int *numPurchased)
{
	if (prodNum != -1)
	{
			// adjust for special events in the nbd and r models...
			double	locDemandMod = 0.0;
			long	pcn;
			
			locDemandMod = segment->iCatQuantityPurchasedModifier;
			
			if (segment->iIndividualDemandMods)
			{
				for (pcn = 0; pcn < PRODUCTCHANNEL.size(); pcn++)
				{
					if ((PRODUCTCHANNEL[pcn].iNumPurchasedDemandMod != 0.0) && (segment->GetProdNumFromProdChanNum(pcn) == prodNum))
					{
						locDemandMod += PRODUCTCHANNEL[pcn].iNumPurchasedDemandMod;
						break;
					} 
				}  // next pcn
			}

			double	locNumToBuy = repurchaseModel->iAverageMaxUnits *  (1.0 + locDemandMod);
			int	intNumPurchased = (int) locNumToBuy;

			double	oddsBuyingOneMore = locNumToBuy - intNumPurchased;
			if (WillDo0to1(oddsBuyingOneMore))
			{
				*numPurchased = intNumPurchased + 1;
			}
			else
			{
				*numPurchased = intNumPurchased;
			}
	}
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	Consumer::NIMORepurchasing(double locDemandMod)
{
	if(WillDo0to1(iRepurchaseProb))
	{
		// need to skip a trip for BOGO
		if (iFlags & kSet_BOGOSkipNextTrip)
		{
			YallComeBackNow();
			iFlags &= kClr_BOGOSkipNextTrip;
		}
		else
		{
			iFlags |= kSet_ReadyToBuy;
			iPurchaseState = kPurchaseState_WaitForRepurchase;
		}
	}
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
/*void	Consumer::DoTasksNIMO(double locDemandMod)
{
	//only do this if it is time to shopping, or a trip is scheduled...
	if (iRepurchaseCount == 0)
		CompleteShoppingListNIMO(locDemandMod);
}

// ------------------------------------------------------------------------------
//    For each task
//        Figure out how many tasks I'll need for a shopping trip interval
//        Round up fractional to whole tasks
//        Add those to shopping list
// ------------------------------------------------------------------------------
void	Consumer::CompleteShoppingListNIMO(double locDemandMod)
{
//    For each consumer task
//        Figure out how many tasks I'll do in a shopping trip interval
//		  See how much product they use up
//        Round up fractional to whole tasks
//        Add those to shopping list

	int	consTaskNum;
	double	numTasksToDoFloat;
	//int	prodTaskNum;
	//double	numTasksCanDoWithProdInStock;
	//int	prodNum;
	//int	numTasksToBuy;
	//double	numSKUsToBuy;
	//int	slIndex = ShopListIndex(group, guyNum);
	//int*	shopListTasksNeededPtr;
	//int*	shopListProdsPtr;

	// need to make a temporary list of prods instock, for planning purposes,
	// which allows the consumer to estimate product needs over the next shopping trip interval
	//KMK 10/22/04 CreateTempProdsInStockList(group, guyNum);

	// need a temporary shopping list, to store fractional tasks required
	// we would not have fractional tasks before this, because if we ran out while doing a task,
	// we would have Added an integer task, and running out is the only way
	// there could already be something on this consumer's shopping list
	//KMK 10/22/04 CreateTempShoppingList();

	// For each of the tasks
	// we only have one task for NIMO
	// SSN 11/9/05
	//for (consTaskNum = 0; consTaskNum < segment->iTaskRates.size(); consTaskNum++)
	//{
		//shopListTasksNeededPtr = &((iPopulation->iShopListTasksNeeded)[slIndex + consTaskNum]);
		//shopListProdsPtr = &((iPopulation->iShopListProds)[slIndex + consTaskNum]);

		numTasksToDoFloat = 1;
		// pick from an exponential distribution
		ScheduleNextShoppingTrip(&numTasksToDoFloat);

	//} // go to next task

	//KMK 10/22/04 UpdateRealShopList(group, guyNum);
	//KMK 10/22/04 RestoreProdsInStockList(group, guyNum);
}*/


