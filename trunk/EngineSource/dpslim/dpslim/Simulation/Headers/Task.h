#pragma once
#define _CRT_SECURE_NO_WARNINGS

#include <vector>

using namespace std;

class PantryTask
{
private:
	vector< double > rates;
public:
	PantryTask(int);
	void SetRate(int, double);
	double GetRate(int);
	double GetAmountUsed(int, double, double);
};