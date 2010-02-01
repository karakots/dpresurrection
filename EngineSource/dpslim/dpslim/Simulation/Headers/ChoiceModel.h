#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ChoiceModel.h
//
// Copyright 2005	DecisionPower
//
// Choice Model for Microsegment


// encapsulating the choice math
class ChoiceModel
{
public:
	int iChoiceModel;
	int iGCMf1_PersuasionScore;
	int iGCMf2_PersuasionValComp;
	int iGCMf3_PersuasionContrib;
	int iGCMf4_UtilityScore;
	int iGCMf5_CombPartUtilities;
	int iGCMf6_PriceContribution;
	int iGCMf7_PriceScore;
	int iGCMf8_PriceValueSource;
	double iGCM_referencePrice;
	int iGCMf9_ChoiceProbability;
	int iGCMf10_InertiaModel;
	int iGCMf11_ErrorTerm;
	double iGCM_ErrorTermUserValue;
	double iGCM_ErrorTermSD;

public:
	int ClearSiblingPurchase();
	int MsgSensitivityCheck(double valToCheck);

	int ScaleScoresNonLogit();

	double GetErrVal(double scoreIn, double utility);

	int AddErrToScore();

	double MsgContribution(double persuasion, double sensitivity);

	double CombineScores(double score1, double score2);

	int DoLogitCalc();

public:

	// Constructors & destructors
	ChoiceModel();
	~ChoiceModel();

private:

	
	
	int AddScores();
	
	int MessSensExp();

};

