// TimeSeriesData.cpp : This is a abstract class used to define
// arbitrary time series data
//
// Copyright 2007 by  DecisionPower
//
// author: Isaac Noble
// creation data: 9/01/2007


#include "TimeSeriesData.h"

// ------------------------------------------------------------------------------
// Basic Constructors
// ------------------------------------------------------------------------------
TimeSeriesData::TimeSeriesData()
{
	segment_id = -1;
	product_id = -1;
	channel_id = -1;
	start_date = CTime();
	end_date = CTime();
}

TimeSeriesData::TimeSeriesData(int segment, int product, int channel, CTime& start, CTime& end)
{
	segment_id = segment;
	product_id = product;
	channel_id = channel;
	start_date = CTime(start.GetYear(), start.GetMonth(), start.GetDay(), 0, 0, 0, 0);
	end_date = CTime(end.GetYear(), end.GetMonth(), end.GetDay(), 0, 0, 0, 0);
}

// ------------------------------------------------------------------------------
// Basic Accessors
// ------------------------------------------------------------------------------
void TimeSeriesData::SetSegment(int segment)
{
	segment_id = segment;
}

void TimeSeriesData::SetProduct(int product)
{
	product_id = product;
}

void TimeSeriesData::SetChannel(int channel)
{
	channel_id = channel;
}

void TimeSeriesData::SetStartDate(CTime & date)
{
	start_date = CTime(date.GetYear(), date.GetMonth(), date.GetDay(), 0, 0, 0, 0);
}

void TimeSeriesData::SetEndDate(CTime & date)
{
	end_date = CTime(date.GetYear(), date.GetMonth(), date.GetDay(), 0, 0, 0, 0);
}

int TimeSeriesData::GetSegment()
{
	return segment_id;
}

int TimeSeriesData::GetProduct()
{
	return product_id;
}

int TimeSeriesData::GetChannel()
{
	return channel_id;
}

CTime TimeSeriesData::GetStartDate()
{
	return start_date;
}

CTime TimeSeriesData::GetEndDate()
{
	return end_date;
}














