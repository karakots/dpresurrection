// ChoiceModel.cpp
//
// Copyright 2005-2006	DecisionPower, Inc.
//
// Choice Model for Microsegment
// Created by Steve Noble


#include "MicroSegmentConst.h"
#include "ChoiceModel.h"
#include "RandLib.h"

ChoiceModel::ChoiceModel()
{
}

ChoiceModel::~ChoiceModel()
{
}

int ChoiceModel::ClearSiblingPurchase()
{
	if (iChoiceModel == kModel_General && iGCMf10_InertiaModel == kGCMf10_SameSKU)
		return true;

	return false;
}

// will we make a purchase with a given sensitivity?
int ChoiceModel::MsgSensitivityCheck(double valToCheck)
{
	switch (iChoiceModel)
	{
		case kModel_Bass:
		case kModel_CrossingTheChasm:
		case kModel_Emergent:
		{
			// message sensitivity of 0 or 10 will result in no purchases
			// I am guessing these are hardcoded mins and maxs
			if ((valToCheck == 0) || (valToCheck == 10))
				return false;
			break;
		}
		case kModel_General:	// OK
		{
			// if we are multiplying in the message score, then we want to return -1
			// for extreme values of message sensitivity
			switch (iGCMf3_PersuasionContrib)
			{
				case kGCMf3_Addition:
					break;
				case kGCMf3_Multiplication:
					// KMK 10/04/05	if ((localMsgSensitivity == 0) || (localMsgSensitivity == 10))
					if (valToCheck == 0)
						return false;
					break;
			}
			break;
		}
	}

	return true;
}

// leagacy - should be removed when we reserect the TBB code
int ChoiceModel::ScaleScoresNonLogit()
{
	switch (iChoiceModel)
	{
		case kModel_Bass:
		case kModel_CrossingTheChasm:
		case kModel_Emergent:
			return true;
		case kModel_General:	// OK
		{
			switch (iGCMf9_ChoiceProbability)
			{
				case kGCMf9_ShareOfScore:
					return true;
			}
			break;
		}
	} //end switch

	return false;
}



// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
double	ChoiceModel::GetErrVal(double scoreIn, double utility)
{
	double	valToAdd;
	double	average;

	switch (iGCMf11_ErrorTerm)
	{
		case kGCMf11_UserValue:
		{
			valToAdd = gennor(iGCM_ErrorTermUserValue, iGCM_ErrorTermSD) - 
				(iGCM_ErrorTermUserValue / 2.0);
			break;
		}
		case kGCMf11_Utility:
		{
			average = utility * iGCM_ErrorTermUserValue;	// KMK 4/30/05
			valToAdd = gennor(average, iGCM_ErrorTermSD) - (average / 2.0);
			break;
		}
		case kGCMf11_Score:
		{
			average = scoreIn * iGCM_ErrorTermUserValue;
			valToAdd = gennor(average , iGCM_ErrorTermSD) - (average / 2.0);
			break;
		}
		default:
			valToAdd = 0.0;
	}
	return valToAdd;
}


int	ChoiceModel::DoLogitCalc()
{
	switch (iChoiceModel)
	{
	case kModel_Bass:
	case kModel_CrossingTheChasm:
	case kModel_Emergent:
		return false;
	case kModel_General:	// OK
		{
			switch (iGCMf9_ChoiceProbability)
			{
			case kGCMf9_ShareOfScore:
				return false;
			}	
		}
	}

	// default
	return true;
}

int	ChoiceModel::AddScores()
{
	switch (iChoiceModel)
	{
	case kModel_Bass:
	case kModel_CrossingTheChasm:
	case kModel_Emergent:
	case kModel_EmergentLogit:
		return false;
	
	case kModel_General:	// OK
		{
			switch (iGCMf3_PersuasionContrib)
			{
			case kGCMf3_Multiplication:
				return false;
			}
		}
	}

	return true;
}

int	ChoiceModel::MessSensExp()
{
	switch (iChoiceModel)
	{
	case kModel_EmergentLogit:
		return true;
	case kModel_Bass:
	case kModel_CrossingTheChasm:
	case kModel_Emergent:
		return false;
		break;
	case kModel_Linear:
	case kModel_LinearSOV:
		return false;
	case kModel_General:	// OK
		{
			switch (iGCMf1_PersuasionScore)
			{
			case kGCMf1_Multiplication:
				return false;
			case kGCMf1_Exponentiation:
				return true;
			}
		}
	}

	// default
	return false;
}

int	ChoiceModel::AddErrToScore()
{
	switch (iChoiceModel)
	{
	case kModel_EmergentLogit:
		return false;
	case kModel_Bass:
	case kModel_CrossingTheChasm:
	case kModel_Emergent:
		return false;
	case kModel_Linear:
	case kModel_LinearSOV:
		return false;
	case kModel_General:	// OK
		{	
			switch (iGCMf11_ErrorTerm)
			{
			case kGCMf11_None:
				return false;
			case kGCMf11_UserValue:
			case kGCMf11_Utility:
			case kGCMf11_Score:
				return true;
			}
		}
	}

	// default
	return false;
}

double ChoiceModel::MsgContribution(double persuasion,  double sensitivity)
{
	const double invariant = 5.0;

	if (sensitivity == 0)
		return 0;

	if (persuasion == 0)
		return 0;

	// abuse of notation
	// SSN 6-30-2007
	double k = iGCM_ErrorTermUserValue;

	if ( k < 0.1)
		k = 0.1;

	int neg_flag = false;

	if(persuasion < 0)
	{
		persuasion = -persuasion;
		neg_flag = true;
	}

	double messageScorePortion = 0;

	switch (iGCMf2_PersuasionValComp)
	{
	case kGCMf2_ShareOfVoice:
		{
			// ok ok this is not really share of voice
			// but it is actually better
			// the problem with share of voice is that as the number of products increases
			// the sensitivity o each one drops to zero. So Adding a product means recalibrating
			// SSN 6-30-2007
			// note: F < K & F'(0) = 1
			messageScorePortion = k * persuasion/(persuasion + k);
		}

		break;
	case kGCMf2_Absolute:
		messageScorePortion = persuasion;
		break;
	case kGCMf2_squareRoot:
		// note: F < K  & F'(0) = 1
		messageScorePortion = k * (1 - exp(-persuasion/k));
		break;
	case kGCMf2_base10log:
		// note: F(invariant) = invariant
		messageScorePortion = invariant * log(1 + 10 * persuasion/k)/log(1 + 10*invariant/k);
		break;
	}
		

	messageScorePortion = messageScorePortion * sensitivity;

	if(neg_flag)
	{
		messageScorePortion = -messageScorePortion;
	}

	return messageScorePortion;
}

double ChoiceModel::CombineScores(double score1, double score2)
{
	if(DoLogitCalc() )
		return score1 + score2;

	return score1 * score2;
}