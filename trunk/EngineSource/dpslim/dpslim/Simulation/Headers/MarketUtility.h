#pragma once
#define _CRT_SECURE_NO_WARNINGS
//
// Header File for MarketUtilityScheduler
//
// This is the minimal component
//

#include "stdafx.h"

#include "Display.h"

class MarketUtility : public TimeSeriesData
{
public:
	MarketUtility();
	MarketUtility(MarketUtilityRecordset*);
	MarketUtility(DistDisplayRecordset*);
	MarketUtility(double, double, double, double, int, int, int, CTime&, CTime&);
	void ModifySegment(CMicroSegment*, CTime&);
	bool ActiveOnDate(CTime&);
	bool DeactiveOnDate(CTime&);
	double GetPersuasion();
	double GetAwareness();
	double GetDistribution();
	double GetUtility();
	int IsDisplay();
private:
	double utility;
	double persuasion;
	double awareness;
	double distribution;
	int display;
};