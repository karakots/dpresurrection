// RepurchaseModel.cpp
//
// Copyright 2005	DecisionPower, Inc.
//
// Choice Model for Microsegment
// Created by Steve Noble

#include "MicroSegmentConst.h"
#include "RepurchaseModel.h"
#include "RandLib.h"

RepurchaseModel::RepurchaseModel()
{
	// default values
	type = kRepurchaseModel_NBD;
	iAverageMaxUnits = 0.0;

	iGammaA = 1.0;
	iGammaK = 1.0;
	
	iRepurchase = false;

	iRepurchaseFrequencyUnits = kRepurchaseFreq_Months;
	iRepurchaseFrequencyDuration = 30;
	iNBDstddev = 6;

}

RepurchaseModel::~RepurchaseModel()
{
}

// deterministic
long RepurchaseModel::NumTicks()
{
	switch (iRepurchaseFrequencyUnits)
	{
	case kRepurchaseFreq_Weeks:
		return  (long)(iRepurchaseFrequencyDuration * 7);
	case kRepurchaseFreq_Months:
		return  (long)((iRepurchaseFrequencyDuration * 365)/12);
	case kRepurchaseFreq_Years:
		return  (long)(iRepurchaseFrequencyDuration * 365);
	}

	// case kRepurchaseFreq_Days:
	return (long)iRepurchaseFrequencyDuration;
}

double RepurchaseModel::Ticks(double diversityPercent)
{
	if (!iRepurchase)
		return 0;

	double gamVal;
	double countVal;
	double		a;
	double		r;
	double		days;
	double lambda;
	double prob;

	switch (type)
	{
	case kRepurchaseModel_NBD:
		days = 365.0/iRepurchaseFrequencyDuration;
		a = days/iNBDstddev;
		r = a*days;
		countVal = gengam(a,r);
		lambda = 1.0/countVal;
		prob = 1-exp(-lambda);

		return prob;
	case kRepurchaseModel_TaskBased_NBDrate:	// OK
		a = 1.0/iGammaA;	// location
		r = iGammaK;	// shape
		gamVal = gengam(a, r);
		if (gamVal < 0)
			gamVal = 1;
		else if (gamVal < 0.01)
			gamVal = 0.01;

		countVal = (365.0 / gamVal);

		lambda = 1.0/countVal;
		prob = 1-exp(-lambda);

		return prob;
	}

	return iRepurchaseFrequencyDuration;
}

short RepurchaseModel::NBDScale()
{
	switch (type)
	{
	case kRepurchaseModel_NBD:
		return 0;
	case kRepurchaseModel_TaskBased_NBDrate:	// OK
		return 100;
	}

	return 0;
}

short RepurchaseModel::State()
{
	if (iRepurchase)
		return  kPurchaseState_WaitForRepurchase;

	return kPurchaseState_WaitForFirstMessage;
}
