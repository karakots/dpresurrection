#pragma once
#define _CRT_SECURE_NO_WARNINGS

#include "stdafx.h"

#include "TimeSeriesData.h"

class Distribution : public TimeSeriesData
{
public:
	Distribution(DistDisplayRecordset*);
	Distribution(double, double, double, double, int, int, int, CTime&, CTime&);
	void ModifySegment(CMicroSegment*, CTime&);
	bool ActiveOnDate(CTime&);
	bool DeactiveOnDate(CTime&);
	double GetPreUseDistribution(CTime&);
	double GetPostUseDistribution(CTime&);
	double GetAwareness();
	double GetPersuasion();
private:
	double persuasion;
	double awareness;
	double post_use_dist;
	double pre_use_dist;
};