// CMicroSegment.cpp
//
// Copyright 1998	Salamander Interactive
//
// Multi-agent customer segment population
// -- the secret sauce!
// Created by Ken Karakotsios 5/21/98

#include "CMicroSegment.h"

#include <stdlib.h>
#include <fstream>
#include <math.h>

#include "SocialNetwork.h"
#include "Consumer.h"
#include "ChoiceModel.h"
#include "DBModel.h"
#include "RepurchaseModel.h"
#include "ProductTree.h"
#include "DBProdAttr.h"
#include "PopObject.h"
#include "Product.h"
#include "Attributes.h"
#include "Function.h"
#include "Pantry.h"
#include "Task.h"
#include "Channel.h"
#include "Display.h"
#include "Media.h"
#include "Coupon.h"
#include "MarketUtility.h"
#include "ExternalFactor.h"


//	initialization of static class members
int	CMicroSegment::iNumSegments = 0;
int 	CMicroSegment::iDay = 0;
int 	CMicroSegment::iMonth = 0;
int 	CMicroSegment::iYear = 0;
CTime	CMicroSegment::Current_Time = CTime();
Bucket*	CMicroSegment::myBucket = new Bucket();
int		pcn_key::NumProducts = 0;
StochasticLib1 CMicroSegment::iRandomLib = StochasticLib1(0);


// static reference to model information stored in database
ModelRecordset* CMicroSegment::iModelInfo = 0;

// ------------------------------------------------------------------------------
// constructor
// ------------------------------------------------------------------------------
CMicroSegment::CMicroSegment()
{
	RandomInitialise(1802,9373);
	iNumSegments++;
	
	iProdTree = 0;
	iSegmentMap = map< pair<int,int>, SegmentDataAcc* >();
	choiceModel = new ChoiceModel();
	repurchaseModel = new RepurchaseModel();
	iPopulation = new PopObject();
	myBucket = new Bucket();
	iPopulation->iGroupSize = 10000;
	iTotalMessages = 0.0;
	iPopulation->iPopScaleFactor = 1.0;
	iShareAwarenessWithSibs = false;
	iRejectSibs = false;
	iUseACVDistribution = true;
	iNumDays = 0;
	iPriceSensitivity = 5.0;
	iMessageSensitivity = 5.0;
	iBrandLoyalty = 5.0;
	iChannelLoyalty = 5.0;
	iDiversityPercent = 20.0;
	choiceModel->iChoiceModel = kModel_General;	//OK
	iCatQuantityPurchasedModifier = 0.0;
	iCatShoppingTripFrequencyModifier = 0.0;
	iCatTaskRateModifier = 0.0;
	iMaxDisplayHits = 0;
	iAwarenessDecayRatePreUse = 1;
	iAwarenessDecayRatePostUse = 0;
	iPersuasionDecayRatePreUse = 1;
	iPersuasionDecayRatePostUse = 1;
	iDisplayUtility = 0.0;

	// new general choice model
	choiceModel->iChoiceModel = kModel_General;
	choiceModel->iGCMf1_PersuasionScore = kGCMf1_Multiplication;
	choiceModel->iGCMf2_PersuasionValComp = kGCMf2_Absolute;
	choiceModel->iGCMf3_PersuasionContrib = kGCMf3_Addition;
	choiceModel->iGCMf4_UtilityScore = kGCMf4_Multiplication;
	choiceModel->iGCMf5_CombPartUtilities = kGCMf5_ScaledSumOfProducts;
	choiceModel->iGCMf6_PriceContribution = kGCMf6_Addition;
	choiceModel->iGCMf7_PriceScore = kGCMf7_Multiplication;
	choiceModel->iGCMf8_PriceValueSource = kGCMf8_PricePerUse;
	choiceModel->iGCM_referencePrice = 0.0;
	choiceModel->iGCMf9_ChoiceProbability = kGCMf9_ScaledLogit;
	choiceModel->iGCMf10_InertiaModel = kGCMf10_SameBrand;
	choiceModel->iGCMf11_ErrorTerm = kGCMf11_None;
	choiceModel->iGCM_ErrorTermUserValue = 0.0;
	
	
	
	
	iHavePrereqs = false;

	socialNetworks = 0;

	iFunctions = vector< Function* >();
	iBinFunctions = vector< int >();
	product_id_map = ID_Index_map();
	channel_id_map = ID_Index_map();
	iChannels = Cn_container();
	iProducts = Pn_container();
	iSegmentPointers = Seg_container();
	iProductsAvailable = Pcn_container();
	iProductAttributes = Pan_container();
	iAttributePreferences = Can_container();
	iAttributes = An_container();


	iFunctions.push_back(new StepFunction(10,20,-1000000));
	iBinFunctions.push_back(0);
}

Index CMicroSegment::ProdIndex(ID id)
{
	return product_id_map[id];
}

Index CMicroSegment::ChanIndex(ID id)
{
	return channel_id_map[id];
}

Pcn CMicroSegment::GetPcn(Pcn_iter iter)
{
	return Pcn((*iter)->iProductIndex, (*iter)->iChannelIndex);
}

void CMicroSegment::Initialize()
{
	reset_products();
	reset_attributes();
	create_consumer_population();
}


void CMicroSegment::Reset()
{
	reset_products();
	reset_attributes();
	reset_population_and_context();
	clean_up();
	initial_condition_processing();
}



// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	CMicroSegment::create_consumer_population()
{	

	iPopulation->CreatePopulaionCore(false, choiceModel->iChoiceModel, this, 
		(int)iProductsAvailable.size(), (int)iProducts.size(), (int)iAttributePreferences.size(), 0.0, 0,
		repurchaseModel->type);	// OK

	reset_population_and_context();  // 4/30/02

}

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::reset_population_and_context()
{
	iNeedToCalcAttrScores = true;
	iCatQuantityPurchasedModifier = 0.0;
	iCatShoppingTripFrequencyModifier = 0.0;
	iCatTaskRateModifier = 0.0;
	
	iTotalMessages = 0.0;
	
	reset_new_population();
	
	if(this->iModelInfo->using_checkpoint)
	{
		// Convert a TCHAR string to a LPCSTR
		CT2CA pszConvertedAnsiString(this->iModelInfo->checkpoint_file);

		// construct a std::string using the LPCSTR input
		std::string strStd (pszConvertedAnsiString);

		string file = "checkpoints\\";
		file += strStd;
		file += "\\popdata";
	
		CT2CA pszConvertedAnsiString2(iNameString);
		strStd = std::string(pszConvertedAnsiString2);

		file += strStd;
		if(!this->iPopulation->ReadFromFile(file))
		{
			//static string errorString("Check Point Data Invalid");
			//iCtr->DBSetString(0,0,0, &errorString, kCntr_SimStatus);
			//WriteToSessionLog(kWarn_CheckPointInvalid, -1, -1, 0);
		}
	}
}

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	CMicroSegment::reset_new_population()
{
	Consumer*	aConsumer;

	// set some basic state

	iPopulation->ResetPersuasion();	// OK
	iPopulation->ResetAwareness();
	iPopulation->ResetProductTries();
	iPopulation->ResetProdsEverBought();


	double avg_size = 0.0;
	double num_pcn = 0.0;
	Index pn;
	Pcn pcn;
	ProductTree::List* leafs = ProductTree::theTree->LeafNodes();
	for(ProductTree::Iter iter = leafs->begin(); iter != leafs->end(); ++iter)
	{
		pn = *iter;
		for(Cn_iter iter = iChannels.begin(); iter < iChannels.end(); ++iter)
		{
			pcn = Pcn(pn, (*iter)->iChannelIndex);
			avg_size += iProductsAvailable[pcn]->iSize;
			num_pcn++;
		}
	}

	avg_size = avg_size / num_pcn;


	for (int i=0; i < iPopulation->iConsumerList.size(); i++)
	{
		aConsumer = iPopulation->iConsumerList[i];
		aConsumer->iBoughtThisTick = false;
		aConsumer->iRecommend = false;
		aConsumer->iProductLastBought = kNoProductBought;
		aConsumer->iPreferredChannel = kNoChannel;

		aConsumer->iRepurchaseProb = repurchaseModel->Ticks(iDiversityPercent);

		//Pantry Model Parameters
		if(aConsumer->iTasks[0] != 0)
		{
			delete aConsumer->iTasks[0];
		}
		PantryTask* ptask = new PantryTask(1);
		double rate = aConsumer->iRepurchaseProb*repurchaseModel->iAverageMaxUnits*avg_size;
		ptask->SetRate(0, rate);
		aConsumer->iTasks[0] = ptask;
		//double consume = -log(RandomUniform())*(1/aConsumer->iShoppingChance)*rate;
		double days = 0;
		while(!WillDo0to1(aConsumer->iRepurchaseProb))
		{
			days++;
		}
		aConsumer->iDaysSinceLastShopping = days;
		aConsumer->pantry->SetBin(0,20-RandomUniform()*avg_size);
		aConsumer->pantry->SetActivationEnergy(0.0);
		aConsumer->iShoppingChance = aConsumer->iRepurchaseProb;
		aConsumer->iNBDscale = repurchaseModel->NBDScale();
	}
}



// ------------------------------------------------------------------------------
// destructor
// ------------------------------------------------------------------------------
CMicroSegment::~CMicroSegment()
{
	iPopulation->DestroyConsumerPopulation();
	delete iPopulation;
		
	
	iNumSegments--;

	for (Pn_iter citer = iProducts.begin(); citer != iProducts.end(); citer++)
	{
		(*citer)->iPrerequisiteNum.clear();
		(*citer)->iInCompatibleNum.clear();
	}

	for(Pcn_iter iter = iProductsAvailable.begin(); iter != iProductsAvailable.end(); ++iter)
	{
		(*iter)->iMarketUtility.clear();
	}

	for(Pan_container::iterator iter = iProductAttributes.begin(); iter != iProductAttributes.end(); ++iter)
	{
		iter->second->clear();
		delete iter->second;
	}

	iProducts.clear();
	iProductsAvailable.clear();
	iChannels.clear();
	iFunctions.clear();
	iBinFunctions.clear();
	iTasks.clear();
	iSegmentNames.clear();
	iProductAttributes.clear();
	iAttributePreferences.clear();
	iAttributes.clear();
	iMassMedia.clear();
	iCoupons.clear();
	iExternalFactor.clear();
	product_id_map.clear();
	channel_id_map.clear();
	iSegmentNames.clear();
	iSegmentPointers.clear();

	if (socialNetworks != 0)
	{
		vector<SocialNetwork*>::iterator ptr;

		for (ptr = socialNetworks->begin(); ptr != socialNetworks->end(); ++ptr)
		{
			SocialNetwork* network = *ptr;

			delete network;
		}

		delete socialNetworks;
	}
}



// ------------------------------------------------------------------------------
// Pick some consumers to give samples to
// ------------------------------------------------------------------------------
void CMicroSegment::distribute_samples(int numSamplesThisTick, Pcn pcn, double sizeOfSample, 
			double persuasion)
{
	
}



void CMicroSegment::process_coupons()
{
	for(C_iter iter = iCoupons.begin(); iter != iCoupons.end(); ++iter)
	{
		Coupon* coupon = iter->second;

		//Compute number of redemptions
		double redemption = coupon->GetRedemption();
		double reach = coupon->GetReach();
		double impressions = redemption * reach * iPopulation->iConsumerList.size();

		//Redemptions ar front loaded so a scaling factor is used
		CTimeSpan current = coupon->GetEndDate() - Current_Time;
		double scale_factor = 2.0 * ((double)current.GetDays() / (double) coupon->GetSpan());
		double num_impressions = (impressions * scale_factor)/((double) coupon->GetSpan());

		double remainder = num_impressions - (int)num_impressions;
		if(WillDo0to1(remainder))
		{
			++num_impressions;
		}

		redeem_coupons(num_impressions, coupon);
	}
}


// ------------------------------------------------------------------------------
// Coupons are currently a hack and need to fixed...this code is gross beware...
// ------------------------------------------------------------------------------
void CMicroSegment::redeem_coupons(int numRedemptionsThisTick, Coupon* coupon)
{
	int		numRedemptions = 0;
	int		aRandomPerson;
	Consumer*	aGuy;
	int		bound;
	int		rejectedProduct;
	int		isBOGO = false;

	//iProductsAvailable[prodChanNum].iNumCouponsToBeRedeemed += numRedemptionsThisTick;

	bound = (int)iPopulation->iConsumerList.size();
	//isBOGO = (iAdSchedule[couponIndex].iAdvertType == kAdvertType_BOGO);

	// If product is eliminated by this segment, then throw the coupons away
	int	oneProd = 0;

	Pcn pcn = Pcn(ProdIndex(coupon->GetProduct()), ChanIndex(coupon->GetChannel()));

	myBucket->Reset(bound);

	//maxTries = 8 * bound; doesn't make a significant difference in APDO scenario
	while (!myBucket->Empty() && (numRedemptions < numRedemptionsThisTick))
	{
		// 	Pick a consumer at random
		aRandomPerson = myBucket->Draw();
		aGuy = iPopulation->iConsumerList[aRandomPerson];

		//if(!aGuy->AlreadyHasCoupon(couponIndex))
		{
			rejectedProduct = true;
			rejectedProduct = aGuy->ConsumerTriedAndRejectedProduct(pcn.pn);
			rejected_by_channel_or_durable(&rejectedProduct, aGuy, coupon, pcn, true);

			if (!rejectedProduct)
			{
				
				make_coupon_redeemer(aGuy, coupon, coupon->GetPersuasion(), isBOGO, pcn);
				numRedemptions++;	// we want to have neg people not redeem coupons
			}
		}
	}
}


// ------------------------------------------------------------------------------
// KMK 1/10/05 make sure that any durable/refill constraints are met
// NULL for aGuy will skip channel check
// ------------------------------------------------------------------------------
void	CMicroSegment::rejected_by_channel_or_durable(int *rejectedProduct, Consumer* aGuy, Coupon* coupon, Pcn pcn, int checkChannel)
{
	if (!(*rejectedProduct))
	{
		return;
	}

	if(iProdTree->IsLeaf(pcn.pn))
	{
		*rejectedProduct = is_rejected_by_channel_or_durable(coupon, aGuy, pcn, checkChannel);
	}
	else
	{
		int rejected = false;
		ProductTree::LeafNodeList leafs(pcn.pn);
		ProductTree::Iter iter;
		for(iter = leafs.begin(); iter != leafs.end(); ++iter)
		{
			Pcn lpcn = Pcn(*iter,pcn.cn);
			rejected = is_rejected_by_channel_or_durable(coupon, aGuy, lpcn, checkChannel);
			if(!rejected)
			{
				*rejectedProduct = rejected;
				return;
			}
		}
		*rejectedProduct = rejected;
		return;
	}
		
}

int CMicroSegment::is_rejected_by_channel_or_durable(Coupon* coupon, Consumer* aGuy, Pcn pcn, int checkChannel)
{
	int	rejected;
	int	havePurchasedPrereq;
	if (iHavePrereqs && iProducts[pcn.pn]->iHavePrepreqs)
	{
		havePurchasedPrereq = aGuy->HavePurchasedPrereqProduct(pcn.pn);
	}
	else
	{
		havePurchasedPrereq = true;
	}

	rejected = !havePurchasedPrereq;

	if (!rejected && checkChannel != 0)
	{
		//if the consumer has a preferred channel, and the coupon doesn't match then reject it...
		if(aGuy->iPreferredChannel != kNoChannel && coupon->GetChannel() != -1 && pcn.cn != aGuy->iPreferredChannel)
		{
			return true;
		}
	}

	return rejected;
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::make_coupon_redeemer(Consumer* aGuy, Coupon* coupon, double couponPersuasion, int isBOGO, Pcn pcn)
{
	if(coupon->GetChannel() == -1)
	{
		aGuy->ChooseAChannel();
		pcn.cn = aGuy->iPreferredChannel;
	}

	iProductsAvailable[pcn]->iNumCouponsToBeRedeemed += 1;

	//Perform purchase using 2.3 method to determine num_units bought
	//This will ignore external factors for the time being
	double avg_units = repurchaseModel->iAverageMaxUnits;
	int num_units = (int)avg_units;
	if(RandomUniform() < (avg_units - (double)num_units))
	{
		num_units++;
	}
	//BOGO QAD
	if (isBOGO)
	{
		num_units *= 2;
	}

	aGuy->shopping_cart.clear();
	for(int i = 0; i <  num_units; i++)
	{
		aGuy->shopping_cart.push_back(pcn);
		aGuy->pantry->AddProduct(pcn);
		iProductsAvailable[pcn]->iNumCouponPurchases += 1;
	}
	iProductsAvailable[pcn]->iNotProcessed = true;
	iProductsAvailable[pcn]->iUsingPriceType = kPriceTypeUnpromoted;
	aGuy->PurchaseProducts(false, false);
	aGuy->shopping_cart.clear();
	

	// fill the consumer with awareness (and possibly persuasion) for the product
	aGuy->MakeAware(pcn.pn, couponPersuasion);	// OK
}

void CMicroSegment::distribute_media()
{
	process_mass_media();
	process_coupons();
}


void CMicroSegment::process_mass_media()
{
	for(MM_iter iter = iMassMedia.begin(); iter != iMassMedia.end(); ++iter)
	{
		Media* mass_media = iter->second;

		//Conver GRPs to number of impressions
		//GRPs is given in a penetraion percentage (eg 50% means half of population will receive impression on average)
		double GRPs = mass_media->GRPs(Current_Time);
		double impressions = (GRPs / 100.0) * iPopulation->iConsumerList.size();
		int num_impressions = (int)impressions;
		double remainder = impressions - num_impressions;
		if(WillDo0to1(remainder))
		{
			++num_impressions;
		}

		broadcast_ad(num_impressions, ProdIndex(mass_media->GetProduct()), mass_media->GetPersuasion(), mass_media->GetAwareness());
	}
}



// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::broadcast_ad(int numImpressionsThisTick, Index pn, double persuasion, double awareness)
{
	long	aRandomPerson;
	int		bound;
	int		impressionsDelivered = 0;

	// because we only hold integers for the message count, we can't directly handle
	// fractional values.  So what we do is randomly Add in a message if the strength
	// is less than 1.  We'll do this in proportion to the strength.  SO a strength
	// of .25 means that 1 out of 4 times, we'll actually Add the message in.
	//integerPart = (int) messageStrength;
	//fractPart =  messageStrength - ((double)integerPart);
	//shortMessageStrength = fractPart * kADStrengthScaleFactor;
	bound = (int)iPopulation->iConsumerList.size();

	while (impressionsDelivered < numImpressionsThisTick)
	{
		// pick people at random
		aRandomPerson = LocalRand(bound);

		Consumer* consumer = iPopulation->iConsumerList[aRandomPerson];

		consumer->ProcessAdAwarenessAndPersuasion(awareness, pn, &impressionsDelivered, persuasion);

	}
	
	// log impressions delivered	KMK 2/13/04
	iProducts[pn]->iGRPsThisTick += impressionsDelivered;
}

void CMicroSegment::external_factors()
{
	iCatQuantityPurchasedModifier = 0;
	for(EF_iter iter = iExternalFactor.begin(); iter != iExternalFactor.end(); ++iter)
	{
		iCatQuantityPurchasedModifier += iter->second->GetDemand();
	}
}

double	CMicroSegment::ScaledOddsOfDisplay(Pcn pcn, MarketUtility* market_utility)
{
	// need to adjust awareness for distribution:
	// If an item has 50% ACV Dist, and 20% ACV Any Display, then 40% (20/50) of the trips 
	// when an agent enters a store where the item is stocked the agent can encounter the display

	// pre use distribution is used here as it is inndependent of whether or not the agent knows where to find the product
	// which is what the post use distribution is supposed to be modeling.

	double	dist = iProductsAvailable[pcn]->iPreUseDistributionPercent;

	double scaledOddsOfDisplayEncounter = market_utility->GetDistribution();

	if (dist == 0.0 || scaledOddsOfDisplayEncounter == 0.0)
	{
		return 0.0;
	}

	if (dist < 1.0)
	{
		scaledOddsOfDisplayEncounter /= dist;
		if (scaledOddsOfDisplayEncounter > 1.0)
		{
			scaledOddsOfDisplayEncounter = 1.0;
		}
	}

	return scaledOddsOfDisplayEncounter;
}

double CMicroSegment::AttributeScore(ID attr_id, DynamicAttribute* attribute, int post_use)
{
	if(post_use)
	{
		return iAttributePreferences[attr_id]->GetPostUse()*attribute->PostUse();
	}
	else
	{
		return iAttributePreferences[attr_id]->GetPreUse()*attribute->PreUse();
	}

}

void CMicroSegment::AddDynamicAttribute(ProductAttribute *attribute)
{
	double aware_prob = iAttributes[attribute->GetAttribute()]->GetInitialAwareness();
	for(Index i = 0; i < iPopulation->iConsumerList.size(); i++)
	{
		if(WillDo0to1(aware_prob))
		{
			iPopulation->iConsumerList[i]->dynamic_attributes[ProdIndex(attribute->GetProduct())][attribute->GetAttribute()] = new DynamicAttribute(attribute, 1);
		}
		else
		{
			iPopulation->iConsumerList[i]->dynamic_attributes[ProdIndex(attribute->GetProduct())][attribute->GetAttribute()] = new DynamicAttribute(attribute, 0);
		}
	}
}

void CMicroSegment::WriteAttributes(ofstream& file)
{
	int prod_id;
	int attr_id;
	//Write Product Attributes
	for(Pn_iter p_iter = iProducts.begin(); p_iter != iProducts.end(); ++p_iter)
	{
		for(An_iter a_iter = iAttributes.begin(); a_iter != iAttributes.end(); ++a_iter)
		{
			prod_id = (*p_iter)->iProductID;
			attr_id = a_iter->second->GetAttribute();
			if(a_iter->second->GetType() == 0 && iProductAttributes.find(prod_id) != iProductAttributes.end() && iProductAttributes[prod_id]->find(attr_id) != iProductAttributes[prod_id]->end())
			{
				
				file << prod_id << " ";
				file << attr_id << " ";
				file << (double)(*iProductAttributes[prod_id])[attr_id]->GetPreUse() << " ";
				file << (double)(*iProductAttributes[prod_id])[attr_id]->GetPostUse() << " ";
			}
		}
	}
	file << (int)(-1) << " ";

	//Write consumer preferences
	for(Can_iter c_iter = iAttributePreferences.begin(); c_iter != iAttributePreferences.end(); ++c_iter)
	{
		file << (int)c_iter->first << " ";
		file << (double)c_iter->second->GetPreUse() << " ";
		file << (double)c_iter->second->GetPostUse() << " ";
		file << (double)c_iter->second->GetPriceSensitivity() << " ";
	}

	file << (int)(-1) << " ";

}

void CMicroSegment::ReadAttributes(ifstream& file)
{
	int prod_id;
	int attr_id;
	double pre_use;
	double post_use;
	double sensitivity;
	//Read Product Attributes
	file >> prod_id;
	while(prod_id != -1)
	{
		file >> attr_id;
		file >> pre_use;
		file >> post_use;
		//This is going to be a memory leak...
		(*iProductAttributes[prod_id])[attr_id] = new ProductAttribute(pre_use, post_use, attr_id, iSegmentID, prod_id, -1, iModelInfo->start_date, iModelInfo->end_date);
		file >> prod_id;
	}

	//Read Consumer Preferences
	file >> attr_id;
	while(attr_id != -1)
	{
		file >> pre_use;
		file >> post_use;
		file >> sensitivity;
		//This also may be a memory leak...
		iAttributePreferences[attr_id] = new ConsumerPref(pre_use, post_use, sensitivity, attr_id, iSegmentID, -1, -1, iModelInfo->start_date, iModelInfo->end_date);
		file >> attr_id;
	}
}

