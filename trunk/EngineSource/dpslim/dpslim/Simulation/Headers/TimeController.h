// TimeController.h : This is the new time controller
// A vector is used to store all the events on any given day
// Alternatively a map between CTimes and Events could be used.
//
// Copyright 2007 by  DecisionPower
//
// author: Isaac Noble
// creation data: 9/01/2007



#pragma once
#define _CRT_SECURE_NO_WARNINGS

//Precompiled Header -- contains CMicroSegment.h and MSDatabase.h
#include "stdafx.h"

//TimeSeriesData to be used by the time controller
#include "TimeSeriesData.h"

class TimeController
{
public:
	//Basic Constuctors
	TimeController();
	TimeController(CTime&,CTime&);
	~TimeController();

	//Gets all events that are becoming active for a given CTime
	vector<TimeSeriesData*>* ActiveOnDate(CTime&);
	//Gets all events for a given day, with day 0 being the model start date
	vector<TimeSeriesData*>* ActiveOnDate(int);

	
	//Gets all events that are becoming active for a given CTime
	vector<TimeSeriesData*>* DeactiveOnDate(CTime&);
	//Gets all events for a given day, with day 0 being the model start date
	vector<TimeSeriesData*>* DeactiveOnDate(int);



	//Adds events to the time controller
	void AddData(TimeSeriesData*);
private:
	//Used for extending the last day of the time controller to accomidate the data
	void ExtendData(TimeSeriesData*);

	//Data array...perhaps a map would be better: map<CTime* , vector< TimeSeriesData* > >???
	vector< vector< TimeSeriesData* >* > active_on_date;
	vector< vector< TimeSeriesData* >* > deactive_on_date;

	//Model start date
	CTime start_date;
	CTime end_date;
};