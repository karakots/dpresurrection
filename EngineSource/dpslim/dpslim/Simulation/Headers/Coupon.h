#pragma once
#define _CRT_SECURE_NO_WARNINGS

#include "stdafx.h"

#include "TimeSeriesData.h"

class Coupon : public TimeSeriesData
{
public:
	Coupon(MassMediaRecordset*);
	Coupon(double, double, double, double, int, int, int, CTime&, CTime&);
	void ModifySegment(CMicroSegment*, CTime&);
	bool ActiveOnDate(CTime&);
	bool DeactiveOnDate(CTime&);
	double	GetRedemption();
	double	GetReach();
	double  GetSpan();
	const CTime & GetEndDate();
	double	GetPersuasion();
	double	GetAwareness();
private:
	double persuasion;
	double awareness;
	double redemption;
	double reach;
	double span;
	CTime  end;
};