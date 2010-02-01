// Purchasing.cpp
//
// Copyright 1998 - 2001	DecisionPower, Inc.
//
// Purchasing model for Microsegment
// Created by Ken Karakotsios aug 12 1998

#include "CMicroSegment.h"


#include <stdlib.h>
#include <math.h>

#include "SocialNetwork.h"
#include "Consumer.h"
#include "RepurchaseModel.h"
#include "DBModel.h"
#include "ProductTree.h"
#include "Product.h"
#include "PopObject.h"
#include "Channel.h"
#include "ChoiceModel.h"
#include "Display.h"

// ------------------------------------------------------------------------------
// Make sure that the sum of the probabilities for BOGO, promoted and unpromoted prices Adds up to 100%
// ------------------------------------------------------------------------------
void	CMicroSegment::check_for_active_products(void)
{
	double	probSum;
	int	ptType;
	double priceSum;
	Index pn;

	for(Pn_iter iter = iProducts.begin(); iter != iProducts.end(); ++iter)
	{
		(*iter)->iProdActiveInSomeChannel = false;
	}

	for (Pcn_iter iter = iProductsAvailable.begin(); iter != iProductsAvailable.end(); ++iter)
	{
		pn = GetPcn(iter).pn;
		probSum = 0.0;
		priceSum = 0.0;

		if (!iModelInfo->promoted_price)
		{	
			probSum = 1.0;
			priceSum = abs((*iter)->iPrice[kPriceTypeUnpromoted]);
		}
		else
		{
			for (ptType = 0; ptType < kNumPriceTypes; ptType++)	// KMK 4/30/05
			{
				// negative distribution is user error: note in log
				if ((*iter)->iProbPrice[ptType] < 0)
				{
					(*iter)->iProbPrice[ptType] = 0.0;
				}

				probSum += (*iter)->iProbPrice[ptType];
				priceSum += abs((*iter)->iPrice[ptType]);
			}
		}

		if (!iModelInfo->distribution)
		{
			(*iter)->iPreUseDistributionPercent = 1.0;
		}

		// activate product in channel ?
		if ((*iter)->iPreUseDistributionPercent > 0 && priceSum > 0 && probSum > 0)
		{
			if ((*iter)->iActive == false)
			{
				iNeedToCalcAttrScores = true;
			}
			(*iter)->iActive = true;
			iProducts[pn]->iProdActiveInSomeChannel = true;	//  SSN
		}
		else
		{
			(*iter)->iActive = false;
		}
	}
}


// ------------------------------------------------------------------------------
// Each consumer needs a preferred channel to buy from.
// If a consumer does not have one, give it one
// ------------------------------------------------------------------------------
void	CMicroSegment::assign_initial_channelsto_consumers(void)
{
	Consumer *aGuy;

	for (size_t indexInGroup = 0; indexInGroup < iPopulation->iConsumerList.size(); indexInGroup++)
	{
		aGuy = iPopulation->iConsumerList[indexInGroup];
		aGuy->SelectChannel();
	}
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	CMicroSegment::clean_up(void)
{
	for (size_t i = 0; i < iPopulation->iConsumerList.size(); i++)
	{
		iPopulation->iConsumerList[i]->iBoughtThisTick = false;
		iPopulation->iConsumerList[i]->iRecommend = false;
	}

	//Clear out channel data
	for (Pcn_iter iter = iProductsAvailable.begin(); iter != iProductsAvailable.end(); ++iter)
	{	
		(*iter)->iTotalPurchaseOccasionsThisTick = 0;

		for (int ptNum = 0; ptNum < kNumPriceTypes; ptNum++)
		{
			(*iter)->iAmountPurchasedThisTick[ptNum] = 0;
		}

		(*iter)->iNumCouponPurchases = 0;
		
		(*iter)->iNumCouponsToBeRedeemed = 0;

		(*iter)->iNumTrips = 0;
	}

	for (Pn_iter iter = iProducts.begin(); iter != iProducts.end(); ++iter)
	{
		(*iter)->iGRPsThisTick = 0;
		(*iter)->iAddsThisTick = 0;
		(*iter)->iDropsThisTick = 0;
	}
	
	iCatQuantityPurchasedModifier = 0.0;
	iCatShoppingTripFrequencyModifier = 0.0;
	iCatTaskRateModifier = 0.0;
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	CMicroSegment::SimStep(void)
{
	check_for_active_products();

	if(iNeedToCalcAttrScores)
	{
		calc_product_scores();
	}
	if(iNeedToCalcPriceScores)
	{
		calc_price_scores();
	}

	distribute_media();

	if(this->iNumDays % this->iModelInfo->access_time == 0)
	{
		for (Index pn = 0; pn < iProducts.size(); pn++)
		{
				for (int guyNum = 0; guyNum < iPopulation->iConsumerList.size(); guyNum++)
				{
					Consumer* aGuy = iPopulation->iConsumerList[guyNum];
					aGuy->Aware(pn);
				}
		}
	}

	external_factors();

	consumer_shopping();

	process_social_networks();

	write_purchase_info_to_file();


	if(this->iModelInfo->writing_checkpoint)
	{
		CTime date = Current_Time;
		CTime modelEndDate = CTime(iModelInfo->end_date.GetYear(),iModelInfo->end_date.GetMonth(), iModelInfo->end_date.GetDay(),0,0,0,0);
		CTimeSpan diffEnd = modelEndDate - date;
		int diffEndInDays = (int)diffEnd.GetDays();
		if(diffEndInDays == 0)
		{
			// Convert a TCHAR string to a LPCSTR
			CT2CA pszConvertedAnsiString (this->iModelInfo->checkpoint_file);

			// construct a std::string using the LPCSTR input
			std::string strStd (pszConvertedAnsiString);
			string file = "checkpoints\\";
			file += strStd;
			file += "\\popdata";

			CT2CA pszConvertedAnsiString2 (this->iNameString);
			strStd = std::string(pszConvertedAnsiString2);

			file += strStd;
			this->iPopulation->WriteToFile(file);
		}
	}

	iNumDays++;

	clean_up();
}


// ------------------------------------------------------------------------------
// Need to recompute the percent, to account for eliminated products.
// ------------------------------------------------------------------------------
void CMicroSegment::initial_condition_processing(void)
{
	bool dayZeroReset = false;

	if (iModelInfo->reset_panel_data_day == 0)
	{
		// if we are reseting on day zero
		// we fool the initializers into thinking the reset is tomorrow
		iModelInfo->reset_panel_data_day = 1;
		dayZeroReset = true;
	}

	// normalize share
	double total = 0.0;
	for(Pn_iter iter = iProducts.begin(); iter != iProducts.end(); ++iter)
	{
		total += (*iter)->iInitialSharePct;
	}

	if (total > 0)
	{
		for(Pn_iter iter = iProducts.begin(); iter != iProducts.end(); ++iter)
		{
			(*iter)->iInitialSharePct /= total;
		}
	}

	assign_initial_channelsto_consumers();
	set_initial_awareness();
	set_initial_penetration();
	set_initial_persuasion();
	set_initial_share();

	// reset panel data if needed

	if (dayZeroReset)
	{
		// now when we continue or purchasing the
		// initial conditions will not affect us
		iModelInfo->reset_panel_data_day = 0;
	}
}



void CMicroSegment::set_initial_awareness()
{
	Index pn;
	for(Pn_iter iter = iProducts.begin(); iter != iProducts.end(); ++iter)
	{
		pn = (*iter)->iProductIndex;
		for (size_t indexInGroup = 0; indexInGroup < iPopulation->iConsumerList.size(); indexInGroup++)
		{
			Consumer* aGuy = iPopulation->iConsumerList[indexInGroup];
			if (WillDo0to1((*iter)->iInitialAwareness))
			{
				aGuy->MakeConsumerAware(pn, false);
			}
		}
	}
}

void CMicroSegment::set_initial_penetration()
{
	Index pn;
	double odds = 0.0;
	for(Pn_iter iter = iProducts.begin(); iter != iProducts.end(); ++iter)
	{
		pn = (*iter)->iProductIndex;
		for (size_t indexInGroup = 0; indexInGroup < iPopulation->iConsumerList.size(); indexInGroup++)
		{
			odds = (*iter)->iInitialSharePct;

			Consumer* aGuy = iPopulation->iConsumerList[indexInGroup];
			if( odds < (*iter)->iInitialPenetrationPct)
			{
				odds = (*iter)->iInitialPenetrationPct;
			}

			if (WillDo0to1(odds))
			{
				aGuy->ConsumerIsTryingProduct((*iter)->iProductIndex, false);

				// may need to reject product here	start KMK 6/3/04
				// Penetration should Add how many tries?  I think 3
				aGuy->SeeIfRepurchaserDrops(pn, 0);
				aGuy->SeeIfRepurchaserDrops(pn, 1);
				aGuy->SeeIfRepurchaserDrops(pn, 2);
			}
		}
	}	
}

void CMicroSegment::set_initial_persuasion()
{
	Index pn;
	for(Pn_iter iter = iProducts.begin(); iter != iProducts.end(); ++iter)
	{
		pn = (*iter)->iProductIndex;
		for (size_t indexInGroup = 0; indexInGroup < iPopulation->iConsumerList.size(); indexInGroup++)
		{
			Consumer* aGuy = iPopulation->iConsumerList[indexInGroup];
			if (aGuy->Aware(pn))
			{
				aGuy->Persuade(pn, (*iter)->iInitialPersuasion, false);
			}
		}
	}
}

void CMicroSegment::set_initial_share()
{
	Index pn;
	double odds = 0.0;
	
	for(Pn_iter iter = iProducts.begin(); iter != iProducts.end(); ++iter)
	{
		pn = (*iter)->iProductIndex;
		
		odds = (*iter)->iInitialSharePct;

		// since we are only concerned with agents who have tried
		// we adjust prob
		if ((*iter)->iInitialPenetrationPct > 0)
		{
			odds /= (*iter)->iInitialPenetrationPct;
		}

		if (odds > 0)
		{
			for (size_t indexInGroup = 0; indexInGroup < iPopulation->iConsumerList.size(); indexInGroup++)
			{
				Consumer* aGuy = iPopulation->iConsumerList[indexInGroup];
				if(aGuy->EverTriedProduct(pn, false))
				{
					if (WillDo0to1(odds))
					{
						aGuy->iProductLastBought = pn;

						//// pick a channel with resppect to the channel choosing probs
						//aGuy->ChooseAChannel();

						//Pcn pcn(pn, aGuy->iPreferredChannel);

						//iProductsAvailable[pcn]->iAmountPurchasedThisTick[kPriceTypeUnpromoted] += 1;
					}
				}
			}
		}
	}
}

// ------------------------------------------------------------------------------
// KMK 7/27/04 A product can be available at a promoted, unpromoted, or BOGO price
// If it is not unpromoted, the split betwen bogo and promoted needs to be each's probability out of the sum of their
// probabilities, because it is a conditional probability, conditioned on the price not being unpromoted
// KMK 4/30/05 The conditional probabilities have already been adjusted at the SSIO input for BOGO.
// Therefore, there is no need to adjust the price here, again.
// ------------------------------------------------------------------------------
double CMicroSegment::SetPrice(Pcn pcn)
{
	int priceType = -1;
	if (!iModelInfo->promoted_price)
	{	
		iProductsAvailable[pcn]->iUsingPriceType = kPriceTypeUnpromoted;
		return iProductsAvailable[pcn]->iPrice[kPriceTypeUnpromoted];
	}

	// modified to assume prices are ACV
	// this means we need to divide by the distribution to get the right prob.
	// SSN 10/26/2006

	double dist = iProductsAvailable[pcn]->iPreUseDistributionPercent;

	if (!iUseACVDistribution)
		dist = 1.0;

	// double	priceTypeRandVal = LocalRand(RAND_MAX) / (double) RAND_MAX;
	double	priceTypeRandVal = dist * LocalRandUniform();

	double	ptTest = iProductsAvailable[pcn]->iProbPrice[kPriceTypePromoted];
	double	interval;

	if (priceTypeRandVal < ptTest)
	{
		iProductsAvailable[pcn]->iUsingPriceType = kPriceTypePromoted;
		return iProductsAvailable[pcn]->iPrice[kPriceTypePromoted];
	}

	interval = iProductsAvailable[pcn]->iProbPrice[kPriceTypeBOGO];
	if (interval > 0.0)
	{
		ptTest += interval;
		if (priceTypeRandVal < ptTest)
		{
			iProductsAvailable[pcn]->iUsingPriceType = kPriceTypeBOGO;
			return iProductsAvailable[pcn]->iPrice[kPriceTypeBOGO];
		}
	}

	interval = iProductsAvailable[pcn]->iProbPrice[kPriceTypeReduced];
	if (interval > 0.0)
	{
		ptTest += interval;
		if (priceTypeRandVal < ptTest)
		{
			iProductsAvailable[pcn]->iUsingPriceType = kPriceTypeReduced;
			return iProductsAvailable[pcn]->iPrice[kPriceTypeReduced];
		}
	}

	interval = iProductsAvailable[pcn]->iProbPrice[kPriceTypeDisplayPercent];
	if (interval > 0.0)
	{
		ptTest += interval;
		if (priceTypeRandVal < ptTest)
		{
			iProductsAvailable[pcn]->iUsingPriceType = kPriceTypeDisplayPercent;
			return iProductsAvailable[pcn]->iPrice[kPriceTypeDisplayPercent];
		}
	}

	interval = iProductsAvailable[pcn]->iProbPrice[kPriceTypeDisplayAbsolute];
	if (interval > 0.0)
	{
		ptTest += interval;
		if (priceTypeRandVal < ptTest)
		{
			iProductsAvailable[pcn]->iUsingPriceType = kPriceTypeDisplayAbsolute;
			return iProductsAvailable[pcn]->iPrice[kPriceTypeDisplayAbsolute];
		}
	}
	
	iProductsAvailable[pcn]->iUsingPriceType = kPriceTypeUnpromoted;
	return iProductsAvailable[pcn]->iPrice[kPriceTypeUnpromoted];
}


// ------------------------------------------------------------------------------
// adjust everyone's repeat purchase timer...
// ------------------------------------------------------------------------------
void	CMicroSegment::consumer_shopping(void)
{
	for (size_t i=0; i < iPopulation->iConsumerList.size(); ++i)
	{
		iPopulation->iConsumerList[i]->Shop();
	}
}

// ------------------------------------------------------------------------------
// Recommend our purchased product to the people we know
// (But not for repeat purchases)
// ------------------------------------------------------------------------------
void	CMicroSegment::process_social_networks(void)
{
	static Bucket bucket = Bucket();

	if (socialNetworks != 0)
	{
		// TODO: pick guys from a bucket
		int numGuys = (int)iPopulation->iConsumerList.size();

		

		bucket.Reset(numGuys);

		while(!bucket.Empty())
		{
			int guy = bucket.Draw();

			Consumer* aGuy = iPopulation->iConsumerList[guy];

			if (aGuy->iRecommend)
			{
				// he has something to talk about

				vector<SocialNetwork*>::iterator netPtr;

				for( netPtr = socialNetworks->begin(); netPtr != socialNetworks->end(); ++netPtr)
				{
					SocialNetwork* network = *netPtr;

					network->MakeRecommendations(guy);
				}

				aGuy->iRecommend = false;
			}
		} 
	} 

	return;
}


// ------------------------------------------------------------------------------
// See if there is any of specific product to buy on current tick
// This implements limied product availability
// ------------------------------------------------------------------------------
int	CMicroSegment::IsProductInStock(Pcn pcn)
{
	double	distPercent;
	int preUse = iProductsAvailable[pcn]->iHaveNotUsedProductBefore;

	if (preUse)
	{
		distPercent = iProductsAvailable[pcn]->iPreUseDistributionPercent;
	}
	else
	{
		distPercent = iProductsAvailable[pcn]->iPostUseDistributionPercent;
	}

	if (distPercent < 1.0)
	{
		if  (!WillDo0to1(distPercent))
		{
			return false;
		}
	}

	return true;
}



