#pragma once
#define _CRT_SECURE_NO_WARNINGS

// MicorSegmentConst.H
//
// Copyright 2005	DecisionPower
//
// Enums and defines used by MicroSegment code

#define	kMaxFloat	1e38
#define	kMaxDouble	1e307

#define	kDoSibs		true
#define	kDontDoSibs	false
#define kUseVersion2Speedup	true

enum {
	kTransactionResolution_low = 1,
	kTransactionResolution_high_each_purchase,
	kTransactionResolution_low_with_awareness,
	kTransactionResolution_high_each_day,
	kTransactionResolution_old_low_with_awareness
};

enum {
	kFeaturePreference_strongLike = 1,
	kFeaturePreference_featureWeakness,
	kFeaturePreference_productEliminatedDisliked,
	kFeaturePreference_productEliminatedWeak,
	kFeaturePreference_dontCare,
	kAdvertising_TooFewImpressions,
	kAdvertising_TooManyImpressions,
	kProduct_OutOfStock
};

enum {
	kRepurchaseFreq_Days = 1,
	kRepurchaseFreq_Weeks,
	kRepurchaseFreq_Months,
	kRepurchaseFreq_Years
};

#define kdoAddDrops			true
#define kdontdoAddDrops		false

// the following go with iRepurchaseModel
enum {
	kRepurchaseModel_NBD,
	kRepurchaseModel_TaskBased_NBDrate
};

// task-based buying comments
#define	kNoTask						-1
#define	kPrevUsedProdsOnly			1
#define kPrevUsedAndUnusedProds		2
#define	kMaxAmtProdInStock			1000
#define	kMaxAmtProdInStockFloat		1000.0
#define	kNoProduct					-1
#define	kEmptyListItem				-2
#define	kUsedUpThisProduct			-3

// the following go with iChoiceModel
enum {
	kModel_Bass = 1,
	kModel_CrossingTheChasm,
	kModel_Emergent,
	kModel_EmergentLogit,
	kModel_Linear,
	kModel_LinearSOV,
	kModel_General	// OK
};
#define KConstant_e		2.718282

enum{
	kPurchaseState_WaitForFirstMessage = 0,
	kPurchaseState_ProductAwareness,
	kPurchaseState_WaitForRepurchase
};

#define	kCompressedPopThreshold		5000

#define	kMinFriendDifferenceLimit					5
#define	kFriendToRandomGuyRatio						25

// some scale factors (not model parameters)
#define	kADStrengthScaleFactor						1000.0
#define kMessageWeightScaleFactor					1000000.0
#define	kMaxScaledBuyDesire							1000.0
#define	kMaxLocalRandVal							10000
#define	kFloatMaxLocalRandVal						10000.0
#define	kMaxInitialBuyPctFactor						10000.0

enum {
	kAdvertScope_AllProdsAllChannels = 1,
	kAdvertScope_AllProdsOneChannel,
	kAdvertScope_OneProdAllChannels,
	kAdvertScope_OneProdOneChannel
};

#define	kNumDisplayTypes	5

const short kNotGoodEnough = -1;
const long	kNoLimit = -1;

enum {
	kPriceTypeUnpromoted = 0,
	kPriceTypePromoted,
	kPriceTypeBOGO,
	kPriceTypeReduced,
	kPriceTypeDisplayPercent,
	kPriceTypeDisplayAbsolute,
	kNumPriceTypes
};

//
// individual for multi-agent comsumer model
//
#define	kMaxMessageImpact	10.0
#define	kNoProductBought	1024		// for use with iProductLastBought

enum {
	kPopSpending_notBuying = 0,
	kPopSpending_CantAffordAnything,
	kPopSpending_CantAffordWhatIWant,
	kPopSpending_HaveEnoughMoney,
	kPopSpending_CouldBuyMore,
	kPopSpending_HaveLotsOfMoney
};

#define kNoChannel		255
#define kRejectedBit	0x80
#define kTriedSib		0x40
#define kProdTriesMask	0x3
#define kMaxRepurchaseDays	(kMaxShort/2)



enum {
	kGrowthRatePeople	= 1,
	kGrowthRatePercent
};

enum {
	kGrowthRateMonth = 1,
	kGrowthRateYear
};


#define kMaxNumFriends		100		// this one is OK to keep
const unsigned char	kNoFeature = 0xff;



// choiceModel->iGCMf1_PersuasionScore
enum {
	kGCMf1_Multiplication = 1,
	kGCMf1_Exponentiation
};
// choiceModel->iGCMf2_PersuasionValComp
enum {
	kGCMf2_ShareOfVoice	= 1,
	kGCMf2_Absolute,
	kGCMf2_squareRoot,
	kGCMf2_base10log
};
// choiceModel->iGCMf3_PersuasionContrib
enum {
	kGCMf3_Addition	= 1,
	kGCMf3_Multiplication
};
// iGCMf4_UtilityScore
enum {
	kGCMf4_Multiplication = 1,
	kGCMf4_Exponentiation
};
// iGCMf5_CombPartUtilities
enum {
	kGCMf5_UnscaledSumOfProducts = 1,
	kGCMf5_ScaledSumOfProducts,
	kGCMf5_Log10ScaledSumOfProducts
};
// iGCMf6_PriceContribution
enum {
	kGCMf6_Addition	= 1,
	kGCMf6_Subtraction,
	kGCMf6_Multiplication,
	kGCMf6_Division
};
// iGCMf7_PriceScore
enum {
	kGCMf7_Multiplication	= 1,
	kGCMf7_Exponentiation
};
// iGCMf8_PriceValueSource
enum {
	kGCMf8_AbsolutePrice	= 1,
	kGCMf8_PricePerUse,
	kGCMf8_RelativePrice,
	kGCMf8_ReferencePrice
};
// choiceModel->iGCMf9_ChoiceProbability
enum {
	kGCMf9_ShareOfScore	= 1,
	kGCMf9_Logit,
	kGCMf9_ScaledLogit
};
// iGCMf10_InertiaModel
enum {
	kGCMf10_SameSKU	= 1,
	kGCMf10_SameBrand
};
// choiceModel->iGCMf11_ErrorTerm
enum {
	kGCMf11_None	= 1,
	kGCMf11_UserValue,
	kGCMf11_Utility,
	kGCMf11_Score
};


// 	iAwarenessModelType OK
enum {
	kAwarenessOnly	= 1,
	kAwarenessAndPersuasion
};

// iSocNetModelType
enum {
	kSocNetTalkOnPurchase = 1,
	kSocNetTalkAnytime
};

enum {
	kAdvertType_None = 0,
	kAdvertType_MassMedia,
	kAdvertType_Coupon,
	kAdvertType_Sample,
	kAdvertType_Event_units_purchased,
	kAdvertType_Event_shopping_trips,
	kAdvertType_Event_price_disutility,
	kAdvertType_Distribution,
	kAdvertType_Display,	// VdM 10/31/2003
	kAdvertType_Display1,	// KMK 6/28/05
	kAdvertType_Display2, 
	kAdvertType_Display3, 
	kAdvertType_Display4, 
	kAdvertType_Display5, 
	kAdvertType_BOGO,		// VdM 7/29/04

};

// warning messages
#define	kMaxSessionLogWarnings	250
enum {
		kWarn_utilityOverflow = 1,
		kWarn_priceProbabilityError,
		kWarn_utilityRange,
		kWarn_BOGONoUnpromotedPrice,
		kWarn_BOGOCouponNoUnpromotedPrice,
		kWarn_SampleDistribution,
		kWarn_CouponDistribution,
		kWarn_AdImpressionDistribution,
		kWarn_ForgetMessages,
		kWarn_RandomNmber,
		kWarn_TooManyPriceDisplays,
		kWarn_DisplayIncorrectlySpecified,
		kWarn_ZeroUnitsPerPurchase,
		kWarn_TripModificationError,
		kWarn_TooManyWarnings,
		kWarn_CheckPointInvalid
};

#define	kDoForceChange		true
#define kDoNotForceChange	false


