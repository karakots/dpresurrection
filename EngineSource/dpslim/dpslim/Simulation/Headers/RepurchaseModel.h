#pragma once
#define _CRT_SECURE_NO_WARNINGS

// RepurchaseModel.h
//
// Copyright 2005	DecisionPower
//
// How consumers repurchase

// encapsulating the choice math
class RepurchaseModel
{
public:	// fields
	int type;
	double iAverageMaxUnits;

	double iGammaA;
	double iGammaK;
	int iRepurchase;

	int iRepurchaseFrequencyUnits;
	double iRepurchaseFrequencyDuration;
	double iNBDstddev;

	
public:	// queries

	// deterministic
	long NumTicks();

	// includes random variation
	double Ticks(double percentDiverse);

	// initial purchase state
	short State();

	// initial scale for NBD
	short NBDScale();


public: 

	// Constructors & destructors
	RepurchaseModel();
	~RepurchaseModel();
};

