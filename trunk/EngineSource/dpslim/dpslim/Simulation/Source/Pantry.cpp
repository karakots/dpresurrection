#include "Pantry.h"
#include "Product.h"
#include "Function.h"
#include "Task.h"

Pantry::Pantry(CMicroSegment *segment)
{
	my_segment = segment;
	bins = vector< double >();
}

void Pantry::AddBin()
{
	bins.push_back(0);
}

void Pantry::SetBin(int bin, double value)
{
	bins[bin] = value;
}

void Pantry::Reset()
{
	for(size_t i = 0;  i < bins.size(); ++i)
	{
		bins[i] = 0;
	}
}

void Pantry::AddProduct(Pcn pcn)
{
	for(size_t i = 0; i < bins.size(); ++i)
	{
		bins[i] += my_segment->iProducts[pcn.pn]->bin_scale_factors[i]*my_segment->iProductsAvailable[pcn]->iSize;
	}
}

double Pantry::ComputeUtility(Pcn pcn)
{
	double utility = 0;
	double temp_bin = 0;
	for(size_t i = 0; i < bins.size(); ++i)
	{
		temp_bin = bins[i] + my_segment->iProducts[pcn.pn]->bin_scale_factors[i]*my_segment->iProductsAvailable[pcn]->iSize;
		utility += my_segment->iFunctions[my_segment->iBinFunctions[i]]->Integrate(bins[i],temp_bin);
	}

	return utility;
}

void Pantry::PerformTask(PantryTask* task, int num_days)
{
	for( int i = 0; i < bins.size(); ++i)
	{
		bins[i] = bins[i] - task->GetAmountUsed(i, num_days, my_segment->iCatQuantityPurchasedModifier);
	}
}

double Pantry::GetActivationEnergy()
{
	return activation_energy;
}

void Pantry::SetActivationEnergy(double energy)
{
	activation_energy = energy;
}