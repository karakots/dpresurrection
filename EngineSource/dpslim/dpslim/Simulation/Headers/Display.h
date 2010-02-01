#pragma once
#define _CRT_SECURE_NO_WARNINGS

#include "stdafx.h"

#include "TimeSeriesData.h"

class Display : public TimeSeriesData
{
public:
	Display();
	Display(DistDisplayRecordset*);
	Display(double, double, double, int, int, int, CTime&, CTime&);
	void ModifySegment(CMicroSegment*, CTime&);
	bool ActiveOnDate(CTime&);
	bool DeactiveOnDate(CTime&);
	double GetPersuasion();
	double GetAwareness();
	double GetDistribution();
	void SetAwareness(double);
	void SetPersuasion(double);
	void SetDistribution(double);
private:
	double persuasion;
	double awareness;
	double distribution;
};