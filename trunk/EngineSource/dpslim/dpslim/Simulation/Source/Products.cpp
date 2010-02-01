// Products.cpp
//
// Copyright 1998	Salamander Interactive
//
// Handles the product, feature and channel specific activities
// Created by Ken Karakotsios 11/6/98

#include "CMicroSegment.h"

#include "Product.h"
#include "Consumer.h"
#include "RepurchaseModel.h"
#include "Channel.h"
#include "Attributes.h"
#include "PopObject.h"
#include "ChoiceModel.h"
#include "Display.h"

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
Product::Product()
{
	iBrandNum = 0;
	iProductIndex = 0;
	iProductID = 0;
	iPreUseFeatureScore = 0;
	iPostUseFeatureScore = 0;
	iPreUsePriceScore = 0;
	iPostUsePriceScore = 0;
	iInitialAwareness = 0;
	iInitialDropRate = 0;
	iDropRateDecay = 0;
	iBase_price = 0;
	iEq_units = 0;
	iNumAwareSofar = 0;					
	iInitialPenetrationPct = 0;	
	iInitialSharePct = 0;
	iTotalInitialUnitsSold = 0;
	iProdActiveInSomeChannel = 0;
	iTotalPersuasion = 0;
	iNumEverTriedProduct = 0;
	iRepeatPurchasersCumulative = 0;
	iTotalRepeatPurchaseOccasionsCumulative = 0;
	iInitialPersuasion = 0;
	iGRPsThisTick = 0;
	iAddsThisTick = 0;
	iDropsThisTick = 0;
	iHavePrepreqs = 0;

	iProdName = CString();
	bin_scale_factors = vector< double >();
	bin_scale_factors.push_back(1);
	iPrerequisiteNum = vector<Index>();
	iInCompatibleNum = vector<Index>();
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	CMicroSegment::initialize_product_channel_record(ProductChannelRecord* aPCR)
{
	aPCR->iProdName.Empty();
	aPCR->iChanName.Empty();
	aPCR->iActive = false;
	aPCR->iSize = 1;
	aPCR->iNumTrips = 0;
	aPCR->iNotProcessed = true;
	int	ptNum;
	for (ptNum = 0; ptNum < kNumPriceTypes; ptNum++)
	{
		aPCR->iPrice[ptNum] = 0.0;
		aPCR->iProbPrice[ptNum] = 0.0;
		aPCR->iAmountPurchasedThisTick[ptNum] = 0;
		aPCR->iPreProdPriceScores[ptNum] = 0.0;
		aPCR->iPostProdPriceScores[ptNum] = 0.0;
	}
	aPCR->iNumCouponPurchases = 0;
	aPCR->iNumCouponsToBeRedeemed = 0;
	aPCR->iAmConsideredForThisConsumer = false;
	aPCR->iPreUseDistributionPercent = 0.0;
	aPCR->iPostUseDistributionPercent = 0.0;
	aPCR->iTotalPurchaseOccasionsThisTick = 0;
	aPCR->iMarketUtility.clear();
	aPCR->msgWeightedScores = 0.0;
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::build_product_channel_list()
{
	iProductsAvailable.resize(iProducts.size() * iChannels.size());

	for (Cn_iter cn_iter = iChannels.begin(); cn_iter != iChannels.end(); ++cn_iter)
	{
		for(Pn_iter pn_iter = iProducts.begin(); pn_iter != iProducts.end(); ++pn_iter)
		{
			(*pn_iter)->iProdActiveInSomeChannel = false;
			add_product_channel((*pn_iter)->iProductIndex, (*cn_iter)->iChannelIndex);
		}
	}
	iNeedToCalcAttrScores = true;
}

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::add_product_channel(Index pn, Index cn)
{
	if (pn == -1 || cn == -1)
	{
		return;
	}

	ProductChannelRecord* tempPCR = new ProductChannelRecord();

	initialize_product_channel_record(tempPCR);

	tempPCR->iChanName = iChannels[cn]->iChanName;
	tempPCR->iChannelID = iChannels[cn]->iChannelID;
	tempPCR->iChannelIndex = iChannels[cn]->iChannelIndex;

	tempPCR->iProdName = iProducts[pn]->iProdName;
	tempPCR->iProductID = iProducts[pn]->iProductID;
	tempPCR->iProductIndex = iProducts[pn]->iProductIndex;

	Pcn pcn = Pcn(pn, cn);

	iProductsAvailable[pcn] = tempPCR;

	int i = iProductsAvailable.size();

	product_id_map[iProducts[pn]->iProductID] = iProducts[pn]->iProductIndex;
	channel_id_map[iChannels[cn]->iChannelID] = iChannels[cn]->iChannelIndex;
}

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::compute_cumulative_channel_percents(void)
{
	double	prevCumPct = 0.0;
	double	pctChoice;
	double  scale = iChannels.size();

	for (Cn_iter iter = iChannels.begin(); iter != iChannels.end(); ++iter)
	{
		pctChoice = (*iter)->iPctChanceChosen;
		(*iter)->iCumPctChanceChosen = pctChoice + prevCumPct;
		prevCumPct += pctChoice;
	}

	// normalize
	for (Cn_iter iter = iChannels.begin(); iter != iChannels.end(); ++iter)
	{
		if (prevCumPct > 0.0)
		{
			(*iter)->iCumPctChanceChosen /= prevCumPct;
		}
		else if (scale > 0.0)
		{
			(*iter)->iCumPctChanceChosen = 1.0/scale;
		}
	}
}

void CMicroSegment::reset_attributes()
{
	for(Pan_container::iterator iter = iProductAttributes.begin(); iter != iProductAttributes.end(); ++iter)
	{
		iter->second->clear();
		delete iter->second;
	}
	iProductAttributes.clear();
	for(Pn_iter iter = iProducts.begin(); iter != iProducts.end(); ++iter)
	{
		ID prod_id = (*iter)->iProductID;
		if(iProductAttributes.find(prod_id) == iProductAttributes.end())
		{
			iProductAttributes[prod_id] = new map<ID, ProductAttribute*>();
		}
	}
}

void CMicroSegment::reset_products()
{
	for(Pn_iter iter = iProducts.begin(); iter != iProducts.end(); ++iter)
	{
		(*iter)->iTotalPersuasion = 0;
		(*iter)->iNumAwareSofar = 0;
		(*iter)->iNumEverTriedProduct = 0;
		(*iter)->iRepeatPurchasersCumulative = 0;
		(*iter)->iTotalRepeatPurchaseOccasionsCumulative = 0;
	}
}

// ------------------------------------------------------------------------------
// 
// ------------------------------------------------------------------------------
void	CMicroSegment::calc_product_scores()
{
	ID p_id;
	ID a_id;
	for(Pn_iter pn_iter = iProducts.begin(); pn_iter != iProducts.end(); ++pn_iter)
	{
		(*pn_iter)->iPreUseFeatureScore = 0;
		(*pn_iter)->iPostUseFeatureScore = 0;
		(*pn_iter)->iPreUsePriceScore = 0;
		(*pn_iter)->iPostUsePriceScore = 0;
		for(Can_iter can_iter = iAttributePreferences.begin(); can_iter != iAttributePreferences.end(); ++can_iter)
		{
			p_id = (*pn_iter)->iProductID;
			a_id = can_iter->first;
			if(iProductAttributes.find(p_id) != iProductAttributes.end() && iProductAttributes[p_id]->find(a_id) != iProductAttributes[p_id]->end())
			{
				(*pn_iter)->iPreUseFeatureScore += can_iter->second->GetPreUse()*(*iProductAttributes[p_id])[a_id]->GetPreUse();
				(*pn_iter)->iPostUseFeatureScore += can_iter->second->GetPostUse()*(*iProductAttributes[p_id])[a_id]->GetPostUse();
				(*pn_iter)->iPreUsePriceScore += can_iter->second->GetPriceSensitivity()*(*iProductAttributes[p_id])[a_id]->GetPreUse();
				(*pn_iter)->iPostUsePriceScore += can_iter->second->GetPriceSensitivity()*(*iProductAttributes[p_id])[a_id]->GetPostUse();
			}
		}
	}

	iNeedToCalcAttrScores = false;

	calc_price_scores();
}

// ------------------------------------------------------------------------------
// 
// ------------------------------------------------------------------------------
void CMicroSegment::calc_price_scores(void)
{
	double	totalPrice[kNumPriceTypes];
	Index	pn;
	Pcn		pcn;
	double	averageUnpromotedPrice = 1;
	double	numPrices;

	if(choiceModel->iGCMf8_PriceValueSource == kGCMf8_RelativePrice)
	{
		averageUnpromotedPrice = 0.0;
		numPrices = 0;

		for (Pcn_iter iter = iProductsAvailable.begin(); iter != iProductsAvailable.end(); ++iter)
		{
			if ((*iter)->iActive)
			{
				averageUnpromotedPrice += (*iter)->iPrice[kPriceTypeUnpromoted];
				numPrices += 1;
			}
		}

		if (numPrices > 0)
		{
			averageUnpromotedPrice /= numPrices;
		}
	}

	for (Pcn_iter iter = iProductsAvailable.begin(); iter != iProductsAvailable.end(); ++iter)
	{
		pcn = GetPcn(iter);
		if ((*iter)->iActive)
		{
			pn = pcn.pn;

			int	ptNum;	// KLMK 4/30/05
			for (ptNum= 0; ptNum < kNumPriceTypes; ptNum++)
			{
				totalPrice[ptNum] = (*iter)->iPrice[ptNum];
			}

			
			switch (choiceModel->iGCMf8_PriceValueSource)
			{
				case kGCMf8_AbsolutePrice:
				case kGCMf8_PricePerUse:
					break;
				case kGCMf8_RelativePrice:
					for (ptNum = 0; ptNum < kNumPriceTypes; ptNum++)	// KMK 4/30/05
						totalPrice[ptNum] /= averageUnpromotedPrice;
					break;
				case kGCMf8_ReferencePrice:
					for (ptNum = 0; ptNum < kNumPriceTypes; ptNum++)	// KMK 4/30/05
						totalPrice[ptNum] /= iProducts[pn]->iBase_price;
					break;
			}


			for (ptNum = 0; ptNum < kNumPriceTypes; ptNum++)	// KMK 4/30/05
			{
				//Pre Use Score
				(*iter)->iPreProdPriceScores[ptNum] = -totalPrice[ptNum] * (iPriceSensitivity + iProducts[pn]->iPreUsePriceScore);

				//Post Use Score
				(*iter)->iPostProdPriceScores[ptNum] = -totalPrice[ptNum] * (iPriceSensitivity + iProducts[pn]->iPostUsePriceScore);
			}

		}
	}

	iNeedToCalcPriceScores = false;
}


