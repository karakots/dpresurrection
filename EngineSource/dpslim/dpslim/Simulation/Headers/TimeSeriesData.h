// TimeSeriesData.h : This is a abstract class used to define
// arbitrary time series data
//
// Copyright 2007 by  DecisionPower
//
// author: Isaac Noble
// creation data: 9/01/2007

#pragma once
#define _CRT_SECURE_NO_WARNINGS

#include "stdafx.h"
#include "DateMath.h"

class TimeSeriesData
{
public:
	//Basic Constructors
	TimeSeriesData();
	TimeSeriesData(int, int, int, CTime&, CTime&);

	//Accessors
	void SetSegment(int);
	void SetProduct(int);
	void SetChannel(int);
	void SetStartDate(CTime &);
	void SetEndDate(CTime &);
	int GetSegment();
	int GetProduct();
	int GetChannel();
	CTime GetStartDate();
	CTime GetEndDate();

	//Virtual functions

	//This function defines the dates when the event will be
	//active and processed by the time controller
	virtual bool ActiveOnDate(CTime&) = 0;

	//This function defines the date(s) that the time series data
	//will be deactivated
	virtual bool DeactiveOnDate(CTime&) = 0;


	//All time series data should know how it modifies the MicroSegment
	//For example, MassMedia should call segment->BroadcastAd(...)
	virtual void ModifySegment(CMicroSegment*,CTime&) = 0;
private:

	//Private Data
	//Not really private due to accesors but hey...
	CTime start_date;
	CTime end_date;
	int segment_id;
	int product_id;
	int channel_id;
};