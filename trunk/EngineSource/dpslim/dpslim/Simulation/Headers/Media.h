#pragma once
#define _CRT_SECURE_NO_WARNINGS

#include "stdafx.h"
#include "TimeSeriesData.h"

class Media : public TimeSeriesData
{
public:
	Media(MassMediaRecordset*);
	Media(double, double, double, int, int, int, CTime&, CTime&);
	void ModifySegment(CMicroSegment*, CTime&);
	bool ActiveOnDate(CTime&);
	bool DeactiveOnDate(CTime&);
	double GRPs(CTime&);
	double GetPersuasion();
	double GetAwareness();
private:
	double persuasion;
	double awareness;
	double grps;
};