// Product.h : Seperated out the product structure and made it a class
//
// Copyright 2007 by  DecisionPower
//
// author: Isaac Noble
// creation data: 9/01/2007


#include "CMicroSegment.h"

class	Product
{
public:
	Product();
	vector< double > bin_scale_factors;
	
	
	vector<Index>	iPrerequisiteNum;
	vector<Index>	iInCompatibleNum;
	CString			iProdName;
	int				iBrandNum;
	int				iProductIndex;
	int				iProductID;
	double			iPreUseFeatureScore;
	double			iPostUseFeatureScore;
	double			iPreUsePriceScore;
	double			iPostUsePriceScore;
	double			iInitialAwareness;
	double			iInitialDropRate;
	double			iDropRateDecay;
	double			iBase_price;
	double			iEq_units;
	double			iNumAwareSofar;					// tracks how much awareness has been set at each step of the initialization process
	double			iInitialPenetrationPct;				// non-persistent
	double			iInitialSharePct;					// non-persistent
	int				iTotalInitialUnitsSold;
	int				iProdActiveInSomeChannel;
	double			iTotalPersuasion;
	int				iNumEverTriedProduct;
	int				iRepeatPurchasersCumulative;
	int				iTotalRepeatPurchaseOccasionsCumulative;
	double			iInitialPersuasion;
	double			iGRPsThisTick;
	int				iAddsThisTick;
	int				iDropsThisTick;
	int				iHavePrepreqs;

	Product(const Product& x)
	{
		unsigned int	i;
		iProdName = x.iProdName;
		iProductID = x.iProductID;
		iPreUseFeatureScore = x.iPreUseFeatureScore;
		iPostUseFeatureScore = x.iPostUseFeatureScore;
		iPreUsePriceScore = x.iPreUsePriceScore;
		iPostUsePriceScore = x.iPostUsePriceScore;
		iInitialAwareness = x.iInitialAwareness;
		iInitialDropRate = x.iInitialDropRate;
		iDropRateDecay = x.iDropRateDecay;
		iBase_price = x.iBase_price;
		iEq_units = x.iEq_units;
		iGRPsThisTick = x.iGRPsThisTick;
		iHavePrepreqs = x.iHavePrepreqs;
		iInitialPersuasion = x.iInitialPersuasion;
		iNumAwareSofar = x.iNumAwareSofar;
		iInitialPenetrationPct = x.iInitialPenetrationPct;
		iInitialSharePct = x.iInitialSharePct;
		iTotalInitialUnitsSold = x.iTotalInitialUnitsSold;
		
		iProdActiveInSomeChannel = x.iProdActiveInSomeChannel;

		for (i=0; i<x.iPrerequisiteNum.size(); i++)
		{
			iPrerequisiteNum.push_back(x.iPrerequisiteNum[i]);
		}
		for (i=0; i<x.iInCompatibleNum.size(); i++)
		{
			iInCompatibleNum.push_back(x.iInCompatibleNum[i]);
		}
		for (i=0; i<x.bin_scale_factors.size(); i++)
		{
			bin_scale_factors.push_back(x.bin_scale_factors[i]);
		}
	}

	Product(const Product* x)
	{
		Product();
		unsigned int	i;
		iProdName = x->iProdName;
		iProductID = x->iProductID;
		iPreUseFeatureScore = x->iPreUseFeatureScore;
		iPostUseFeatureScore = x->iPostUseFeatureScore;
		iPreUsePriceScore = x->iPreUsePriceScore;
		iPostUsePriceScore = x->iPostUsePriceScore;
		iInitialAwareness = x->iInitialAwareness;
		iInitialDropRate = x->iInitialDropRate;
		iDropRateDecay = x->iDropRateDecay;
		iBase_price = x->iBase_price;
		iEq_units = x->iEq_units;
		iGRPsThisTick = x->iGRPsThisTick;
		iHavePrepreqs = x->iHavePrepreqs;
		iInitialPersuasion = x->iInitialPersuasion;
		iNumAwareSofar = x->iNumAwareSofar;
		iInitialPenetrationPct = x->iInitialPenetrationPct;
		iInitialSharePct = x->iInitialSharePct;
		iTotalInitialUnitsSold = x->iTotalInitialUnitsSold;
		
		iProdActiveInSomeChannel = x->iProdActiveInSomeChannel;

		for (i=0; i<x->iPrerequisiteNum.size(); i++)
		{
			iPrerequisiteNum.push_back(x->iPrerequisiteNum[i]);
		}
		for (i=0; i<x->iInCompatibleNum.size(); i++)
		{
			iInCompatibleNum.push_back(x->iInCompatibleNum[i]);
		}
		for (i=0; i<x->bin_scale_factors.size(); i++)
		{
			bin_scale_factors.push_back(x->bin_scale_factors[i]);
		}
	}

	Product& operator=(const Product& x)
	{
		unsigned int	i = 0;
		iProdName = x.iProdName;
		iProductID = x.iProductID;
		iPreUseFeatureScore = x.iPreUseFeatureScore;
		iPostUseFeatureScore = x.iPostUseFeatureScore;
		iPreUsePriceScore = x.iPreUsePriceScore;
		iPostUsePriceScore = x.iPostUsePriceScore;
		iInitialAwareness = x.iInitialAwareness;
		iInitialDropRate = x.iInitialDropRate;
		iDropRateDecay = x.iDropRateDecay;
		iBase_price = x.iBase_price;
		iEq_units = x.iEq_units;
		iGRPsThisTick = x.iGRPsThisTick;
		iHavePrepreqs = x.iHavePrepreqs;
		iInitialPersuasion = x.iInitialPersuasion;
		iNumAwareSofar = x.iNumAwareSofar;
		iInitialPenetrationPct = x.iInitialPenetrationPct;
		iInitialSharePct = x.iInitialSharePct;
		iTotalInitialUnitsSold = x.iTotalInitialUnitsSold;
		iProdActiveInSomeChannel = x.iProdActiveInSomeChannel;

		for (i=0; i<x.iPrerequisiteNum.size(); i++)
		{
			iPrerequisiteNum.push_back(x.iPrerequisiteNum[i]);
		}
		for (i=0; i<x.iInCompatibleNum.size(); i++)
		{
			iInCompatibleNum.push_back(x.iInCompatibleNum[i]);
		}
		for (i=0; i<x.bin_scale_factors.size(); i++)
		{
			bin_scale_factors.push_back(x.bin_scale_factors[i]);
		}
		return *this;
	}

};