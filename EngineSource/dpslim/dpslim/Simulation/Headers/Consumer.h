#pragma once
#define _CRT_SECURE_NO_WARNINGS
// Consumer.h
//
// Copyright 2005	DecisionPower
//
// Consumer Class
//
// Author;	Steven S. Noble
// Date:	11/8/2005

#include "stdafx.h"

// forward declare
// this is a temporary until we complete the change
class ChoiceModel;
class RepurchaseModel;
class MarketUtility;
class CMicroSegment;
class Pantry;
class DynamicAttribute;

typedef map<int, DynamicAttribute*>::const_iterator da_iter;

class	Consumer
{
public:
	// access
	// SSN microsegment refactoring SSN 11/2/2005
	// moved from Microsegment
	CMicroSegment*	segment;

	//Choice model still used to compute attribute utility
	ChoiceModel*	choiceModel;

	//Repurchase model still used by initial purchases
	RepurchaseModel* repurchaseModel;
	Pantry*	pantry;
	vector< PantryTask* > iTasks;

	//Dynamic Attribute Model
	vector< map<int, DynamicAttribute*> > dynamic_attributes;
	double DynamicScore(Pcn, int);

	long			id;						// who am i

	// the following should be bundled up into on structure
	int						numProducts;
	vector<int>				productAwareness;
	vector<double>			productPersuasion;
	vector<unsigned int>	productsBought;
	vector<int>				productTries;
	
	// not using SSN
	//int*			productsInStock;

	// A set of coupons held by the consumer
	set< int > *coupons;

	// Used to track consumer consideration set
	static vector< Pcn > products_in_channel;

	// Contains the products active in the current channel
	static vector< Pcn > consideration_set;

	// Contains products the consumer is going to buy
	static vector< Pcn > shopping_cart;

	int			iBoughtThisTick;					// various BOOLs to keep track of activities
	int			iRecommend;
	int			iProductLastBought;		// the number of the product I last bought (prodNum)
	int			iPreferredChannel;		// channel that consumer likes to buy from, **characterizes consumer
	int			iPreferredProduct;		// consumers preferred product (can be anywhere on product tree)
	double		iShoppingChance;
	int			iDaysSinceLastShopping;
	double		iInStoreSwitchFactor;


	int	iNBDscale;				// used in NBD models

	// determines when purchase occurs ? (SSN)
	double	iRepurchaseProb;		// **characterizes consumer

	// Queries
	int	EverTriedProduct(Index pn, int doSibs);
	int	Aware(Index pn);
	int	AlreadyHasCoupon(int);
	int	CountProductTrials(Index pn);
	

	void	ReadFromFile(ifstream & file);
	void	WriteToFile(ofstream & file);


	// actions
	void	MakeAware(Index pn, double persuasion);
	void	MakeConsumerAware(Index pn, int doSibs);
	// first does draw then calls the AddPersuasion
	// KMK 9/15/04 Persuade should not modify persuasionUnitsDelivered <-- Old Note: SSN
	void	Persuade(Index pn, double persuasion, int doSibs);	

	void	ProcessAdAwarenessAndPersuasion(double awareness, Index pn, int *impressionsDelivered, double persuasion);

	void	MakeConsumerTrySample(Index pcn, double sizeOfSample, double samplePersuasion);


	

	// copy and compare
	void	CopyTo(Consumer* destGuy);

	// needed public for reporting
	

protected:

	int				NumTimesTriedProduct(Index pn);
	double			CountAllMessagesForThisProduct(Index pn);
	void			CountMyAvailableProdMsgs();
	


	private:
	double	iTotalMessages;

	// queries

	void	LosePersuasion(Index, int);

	double	AwareProb(Index);
	double	PersuasionLoss(Index);
	

	double	FormLogProduct(int *numActiveProdsAvailable);

	// logic for selecting a channel
	// returns a preferred brand if there is one
		
	void	RejectProductNow(Index pn);

	int	ConsumerDidTrySibs(Index pn);
	int	ConsumerDidRepeatSibs(Index pn);

	int	RejectedAllAvailableProducts();

	// deterministic
	void	AddPersuasion(Index pn, int numPersuasionUnitsToAdd);

	

	//New Shopping
	public:
	void	Shop();
	int	ConsumerTriedAndRejectedProduct(Index);

	//Also used by initial purchases
	void	SelectChannel();
		void	MaybeSwitchChannels();;
		void	ChooseAChannel();
		int		ConsumerIsTryingProduct(Index,int);
		void	SeeIfRepurchaserDrops(Index, int);
	

	//Also used by coupon code
	int	HavePurchasedPrereqProduct(Index);

	

	//Used by microsegment when processing displays
	

	

	public:
	
	void	PerformTasks();

	void	GoShopping();
		void	DoShoppingPreprocessing();
			void	GetProductsInChannel();
				int	IsChannelOK(Pcn);
			void	ProcessDisplays();
				void	ProcessDisplays(Pcn , int&);
					void	DistributionDrivenAwareness(Pcn);
					
			void	ComputeProductUtilities();
				void ComputeProductScores(Pcn);
				void	ComputeProductUtility(Pcn);
			void	ConsiderBrandChange();
			void	ConsiderInStoreSwitching();
		void	BuildConsiderationSet();
			int	CheckConstraints(Pcn);
				
		Pcn	ChoosePreferredProduct();
			void	ScaleScoresLogit();
			double	GetLogitProbabilityFromUtility();
		void	UpdateCart(Pcn);
			void	UpdatePantryScores();
			void	ComputeScoreNoPantry();
		void	PurchaseProducts(int, int);
			int	CheckForSwitches(int);
			
				int UpdateConsumerProductPurchase(Index);
			

	//Initial Conditions
	public:
	void	InitializeRepurchaseCount(int seedWithRepurchasers);

	int	SeeIfConsumerWillHaveTriedProduct(int prodNum, int multiOK);	
	
	int	HasNegMsgs(Index pn);
	
	
	

	

	

	protected:
	
		
	
	
	// Specific purchasing and behavior models
	

	
	
	

	
	
	
	

	// shopping
	

	

	

	private:
	
	// consumer choice	
	
	
};

// These are  temporary macros
// as is well known, all macros are bad
//#define PRODUCT segment->iProducts
//#define PRODUCTCHANNEL segment->iProductsAvailable
//#define POPULATION segment->iPopulation
//#define TASKS segment->iTaskRates

// these are not temporary macros
// as is well known macros can be a very efficient way to encode data
#define	kClrAll_iFlag				0x00000000
#define	kSet_BoughtThisTick			0x00000001	// iBoughtThisTick
#define	kClr_BoughtThisTick			0xfffffffe
//#define	kSet_NeedsMetLastPurchase	0x0002	// for modeling churn in need-based buying
#define	kClr_NeedsMetLastPurchase	0xfffffffd
#define	kSet_Recommend				0x00000004	// determines whether or not purchases
#define kClr_Recommend				0xfffffffb	// result in a recommendation

#define	kSet_PassOnNegMsg			0x00000008	// For talk on purchase only; should schedule sending negative message
#define kClr_PassOnNegMsg			0xfffffff7
#define	kSet_MadeAwareThisTick		0x00000008	// for anytime talking only
#define kClr_MadeAwareThisTick		0xfffffff7

#define	kSet_AmBuying				0x00000010	// if iMaxBuyPercentage les than 100%, some consumers don't buy
#define kClr_AmBuying				0xffffffef
#define	kSet_HaveCoupon				0x00000020	// only one coupon per consumer right now
#define kClr_HaveCoupon				0xffffffdf
#define	kSet_ReadyToBuy				0x00000040	// to handle triggering purchase when counter is not yet zero
#define kClr_ReadyToBuy				0xffffffbf
#define	kSet_UsesCoupons			0x00000080	// has redeemed a coupon before
#define kClr_UsesCoupons			0xffffff7f
#define	kSet_GotSample				0x00000100	// has received a sample
#define kClr_GotSample				0xfffffeff
#define	kSet_WaitingForNextProduct	0x00000200	// reject all available products
#define kClr_WaitingForNextProduct	0xfffffdff
#define	kSet_ConsiderSiblingPurch	0x00000400	// consider repurchase among siblings
#define kClr_ConsiderSiblingPurch	0xfffffbff
#define	kSet_IgnoreBrandLoyalty		0x00000800	// when repeat purchasing selects change
#define kClr_IgnoreBrandLoyalty		0xfffff7ff
#define	kSet_ShoppingListItems		0x00001000	// when something is put on the consumer's shopping list in TBB
#define kClr_ShoppingListItems		0xffffefff
#define	kSet_LastProdBoughtUnavailable	0x00002000	// when prod last bought goes away
#define	Clr_LastProdBoughtUnavailable	0xffffdfff
#define	kSet_RanOut					0x00004000	// when consumer ran out of a needed product in TBB
#define kClr_RanOut					0xffffbfff
#define	kSet_BOGOSkipNextTrip		0x00008000	// consumer bought on BOGO,will skip next rrip
#define kClr_BOGOSkipNextTrip		0xffff7fff
#define	kSet_HaveBOGOCoupon			0x00010000	// consumer has a bogo coupon
#define kClr_HaveBOGOCoupon			0xfffeffff
	
#define	kAware		1
#define kUnaware	0
