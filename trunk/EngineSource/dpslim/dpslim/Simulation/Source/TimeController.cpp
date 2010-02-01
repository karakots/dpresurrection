// TimeController.cpp : This is the new time controller
// Reading the data is complicated by the fact that the data is
// stored in a vector, need to investigate whether a map is a
// better choice
//
// Copyright 2007 by  DecisionPower
//
// author: Isaac Noble
// creation data: 9/01/2007

#include "TimeController.h"

// ------------------------------------------------------------------------------
// Basic Constructors
// ------------------------------------------------------------------------------
TimeController::TimeController()
{
	this->start_date = CTime();
}

TimeController::TimeController(ATL::CTime & start_date, ATL::CTime & end_date)
{
	this->start_date = CTime(start_date.GetYear(),start_date.GetMonth(),start_date.GetDay(), 0, 0, 0, 0);
	this->end_date = CTime(end_date.GetYear(),end_date.GetMonth(),end_date.GetDay(), 0, 0, 0, 0);

	CTimeSpan span = end_date - start_date;

	for(size_t i = 0; i <= span.GetDays() + 1; ++i)
	{
		active_on_date.push_back(new vector<TimeSeriesData*>());
		deactive_on_date.push_back(new vector<TimeSeriesData*>());
	}
}

TimeController::~TimeController()
{
	for(size_t i = 0; i < active_on_date.size(); ++i)
	{
		active_on_date[i]->clear();
		delete active_on_date[i];
	}

	for(size_t i = 0; i < deactive_on_date.size(); ++i)
	{
		deactive_on_date[i]->clear();
		delete deactive_on_date[i];
	}


	active_on_date.clear();
	deactive_on_date.clear();
}

// ------------------------------------------------------------------------------
// Adds the data to the time controller
// ------------------------------------------------------------------------------
void TimeController::AddData(TimeSeriesData * data)
{
	//Initialize
	int day = 0;
	CTime date = CTime(start_date.GetYear(),start_date.GetMonth(),start_date.GetDay(), 0, 0, 0, 0);

	if(data->GetStartDate() < start_date && data->GetEndDate() >= start_date)
	{
		data->SetStartDate(start_date);
	}
	if(data->GetEndDate() > end_date && data->GetStartDate() <= end_date)
	{
		data->SetEndDate(end_date);
	}

	//First see if array needs to be extended
	ExtendData(data);

	//Loop over all dates and add events when the data is active
	while(date <= data->GetEndDate())
	{
		//Check if active on date...this is virtual so each type of time series data
		//can determine when it needs to be active
		if(data->ActiveOnDate(date))
		{
			//Add the event for this day
			active_on_date[day]->push_back(data);
		}

		if(data->DeactiveOnDate(date))
		{
			//Add the event for this day
			deactive_on_date[day]->push_back(data);
		}

		//Increment the day and the date
		++day;
		date += CTimeSpan(1,0,0,0);
	}
}

// ------------------------------------------------------------------------------
// This function extends the data array to include the last possible active date
// for the data...assumes that the data will not be active after the end_date
// This is where things get a little dirty
// ------------------------------------------------------------------------------
void TimeController::ExtendData(TimeSeriesData* data)
{
	CTimeSpan span = data->GetEndDate() - start_date;
	if(span.GetDays()+1 <= active_on_date.size())
	{
		return;
	}
	else
	{
		for(size_t i = active_on_date.size(); i <= span.GetDays(); ++i)
		{
			active_on_date.push_back(new vector<TimeSeriesData*>());
			deactive_on_date.push_back(new vector<TimeSeriesData*>());
		}
	}
}

// ------------------------------------------------------------------------------
// Gets all all the events active on a given date
// ------------------------------------------------------------------------------
vector<TimeSeriesData*>* TimeController::ActiveOnDate(ATL::CTime & date)
{
	//I think this is the correct day :)
	CTimeSpan span = date - start_date;
	return active_on_date[(size_t)span.GetDays()];
}

vector<TimeSeriesData*>* TimeController::ActiveOnDate(int day)
{
	//Much simpler, this would be the preferred interface
	return active_on_date[day];
}

// ------------------------------------------------------------------------------
// Gets all all the events on a given data
// ------------------------------------------------------------------------------
vector<TimeSeriesData*>* TimeController::DeactiveOnDate(ATL::CTime & date)
{
	//I think this is the correct day :)
	CTimeSpan span = date - start_date;
	return deactive_on_date[(size_t)span.GetDays()];
}

vector<TimeSeriesData*>* TimeController::DeactiveOnDate(int day)
{
	//Much simpler, this would be the preferred interface
	return deactive_on_date[day];
}
