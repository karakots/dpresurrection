#pragma once
#define _CRT_SECURE_NO_WARNINGS

#include "stdafx.h"

#include "TimeSeriesData.h"

class ExternalFactor : public TimeSeriesData
{
public:
	ExternalFactor(ProductEventRecordset*);
	ExternalFactor(double, int, int, int, CTime&, CTime&);
	void ModifySegment(CMicroSegment*, CTime&);
	bool ActiveOnDate(CTime&);
	bool DeactiveOnDate(CTime&);
	double GetDemand();
private:
	double demand;
};