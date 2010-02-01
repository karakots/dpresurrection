#pragma once
#define _CRT_SECURE_NO_WARNINGS

#include "stdafx.h"

class CMicroSegment;

class Pantry
{
private:
	vector< double > bins;
	CMicroSegment* my_segment;
	double activation_energy;

public:
	Pantry( CMicroSegment* );
	void AddBin();
	void SetBin(int,double);
	void Reset();
	void PerformTask(PantryTask*, int);
	void AddProduct(Pcn);
	double ComputeUtility(Pcn);
	double GetActivationEnergy();
	void SetActivationEnergy(double);
};