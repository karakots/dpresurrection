

#include "randlib.h"
#include "CMicroSegment.h"
#include "Task.h"

using namespace std;

PantryTask::PantryTask(int bins)
{
	rates = vector< double >();
	for(int i = 0; i < bins; i++)
	{
		rates.push_back(0);
	}
}

void PantryTask::SetRate(int bin, double rate)
{
	rates[bin] = rate;
}

double PantryTask::GetRate(int bin)
{
	return rates[bin];
}

double PantryTask::GetAmountUsed(int bin, double num_days, double demand_modifer)
{
	return CMicroSegment::iRandomLib.Poisson(rates[bin]*num_days*(1+demand_modifer));
	//return RandomPoisson(rates[bin]*num_days*(1+demand_modifer));
	//return rates[bin]*num_days*(1+demand_modifer);
}