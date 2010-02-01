#pragma once
#define _CRT_SECURE_NO_WARNINGS

#include "stdafx.h"

class InitialCondition
{
public:
	InitialCondition();
	InitialCondition(InitialCondsRecordset*);

	int GetSegmentID();
	int GetProductID();

	double GetPenetration();
	double GetAwareness();
	double GetPersuasion();
	double GetShare();

private:
	int segment_id;
	int product_id;
	double persuasion;
	double awareness;
	double share;
	double penetration;
};