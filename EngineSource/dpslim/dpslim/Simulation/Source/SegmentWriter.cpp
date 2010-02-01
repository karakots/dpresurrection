// MSDatabaseMethods.cpp
//
// Copyright 2006 DecisionPower, Inc.
//
// Database calls for the microsegment
// Created by Isaac Noble 5/13/06

#include "SegmentWriter.h"

#include "DBSegData.h"
#include "ProductTree.h"
#include "Product.h"
#include "PopObject.h"

SegmentDataAcc::SegmentDataAcc(const CTime& initTime)
{
	m_segmentID = 1;
  	m_productID = 1;
	m_channelID = 1;

	m_date = initTime;
	m_lastWrite = initTime;

	m_percent_aware_sku_cum = 0.0;
	m_persuasion_sku = 0.0;
	m_GRPs_SKU_tick = 0.0;
	m_promoprice = 0.0;
	m_unpromoprice = 0.0;
	m_sku_dollar_purchased_tick = 0.0;
	m_percent_preuse_distribution_sku = 0.0;
	m_percent_on_display_sku = 0.0;
	m_percent_sku_at_promo_price = 0.0;
	m_num_sku_bought = 0.0;
	m_num_units_unpromo = 0.0;
	m_num_units_promo = 0.0;
	m_num_units_display = 0.0;
	m_display_price = 0.0;
	m_percent_at_display_price = 0.0;
	m_eq_units = 0.0;
	m_volume = 0.0;

	m_num_adds_sku = 0;
	m_num_drop_sku = 0;
	m_num_coupon_redemptions = 0;
	m_num_units_bought_on_coupon = 0;
	m_num_sku_triers = 0;
	m_num_sku_repeaters = 0;
	m_num_sku_repeater_trips_cum = 0;
	num_trips = 0;
}

CTime SegmentDataAcc::getDate()
{
	return m_date;
}

CTime SegmentDataAcc::getLastWrite()
{
	return m_lastWrite;
}

void SegmentDataAcc::AddInfo(CMicroSegment* segment, Pcn pcn)
{
	Index pn = pcn.pn;
	Index cn = pcn.cn;
	ProductTree::LeafNodeList leafs(pcn.pn);
	double scale_factor = segment->iPopulation->iPopScaleFactor;
	

	m_date = segment->Current_Time;


	m_segmentID = segment->iSegmentID;

	m_productID = segment->iProductsAvailable[pcn]->iProductID;

	m_channelID = segment->iProductsAvailable[pcn]->iChannelID;

	// Product Adds
	// This should be calculated correctly for non-leafs nodes elseware
	m_num_adds_sku += (long)(scale_factor * (double)segment->iProducts[pn]->iAddsThisTick);

	// Product Drops
	// This should be calculated correctly for non-leafs nodes elseware
	m_num_drop_sku += (long)(scale_factor * (double)segment->iProducts[pn]->iDropsThisTick);

	// Product Awareness
	double	numAware = 0;
	double	temp = 0;
	for(ProductTree::Iter iter = leafs.begin(); iter != leafs.end(); ++iter)
	{
		Index leafPn = *iter;
		temp = segment->iProducts[leafPn]->iNumAwareSofar ;
		if( temp > numAware )
		{
			numAware = temp;
		}
	}

	m_percent_aware_sku_cum += 100.0 * numAware/ ((double) segment->iPopulation->iConsumerList.size());

	// Product Persuasion
	double	totalPersuasion = 0;
	temp = 0;
	for(ProductTree::Iter iter = leafs.begin(); iter != leafs.end(); ++iter)
	{
		Index leafPn = *iter;
		
		totalPersuasion += segment->iProducts[leafPn]->iTotalPersuasion;		
	}

	if (numAware > 0)
	{
		m_persuasion_sku += totalPersuasion/numAware;
	}

	// GRPs for SKU (this tick)
	// This should be calculated correctly for non-leafs nodes elseware
	m_GRPs_SKU_tick += 100.0 * segment->iProducts[pn]->iGRPsThisTick / ((double) segment->iPopulation->iConsumerList.size());

	// NOTE Items from here on do not depend on Segment
	// Promoted Price in Channel
	m_promoprice += segment->iProductsAvailable[pcn]->iPrice[kPriceTypePromoted];

	// Un-promoted Price in Channel 
	m_unpromoprice += segment->iProductsAvailable[pcn]->iPrice[kPriceTypeUnpromoted];


	m_display_price += segment->iProductsAvailable[pcn]->iPrice[kPriceTypeDisplayAbsolute];

	// TODO: need to track BOGO price in the tansaction file

	// MISNAMED SSN: actually how many coupons were received
	m_num_coupon_redemptions += (long)(scale_factor * (double)segment->iProductsAvailable[pcn]->iNumCouponsToBeRedeemed);

	// units bought on coupon this day in Channel
	m_num_units_bought_on_coupon += (long)(scale_factor * (double)segment->iProductsAvailable[pcn]->iNumCouponPurchases);

	// SKUs bought this day in Channel
	int	ptNum;
	double sum = 0;	// KMK 4/30/05
	double sum_unpromo = 0;
	double sum_display = 0;
	double sum_promo = 0;
	double sum_eq = 0;
	double sum_vol = 0;
	double sum_temp = 0;
	for(ProductTree::Iter iter = leafs.begin(); iter != leafs.end(); ++iter)
	{
		Index  leafPn = *iter;
		Pcn leafPcn = Pcn(leafPn, cn);
		sum_temp = 0.0;
		for (ptNum = 0; ptNum < kNumPriceTypes; ptNum++)
		{
			sum_temp += segment->iProductsAvailable[leafPcn]->iAmountPurchasedThisTick[ptNum];
		}
		sum += sum_temp;
		sum_eq += sum_temp * segment->iProducts[leafPn]->iEq_units;
		sum_vol += sum_temp * segment->iProductsAvailable[leafPcn]->iSize;
		sum_unpromo += segment->iProductsAvailable[leafPcn]->iAmountPurchasedThisTick[kPriceTypeUnpromoted];
		sum_display += segment->iProductsAvailable[leafPcn]->iAmountPurchasedThisTick[kPriceTypeDisplayAbsolute];
		sum_promo += segment->iProductsAvailable[leafPcn]->iAmountPurchasedThisTick[kPriceTypePromoted];
	}
	m_num_sku_bought += scale_factor * sum;
	m_volume += scale_factor * sum_vol;
	m_eq_units += scale_factor * sum_eq;

	m_num_units_unpromo += scale_factor * sum_unpromo;
	m_num_units_display += scale_factor * sum_display;
	m_num_units_promo += scale_factor * sum_promo;

	// Num Trips
	sum = 0;
	for(ProductTree::Iter iter = leafs.begin(); iter != leafs.end(); ++iter)
	{
		Index  leafPn = *iter;
		Pcn leafPcn = Pcn(leafPn, cn);
		sum += segment->iProductsAvailable[leafPcn]->iNumTrips;
	}
	num_trips += (long)(scale_factor * (double)sum);
	
	// SKU Triers in Cumulative Channel
	m_num_sku_triers += (long)(scale_factor * (double)segment->iProducts[pn]->iNumEverTriedProduct);

	// SKU Repeaters Cumulative in Channel
	m_num_sku_repeaters += (long)(scale_factor * (double)segment->iProducts[pn]->iRepeatPurchasersCumulative);

	m_num_sku_repeater_trips_cum += (long)(scale_factor * (double)segment->iProducts[pn]->iTotalRepeatPurchaseOccasionsCumulative);

	// SKU $ purchased this tick in Channel
	sum = 0;	// KMK 4/30/05
	for(ProductTree::Iter iter = leafs.begin(); iter != leafs.end(); ++iter)
	{
		Index  leafPn = *iter;
		Pcn leafPcn = Pcn(leafPn, cn);
		for (ptNum = 0; ptNum < kNumPriceTypes; ptNum++)
		{
			sum += segment->iProductsAvailable[leafPcn]->iAmountPurchasedThisTick[ptNum] * segment->iProductsAvailable[leafPcn]->iPrice[ptNum] * segment->iProductsAvailable[leafPcn]->iSize;
		}
	}
	//aFloatVal = (iProductsAvailable[pcn].iAmountPurchasedThisTickUnpromoted * iProductsAvailable[pcn].iUnpromotedPrice) + 
	//	(iProductsAvailable[pcn].iAmountPurchasedThisTickPromoted * iProductsAvailable[pcn].iPromotedPrice) +
	//	(iProductsAvailable[pcn].iAmountPurchasedThisTickBOGO * iProductsAvailable[pcn].iBOGOPrice);
	m_sku_dollar_purchased_tick += scale_factor * sum;

	// % Distribution for SKU in Channel
	m_percent_preuse_distribution_sku += 100 * segment->iProductsAvailable[pcn]->iPreUseDistributionPercent;

	// % on Display for SKU in Channel
	sum = 0.0;
	for (MU_iter iter = segment->iProductsAvailable[pcn]->iMarketUtility.begin(); iter != segment->iProductsAvailable[pcn]->iMarketUtility.end(); ++iter)
	{
			sum += segment->ScaledOddsOfDisplay(pcn, iter->second);
	}
	
	m_percent_on_display_sku += sum * 100.0;

	double scale = 0;

	if (segment->iProductsAvailable[pcn]->iPreUseDistributionPercent > 0)
	{
		if (segment->iUseACVDistribution)
		{
			scale = 100.0/(segment->iProductsAvailable[pcn]->iPreUseDistributionPercent);
		}
		else
		{
			scale = 100;
		}
	}


	// % SKU at promoted price in Channel
	if (scale > 0.0)
	{
		double val = scale * segment->iProductsAvailable[pcn]->iProbPrice[kPriceTypePromoted];

		if (val < 100)
		{
			m_percent_sku_at_promo_price += val;
		}
		else
		{
			m_percent_sku_at_promo_price += 100.0;
		}

		val = scale * segment->iProductsAvailable[pcn]->iProbPrice[kPriceTypeDisplayAbsolute];

		if (val < 100)
		{
			m_percent_at_display_price += val;
		}
		else
		{
			m_percent_at_display_price += 100.0;
		}
	}
}

void SegmentDataAcc::getInfo(SegmentData* segRecord)
{
	segRecord->m_segmentID = m_segmentID;		
	segRecord->m_productID = m_productID;
	segRecord->m_channelID = m_channelID;

	// segRecord->m_date = m_date;
	// making this ouput in daylight savings time as needed
	// SSN 3/28/2007
	segRecord->m_date = CTime(m_date.GetYear(),m_date.GetMonth(), m_date.GetDay(),0,0,0,-1);

	segRecord->m_percent_aware_sku_cum = (float)m_percent_aware_sku_cum;
	segRecord->m_persuasion_sku = (float)m_persuasion_sku;
	segRecord->m_GRPs_SKU_tick = (float)m_GRPs_SKU_tick;
	segRecord->m_promoprice = (float)m_promoprice;
	segRecord->m_unpromoprice = (float)m_unpromoprice;
	segRecord->m_sku_dollar_purchased_tick = (float)m_sku_dollar_purchased_tick;
	segRecord->m_percent_preuse_distribution_sku = (float)m_percent_preuse_distribution_sku;
	segRecord->m_percent_on_display_sku = (float)m_percent_on_display_sku;
	segRecord->m_percent_sku_at_promo_price = (float)m_percent_sku_at_promo_price;
	segRecord->m_num_units_unpromo = (float)m_num_units_unpromo;
	segRecord->m_num_units_promo = (float)m_num_units_promo;
	segRecord->m_num_units_display = (float)m_num_units_display;
	segRecord->m_display_price = (float)m_display_price;
	segRecord->m_percent_at_display_price = (float)m_percent_at_display_price;
	segRecord->m_eq_units = (float)m_eq_units;
	segRecord->m_volume = (float)m_volume;

	segRecord->m_num_adds_sku = m_num_adds_sku;
	segRecord->m_num_drop_sku = m_num_drop_sku;
	segRecord->m_num_coupon_redemptions = m_num_coupon_redemptions;
	segRecord->m_num_units_bought_on_coupon = m_num_units_bought_on_coupon;
	segRecord->m_num_sku_bought = (float)m_num_sku_bought;
	segRecord->m_num_sku_triers = m_num_sku_triers;
	segRecord->m_num_sku_repeaters = m_num_sku_repeaters;
	segRecord->m_num_sku_repeater_trips_cum = m_num_sku_repeater_trips_cum;
	segRecord->num_trips = num_trips;
}

void SegmentDataAcc::reset(const CTime & time)
{
	m_lastWrite = time;
	m_segmentID = 1;
  	m_productID = 1;
	m_channelID = 1;

	m_date = CTime();

	m_percent_aware_sku_cum = 0.0;
	m_persuasion_sku = 0.0;
	m_GRPs_SKU_tick = 0.0;
	m_promoprice = 0.0;
	m_unpromoprice = 0.0;
	m_sku_dollar_purchased_tick = 0.0;
	m_percent_preuse_distribution_sku = 0.0;
	m_percent_on_display_sku = 0.0;
	m_percent_sku_at_promo_price = 0.0;
	m_num_sku_bought = 0.0;
	m_num_units_unpromo = 0.0;
	m_num_units_promo = 0.0;
	m_num_units_display = 0.0;
	m_display_price = 0.0;
	m_percent_at_display_price = 0.0;
	m_eq_units = 0.0;
	m_volume = 0.0;

	m_num_adds_sku = 0;
	m_num_drop_sku = 0;
	m_num_coupon_redemptions = 0;
	m_num_units_bought_on_coupon = 0;
	m_num_sku_triers = 0;
	m_num_sku_repeaters = 0;
	m_num_sku_repeater_trips_cum = 0;
	num_trips = 0;
}