#pragma once
#define _CRT_SECURE_NO_WARNINGS

#include "stdafx.h"

#include "TimeSeriesData.h"

class Price : public TimeSeriesData
{
public:
	Price(ProdChanRecordset*);
	Price(double, double, CString, int, int, int, CTime&, CTime&);
	void ModifySegment(CMicroSegment*, CTime&);
	bool ActiveOnDate(CTime&);
	bool DeactiveOnDate(CTime&);
	int GetType();
	double	GetPrice(CTime&);
	double	GetDistribution(CTime&);
	bool	Deactivate(CTime&);
private:
	int GetType(CString type);
	double price;
	int type;
	double distribution;
};