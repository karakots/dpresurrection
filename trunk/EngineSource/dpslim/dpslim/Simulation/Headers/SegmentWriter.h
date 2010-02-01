#pragma once
#define _CRT_SECURE_NO_WARNINGS
// SegmentWriter.h
//
// Copyright 2006	DecisionPower
//
// SegmentWriter Class
//
// Author;	Isaac S. Noble
// Date:	6/1/2006

#include "stdafx.h"

class SegDataRecordset;
class CMicroSegment;
class SegmentData;

class SegmentDataAcc
{
public:

	SegmentDataAcc(const CTime&);
	void AddInfo(CMicroSegment*, Pcn pcn);
	void getInfo(SegmentData*);
	void reset(const CTime&);
	CTime getDate();
	CTime getLastWrite();

private:
	CTime	m_lastWrite;
	long	m_segmentID;
  	long	m_productID;
	long	m_channelID;

	CTime	m_date;

	double	m_percent_aware_sku_cum;
	double	m_persuasion_sku;
	double	m_GRPs_SKU_tick;
	double	m_promoprice;
	double	m_unpromoprice;
	double	m_sku_dollar_purchased_tick;
	double	m_percent_preuse_distribution_sku;
	double	m_percent_on_display_sku;
	double	m_percent_sku_at_promo_price;
	double	m_num_sku_bought;
	double	m_num_units_unpromo;
	double	m_num_units_promo;
	double	m_num_units_display;
	double	m_display_price;
	double	m_percent_at_display_price;
	double	m_eq_units;
	double	m_volume;

	long	m_num_adds_sku;
	long	m_num_drop_sku;
	long	m_num_coupon_redemptions;
	long	m_num_units_bought_on_coupon;
	long	m_num_sku_triers;
	long	m_num_sku_repeaters;
	long	m_num_sku_repeater_trips_cum;
	long	num_trips;
};