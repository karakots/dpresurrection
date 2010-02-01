#pragma once
#define _CRT_SECURE_NO_WARNINGS
// CMicroSegment.h
//
// Copyright 1998	Salamander Interactive
//
// Multi-agent customer segment population
// -- the secret sauce!
// Created by Ken Karakotsios 5/21/98


//#ifdef Windows_Platform
//#include <windows.h>
//#endif  // Windows_Platform

#ifndef WINVER
#define WINVER 0x0501
#endif

#include <hash_map>
#include <vector>
#include <limits>

using namespace std;
using namespace stdext;


//#include "ChoiceModel.h"
//#include "EventControl.h"
//#include "Function.h"
//#include "Product.h"
//#include "PopObject.h"
//#include "Attributes.h"
//#include "Channel.h"
//#include "Task.h"
//#include "TimeSeriesData.h"

#include "randlib.h"
#include "stocc.h"
#include "DBSegment.h"
#include "DateMath.h"
#include "microsegmentconst.h"


class Display;
class SocialNetwork;
class RepurchaseModel;
class Bucket;
class ModelRecordset;
class ProductTree;
class SegDataRecordset;
class SegmentDataAcc;
class ChoiceModel;
class Function;
class Product;
class Feature;
class PopObject;
class PantryTask;
class Channel;
class Consumer;
class CMicroSegment;
class ProductChannelRecord;
class Media;
class Coupon;
class Display;
class MarketUtility;
class Price;
class Distribution;
class ExternalFactor;
class Attribute;
class ProductAttribute;
class ConsumerPref;
class DynamicAttribute;
class InitialCondition;
class MSDatabase;

// ------------------------------------------------------------------------------
// Type defines for Consumer Segment
// ------------------------------------------------------------------------------
typedef size_t Index;
typedef int ID;

class pcn_key : public pair<Index, Index>
{
public:
	static int NumProducts;
	Index pn;
	Index cn;
	pcn_key() : pair< Index, Index > (0, 0)
	{
		pn = first;
		cn = second;
	}
	pcn_key(Index p, Index c) : pair<Index, Index> (p, c)
	{
		pn = first;
		cn = second;
	}

	pcn_key(Index pcn)
	{
		cn = pcn/NumProducts;
		pn = pcn - cn*NumProducts;
	}

	inline bool operator == (const pcn_key & a_key)
	{
		if(a_key.pn == pn && a_key.cn == cn)
		{
			return true;
		}

		return false;
	}

	inline operator Index()
	{
		return pn + cn*NumProducts;
	}
};

typedef pcn_key Pcn;

//Product Containers
typedef vector<Product*> Pn_container;
typedef vector<Channel*> Cn_container;
typedef vector<CMicroSegment*> Seg_container;
typedef vector<ProductChannelRecord*> Pcn_container;
typedef map<ID, Index> ID_Index_map;

//Attribute Containers
typedef map<ID, map<ID, ProductAttribute*>*> Pan_container;
typedef map<ID, ConsumerPref*> Can_container;
typedef map<ID, Attribute*> An_container;

//Marketing Containers
typedef map<long, MarketUtility*> MU_container;
typedef map<long, Media*> MM_container;
typedef map<long, Coupon*> C_container;
typedef map<long, ExternalFactor*> EF_container;

//Iterators
typedef ID_Index_map::const_iterator id_map_iter;
typedef Pcn_container::const_iterator Pcn_iter;
typedef Pan_container::const_iterator Pan_iter;
typedef Can_container::const_iterator Can_iter;
typedef An_container::const_iterator An_iter;
typedef Pn_container::const_iterator Pn_iter;
typedef Cn_container::const_iterator Cn_iter;
typedef Seg_container::const_iterator Seg_iter;
typedef	MU_container::const_iterator MU_iter;
typedef MM_container::const_iterator MM_iter;
typedef C_container::const_iterator C_iter;
typedef EF_container::const_iterator EF_iter;


class	ProductChannelRecord 
{
public:
	CString				iProdName;
	CString				iChanName;
	int					iProductID;
	int					iChannelID;
	int					iProductIndex;
	int					iChannelIndex;
	double				iNumTrips;
	double				iPrice[kNumPriceTypes];
	double				iProbPrice[kNumPriceTypes];
	double				iPreProdPriceScores[kNumPriceTypes];
	double				iPostProdPriceScores[kNumPriceTypes];
	int					iUsingPriceType;
	int					iActive;
	int					iAmountPurchasedThisTick[kNumPriceTypes];
	int					iTotalPurchaseOccasionsThisTick;
	int					iNumCouponPurchases;
	int					iNumCouponsToBeRedeemed;
	double				msgWeightedScores;
	double				persuasionScore;
	double				priceScore;
	double				pantryScore;
	double				attributeScore;
	double				errorScore;
	double				overallScore;	
	int					iAmConsideredForThisConsumer;
	double				iPreUseDistributionPercent;
	double				iPostUseDistributionPercent;
	double				iDistributionAwareness;
	double				iDistributionPersuasion;
	MU_container		iMarketUtility;
	int					iHaveNotUsedProductBefore;
	int					iNotProcessed;
	double				iSize;

};

class CMicroSegment 
{
public:

//*****************************************************************************
//PUBLIC FIELDS
//*****************************************************************************

	static CTime Current_Time;
	static int iDay;
	static int iMonth;
	static int iYear;
	static ModelRecordset* iModelInfo;
	static StochasticLib1 iRandomLib;
	
	double		iMessageSensitivity;
	double		iChannelLoyalty;
	double		iBrandLoyalty;
	
	double		iAwarenessDecayRatePreUse;
	double		iAwarenessDecayRatePostUse;
	double		iPersuasionDecayRatePreUse;
	double		iPersuasionDecayRatePostUse;
	double		iTotalMessages;
	double		iCatQuantityPurchasedModifier;
	double		iCatShoppingTripFrequencyModifier;
	double		iCatTaskRateModifier;
	double		iDisplayUtility;
	int			iSegmentID;
	int			iNumDays;
	int			iMaxDisplayHits;
	int			iHavePrereqs; //BOOL
	int			iShareAwarenessWithSibs; //BOOL
	int			iRejectSibs; //BOOL
	
	CString		iNameString;

	ProductTree*					iProdTree;
	PopObject*						iPopulation;
	vector< Function* >				iFunctions;
	vector< int >					iBinFunctions;
	vector< PantryTask* >			iTasks;

	Cn_container			iChannels;
	Pn_container			iProducts;
	Seg_container			iSegmentPointers;
	Pcn_container			iProductsAvailable;
	


	
//*****************************************************************************
//PUBLIC METHODS
//*****************************************************************************

	//Default Consturctor
	CMicroSegment();
	~CMicroSegment();

	//Database Interface
	void	ReadSegmentAndModel(	SegmentRecordset* segment, 
									ModelRecordset* model, 
									bool);
	void	LoadDataFromDatabase(	map<int, Product*> & products, 
									map<int, Channel*> & channels, 
									map<int, double> & channel_choice,
									vector<pair<int,int>> & tree,
									map<int, Attribute*> & attributes,
									map<pair<int, int>, double> & product_sizes,
									map<pair<int, int>, double> & prerequistes,
									map<int, InitialCondition*> & initial_conditions);
	void	DBOutput(MSDatabase*);

	//Set-Up Functions
	void	Initialize();
	void	Reset();
	void	WriteAttributes(ofstream&);
	void	ReadAttributes(ifstream&);

	//Simulation
	void	SimStep(void);

	//Utility Functions
	Index	ProdIndex(ID);
	Index	ChanIndex(ID);
	Pcn		GetPcn(Pcn_iter);

	//Shopping Helpers
	double	SetPrice(Pcn);
	int		IsProductInStock(Pcn);
	double ScaledOddsOfDisplay(Pcn, MarketUtility*);

	//Dynamic Attribute
	double	AttributeScore(ID, DynamicAttribute*, int);
	void	AddDynamicAttribute(ProductAttribute*);

	//Time Controller Interface	
	void	AddMassMedia(Media*);
	void	RemoveMassMedia(Media*);
	void	AddCouponDrop(Coupon*);
	void	RemoveCouponDrop(Coupon*);
	void	AddMarketUtility(MarketUtility*);
	void	RemoveMarketUtility(MarketUtility*);
	void	AddExternalFactor(ExternalFactor*);
	void	RemoveExternalFactor(ExternalFactor*);
	void	UpdateDistribution(Distribution*);
	void	UpdatePrice(Price*);
	void	UpdateProductAttribute(ProductAttribute*);
	void	UpdateConsumerPreference(ConsumerPref*);

	ChoiceModel* GetChoiceModel() { return choiceModel; };
	PopObject* Population() { return iPopulation; };
	RepurchaseModel* GetRepurchaseModel() { return repurchaseModel; };

	// elevated to public status for use in SegmentWriter	
	int			iUseACVDistribution; //BOOL

private:
//*****************************************************************************
//PRIVATE FIELDS
//*****************************************************************************

	static Bucket	*myBucket;
	static int		iNumSegments;

	double		iPriceSensitivity;
	double		iDiversityPercent;

	int			iNeedToCalcAttrScores; //BOOL
	int			iNeedToCalcPriceScores; //BOOL

	map< pair<int, int>, SegmentDataAcc* >		iSegmentMap;
	vector<SocialNetwork*>*		socialNetworks;
	vector<string>				iSegmentNames;
	ChoiceModel*				choiceModel;
	RepurchaseModel*			repurchaseModel;
	Pan_container				iProductAttributes;
	Can_container				iAttributePreferences;
	An_container				iAttributes;
	MM_container				iMassMedia;
	C_container					iCoupons;
	EF_container				iExternalFactor;
	ID_Index_map				product_id_map;
	ID_Index_map				channel_id_map;


//*****************************************************************************
//PRIVATE METHODS
//*****************************************************************************
	
	//Database Reader Helper Functions
	void	read_product_and_channels(map<int, Product*> &, map<int, Channel*> &, map<int, double> &);
	void	read_product_tree(vector<pair<int,int>> &);
	void	read_product_dependencies(map<pair<int, int>, double> &);
	void	read_initial_share_and_penetration(map<int, InitialCondition*> &);
	void	read_product_size(map<pair<int, int>, double> &);
	void	read_product_attributes(map<int, Attribute*> &);
	
	
	//Sim Step Functions
	void	clean_up(void);
	void	consumer_shopping(void);
	void	process_social_networks(void);
	void	distribute_media();
	void	external_factors();

	//Set-up and reset consumer population
	void	create_consumer_population();
	void	reset_population_and_context();
	void	reset_new_population();
	void	reset_products();
	void    reset_attributes();

	//Set-up products and channels
	void	initialize_product_channel_record(ProductChannelRecord* aPCR);
	void	initialize_product(Product *aProduct);
	void	add_product_channel(Index, Index);
	void	build_product_channel_list();
	void	compute_cumulative_channel_percents(void);


	//Initial conditions processing
	void	initial_condition_processing(void);
	void	assign_initial_channelsto_consumers(void);
	void	set_initial_persuasion();
	void	set_initial_awareness();
	void	set_initial_penetration();
	void	set_initial_share();

	//Price and attribute processing
	void	calc_product_scores();
	void	calc_price_scores(void);

	//Media processing
	void	process_mass_media();
	void	process_coupons();
	void	broadcast_ad(int numImpressionsThisTick, Index pn, double persuasion, double awareness);
	//[TBD] Fix samples
	void	distribute_samples(int numRedemptionsThisTick, Pcn pcn, double sizeOfSample,
			double persuasion);
	//Coupon Code -> Needs to be fixed
	void	redeem_coupons(int numRedemptionsThisTick, Coupon* coupon);
	void	make_coupon_redeemer(Consumer* aGuy, Coupon* coupon, double couponPersuasion, int isBOGO, Pcn pcn);
	void	rejected_by_channel_or_durable(int *rejectedProduct, Consumer* aGuy, Coupon* coupon, Pcn pcn, int checkChannel);
	int		is_rejected_by_channel_or_durable(Coupon* coupon, Consumer* aGuy, Pcn pcn, int checkChannel);

	//Database helpers
	void	write_purchase_info_to_file(void);
	void	accumlate_data(Pcn pcn);
	void	check_for_active_products(void);
};




class EnvCheck
{
private:
	int varSet;
public:
	EnvCheck(const char* var)
	{
		varSet = false;
		char* val = getenv(var);
		if (val && strcmp(val, "true") == 0)
		{
			varSet = true;
		}
	}
	operator int() { return varSet; }
};



