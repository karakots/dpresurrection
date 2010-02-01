// Messages.cpp
//
// Copyright 1998	Salamander Interactive
//
// Handles the messaging for the microsegment
// Created by Ken Karakotsios 11/6/98

//
// VdM 2/17/04
// Added SKUsAtPrice in HereIsProductISell
// VdM 2/25/04
// replaced kPriceType_ with kPricingType_ constant in HereIsProductISell
//


#ifndef __MICROSEG__
#include "CMicroSegment.h"
#endif

#include <stdlib.h>
#include <math.h>
#include "DBNetworkParams.h"

#include "SocialNetwork.h"
#include "Consumer.h"
#include "RepurchaseModel.h"
#include "DBModel.h"
#include "Marketutility.h"

#include "ProductTree.h"

using namespace std;


// ------------------------------------------------------------------------------
// Respond to incoming asynchronous data.
// ------------------------------------------------------------------------------
int	CMicroSegment::AcceptNoAckMessage(int inMessage)
{
	switch(iCtr->Get_MsgInstr(inMessage))
	{
		__MSG_RESPONSE__(kMsg, ResetNoAck)
		__MSG_RESPONSE__(kMsg_, YouAreInspected)

		// products
		__MSG_RESPONSE__(kMarketSimMessage_, SellProduct)
		__MSG_RESPONSE__(kMarketSimMessage_, HereIsProductISell)
		__MSG_RESPONSE__(kMarketSimMessage_, HereIsProductINoLongerSell)
		__MSG_RESPONSE__(kMarketSimMessage_, HereIsMrktControlInfo)
		__MSG_RESPONSE__(kMarketSimMessage_, HereIsTokenControl)
		__MSG_RESPONSE__(kMarketSimMessage_, HereIsProductColor)
		__MSG_RESPONSE__(kMarketSimMessage_, HereIsProductDropRateInfo)

		__MSG_RESPONSE__(kMarketSimMessage_, PopulationSize)
		//__MSG_RESPONSE__(kMarketSimMessage_, HereIsInitialUnitsSold)
		__MSG_RESPONSE__(kMarketSimMessage_, HereIsInitialPenetration)
		__MSG_RESPONSE__(kFinMessage_, HereIsCurrentTime)

		// advertising
		//__MSG_RESPONSE__(kMarketSimMessage_, HereIsStrategyData)
		__MSG_RESPONSE__(kMarketSimMessage_, HereIsStrategyData1)
		__MSG_RESPONSE__(kMarketSimMessage_, HereIsStrategyData2)

		
		__MSG_RESPONSE__(kMarketSimMessage_, TaskEvent)
		__MSG_RESPONSE__(kMarketSimMessage_, HereIsMarketUtilityInfo)
		__MSG_RESPONSE__(kMarketSimMessage_, HereIsASpecialEvent)


		// SSIO
		__MSG_RESPONSE__(kMarketSimMessage_, HereIsMicroSegValue)

		// Channel
		__MSG_RESPONSE__(kMarketSimMessage_, TellMePenetration)
		__MSG_RESPONSE__(kMarketSimMessage_, TellMeAddDrops)
		__MSG_RESPONSE__(kMarketSimMessage_, HereIsProductQuantityAvailable)

		// segments
		__MSG_RESPONSE__(kMarketSimMessage_, HereIsSegmentName)
		__MSG_RESPONSE__(kMarketSimMessage_, HereIsSegmentPointer)
		__MSG_RESPONSE__(kMarketSimMessage_, CreateOrCloseTransactionFile)

		// VdM 7/25/01
		// user tasks
		__MSG_RESPONSE__(kMarketSimMessage_, HereIsUserTaskSegInfo);
		__MSG_RESPONSE__(kMarketSimMessage_, HereIsUserTaskProdInfo);

		// VdM 4/22/02
		// share and penetration
		__MSG_RESPONSE__(kMarketSimMessage_, HereIsSharePenetration);		
		
		// VdM 7/25/04
		__MSG_RESPONSE__(kMarketSimMessage_, HereIsProductMatrixInfo);


		__MSG_RESPONSE__(kMarketSimMessage_, HereIsSocNetValue);
		
		default:
			return false;
	}
}


// ------------------------------------------------------------------------------
//	kMarketSimMessageParam_ProductName
//	kMarketSimMessageParam_ChannelName
//	kMarketSimMessageParam_SegmentName
//	kMarketSimMessageParam_Amount
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_HereIsProductQuantityAvailable(int inMessage)
{
	int	amount = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_Amount);
	// -1 means no limit
	if (amount == -1)
		return;

	if (string::StrCmp(&iNameString, (string *)
		(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_SegmentName))))
	{
		// get the product and channel name
		string	*prodName;
		string	*channelName;
		int		prodChanIndex;
		int		foundIt = false;

		prodName = (string *)(iCtr->Get_MsgParam(inMessage,
			kMarketSimMessageParam_ProductName));
		channelName = (string *)(iCtr->Get_MsgParam(inMessage,
			kMarketSimMessageParam_ChannelName));

		// find the product and channel in the iProdsAvailable array
		for (prodChanIndex =0; prodChanIndex < iProductsAvailable.size(); prodChanIndex++)
		{
			if ((string::StrCmp(&((iProductsAvailable[prodChanIndex]).iProdName), prodName)) && 
				(string::StrCmp(&((iProductsAvailable[prodChanIndex]).iChanName), channelName)))
			{
				foundIt = true;
				break;
			}
		}

		if (!foundIt)
			return;
		else
			iProductsAvailable[prodChanIndex].iUnitsAvailableForSale = amount;
	}
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
int CMicroSegment::TranslateStratType(int typeIn)
{
	switch (typeIn)
	{
		case kCommunicationType_Advertisement:
			return kAdvertType_MassMedia;
			break;
		case kCommunicationType_AdVariableImpressions:
		case kCommunicationType_AdVariableImpressionsPrint:
		case kCommunicationType_AdVariableImpressionsEvent:
			return kAdvertType_MassMedia;
			break;
		case kCommunicationType_Coupon:
			return kAdvertType_Coupon;
			break;
		case kCommunicationType_BOGO:
			return kAdvertType_BOGO;
			break;
		case kCommunicationType_sample:
			return kAdvertType_Sample;
			break;
		case kCommunicationType_event_units_purchased:
			return kAdvertType_Event_units_purchased;
			break;
		case kCommunicationType_event_shopping_trips:
			return kAdvertType_Event_shopping_trips;
			break;
		case kCommunicationType_distribution:
			return kAdvertType_Distribution;
			break;
		case kCommunicationType_Display1:
			return kAdvertType_Display1;
			break;
		case kCommunicationType_Display2:
			return kAdvertType_Display2;
			break;
		case kCommunicationType_Display3:
			return kAdvertType_Display3;
			break;
		case kCommunicationType_Display4:
			return kAdvertType_Display4;
			break;
		case kCommunicationType_Display5:
			return kAdvertType_Display5;
			break;
		case kCommunicationType_display:
			return kAdvertType_Display;
			break;
		case kCommunicationType_event_price_disutility:
			return kAdvertType_Event_price_disutility;
			break;
		default:
			return -1;
	}
}


// ------------------------------------------------------------------------------
// iCtr->Set_MsgParam(msgOut, kDataMsgParam_SendingNode, (int) myNodePtr);
// iCtr->Set_MsgParam(msgOut, kMarketSimMessageParam_CompanyName, (int)&iCompanyName);
// iCtr->Set_MsgParam(msgOut, kMarketSimMessageParam_ProductName, (int)&iProductName);
// iCtr->Set_MsgParam(msgOut, kMarketSimMessageParam_ChannelName, (int)&iChannelName);
// iCtr->Set_MsgParam(msgOut, kMarketSimMessageParam_StrategyName, (int)&iName);
// iCtr->Set_MsgParam(msgOut, kMarketSimMessageParam_StrategyType, (int)iStratType);

// param 1 = name of the segment to whom this task is targeted
// kMarketSimMessageParam_StratParam1
// param 2 = duration (in days) of the task
// kMarketSimMessageParam_StratParam2
// param 3 = number of impressions
// kMarketSimMessageParam_StratParam3
// param 4 = cost per impression
// kMarketSimMessageParam_StratParam4
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_HereIsStrategyData1(int msgIn)
{
	// figure number of impressions
	int	tempLong = iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_StratParam3);
	double	impressions = MSG_LONG_TO_FLOAT(tempLong);
	int	stratType = iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_StrategyType);

	// make sure this is an advertising message
	switch (stratType)
	{
		case kCommunicationType_Advertisement:
		case kCommunicationType_AdVariableImpressions:
		case kCommunicationType_AdVariableImpressionsPrint:
		case kCommunicationType_AdVariableImpressionsEvent:
		case kCommunicationType_Coupon:
		case kCommunicationType_BOGO:
		case kCommunicationType_sample:
		case kCommunicationType_event_units_purchased:
		case kCommunicationType_event_shopping_trips:
		case kCommunicationType_Display1:
		case kCommunicationType_Display2:
		case kCommunicationType_Display3:
		case kCommunicationType_Display4:
		case kCommunicationType_Display5:
		case kCommunicationType_display:
		case kCommunicationType_distribution:
		case kCommunicationType_event_price_disutility:		// hiho should we do anytiong here, or wait unti we encounter the message in real time?
		{
			string	*segmentName;
			string	*adName;
			string	*prodName;
			string	*channelName;
			int		prodChanIndex;
			string	allText;
			int		prodNum;
			int		duration;
			double		scaledImpresions;

			int locAdvertType = TranslateStratType(stratType);
			switch (stratType)
			{
				case kCommunicationType_Advertisement:
					scaledImpresions = impressions;
					break;
				case kCommunicationType_AdVariableImpressions:
				case kCommunicationType_AdVariableImpressionsPrint:
				case kCommunicationType_AdVariableImpressionsEvent:
					scaledImpresions = (impressions  / 100.0);
					break;
				case kCommunicationType_Coupon:
					scaledImpresions = (impressions  / 100.0);
					break;
				case kCommunicationType_BOGO:
					scaledImpresions = (impressions  / 100.0);
					break;
				case kCommunicationType_sample:
					scaledImpresions = (impressions  / 100.0);
					break;
				case kCommunicationType_event_units_purchased:
					scaledImpresions = (impressions  / 100.0);
					break;
				case kCommunicationType_event_shopping_trips:
					scaledImpresions = (impressions  / 100.0);
					break;
				case kCommunicationType_distribution:
					scaledImpresions = (impressions  / 100.0);
					break;
				case kCommunicationType_Display1:
				case kCommunicationType_Display2:
				case kCommunicationType_Display3:
				case kCommunicationType_Display4:
				case kCommunicationType_Display5:
				case kCommunicationType_display:
					scaledImpresions = (impressions  / 100.0);
					break;
				case kCommunicationType_event_price_disutility:
					// no need to scale impressions for this
					scaledImpresions = impressions;
					break;
				default:
					;
			}

			string::StrCpy((iCtr->iCommonStringPtrs)[kStr_Allm], &allText);
			// this message will tell me that an advertising task has been 
			// created.
			// I need to know the task name, the number of impressions, and
			// filter on the segment name
			segmentName = (string *)(iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_StratParam1));

			// only care if this is to me or to "all" segments
			// put a special case in for distribution, which in the version 3.2 SSIO has no segment associated with it KMK 11/4/03
			if ((stratType == kCommunicationType_distribution) || 
				(stratType == kCommunicationType_display) || (stratType == kCommunicationType_Display1) || (stratType == kCommunicationType_Display2) || 
				(stratType == kCommunicationType_Display3) || (stratType == kCommunicationType_Display4) || (stratType == kCommunicationType_Display5) ||
				string::StrCmp(segmentName, &iNameString) || string::StrCmpNoCase(segmentName, &allText))
			{
				int i;
				int	foundIt = false;
				// this is my segment. 
				
				prodName = (string *)(iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_ProductName));
				channelName = (string *)(iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_ChannelName));
				duration = iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_StratParam2);

				// if this is for a product that is eliminated, ignore it
				// coupon, sample, advertisement
				// if (foundIt && (stratType == kCommunicationType_Coupon))
				/*
				if (stratType != kCommunicationType_event)
				{
					for (prodChanIndex =0; prodChanIndex < iProductsAvailable.size(); prodChanIndex++)
					{
						if ((string::StrCmp(&((iProductsAvailable[prodChanIndex]).iProdName), prodName)) && 
							(string::StrCmp(&((iProductsAvailable[prodChanIndex]).iChanName), channelName)))
						{
							prodNum = GetProdNumFromProdChanNum(prodChanIndex);
							if ((prodNum != kNoProductBought) && (prodNum != -1))
							{
								if (iProducts[prodNum].iEliminated)
									return;
							}
						}
					}
				}
				*/

				// get the task's name
				adName = (string *)(iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_StrategyName));
				//check to see if we have this one already
				// we never will the first time through
				int	limit = iAdSchedule.size();
				
				// don;t need to search the advert list the first time we are building it
				/*if (iSearchAdverts)
				{
					for (i = 0; i < limit; i++)
					{
						if (string::StrCmp(adName, &(iAdSchedule[i].name)))
						//if (string::StrCmp(adName, &(iAdSchedule[i].name)))
						{
							foundIt = true;
							break;
						}
					}
				}*/
				
				// Record the task name
				if (iAdMap.find(*adName) == iAdMap.end())
				{
					Advert	tempAdd;
					InitializeAdvert(&tempAdd);	
					i = iAdSchedule.size();
					iAdSchedule.push_back(tempAdd);
					iAdMap[(*adName)] = i;
				}
				else
				{
					i = iAdMap[(*adName)];
				}
				string::StrCpy(adName, &(iAdSchedule[i].name));

				// figure number of impressions
				//tempLong = iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_StratParam3);
				//double impressions = MSG_LONG_TO_FLOAT(tempLong);

				iAdSchedule[i].numImpressions = scaledImpresions;
				switch (stratType)
				{
					case kCommunicationType_Advertisement:
						//iAdSchedule[i].numImpressions = impressions;
						//iAdSchedule[i].messageStrength = strength;
						iAdSchedule[i].iAdvertType = kAdvertType_MassMedia;
						// cost per impression
						tempLong = iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_StratParam4);
						iAdSchedule[i].costPerImpression = MSG_LONG_TO_FLOAT(tempLong);
						break;

					case kCommunicationType_AdVariableImpressions:
					case kCommunicationType_AdVariableImpressionsPrint:
					case kCommunicationType_AdVariableImpressionsEvent:
						// remember impressions as a percentage
						//iAdSchedule[i].numImpressions = (impressions  / 100.0);
						//iAdSchedule[i].messageStrength = strength;
						iAdSchedule[i].isPercentage = true;
						iAdSchedule[i].iAdvertType = kAdvertType_MassMedia;
						// cost per impression
						tempLong = iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_StratParam4);
						iAdSchedule[i].costPerImpression = MSG_LONG_TO_FLOAT(tempLong);
						break;

					case kCommunicationType_Coupon:
						// remember impressions as a percentage
						//iAdSchedule[i].numImpressions = (impressions  / 100.0);
						// in the case of a coupon, message strength is also a percentage
						// can't have more than 100% of redemptions
						//if (iAdSchedule[i].messageStrength > 100.0)
						//	iAdSchedule[i].messageStrength = 100.0;
						//iAdSchedule[i].messageStrength = (strength  / 100.0);
						iAdSchedule[i].isPercentage =true;
						iAdSchedule[i].iAdvertType = kAdvertType_Coupon;
						// cost per impression
						tempLong = iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_StratParam4);
						iAdSchedule[i].costPerImpression = MSG_LONG_TO_FLOAT(tempLong);
						// for BOGO, cost per impression is percent skipping next time
						break;

					case kCommunicationType_BOGO:
						// remember impressions as a percentage
						//iAdSchedule[i].numImpressions = (impressions  / 100.0);
						// in the case of a coupon, message strength is also a percentage
						// can't have more than 100% of redemptions
						//if (iAdSchedule[i].messageStrength > 100.0)
						//	iAdSchedule[i].messageStrength = 100.0;
						//iAdSchedule[i].messageStrength = (strength  / 100.0);
						iAdSchedule[i].isPercentage =true;
						iAdSchedule[i].iAdvertType = kAdvertType_BOGO;
						// cost per impression
						tempLong = iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_StratParam4);
						iAdSchedule[i].costPerImpression = MSG_LONG_TO_FLOAT(tempLong);
						break;

					case kCommunicationType_sample:
						// remember impressions as a percentage
						//iAdSchedule[i].numImpressions = (impressions  / 100.0);
						// in the case of a coupon, message strength is also a percentage
						// can't have more than 100% of redemptions
						//if (iAdSchedule[i].messageStrength > 100.0)
						//	iAdSchedule[i].messageStrength = 100.0;
						//iAdSchedule[i].messageStrength = (strength  / 100.0);
						iAdSchedule[i].isPercentage = true;
						iAdSchedule[i].iAdvertType = kAdvertType_Sample;
						// cost per impression
						tempLong = iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_StratParam4);
						iAdSchedule[i].costPerImpression = MSG_LONG_TO_FLOAT(tempLong);
						break;

					case kCommunicationType_event_units_purchased:
						// remember impressions as a percentage
						//iAdSchedule[i].numImpressions = (impressions  / 100.0);
						// in the case of a coupon, message strength is also a percentage
						// can't have more than 100% of redemptions
						iAdSchedule[i].isPercentage = true;
						iAdSchedule[i].iAdvertType = kAdvertType_Event_units_purchased;
						break;

					case kCommunicationType_event_price_disutility:
						iAdSchedule[i].isPercentage = false;
						iAdSchedule[i].iAdvertType = kAdvertType_Event_price_disutility;
						break;

					case kCommunicationType_event_shopping_trips:
						// remember impressions as a percentage
						//iAdSchedule[i].numImpressions = (impressions  / 100.0);
						// in the case of a coupon, message strength is also a percentage
						// can't have more than 100% of redemptions
						iAdSchedule[i].isPercentage = true;
						iAdSchedule[i].iAdvertType = kAdvertType_Event_shopping_trips;
						break;

					case kCommunicationType_distribution:
						double	postDist;
						// remember distribution (pre-use) as a percentage
						//iAdSchedule[i].numImpressions = (impressions  / 100.0);
						// in the case of a coupon, message strength is also a percentage
						// can't have more than 100% of redemptions
						iAdSchedule[i].isPercentage = true;
						iAdSchedule[i].iAdvertType = kAdvertType_Distribution;
						// remember distribution (post-use) as a percentage
						tempLong = iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_StratParam4);
						postDist = MSG_LONG_TO_FLOAT(tempLong);
						iAdSchedule[i].costPerImpression = postDist / 100.0;
						break;

					case kCommunicationType_Display1:
						iAdSchedule[i].isPercentage =true;
						iAdSchedule[i].iAdvertType = kAdvertType_Display1;
						break;
					case kCommunicationType_Display2:
						iAdSchedule[i].isPercentage =true;
						iAdSchedule[i].iAdvertType = kAdvertType_Display2;
						break;
					case kCommunicationType_Display3:
						iAdSchedule[i].isPercentage =true;
						iAdSchedule[i].iAdvertType = kAdvertType_Display3;
						break;
					case kCommunicationType_Display4:
						iAdSchedule[i].isPercentage =true;
						iAdSchedule[i].iAdvertType = kAdvertType_Display4;
						break;
					case kCommunicationType_Display5:
						iAdSchedule[i].isPercentage =true;
						iAdSchedule[i].iAdvertType = kAdvertType_Display5;
						break;
					case kCommunicationType_display:
						iAdSchedule[i].isPercentage =true;
						iAdSchedule[i].iAdvertType = kAdvertType_Display;
						break;
				}

				iAdSchedule[i].daysDuration = 
					iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_StratParam2);

				iAdSchedule[i].advertProdNum = -1;
				iAdSchedule[i].advertChanNum = -1;
				iAdSchedule[i].advertProdChanNum = -1;

					//get the product and channel names
				//not that for task-based buying, special events  specify the task, not the product
				foundIt = false;
				string::StrCpy(prodName, &(iAdSchedule[i].productName));
				string::StrCpy(channelName, &(iAdSchedule[i].iChannelName));

				iAdSchedule[i].iTreatProdNameAsTaskName = false;

				// "All" for both product and channel
				if ((string::StrCmpNoCase(prodName, &allText)) && 
					(string::StrCmpNoCase(channelName, &allText)))
				{
					iAdSchedule[i].scope = kAdvertScope_AllProdsAllChannels;
					// will use the general putpose demand modifier
				}

				// all channels, one product
				else if (string::StrCmpNoCase(channelName, &allText))
				{
					// set the modifier in all the individual instances of this product in the
					// product-channel array (on each tick)
					iAdSchedule[i].scope = kAdvertScope_OneProdAllChannels;
					// Add the product number
					iAdSchedule[i].advertProdNum = -1;
					if (iAdSchedule[i].iTreatProdNameAsTaskName)
					{
						//int taskNum;
						//for (taskNum = 0; taskNum < iTaskRates.size(); taskNum++)
						//{
						//	if (string::StrCmpNoCase(&(iTaskRates[taskNum].iTaskName), prodName))
						//	{
						//		iAdSchedule[i].taskNum = taskNum;
						//		break;
						//	}
						//}
					}
					else
					{
						FindAdvertProdNum(i);
						//int prodNum;
						//for (prodNum = 0; prodNum < iProducts.size(); prodNum++)
						//{
						//	if (string::StrCmpNoCase(&(iProducts[prodNum].iName), prodName))
						//	{
						//		iAdSchedule[i].advertProdNum = prodNum;
						//		break;
						//	}
						//}
					}
				}

				// all products, one channel
				else if (string::StrCmpNoCase(prodName, &allText))
				{
					// set the demand modifier for all the products in a given channel (on each tick)
					iAdSchedule[i].scope = kAdvertScope_AllProdsOneChannel;
					// Add the channel number
					int chanNum;
					for (chanNum = 0; chanNum < iChannelsAvailable.size(); chanNum++)
					{
						if (string::StrCmpNoCase(&(iChannelsAvailable[chanNum].iChanName), channelName))
						{
							iAdSchedule[i].advertChanNum = chanNum;
							break;
						}
					}
				}

				else // specific product and channel
				{
					iAdSchedule[i].scope = kAdvertScope_OneProdOneChannel;
					if (iAdSchedule[i].iTreatProdNameAsTaskName)
					{
						//int taskNum;
						//iAdSchedule[i].taskNum = -1;
						//for (taskNum = 0; taskNum < iTaskRates.size(); taskNum++)
						//{
						//	if (string::StrCmpNoCase(&(iTaskRates[taskNum].iTaskName), prodName))
						//	{
						//		iAdSchedule[i].taskNum = taskNum;
						//		foundIt = true;	// ??
						//		break;
						//	}
						//}
					}
					else // not TBB
					{
						// get the number of the associated product
						for (prodChanIndex =0; prodChanIndex < iProductsAvailable.size(); prodChanIndex++)
						{
							if ((string::StrCmp(&((iProductsAvailable[prodChanIndex]).iProdName), prodName)) && 
								(string::StrCmp(&((iProductsAvailable[prodChanIndex]).iChanName), channelName)))
							{
								iAdSchedule[i].advertProdChanNum = prodChanIndex;
								iAdSchedule[i].advertProdNum = GetProdNumFromProdChanNum(prodChanIndex);
								iAdSchedule[i].advertChanNum = FastGetChannelNumFromProdChannelNum(prodChanIndex);
								break;
							}
						}

						// if we didn't find it, we should Add the product/channel pair
						// but we don;t Add a task if we did not find it...
						// This code is never reached so it is being removed Isaac 6-21-2006
						/*
						if (!foundIt)
						{
							//AddProdChannel(prodName, channelName, 0, -1, false);
							// look for it again
							for (prodChanIndex =0; prodChanIndex < iProductsAvailable.size(); prodChanIndex++)
							{
								if ((string::StrCmp(&((iProductsAvailable[prodChanIndex]).iProdName), prodName)) && 
									(string::StrCmp(&((iProductsAvailable[prodChanIndex]).iChanName), channelName)))
								{
									// we need to Add the company name to the new entry in iProductsAvailable
									// but we don;t know it here, so we will count on it being set in 
									// RespondTo_HereIsProductISell()
									iAdSchedule[i].advertProdChanNum = prodChanIndex;
									iAdSchedule[i].advertProdNum = GetProdNumFromProdChanNum(prodChanIndex);
									iAdSchedule[i].advertChanNum = FastGetChannelNumFromProdChannelNum(prodChanIndex);
									break;
								} // end prod chanel name compare
							} // end search of list of prod channel pairs
						} // end clause for didn't find prod number
						*/
					} // end check for not TBB
				} // end specific product and channel

			} // end compare for segment name
		} // end check for advert type
	}	//end switch 
}


// ------------------------------------------------------------------------------
// iCtr->Set_MsgParam(msgOut, kDataMsgParam_SendingNode, (int) myNodePtr);
// iCtr->Set_MsgParam(msgOut, kMarketSimMessageParam_CompanyName, (int)&iCompanyName);
// iCtr->Set_MsgParam(msgOut, kMarketSimMessageParam_ProductName, (int)&iProductName);
// iCtr->Set_MsgParam(msgOut, kMarketSimMessageParam_ChannelName, (int)&iChannelName);
// iCtr->Set_MsgParam(msgOut, kMarketSimMessageParam_StrategyName, (int)&iName);
// iCtr->Set_MsgParam(msgOut, kMarketSimMessageParam_StrategyType, (int)iStratType);

// param 1 = name of the segment to whom this task is targeted
// kMarketSimMessageParam_StratParam1
// param 2 = redemption or usage rate
// kMarketSimMessageParam_StratParam2
// param 3 = message awareness
// kMarketSimMessageParam_StratParam3
// param 4 = message persuasion
// kMarketSimMessageParam_StratParam4
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_HereIsStrategyData2(int msgIn)
{
	int stratType = iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_StrategyType);
	// make sure this is an advertising message
	switch (stratType)
	{
		case kCommunicationType_Advertisement:
		case kCommunicationType_AdVariableImpressions:
		case kCommunicationType_AdVariableImpressionsPrint:
		case kCommunicationType_AdVariableImpressionsEvent:
		case kCommunicationType_Coupon:
		case kCommunicationType_BOGO:
		case kCommunicationType_sample:
		case kCommunicationType_event_units_purchased:
		case kCommunicationType_event_shopping_trips:
		case kCommunicationType_Display1:
		case kCommunicationType_Display2:
		case kCommunicationType_Display3:
		case kCommunicationType_Display4:
		case kCommunicationType_Display5:
		case kCommunicationType_display:
		case kCommunicationType_distribution:
		case kCommunicationType_event_price_disutility:		// hiho should we do anytiong here, or wait unti we encounter the message in real time?
		{
			string*	segmentName;
			string	allText;
			segmentName = (string *)(iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_StratParam1));
			string::StrCpy((iCtr->iCommonStringPtrs)[kStr_Allm], &allText);

			// only care if this is to me or to "all" segments
			// distribution goes to all segments by default KMK 11/4/03
			if ((stratType == kCommunicationType_distribution) || 
				(stratType == kCommunicationType_display) || (stratType == kCommunicationType_Display1) || (stratType == kCommunicationType_Display2) || 
				(stratType == kCommunicationType_Display3) || (stratType == kCommunicationType_Display4) || (stratType == kCommunicationType_Display5) ||
				string::StrCmp(segmentName, &iNameString) || string::StrCmpNoCase(segmentName, &allText))
			{
				int		adNum;
				string*	adName;

				// look for the message in the list of adverts....
				adName = (string *)(iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_StrategyName));
				//for (adNum = 0; adNum < iAdSchedule.size();  adNum++)
				// the last one Added is likely to be the match, I think
				int locAdvertType = TranslateStratType(stratType);
				for (adNum = iAdSchedule.size() - 1; adNum >= 0;  adNum--)
				{
					if ((iAdSchedule[adNum].iAdvertType == locAdvertType) && string::StrCmp(adName, &(iAdSchedule[adNum].name)))
					//if (string::StrCmp(adName, &(iAdSchedule[adNum].name)))
					{
						double		rate, aware, persuasion;
						int		tempLong;
						tempLong = iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_StratParam2);
						rate = MSG_LONG_TO_FLOAT(tempLong);
						tempLong = iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_StratParam3);
						aware = MSG_LONG_TO_FLOAT(tempLong);
						tempLong = iCtr->Get_MsgParam(msgIn, kMarketSimMessageParam_StratParam4);
						persuasion = MSG_LONG_TO_FLOAT(tempLong);

						iAdSchedule[adNum].messageStrength = aware;
						iAdSchedule[adNum].iRate = rate;
						iAdSchedule[adNum].iAwareness = aware;
						iAdSchedule[adNum].iPersuasion = persuasion;

						switch (stratType)
						{
							case kCommunicationType_Advertisement:
								break;

							case kCommunicationType_AdVariableImpressions:
							case kCommunicationType_AdVariableImpressionsPrint:
							case kCommunicationType_AdVariableImpressionsEvent:
								break;

							case kCommunicationType_Coupon:
								if (iAdSchedule[adNum].iRate > 100.0)
									iAdSchedule[adNum].iRate = 100.0;
								iAdSchedule[adNum].iRate = (rate  / 100.0);
								break;

							case kCommunicationType_BOGO:
								if (iAdSchedule[adNum].iRate > 100.0)
									iAdSchedule[adNum].iRate = 100.0;
								iAdSchedule[adNum].iRate = (rate  / 100.0);
								break;

							case kCommunicationType_sample:
								if (iAdSchedule[adNum].iRate > 100.0)
									iAdSchedule[adNum].iRate = 100.0;
								iAdSchedule[adNum].iRate = (rate  / 100.0);
								break;

							case kCommunicationType_event_units_purchased:
								break;

							case kCommunicationType_distribution:
								break;

							case kCommunicationType_Display1:
							case kCommunicationType_Display2:
							case kCommunicationType_Display3:
							case kCommunicationType_Display4:
							case kCommunicationType_Display5:
							case kCommunicationType_display:
								break;

							case kCommunicationType_event_price_disutility:
								break;
						} // end type of advert switch

						return;
					} // end found the advert in the list
				} // next advert
			} // end check for this segment (or all segments)
		} // end check for proper advert type
	}	// end switch
}

//
//
// created a message just for displayes
void CMicroSegment::RespondTo_HereIsChannelDisplay(int msgIn)
{
	

}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_ResetNoAck(int inMessage)
{
	int	prodChanNum;

	// SSN Nov 29 2005
	// Added a SimInfo file for NIMO
	// when we reset we need to forget that we may have written to it already
	// This is a static int, so only one segment will write
	wroteSimInfoFile = false;

	//RandomInitialise(1802,9373);

	int	pn;

	// Start - VdM
	// Added these lines - this is done in CreateTransactionFile but I think
	// some logic in there made it so this didn't happen when it should
	for (pn=0; pn < iProducts.size(); pn++)
		CreateTransactionFileP(pn, 0);
	for (pn=0; pn < iProducts.size(); pn++)
		CreateTransactionFileP(pn, 1);
	// End - VdM

	if (iSaveTransactionFile)
	{
		if (iFileBuffer != -1)
		{
			CreateTransactionFile(0);
			CreateTransactionFile(1);
		}
		else
			CreateTransactionFile(1);
	}

	if (iManageSessionLog)
	{
		if (iSessionLogFileBuffer != -1)
		{
			CreateSessionLogFile(0);
			CreateSessionLogFile(1);
		}
		else 
			CreateSessionLogFile(1);
	}

	ResetPopulationAndContext();
	iNumberDaysofSim = 0;
	iWroteTransactionHeader = false;
	iWroteMyTransactionHeader = false;
}



// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_YouAreInspected(int inMessage)
{

	//iInspectingNode = iCtr->Get_MsgParam(inMessage, kDataMsgParam_SendingNode);
}			


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_TellMePenetration(int inMessage)
{
	int		prodChanNum;
	string*	channelName;
	int		group;
	int		guy;
	int		numBought;
	int		msgOut;
	int		allChannels = false;

	int sendingNode = iCtr->Get_MsgParam(inMessage, kDataMsgParam_SendingNode);
	if (iCtr->Get_MsgParam(inMessage, kDataMsgParam_Value2) == NULL)
		allChannels = true;
	else
		channelName = (string *)(iCtr->Get_MsgParam(inMessage, kDataMsgParam_Value2));

	// for each product
	for (prodChanNum = 0; prodChanNum < iProductsAvailable.size(); prodChanNum++)
	{
		if (allChannels || 
			string::StrCmp(channelName, &iProductsAvailable[prodChanNum].iChanName))
		{
			int	produm = GetProdNumFromProdChanNum(prodChanNum);
			iProductsAvailable[prodChanNum].iPenetration = CountTriersOfProd(produm);
			numBought = iProductsAvailable[prodChanNum].iPenetration;

			// have to ge a new msgOut each time, becuase port is
			// destroyed after RcvBroadcastMsg completes
			msgOut = iCtr->GetBroadcastMsg(myNodePtr);
			iCtr->Set_MsgSqTyIn(msgOut, kMsgSequenceNoAck, kMsgNoAckMsg, 
				kMarketSimMessage_HereIsPenetration);
			iCtr->Set_MsgParam(msgOut, kDataMsgParam_SendingNode, (int) myNodePtr);
			iCtr->Set_MsgParam(msgOut, kDataMsgParam_Value2, (int) &iNameString);

			// product name
			iCtr->Set_MsgParam(msgOut, kDataMsgParam_Value3, 
				(int) &iProductsAvailable[prodChanNum].iProdName);
			// sales count
			iCtr->Set_MsgParam(msgOut, kDataMsgParam_Value4, numBought);
			// send it out
			iCtr->RcvBroadcastMsg(myNodePtr, sendingNode, msgOut, kWeb_MyWeb);
		}
	}
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_TellMeAddDrops(int inMessage)
{
	int		prodChanNum;
	string*	channelName;
	int		group;
	int		guy;
	int		numBought;
	int		msgOut;
	int		allChannels = false;
	int		pn;

	int sendingNode = iCtr->Get_MsgParam(inMessage, kDataMsgParam_SendingNode);
	if (iCtr->Get_MsgParam(inMessage, kDataMsgParam_Value2) == NULL)
		allChannels = true;
	else
		channelName = (string *)(iCtr->Get_MsgParam(inMessage, kDataMsgParam_Value2));

	// for each product
	for (prodChanNum = 0; prodChanNum < iProductsAvailable.size(); prodChanNum++)
	{
		if (allChannels || 
			string::StrCmp(channelName, &iProductsAvailable[prodChanNum].iChanName))
		{
			// have to ge a new msgOut each time, becuase port is
			// destroyed after RcvBroadcastMsg completes
			msgOut = iCtr->GetBroadcastMsg(myNodePtr);
			iCtr->Set_MsgSqTyIn(msgOut, kMsgSequenceNoAck, kMsgNoAckMsg, 
				kMarketSimMessage_HereIsAddDrops);
			iCtr->Set_MsgParam(msgOut, kDataMsgParam_SendingNode, (int) myNodePtr);
			iCtr->Set_MsgParam(msgOut, kDataMsgParam_Value2, (int) &iNameString);

			// product name
			iCtr->Set_MsgParam(msgOut, kDataMsgParam_Value3, 
				(int) &iProductsAvailable[prodChanNum].iProdName);

			// Adds & drops
			pn = GetProdNumFromProdChanNum(prodChanNum);
			iCtr->Set_MsgParam(msgOut, kDataMsgParam_Value4,(int) iProducts[pn].iAddsThisTick);
			iCtr->Set_MsgParam(msgOut, kDataMsgParam_Value5,(int) iProducts[pn].iTotalAdds);
			iCtr->Set_MsgParam(msgOut, kDataMsgParam_Value6,(int) iProducts[pn].iDropsThisTick);
			iCtr->Set_MsgParam(msgOut, kDataMsgParam_Value7,(int) iProducts[pn].iTotalDrops);			
			
			// send it out
			iCtr->RcvBroadcastMsg(myNodePtr, sendingNode, msgOut, kWeb_MyWeb);
		}
	}
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_SellProduct(int inMessage)
{
	// Add quantity of product that was sold to me to my store
	// find product-channel pair in list
	string *prodName = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ProductName));
	string *chanName = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ChannelName));
	int quantity = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_Quantity);
	
	for (int i = 0; i < iProductsAvailable.size(); i++)
	{
		if (string::StrCmp(prodName, &iProductsAvailable[i].iProdName) &&
			string::StrCmp(chanName, &iProductsAvailable[i].iChanName))
		{
			iProductsAvailable[i].iCurrentNumBought += quantity;
			iProductsAvailable[i].iTotalNumBought += quantity;
			iTotalSales += quantity;
			break;
		}
	}
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------

void CMicroSegment::ProcessPriceData(string *prodName, string *chanName, int priceType, 
									 double skusAtPrice, double newPrice)
{
	int pn;
	int cn;

	for (int i = 0; i < iProductsAvailable.size(); i++)
	{
		if (string::StrCmp(prodName, &iProductsAvailable[i].iProdName) &&
			string::StrCmp(chanName, &iProductsAvailable[i].iChanName))
		{
			iNeedToCalcFeatureScores = true;
			iNeedToCalcPriceScores = true;

			cn = iProductsAvailable[i].iChannelNum;;
			pn = iProductsAvailable[i].productIndex;

			ProductTree::LeafNodeList prodList(pn);
			for( ProductTree::Iter iter = prodList.begin(); iter != prodList.end(); ++iter)
			{
				pn = *iter;
				i = GetProdChanNumFromProdAndChannelNum(pn, cn);
				switch(priceType)
				{
					case kPricingType_Fixed:
						if ((iProductsAvailable[i].iPrice[kPriceTypeUnpromoted] != newPrice) || (iProductsAvailable[i].iProbPrice[kPriceTypeUnpromoted] != skusAtPrice))
						{
							iProductsAvailable[i].iPrice[kPriceTypeUnpromoted] = newPrice;
							iProductsAvailable[i].iProbPrice[kPriceTypeUnpromoted] = skusAtPrice;
							iProductsAvailable[i].iPrice[kPriceTypeBOGO] = iProductsAvailable[i].iPrice[kPriceTypeUnpromoted] * 0.5;
						}
						break;
					case kPricingType_Fixed_Promoted:
						if ((iProductsAvailable[i].iPrice[kPriceTypePromoted] != newPrice) || (iProductsAvailable[i].iProbPrice[kPriceTypePromoted] != skusAtPrice))
						{
							iProductsAvailable[i].iPrice[kPriceTypePromoted] = newPrice;
							iProductsAvailable[i].iProbPrice[kPriceTypePromoted] = skusAtPrice;
						}
						break;
					case kPricingType_Fixed_BOGO:
						{
							iProductsAvailable[i].iPctBOGOtoSkipNextPurchase = newPrice * .01;	// convert from percent input to probability
							if (iProductsAvailable[i].iPrice[kPriceTypeUnpromoted] != 0.0)
								iProductsAvailable[i].iPrice[kPriceTypeBOGO] = iProductsAvailable[i].iPrice[kPriceTypeUnpromoted] * 0.5;
							iProductsAvailable[i].iProbPrice[kPriceTypeBOGO] = skusAtPrice;
						}
						break;

						// KMK 5/1/05
						// reduced
						// Percent off unpromoted price in effect at that same time. 
						// This is entered as -20 for a 20% price reduction or 20 for a 20% price increase.
					case kPricingType_Fixed_Reduced:
						{
							// convert newprice into a scaling factor
							newPrice = 1.0 + (newPrice * .01);
							if (iProductsAvailable[i].iPrice[kPriceTypeUnpromoted] != 0.0)
								iProductsAvailable[i].iPrice[kPriceTypeReduced] = iProductsAvailable[i].iPrice[kPriceTypeUnpromoted] * newPrice;
							iProductsAvailable[i].iProbPrice[kPriceTypeReduced] = skusAtPrice;
						}
						break;

						// display price percent
						// Percent off unpromoted price in effect at that same time. 
						// This is entered as -20 for a 20% price reduction or 20 for a 20% price increase.
					case kPricingType_Fixed_FeaturePercent:
						{
							// convert newprice into a scaling factor
							newPrice = 1.0 + (newPrice * .01);
							// if (iProductsAvailable[i].iPrice[kPriceTypeDisplayPercent] != 0.0)
							// do not really save cycles to do test
							iProductsAvailable[i].iPrice[kPriceTypeDisplayPercent] = iProductsAvailable[i].iPrice[kPriceTypeUnpromoted] * newPrice;
							iProductsAvailable[i].iProbPrice[kPriceTypeDisplayPercent] = skusAtPrice;
							if (skusAtPrice > 0.0)
								iThereIsDisplayPricing = true;
						}
						break;

						// display price absolute
					case kPricingType_Fixed_FeatureAbsolute:
						if ((iProductsAvailable[i].iPrice[kPriceTypeDisplayAbsolute] != newPrice) || (
							iProductsAvailable[i].iProbPrice[kPriceTypeDisplayAbsolute] != skusAtPrice))
						{
							iProductsAvailable[i].iPrice[kPriceTypeDisplayAbsolute] = newPrice;
							iProductsAvailable[i].iProbPrice[kPriceTypeDisplayAbsolute] = skusAtPrice;
							if (skusAtPrice > 0.0)
								iThereIsDisplayPricing = true;
						}
						break;
				}
			}

			return;
		}
	}

	// didn't find it, so Add it to the list
	// TODO: FIX THIS! SSN 3/16/2005
	// AddProdChannel(prodName, chanName, iChanNodeID, 0.0, 0.0, 0.0, -1, active); ???
	// KMK 4/30/05
	//AddProdChannel(prodName, chanName, iChanNodeID, iProductsAvailable[i].iUnpromotedPrice, iProductsAvailable[i].iPromotedPrice, 
	//	iProductsAvailable[i].iBOGOPrice, iProductsAvailable[i].iPctBOGOtoSkipNextPurchase, active);
	// AddProdChannel(prodName, chanName, iProductsAvailable[i].iPrice, iProductsAvailable[i].iPctBOGOtoSkipNextPurchase, active);
}

void CMicroSegment::RespondTo_HereIsProductISell(int inMessage)
{
	// Add channel-product pair to my list of available products
	// make sure I don't already hold the pair
	//string *prodName = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ProductName));
	//string *chanName = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ChannelName));
	//int 		aLong; 
	//double 		newPrice;
	//int		active;
	//int		priceType;
	//// int		prodNum;
	//
	//// ------ VdM 2/17/04
	//double 		skusAtPrice;
	//aLong = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_SKUsAtPrice);
	//skusAtPrice = MSG_LONG_TO_FLOAT(aLong);
	//skusAtPrice *= .01;	// convert from percent to probability

	//aLong = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_Price);
	//newPrice = MSG_LONG_TO_FLOAT(aLong);
	//active = (int) iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_PriceStatus);

	//priceType = (int)iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_PriceType);
	//

	//ProcessPriceData(prodName, chanName, priceType, skusAtPrice, newPrice, active);
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_HereIsProductINoLongerSell(int inMessage)
{
	// remove channel-product pair from my list of available products
	string *prodName = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ProductName));
	string *chanName = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ChannelName));
	int i, j;
	
	for (i = 0; i < iProductsAvailable.size(); i++)
	{
		if (string::StrCmp(prodName, &iProductsAvailable[i].iProdName) &&
			string::StrCmp(chanName, &iProductsAvailable[i].iChanName))
		{
			break;
		}
	}

	// if there was no match with a product I store, do nothing more
	if (i == iProductsAvailable.size())
		return;

	// close the transaction file
	int	pn = GetProdNumFromProdChanNum(i);
	if (pn < 0)
		return;

	if (pn < iProducts.size())
	    CreateTransactionFileP(pn, 0);


	iProductsAvailable.RemoveAt(i);
	iProductsAvailable.FreeExtra();
	
	iNeedToCalcPriceScores = true;
	iNeedToCalcFeatureScores = true;

	// if this product is no longer sold through any channels, remove its
	// feature info, also (leaving it around will mess up other products'
	// feature scores
	for (i = 0; i < iProductsAvailable.size(); i++)
		if (string::StrCmp(prodName, &iProductsAvailable[i].iProdName))
			break;
	
	if (i == iProductsAvailable.size())	// we didn't find it (i is not a usefule index)
	{
		// no other occurences of this product were found
		int	oneToDelete;
	
		// find this product in the products list
		for (i = 0; i < iProducts.size(); i++)
			if (string::StrCmp(prodName, &iProducts[i].iName))
				break;

		// return if the product was not found
		if (i == iProducts.size())
			return;
		oneToDelete = i;

		
		// clear out the last slot
		iProducts[oneToDelete].iPreUseFeature.clear();
		iProducts[oneToDelete].iPostUseFeature.clear();
		iProducts[oneToDelete].iFeatureIrrellevant.clear();
		iProducts[oneToDelete].iYesNoFeature.clear();
		
		iProducts[oneToDelete].iPreUseFeature.FreeExtra();
		iProducts[oneToDelete].iPostUseFeature.FreeExtra();
		iProducts[oneToDelete].iFeatureIrrellevant.FreeExtra();
		iProducts[oneToDelete].iYesNoFeature.FreeExtra();

		iProducts.RemoveAt(oneToDelete);
		iProducts.FreeExtra();
		//iProdNames.RemoveAt(oneToDelete);
		//iProdFeatureScores.RemoveAt(oneToDelete);
		//iProdBuyPercentage.RemoveAt(oneToDelete);

		iNeedToCalcFeatureScores = true;
	}
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_HereIsMrktControlInfo(int inMessage)
{
	// only care about changing names of products and channels
	int msgType = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_InfoType);
	int i;
	string *oldName, *newName, *name;
	string *channel, *segname, *value;
	int color;
	int	setProdColor;
	
	switch (msgType)
	{
		case kInfoTypeChgSegLoyaltyPercent:
			channel = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_InfoValue1));
			segname = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_InfoValue2));
			value = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_InfoValue3));

			// update buy percent for given channel with value
			if (string::StrCmp(segname, &iNameString))
			{
				int foundIt = false;
				double	tempFloat;
				SIGetFloatFromString(value, &tempFloat);
				tempFloat /= 100.0;	// turn into percentage

				// find a channel that matches
				int chanNum;
				for (chanNum = 0; chanNum < iChannelsAvailable.size(); chanNum++)
				{
					if (string::StrCmp(channel, &(iChannelsAvailable[chanNum].iChanName)))
					{
						iChannelsAvailable[chanNum].iPctChanceChosen = tempFloat;
						// adjust the cumulative percentages
						foundIt = true;
					}
				}
				if (!foundIt)
				{
					// its a new channel.  Add it.
					ChannelRecord	tempCR;
					tempCR.InitializeChannelRecord();
					//tempCR.iChanName.MakeEmpty();
					//tempCR.iChanID = chanID; will learn this later
					tempCR.iPctChanceChosen = tempFloat;
					string::StrCpy(channel, &tempCR.iChanName);
					iChannelsAvailable.push_back(tempCR);

				}
				ComputeCumulativeChannelPercents();
			}
			break;

		case kInfoTypeChgProduct:
			oldName = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_InfoValue1));
			newName = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_InfoValue2));
			for (i = 0; i < iProductsAvailable.size(); i++)
				if (string::StrCmp(oldName, &iProductsAvailable[i].iProdName))
					string::StrCpy(newName, &iProductsAvailable[i].iProdName);
			// also change in the product-feature map
			for (i = 0; i < iProducts.size(); i++)
				if (string::StrCmp(oldName, &iProducts[i].iName))
					string::StrCpy(newName, &iProducts[i].iName);
			break;
			
		case kInfoTypeChgChannel:
			oldName = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_InfoValue1));
			newName = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_InfoValue2));
			for (i = 0; i < iProductsAvailable.size(); i++)
				if (string::StrCmp(oldName, &iProductsAvailable[i].iChanName))
					string::StrCpy(newName, &iProductsAvailable[i].iChanName);
			break;
			
		case kInfoTypeChgProdColor:
			name = (string *)iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_InfoValue1);
			color = (int)iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_InfoValue2);
			setProdColor = false;
			for (i = 0; i < iProductsAvailable.size(); i++)
				if (string::StrCmp(name, &iProductsAvailable[i].iProdName))
				{
					iProductsAvailable[i].iColor = color;
					if (!setProdColor)
						setProdColor = SetProductColor(name, color);
				}
			break;

		case kInfoTypeChgSegment:
			oldName = (string *)iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_InfoValue1);
			newName = (string *)iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_InfoValue2);
			if (string::StrCmp(oldName, newName))
			{
				string::StrCpy(newName, &iNameString);
			}
			break; 

		case kInfoTypeStartSimulation:
			iListenForTokenControl = true;
			break;

		default:
			break;
	}	
}


// ------------------------------------------------------------------------------
// Get an integer value from a floating point number, where the average of a 
// population of such integers will be the floating point value
// ------------------------------------------------------------------------------
void	CMicroSegment::AnnounceNewPopSize(PopObject *population)
{
	int msgOut = iCtr->GetBroadcastMsg(myNodePtr);	
	iCtr->Set_MsgSqTyIn(msgOut, kMsgSequenceNoAck, kMsgNoAckMsg, kMarketSimMessage_HereIsMrktControlInfo);
	iCtr->Set_MsgParam(msgOut, kDataMsgParam_SendingNode, (int) myNodePtr);
	iCtr->Set_MsgParam(msgOut, kMarketSimMessageParam_InfoType, (int) kInfoTypeChgSegmentSize);
	iCtr->Set_MsgParam(msgOut, kMarketSimMessageParam_InfoValue1, (int) &iNameString);	
	iCtr->Set_MsgParam(msgOut, kMarketSimMessageParam_InfoValue2, (int) (population->iGroupSize));
	iCtr->Set_MsgParam(msgOut, kMarketSimMessageParam_InfoValue3, (int)0);
	iCtr->RcvBroadcastMsg(myNodePtr, NULL, msgOut, kWeb_MyWeb);
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_HereIsProductDropRateInfo(int inMessage)
{
	string *prodName = (string *)iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ProductName);
	int	aLong;
	double	initialDropRate;
	double	dropRateDecay;

	aLong = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_InfoValue1);	
	initialDropRate = MSG_LONG_TO_FLOAT(aLong);	
	aLong = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_InfoValue2);	
	dropRateDecay = MSG_LONG_TO_FLOAT(aLong);	

	for (int i = 0; i < iProducts.size(); i++)
	{
		if (string::StrCmp(prodName, &iProducts[i].iName))
		{
			iProducts[i].iInitialDropRate = initialDropRate;
			iProducts[i].iDropRateDecay = dropRateDecay; //  / 100.0;
			return;
		}
	}
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_HereIsProductColor(int inMessage)
{
	string *prodName = (string *)iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ProductName);
	int	color = (int)iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_Color);
	int	setProdColor = false;

	for (int i = 0; i < iProductsAvailable.size(); i++)
	{
		if (string::StrCmp(prodName, &iProductsAvailable[i].iProdName))
		{
			iProductsAvailable[i].iColor = color;
			if (!setProdColor)
				setProdColor = SetProductColor(prodName, color);
		}
	}
}

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
int CMicroSegment::SetProductColor(string *name, int color)
{
	int	p;
	for (p=0; p< iProducts.size(); p++)
	{
		if (string::StrCmp(name, &iProducts[p].iName))
		{
			iProducts[p].iColor = color;
			return true;
		}
	}
	return false;
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_HereIsCurrentTime(int inMessage)
{
	int oldState = iClockRunning;
	iClockRunning = (int) iCtr->Get_MsgParam(inMessage, kFinMessageParam_ClockRunning);

	iDay = iCtr->Get_MsgParam(inMessage, kFinMessageParam_CurrentDay);
	iMonth = iCtr->Get_MsgParam(inMessage, kFinMessageParam_CurrentMonth);
	iYear = iCtr->Get_MsgParam(inMessage, kFinMessageParam_CurrentYear);
	
	// VdM 9/6/04
	if (!iClockRunning)
	{
		int pn;

		// sim run is done and flush any buffers
		for (pn=0; pn<iProducts.size(); pn++)
		{
			if (!(iProducts[pn].iFileIOBufferStr).IsEmpty())
				WriteALine(pn);
		}
	}
	
	//CalcQuantityToBuy();
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_PopulationSize(int inMessage)
{
	iTotalPopulationSize = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_PopulationSize);
	iListenForTokenControl = true;
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_HereIsTokenControl(int inMessage)
{
	int tokType, i, j;
	string	*tokName;
	string	*attrName;
	string	*oldAttrName;
	string	*newAttrName;
	double		attrVal, min, max, postAttrValue;
	int		aLong;
	int		irrelevant = false;
	
	if (!iListenForTokenControl)
		return;

	// care about store product characteristic info and segment info
	tokName = (string *)iCtr->Get_MsgParam(inMessage, 
		kMarketSimMessageParam_TokenControlName);
	tokType = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_TokenControlType);
	attrName = (string *) iCtr->Get_MsgParam(inMessage, 
		kMarketSimMessageParam_TokenControlAttribute);
	
	aLong = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_TokenControlPreValue);
	attrVal = MSG_LONG_TO_FLOAT(aLong);
	aLong = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_TokenControlPostValue);
	postAttrValue = MSG_LONG_TO_FLOAT(aLong);
	newAttrName = (string *) iCtr->Get_MsgParam(inMessage, 
		kMarketSimMessageParam_TokenControlNewAttributeName);
	oldAttrName = (string *) iCtr->Get_MsgParam(inMessage, 
		kMarketSimMessageParam_TokenControlOldAttributeName);
	aLong = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_TokenControlMinVal);	
	min = MSG_LONG_TO_FLOAT(aLong);	
	aLong = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_TokenControlMaxVal);
	max = MSG_LONG_TO_FLOAT(aLong);	

	// note: irrelevant applies ONLY to product features, and not to iFeatures
	if ((min == max) && (min == 0.0))
		irrelevant = true;

	switch (tokType)
	{
		case kTokenControlType_ProdProdChar:
			// tokName = product name
			// attrName = feature name
			// attrVal = feature score for given product
			// see if I store info on the feature represented by this token
			for (i = 0; i < iProducts.size(); i++)
			{
				// try to find the product
				if (string::StrCmp(&iProducts[i].iName, tokName))
					break;
			}

			// if I didn't hold this product, Add it
			if (i == iProducts.size())
				AddAProduct(tokName);

			//{
			//	string		tempLocalStr;
			//	Product			tempProd;
			//	double			tempFeature = 0.0;
			//	int			cn;
			//
			//	InitializeProductArray(&tempProd);
			//	string::StrCpy(tokName, &tempProd.iName);
			//
			//	iProducts.push_back(tempProd);
			//	iEmptyProdCompanyNames = true;
			//	InsertStartingShares(iProducts.size()-1);
				// 4/2/01 We will put AddProductToLastBought back in here,
				// because now it is going to track the actual usage of a product
			//	iPopulation->AddProductToLastBought(iProducts.size(), repurchaseModel->type);
			//	iSegNumProducts = iProducts.size();
				// since we just Added the product, we know it is not yet in iProductsAvailable
				// so we will Add it here. Since we don't know what channel this product will be in,
				// we will Add it to all the channels, as an inactive product
			//	for (cn = 0; cn < iChannelsAvailable.size(); cn++)
			//		AddProdChannel(&(tempProd.iName), &(iChannelsAvailable[cn].iChanName), 0, 0.0, 0.0, 0.0, -1, false);
			//}

			// now check the feature side
			for (j = 0; j < iFeatures->size(); j++)
			{
				if (string::StrCmpNoCase(&(*iFeatures)[j].iName, attrName))
				{
					// copy the info into my feature record
					if (!string::IsEmpty(newAttrName))		// name changing
							string::StrCpy(newAttrName, &(*iFeatures)[j].iName);
					 if (!irrelevant)	// axes will be set by other products
					{
						(*iFeatures)[j].iMin = min;
						(*iFeatures)[j].iMax = max;
					}
					break;
				}
			}
			// if I didn't hold this feature, Add it
			//if (j == iNumFeatures && j < iMaxFeatures)
			if (j == iFeatures->size())
			{
				Feature tempFeature;
				tempFeature.InitializeFeature();
				string::StrCpy(attrName, &tempFeature.iName);
				if (!irrelevant)
				{
					tempFeature.iMin = min;
					tempFeature.iMax = max;
				}
				//iNumFeatures++;
				iFeatures->push_back(tempFeature);
			}

			// store the product-feature score
			if (irrelevant)
			{
				iProducts[i].iPreUseFeature[j] = 0.0;
				iProducts[i].iPostUseFeature[j] = 0.0;
				iProducts[i].iFeatureIrrellevant[j] = true;
			}
			else
			{
				iProducts[i].iPreUseFeature[j] = attrVal;
				iProducts[i].iPostUseFeature[j] = postAttrValue;
				iProducts[i].iFeatureIrrellevant[j] = false;
			}
			iNeedToCalcFeatureScores = true;
			break;

		case kTokenControlType_CSProdChar:
			int	foundIt = false;
			// this is a token from a segment-feature map
			// make sure the message is to this segment
			if (string::StrCmp(tokName, &iNameString))
			{
				// see if I store info on the feature represented by this token
				for (i = 0; i < iFeatures->size(); i++)
				{
					if (string::StrCmpNoCase(&(*iFeatures)[i].iName, attrName))
					{
						(*iFeatures)[i].iPreUsePref = attrVal;
						(*iFeatures)[i].iPostUsePref = postAttrValue;
						if (!irrelevant)
						{
							(*iFeatures)[i].iPrefMinBound = min;
							(*iFeatures)[i].iPrefMaxBond = max;
						}
						foundIt = true;
						break;
					}
				}			
				
				// if we have not already found the token, try looking
				// under the old name
				if (!foundIt && !string::IsEmpty(oldAttrName))
				{
					for (i = 0; i < iFeatures->size(); i++)
					{
						if (string::StrCmpNoCase(&(*iFeatures)[i].iName, oldAttrName))
						{
							// name changing
							string::StrCpy(attrName, &(*iFeatures)[i].iName);
							if (!irrelevant)
							{
								(*iFeatures)[i].iPreUsePref = attrVal;
								(*iFeatures)[i].iPostUsePref = postAttrValue;
								(*iFeatures)[i].iPrefMinBound = min;
								(*iFeatures)[i].iPrefMaxBond = max;
							}
							foundIt = true;
							break;
						}
					}
				}

				// if I didn't hold this feature, Add it
				else if (!foundIt)
				{
					Feature tempFeature;
					tempFeature.InitializeFeature();
					string::StrCpy(attrName, &tempFeature.iName);
					if (!irrelevant)
					{
						tempFeature.iPreUsePref = attrVal;
						tempFeature.iPostUsePref = postAttrValue;
						tempFeature.iPrefMinBound = min;
						tempFeature.iPrefMaxBond = max;
					}
					iFeatures->push_back(tempFeature);
				}
			}
			iNeedToCalcFeatureScores = true;
			break;
	}
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::AddAProduct(string *prodName)
{
	string		tempLocalStr;
	Product			tempProd;
	double			tempFeature = 0.0;
	int			cn;

	InitializeProductArray(&tempProd);
	string::StrCpy(prodName, &tempProd.iName);

	iProducts.push_back(tempProd);
	int pn = iProducts.size() - 1;
	iEmptyProdCompanyNames = true;
	InsertStartingShares(iProducts.size()-1);
	// 4/2/01 We will put AddProductToLastBought back in here,
	// because now it is going to track the actual usage of a product
	// renamed to AddProduct as it does not have anyhtig to do with buying a product SSN 11/3/2005
	// iPopulation->AddProduct(iProducts.size(), repurchaseModel->type);
	iSegNumProducts = iProducts.size();
	// since we just Added the product, we know it is not yet in iProductsAvailable
	// so we will Add it here. Since we don't know what channel this product will be in,
	// we will Add it to all the channels, as an inactive product

	//Products are Added before channels so iChannelsAvailable.size() == 0
	/*for (cn = 0; cn < iChannelsAvailable.size(); cn++)
		//AddProdChannel(&(tempProd.iName), &(iChannelsAvailable[cn].iChanName), 0, -1, false);*/
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::CheckAttrBound(int fNum)
{
	if (((*iFeatures)[fNum].iPreUsePref > (*iFeatures)[fNum].iPrefMaxBond) ||
		((*iFeatures)[fNum].iPostUsePref > (*iFeatures)[fNum].iPrefMaxBond) ||
		((*iFeatures)[fNum].iPreUsePref < (*iFeatures)[fNum].iPrefMinBound) ||
		((*iFeatures)[fNum].iPostUsePref < (*iFeatures)[fNum].iPrefMinBound))
	{
		string	aStr;
		string bStr;
		string::CopyFromCString("Warning: The preference for attribute ", &aStr);
		string::StrCat(&aStr, &((*iFeatures)[fNum].iName));
		string::CopyFromCString(" is above the maximum or below the minimum specified for that attribute for segment  ", &bStr);
		
		// TODO: write to log
	}
}

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_HereIsInitialPenetration(int inMessage)
{
	;
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
/*void CMicroSegment::RespondTo_HereIsInitialUnitsSold(int inMessage)
{
	string *prodName = (string *)iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ProductName);
	int units =  iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_Quantity);

	if (units == 0)
		return;

	if (iTotalPopulationSize == 0)
		return;

	//units /= iPopulation->iPopScaleFactor;
	// don't try to Add more than there are people...
	if (units > iPopulation->iConsumerList.size())
	{
		units = iPopulation->iConsumerList.size();
	}

	int	prodNum;
	for (prodNum = 0; prodNum < iProducts.size(); prodNum++)
	{
		if (string::StrCmp(prodName, &iProducts[prodNum].iName))
		{
			iProducts[prodNum].initialUnitsToSellScaled = units;
			//iProducts[prodNum].initialUnitsToSellScaled /= iPopulation->iPopScaleFactor;
			break;
		}
	}
}*/


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_HereIsMicroSegValue(int inMessage)
{
	string	tmpString;
	string	segName;
	string	prodAttr;
	string	nameString;
	int		type;
	int		index;
	int		value;
	double		tmpFloat;
	int		tmpBool;
	int		featNum;

	int useNameString = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ParamUseNameString);
	if (useNameString)
	{
		string::StrCpy((string *) iCtr->Get_MsgParam(inMessage,kMarketSimMessageParam_ParamNameString),
				&nameString);
		index = DecodeNameString(&nameString);
	}
	else
	{
		index = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ParamIndex); 
	}
	string::StrCpy((string *) iCtr->Get_MsgParam(inMessage,kMarketSimMessageParam_ParamSegName),
				&segName);

	// hiho
	// iSubscriptionTimeHorizonLength = 1;
	// iSubscriptionTimeHorizonUnits = kRepurchaseFreq_Years;
	// iMaxNumSubscriptions = 4;

	if (!string::StrCmp(&segName, &iNameString))
		return;
	
	type = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ParamType);

	if (type == kParamType_string)
		string::StrCpy((string *) iCtr->Get_MsgParam(inMessage,kMarketSimMessageParam_ParamValue),
								&tmpString);
	else if (type == kParamType_model)
		value = iCtr->Get_MsgParam(inMessage,kMarketSimMessageParam_ParamValue);
	else if (type == kParamType_time)
		value = iCtr->Get_MsgParam(inMessage,kMarketSimMessageParam_ParamValue);
	else if (type == kParamType_bool)
		tmpBool = iCtr->Get_MsgParam(inMessage,kMarketSimMessageParam_ParamValue);
	else if (type == kParamType_numeric)
	{
		int tmpLong = iCtr->Get_MsgParam(inMessage,kMarketSimMessageParam_ParamValue);
		tmpFloat = MSG_LONG_TO_FLOAT(tmpLong);
	}

	//if ( (index == kMicroSegParam_attrCapacity) ||
	//				(index == kMicroSegParam_attrThreshold) ||
	//				(index == kMicroSegParam_attrUsageRate) ||
	//				(index == kMicroSegParam_attrUsagePeriod) ||
	//				(index == kMicroSegParam_attrNeedDecayRate))
	//	string::StrCpy((string *) iCtr->Get_MsgParam(inMessage,kMarketSimMessageParam_ParamProdAttr),
	//							&prodAttr);

	int i;

	switch(index)
	{

		// VdM 10/31/2003
		// need to Add these 3
		// display utilities move to end

	

		case kMicroSegParam_displayUtilityScalingFactor:
			Process_iDispUtilScalingFactor(tmpFloat);
			break;

		case kMicroSegParam_maxDisplayHitsPerTrip:
			Process_iMaxDisplayHits(tmpFloat);
			break;

		case kMicroSegParam_size:
			ProcessGroupSizeParameter((int) tmpFloat);
			break;

		case kMicroSegParam_GrowthRateValue:
			ProcessGrowthRateValueParam(tmpFloat);
			break;

		case kMicroSegParam_diversity:
			ProcessDiversityPercentParam(tmpFloat);
			break;

		case kMicroSegParam_priceSens:
			ProcessPriceSensitivityParam(tmpFloat);
			break;

		case kMicroSegParam_featureSens:
			ProcessPickinessParam(tmpFloat);
			break;

		case kMicroSegParam_messageSens:
			ProcessMessageSensitivityParam(tmpFloat);
			break;

		case kMicroSegParam_brandLoyalty:
			ProcessBrandLoyaltyParam(tmpFloat);
			break;


		// start parameters for the general choice model
		case kMicroSegParam_F1AwarenessScore:
		{
			// note that choice model is specified in the SS input before the individual
			// parameters of the general choice model.  If the choice model is not the
			// general one, then we don;t want to load in the settings of the GCM parameters.
			// Instead, we will derive the appropriate settings for the selected choice model
			if (choiceModel.iChoiceModel == kModel_General)	// OK
			{
				switch(value)
				{
					case kGeneralProdChoice_multiply:
						value = kGCMf1_Multiplication;
						break;
					case kGeneralProdChoice_exponential:
						value = kGCMf1_Exponentiation;
						break;
				}
				Process_iGCMf1_PersuasionScore(value);
			}
			break;
		}
		case kMicroSegParam_F2AwarenessValueComputation:
		{
			if (choiceModel.iChoiceModel == kModel_General)	// OK
			{
				switch(value)
				{
					case kGeneralProdChoice_shareofvoice:
						value = kGCMf2_ShareOfVoice;
						break;
					case kGeneralProdChoice_absolute:
						value = kGCMf2_Absolute;
						break;
					case kGeneralProdChoice_squareRoot:
						value = kGCMf2_squareRoot;
						break;
					case kGeneralProdChoice_base10log:
						value = kGCMf2_base10log;
						break;
				}
				Process_iGCMf2_PersuasionValComp(value);
			}
			break;
		}
		case kMicroSegParam_F3AwarenessContributionToOverallScore:
		{
			if (choiceModel.iChoiceModel == kModel_General)	// OK
			{
				switch(value)
				{
					case kGeneralProdChoice_Add:
						value = kGCMf3_Addition;
						break;
					case kGeneralProdChoice_multiply:
						value = kGCMf3_Multiplication;
						break;
				}
				Process_iGCMf3_PersuasionContrib(value);
			}
			break;
		}
		case kMicroSegParam_F4UtilityScore:
		{
			if (choiceModel.iChoiceModel == kModel_General)	// OK
			{
				switch(value)
				{
					case kGeneralProdChoice_multiply:
						value = kGCMf4_Multiplication;
						break;
					case kGeneralProdChoice_exponential:
						value = kGCMf4_Exponentiation;
						break;
				}
				Process_iGCMf4_UtilityScore(value);
			}
			break;
		}
		case kMicroSegParam_F5CombinationOfPartUtilities:
		{
			if (choiceModel.iChoiceModel == kModel_General)	// OK
			{
				switch(value)
				{
					case kGeneralProdChoice_scaledsumofproducts:
						value = kGCMf5_ScaledSumOfProducts;
						break;
					case kGeneralProdChoice_unscaledsumofproducts:
						value = kGCMf5_UnscaledSumOfProducts;
						break;
					case kGeneralProdChoice_log10unscaledsumofproducts:
						value = kGCMf5_Log10ScaledSumOfProducts;
				}
				Process_iGCMf5_CombPartUtilities(value);
			}
			break;
		}
		case kMicroSegParam_F6UtilityContributionToOverallScore:
		{
			if (choiceModel.iChoiceModel == kModel_General)	// OK
			{
				switch(value)
				{
					case kGeneralProdChoice_Add:
						value = kGCMf6_Addition;
						break;
					case kGeneralProdChoice_subtract:
						value = kGCMf6_Subtraction;
						break;
					case kGeneralProdChoice_multiply:
						value = kGCMf6_Multiplication;
						break;
					case kGeneralProdChoice_divide:
						value = kGCMf6_Division;
						break;
				}
				Process_iGCMf6_PriceContribution(value);
			}
			break;
		}
		case kMicroSegParam_F7PriceScore:
		{
			if (choiceModel.iChoiceModel == kModel_General)	// OK
			{
				switch(value)
				{
					case kGeneralProdChoice_multiply:
						value = kGCMf7_Multiplication;
						break;
					case kGeneralProdChoice_exponential:
						value = kGCMf7_Exponentiation;
						break;
				}
				Process_iGCMf7_PriceScore(value);
			}
			break;
		}
		case kMicroSegParam_F8PriceValue:
		{
			if (choiceModel.iChoiceModel == kModel_General)	//	OK
			{
				switch(value)
				{
					case kGeneralProdChoice_absolute:
						value = kGCMf8_AbsolutePrice;
						break;
					case kGeneralProdChoice_priceuse:
						value = kGCMf8_PricePerUse;
						break;
					case kGeneralProdChoice_relative:
						value = kGCMf8_RelativePrice;
						break;
					case kGeneralProdChoice_reference:
						value = kGCMf8_ReferencePrice;
						break;
				}
				Process_iGCMf8_PriceValueSource(value);
			}
			break;
		}
		case kMicroSegParam_ReferencePrice:
			Process_iGCM_referencePrice(tmpFloat);
			break;
		case kMicroSegParam_F9ChoiceProbability:
		{
			if (choiceModel.iChoiceModel == kModel_General)	// OK
			{
				switch(value)
				{
					case kGeneralProdChoice_shareofscore:
						value = kGCMf9_ShareOfScore;
						break;
					case kGeneralProdChoice_logit:
						value = kGCMf9_Logit;
						break;
					case kGeneralProdChoice_scaledlogit:
						value = kGCMf9_ScaledLogit;
						break;
				}
				Process_iGCMf9_ChoiceProbability(value);
			}
			break;
		}
		case kMicroSegParam_F10InertiaModel:
		{
			if (choiceModel.iChoiceModel == kModel_General)	// OK
			{
				switch(value)
				{
					case kGeneralProdChoice_SKU:
						value = kGCMf10_SameSKU;
						break;
					case kGeneralProdChoice_brand:
						value = kGCMf10_SameBrand;
						break;
				}
				Process_iGCMf10_InertiaModel(value);
			}
			break;
		}
		case kMicroSegParam_F11ErrorTerm:
		{
			if (choiceModel.iChoiceModel == kModel_General)	// OK
			{
				switch(value)
				{
					case kGeneralProdChoice_none:
						value = kGCMf11_None;
						break;
					case kGeneralProdChoice_uservalue:
						value = kGCMf11_UserValue;
						break;
					case kGeneralProdChoice_utility:
						value = kGCMf11_Utility;
						break;
					case kGeneralProdChoice_score:
						value = kGCMf11_Score;
						break;
				}
				Process_iGCMf11_ErrorTerm(value);
			}
			break;
		}
		case kMicroSegParam_ErrorTermUserValue:
			Process_iGCM_ErrorTermUserValue(tmpFloat);
			break;
		case kMicroSegParam_ErrorTermStdDev:
			Process_iGCM_ErrorTermSD(tmpFloat);
			break;
		// end parameters for the general choice model

		case kMicroSegParam_UnitsDesiredTrigger:
			Process_iUnitsDesiredTrigger(tmpFloat);
			break;

		case kMicroSegParam_AverageMaxUnits:
			Process_iAverageMaxUnits(tmpFloat);
			break;

		case kMicroSegParam_channelLoyalty:
			ProcessChannelLoyaltyParam(tmpFloat);
			break;

		case kMicroSegParam_repPeriod:
			ProcessRepurchaseFrequencyDurationParam(tmpFloat, false);
			break;

		case kMicroSegParam_repPeriodVariation:
			ProcessNBDspreadParam(tmpFloat, false);
			break;

		case kSetupItem_NBDstddevOptedittext:
			ProcessNBDspreadParam(tmpFloat, false);
			break;		
		
		case kMicroSegParam_initBuyers:
			{
				int	newVal = tmpFloat;
				ProcessInitialPurchasesParameter(newVal);
			}
			break;

		case kMicroSegParam_initBuyingPeriod:
			ProcessInitialPurchasesIntervalParam((int) tmpFloat);
			break;

		case kMicroSegParam_budget:
			ProcessDollarsPerBudgetPeriodParam(tmpFloat);
			break;

		case kMicroSegParam_initSave:
			ProcessStartingCashParam(tmpFloat);
			break;

		case kMicroSegParam_bassInternal:
			ProcessAdoptionInternalValueParam(tmpFloat);
			break;

		case kMicroSegParam_bassExternal:
			ProcessAdoptionExternalValueParam(tmpFloat);
			break;

		case kSetupItem_PreUsePersuasionDecayOptedittext:
			ProcessPersuasionDecayPreUse(tmpFloat);
			break;

		case kSetupItem_PostUsePersuasionDecayOptedittext:
			ProcessPersuasionDecayPostUse(tmpFloat);
			break;

		case kSetupItem_PostPersuasionCredOptedittext:
			ProcessCredWeightWOMpostUseParam(tmpFloat);
			break;

		case kMicroSegParam_A:
			ProcessGammaAParam(tmpFloat);
			break;

		case kMicroSegParam_K:
			ProcessGammaKParam(tmpFloat);
			break;

		case kMicroSegParam_peopleLikeMe:
			ProcessWithinAdoptComNumRelationshipsParam(tmpFloat);
			break;

		case kMicroSegParam_likeMeCommsPost:
			Process_iProbTalkClosePostUse(tmpFloat);
			break;

		case kMicroSegParam_unlikeMeCommsPost:
			Process_iProbTalkDistantPostUse(tmpFloat);
			break;

		case kMicroSegParam_AwarenessWtPersonMessage:
			Process_iAwarenessWeightWOM(tmpFloat);
			break;

		case kMicroSegParam_PersuasionCredPre:
			Process_iCredWeightWOMpreUse(tmpFloat);
			break;

		case kMicroSegParam_PersuasionCredPost:
			ProcessCredWeightWOMpostUseParam(tmpFloat);
			break;

		//case kMicroSegParam_CouponFrontLoading:
		//	Process_iPctCouponsRedeemedWeek1(tmpFloat);
		//	break;

		//case kMicroSegParam_PersuasionChangeProdPurchase:
		//	Process_(tmpFloat); = iNumCloseContacts;
		//	break;


		case kMicroSegParam_likeMeComms:
			ProcessWithinAdoptComFrequencParam(tmpFloat);
			break;

		case kMicroSegParam_peopleUnlikeMe:
			ProcessOutsideAdoptComNumRelationshipsParam(tmpFloat);
			break;

		case kMicroSegParam_unlikeMeComms:
			ProcessOutsideAdoptComFrequencyParam(tmpFloat);
			break;

		case kMicroSegParam_messageTrigger:
			ProcessMinNumMessagesToTriggerPurchaseParam(tmpFloat);
			break;

		case kMicroSegParam_avgMessageLifePreUse:
			ProcessAwarenessDecayPreUseParam(tmpFloat);
			break;

		case kMicroSegParam_avgMessageLifePostUse:
			ProcessAwarenessDecayPostUseParam(tmpFloat);
			break;

		case kMicroSegParam_PersuasionDecayPreUse:
			ProcessPersuasionDecayPreUse(tmpFloat);
			break;

		case kMicroSegParam_PersuasionDecayPostUse:
			ProcessPersuasionDecayPostUse(tmpFloat);
			break;

		//case kMicroSegParam_triggerIncrement:
		//	ProcessDaysBetweenShoppingTripsParam(tmpFloat);
		//	break;

		case kMicroSegParam_color:
			ProcessColorParameter(value);
			break;
				
		// y , n
		case kMicroSegParam_repurchase:
			ProcessRepurchaseParam(tmpBool, false);
			break;

		case kMicroSegParam_seed:
			//iSeedMarketWithRepurchasers = tmpBool;
			break;

		case kMicroSegParam_useBudget:
			ProcessUseBudgetParam(tmpBool, false);
			break;

		case kMicroSegParam_save:	
			ProcessSaveUnspentMoneyParam(tmpBool, false);
			break;

		//case kMicroSegParam_forgetMessages:
		//	ProcessForgetMessagesParam(tmpBool, false);
		//	break;

		case kMicroSegParam_compress:
			ProcessCompressPopulation(tmpBool);
			break;

		case kMicroSegParam_useLocal:
			ProcessUseLocal(tmpBool);
			break;

		case kMicroSegParam_showCurrentShare:
			iDisplayCurrentMarketShare = tmpBool;
			break;

		case kMicroSegParam_showCumulativeShare:
			iDisplayCumulativeMarketShare = tmpBool;
			break;

		// d, w, m , y
		case kMicroSegParam_repTimescale:
			ProcessRepurchaseFrequencyUnitsParam((int) value);
			break;

		case kMicroSegParam_budgetPeriod:
			ProcessBudgetPeriodParam(value);
			break;

		// b, c, e
		case kMicroSegParam_purchaseModel:
			ProcessPurchaseModelParam(value, false);
			break;

		case kMicroSegParam_repModel:
			ProcessRepurchaseModel(value, false);
			break;

		case kMicroSegParam_AwarenessModelType:	// OK
			ProcessAwarenessModelType(value, false);
			break;

		case kMicroSegParam_SocNetModelType:
			ProcessSocNetModelType(value, false);
			break;

		case kMicroSegParam_GrowthRatePeoplePercent:
			ProcessGrowthRatePeoplePercentParam(value);
			break;

		case kMicroSegParam_GrowthRateMonthYear:
			ProcessGrowthRateMonthYearParam(value);
			break;

		// string
		//case kMicroSegParam_productAttr:
			// needs to be done per attribute
			// name will be supplied with all parameters
		//	break;

		case kMicroSegParam_RebuildPopulationNow:
			// using this to read in the database
			// should rename so it is more explicite
			// SS
			// 3//25/2006
			readFromDB();

			break;

		default:

			if (index >= kMicroSegParam_LastCall)
				Process_iDisplayUtility(tmpFloat, index - kMicroSegParam_LastCall);
			break;

	}
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_HereIsASpecialEvent(int inMessage)
{
	string tmpProduct;
	string tmpChannel;
	string tmpName;
	int i;
	int empty = -1;
	int found = false;
	int type;
	
	string::StrCpy((string *) iCtr->Get_MsgParam(inMessage,kMarketSimMessageParam_ProductName),&tmpProduct);
	string::StrCpy((string *) iCtr->Get_MsgParam(inMessage,kMarketSimMessageParam_ChannelName),&tmpChannel);
	string::StrCpy((string *) iCtr->Get_MsgParam(inMessage,kMarketSimMessageParam_Name),&tmpName);
	type = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_TaskType);

	for (i=0; i<iSpecialEvents.size(); i++)
	{
		if (
			(string::StrCmpNoCase(&iSpecialEvents[i].iName,&tmpName)) &&
			(string::StrCmpNoCase(&iSpecialEvents[i].iProductName, &tmpProduct)) &&
			(string::StrCmpNoCase(&iSpecialEvents[i].iChannelName, &tmpChannel))
			)
		{
			found = true;
			break;
		}
		//else if (string::IsEmpty(&iSpecialEvents[i].iName))
		//	empty = i;
	}

	if ( found && (type == ktimelineTaskType_Delete) )
	{
		iSpecialEvents.RemoveAt(i);
		iSpecialEvents.FreeExtra();
	}
	else if (found && (type == ktimelineTaskType_Change) )
	{
		string::StrCpy((string *) iCtr->Get_MsgParam(inMessage,kMarketSimMessageParam_NewName),&tmpName);
		string::StrCpy(&tmpName, &iSpecialEvents[i].iName);
	} 
	else if (!found && (type == ktimelineTaskType_None) && !tmpName.IsEmpty())
	{
		SpecialEventRecord tempEvent;
		InitializeSpecialEvent(&tempEvent);
		string::StrCpy(&tmpName, &tempEvent.iName);
		string::StrCpy(&tmpProduct, &tempEvent.iProductName);
		string::StrCpy(&tmpChannel, &tempEvent.iChannelName);
		iSpecialEvents.push_back(tempEvent);
	}
}
// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_HereIsMarketUtilityInfo(int inMessage)
{
	int mrktutil = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_MarketUtilityPointer);
	MarketUtility* marketUtility = (MarketUtility*) mrktutil;

	string	allText;
	SINumToString(-1, &allText);

	string	segmentName;
	SINumToString(marketUtility->getSegmentID(), &segmentName);

	if( string::StrCmp(&segmentName, &iNameString) || string::StrCmpNoCase(&segmentName, &allText))
	{

		string	prodName;
		SINumToString(marketUtility->getProductID(), &prodName);

		string	channelName;
		SINumToString(marketUtility->getChannelID(), &channelName);
	
		int allProducts = string::StrCmpNoCase(&prodName, &allText);
		int allChannels = string::StrCmpNoCase(&channelName, &allText);	

		// Create Display
		// in the future we will do this somewhere else
		// but we are getting close to deadline... SSN
		Display display;

		// scale to probability
		display.probOfDisplay = 0.01 * marketUtility->getPercentDist();
		display.awareness = marketUtility->getAwareness();
		display.persuasion = marketUtility->getPersuaion();
		display.utility = marketUtility->getUtility();
		display.NIMO_AutoDisplay = false;
		display.onDisplay = false;

		int pcn;
		int pn;
		int cn;
		ProductTree::LeafNodeList* prodList;
		if(allProducts)
		{
			prodList = (ProductTree::LeafNodeList*)ProductTree::theTree->LeafNodes();
		}
		else
		{
			pn = GetProdNumFromProdName(&prodName);
			prodList = new ProductTree::LeafNodeList(pn);
		}

		

		for( ProductTree::Iter iter = prodList->begin(); iter != prodList->end(); ++iter)
		{
			pn = *iter;
			if(allChannels)
			{
				for(cn = 0; cn < iChannelsAvailable.size(); cn++)
				{
					pcn = GetProdChanNumFromProdAndChannelNum(pn, cn);
					if(pcn != -1)
					{
						iProductsAvailable[pcn].displayList.push_back(display);
						iProductsAvailable[pcn].iNoDispSpecifiedYet = false;
					}
				}
			}
			else
			{
				cn = GetChannelNumFromChannelName(&channelName);
				pcn = GetProdChanNumFromProdAndChannelNum(pn, cn);
				if(pcn != -1)
				{
					iProductsAvailable[pcn].displayList.push_back(display);
					iProductsAvailable[pcn].iNoDispSpecifiedYet = false;
				}
			}
		}

		if(!allProducts)
		{
			delete prodList;
		}

	}
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_TaskEvent(int inMessage)
{
	int msgType = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_Type);
	int i;

	switch(msgType)
	{
		// receiving an advertisement during simulation
	case ktimelineTaskType_Comms:
		{
			string taskName;
			int	end = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_End);
			int	start = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_Start);
			string::StrCpy((string *) iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_Name),&taskName);

			int		adIndex;
			int		foundIt = false;
			int		prodChanNum;
			int		stratType = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_StratType);
			int		daysRun = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_DaysRun);

			int locAdvertType = TranslateStratType(stratType);

			// find the name in the ad list
			/*for (adIndex =0; adIndex < iAdSchedule.size(); adIndex++)
			{
				if ((iAdSchedule[adIndex].iAdvertType == locAdvertType) && string::StrCmp(&taskName, &(iAdSchedule[adIndex].name)))
					//if (string::StrCmp(&taskName, &(iAdSchedule[adIndex].name)))
				{
					foundIt = true;
					break;
				}
			}*/
			if(iAdMap.find(taskName) != iAdMap.end())
			{
				foundIt = true;
				adIndex = iAdMap[taskName];
			}

			if (foundIt)
			{
				// see if this is the Ad's last day
				if (end)
				{
					// have already run the ad the specified number of days
					iAdSchedule[adIndex].thisIsMyLastTick = true;
					break;
				}

				iAdSchedule[adIndex].daysRun = daysRun;

				// KMK 3/11/05
				int prodNum = iAdSchedule[adIndex].advertProdNum;

				// SSN 4/10/2006
				// Added Additional test here to make sim run as previously
				if (prodNum != -1 && 
					iAdSchedule[adIndex].iAdvertType != kAdvertType_Distribution &&
					iAdSchedule[adIndex].iAdvertType != kAdvertType_Event_units_purchased &&
					iAdSchedule[adIndex].iAdvertType != kAdvertType_Event_shopping_trips &&
					iAdSchedule[adIndex].iAdvertType != kAdvertType_Event_price_disutility &&
					iAdSchedule[adIndex].iAdvertType != kAdvertType_Event_price_disutility)
				{
					SetMarketed(prodNum, true);
				}

				switch (iAdSchedule[adIndex].iAdvertType)
				{
				case kAdvertType_Event_units_purchased:
					ProcessEventUnitsPurchasedAdType(adIndex);
					break;
				case kAdvertType_Event_shopping_trips:
					ProcessEventShoppingTripsAdType(adIndex);
					break;
				case kAdvertType_Event_price_disutility:
					ProcessEventPriceDisutility(adIndex);
					break;
				case kAdvertType_MassMedia:
					ProcessPromotionAdType(adIndex, start);
					break;
				case kAdvertType_Coupon:	// BOGO handled
					ProcessPromotionAdType(adIndex, start);
					break;
				case kAdvertType_BOGO:
					ProcessPromotionAdType(adIndex, start);
					break;
				case kAdvertType_Sample:
					ProcessPromotionAdType(adIndex, start);
					break;
				case kAdvertType_Distribution:
					ProcessDistributionAdType(adIndex, start);
					break;
				//case kAdvertType_Display1:
				//	ProcessDisplayAdType(adIndex, start, 0);
				//	break;
				//case kAdvertType_Display2:
				//	ProcessDisplayAdType(adIndex, start, 1);
				//	break;
				//case kAdvertType_Display3:
				//	ProcessDisplayAdType(adIndex, start, 2);
				//	break;
				//case kAdvertType_Display4:
				//	ProcessDisplayAdType(adIndex, start, 3);
				//	break;
				//case kAdvertType_Display5:
				//	ProcessDisplayAdType(adIndex, start, 4);
				//	break;
				//case kAdvertType_Display:
				//	ProcessDisplayAdType(adIndex, start, 0);
				//	break;
				}
			} // end check for found it
			break;

		}

	case ktimelineTaskType_Price:			
		{ // Now Handling Price Here
			// Add channel-product pair to my list of available products
			// make sure I don't already hold the pair
			string *prodName = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ProductName));
			string *chanName = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ChannelName));

			// price type
			long priceType = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_StratParam1);

			// price
			long tempLong = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_StratParam2);
			double newPrice = MSG_LONG_TO_FLOAT(tempLong);

			// distribution
			tempLong = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_StratParam3);
			double skusAtPrice = MSG_LONG_TO_FLOAT(tempLong);
			skusAtPrice *= .01;	// convert from percent to probability

			long deactivatePrice = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_StratParam4);

			if (!deactivatePrice)
			{
				ProcessPriceData(prodName, chanName, priceType, skusAtPrice, newPrice);
			}

			return;
		}

	case ktimelineTaskType_None:			// get the compnay name
	case ktimelineTaskType_Dev:
	// price now handled here SSN 3/22/2006
	case ktimelineTaskType_Promo:
	case ktimelineTaskType_Delete:
	case ktimelineTaskType_Change:
	case ktimelineTaskType_SpecialEvent:	// this is currently unused-- special events come in as ads
		return;
	case ktimelineTaskType_Display:
		{ // prcess display
			// tells us what kind of display this is
			// displays are by product and channel
			string allText;
			string::StrCpy((iCtr->iCommonStringPtrs)[kStr_Allm], &allText);
		

			string* prodName = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ProductName));
			string* channelName = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ChannelName));

			double		numImpressions, aware, persuasion;
			int		tempLong;

			tempLong = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_StratParam1);
			numImpressions = MSG_LONG_TO_FLOAT(tempLong);

			tempLong = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_StratParam2);
			aware = MSG_LONG_TO_FLOAT(tempLong);
			tempLong = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_StratParam3);
			persuasion = MSG_LONG_TO_FLOAT(tempLong);

			int displayID = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_StratParam4);

			int allProducts = string::StrCmpNoCase(prodName, &allText);
			int allChannels = string::StrCmpNoCase(channelName, &allText);

			// Create Display
			// in the future we will do this somewhere else
			// but we are getting close to deadline... SSN
			Display display;

			// scale to probability
			display.probOfDisplay = numImpressions/100;
			display.awareness = aware;
			display.persuasion = persuasion;
			display.utility = this->iDisplayUtility[displayID] * this->iDispUtilScalingFactor;

			if (displayID == 0)
			{
				// turn on special NIMO Display Pricing
				display.NIMO_AutoDisplay = true;

				// this is the only type that is an actual display and subject to display hits
				display.onDisplay = true;
			}
			else
			{
				display.NIMO_AutoDisplay = false;
				display.onDisplay = false;
			}

			int pcn;
			int pn;
			int cn;
			ProductTree::LeafNodeList* prodList;
			if(allProducts)
			{
				prodList = (ProductTree::LeafNodeList*)ProductTree::theTree->LeafNodes();
			}
			else
			{
				pn = GetProdNumFromProdName(prodName);
				prodList = new ProductTree::LeafNodeList(pn);
			}

			

			for( ProductTree::Iter iter = prodList->begin(); iter != prodList->end(); ++iter)
			{
				pn = *iter;
				if(allChannels)
				{
					for(cn = 0; cn < iChannelsAvailable.size(); cn++)
					{
						pcn = GetProdChanNumFromProdAndChannelNum(pn, cn);
						if(pcn != -1)
						{
							iProductsAvailable[pcn].displayList.push_back(display);
							iProductsAvailable[pcn].iNoDispSpecifiedYet = false;
							iProductsAvailable[pcn].iTotalDisplayChanceThisTick += numImpressions;
						}
					}
				}
				else
				{
					cn = GetChannelNumFromChannelName(channelName);
					pcn = GetProdChanNumFromProdAndChannelNum(pn, cn);
					if(pcn != -1)
					{
						iProductsAvailable[pcn].displayList.push_back(display);
						iProductsAvailable[pcn].iNoDispSpecifiedYet = false;
						iProductsAvailable[pcn].iTotalDisplayChanceThisTick += numImpressions;
					}
				}
			}

			if(!allProducts)
			{
				delete prodList;
			}
			break;
		} // end display processing
	} // end switch
}

void CMicroSegment::SetMarketed(int prodNum, int isMarketed)
{
	iProducts[prodNum].iMarketed = isMarketed;

	if(!CMicroSegment::prodTree->IsLeaf(prodNum))
	{
		ProductTree::List* leafs = prodTree->ChildNodes(prodNum);

			if(leafs)
			{
				for( ProductTree::Iter iter = leafs->begin(); iter != leafs->end(); ++iter)
				{
					int leafPn = *iter;
					SetMarketed(leafPn, isMarketed);
				}
			}
	}
}

// ------------------------------------------------------------------------------
// 
// ------------------------------------------------------------------------------
void CMicroSegment::ProcessPromotionAdType(int adIndex, int start)
{
	double	numImpressionsToday = 0.0;
	double	totalImpressions;
	int	tempLong;

	int	pn;
	int	chan;
	int	pcn;

	// get the product/channel number
	// TODO need to fix how we identify products and channels for advert SSN 1/9/2006
	GetProdAndChanForAdvert(adIndex, &pn, &chan, &pcn, NULL);

	GetImpressionsFromAd(adIndex, &totalImpressions, &numImpressionsToday);

	if ((pcn >= 0) && (numImpressionsToday > 0))
	{
		// tell the right number of people about the product
		iProductsAvailable[pcn].iProductsAdvertisedThisTick = true;
		double	persuasion = iAdSchedule[adIndex].iPersuasion;
		double	awareness = iAdSchedule[adIndex].iAwareness;
		
		// if we don't have enough impressions to distribute over time
		if (numImpressionsToday < 1.0)
		{

			// TODO: We should not put up a dialogue but put a warning into the log
			// SSN 1/9/2006
			// string warnStr;
			// string::CopyFromCString("ended up with less than 1 impression", &warnStr);
			
			// do them all on one day (the first day)
			if (start) 
			{
				numImpressionsToday = totalImpressions; 

				// do at least one, in the event are putting a small number of 
				// messages into a compressed population
				if (numImpressionsToday < 1.0)
					numImpressionsToday = 1.0;
			}
			else
			{
				numImpressionsToday = 0.0;
			}
		}

		switch (iAdSchedule[adIndex].iAdvertType)
		{
		case kAdvertType_Coupon:	// BOGO handled
			RedeemCoupons(numImpressionsToday, adIndex, persuasion);
			break;
		case kAdvertType_BOGO:
			RedeemCoupons(numImpressionsToday, adIndex, persuasion);
			break;
		case kAdvertType_Sample:
			{
				double	sampleSize = iAdSchedule[adIndex].costPerImpression;
				double	persuasion = iAdSchedule[adIndex].iPersuasion;
				DistributeSamples(numImpressionsToday, pcn, sampleSize, persuasion);
				break;
			}
		case kAdvertType_MassMedia:
			BroadcastTheAd(numImpressionsToday, pcn, iAdSchedule[adIndex].messageStrength, persuasion, awareness);
			break;
		}

		iAdSchedule[adIndex].ranThisTick = true;
	}  // end check for positive number of impressions
}


// ------------------------------------------------------------------------------
// Respond to an event that modifies units purchased
// ------------------------------------------------------------------------------
void CMicroSegment::ProcessEventUnitsPurchasedAdType(int adIndex)
{
	int	pcn;

	// see what kind of demand modifier this is
	switch(iAdSchedule[adIndex].scope)
	{
		case kAdvertScope_AllProdsAllChannels:
			iCatQuantityPurchasedModifier += iAdSchedule[adIndex].numImpressions;
			break;

		case kAdvertScope_AllProdsOneChannel:
		{
			// for task-based buying, the channel is ignored
			if (iAdSchedule[adIndex].iTreatProdNameAsTaskName)
			{
				// because channel is ignored for TBB events, this means that 
				// the demand for all tasks is being specified
				iCatQuantityPurchasedModifier += iAdSchedule[adIndex].numImpressions;	// KMK 6/23/05 I think we want a separate modifier for task rates...
			}
			else // not TBB
			{
				// set the demand mod for all products in the specified channel
				for (pcn = 0; pcn < iProductsAvailable.size(); pcn++)
				{
					if (string::StrCmp(&((iProductsAvailable[pcn]).iChanName), 
						&(iAdSchedule[adIndex].iChannelName)))
					{
						iProductsAvailable[pcn].iNumPurchasedDemandMod += iAdSchedule[adIndex].numImpressions;
						iIndividualDemandMods = true;
						iChannelDemandMods = true;
					}
				}
			}
			break;
		}

		case kAdvertScope_OneProdAllChannels:
		{
			if (iAdSchedule[adIndex].iTreatProdNameAsTaskName)
			{
				/*int	cTaskNum;
				for (cTaskNum = 0; cTaskNum < iTaskRates.size(); cTaskNum++)
				{
					if (string::StrCmp(&((iTaskRates[cTaskNum]).iTaskName), 
						&(iAdSchedule[adIndex].productName)))
					{
						iTaskRates[cTaskNum].iDemandModification += iAdSchedule[adIndex].numImpressions;
						iIndividualDemandMods = true;
					}
				) */

			}
			else	// not TBB
			{
				// set the demandmod for the product in all channels
				for (pcn = 0; pcn < iProductsAvailable.size(); pcn++)
				{
					if (string::StrCmp(&((iProductsAvailable[pcn]).iProdName), 
						&(iAdSchedule[adIndex].productName)))
					{
						iProductsAvailable[pcn].iNumPurchasedDemandMod += iAdSchedule[adIndex].numImpressions;
						iIndividualDemandMods = true;
					}
				}
			}
			break;
		}

		case kAdvertScope_OneProdOneChannel:
		{
			// task event modification ignores channels
			if (iAdSchedule[adIndex].iTreatProdNameAsTaskName)
			{
				//int	cTaskNum;
				//for (cTaskNum = 0; cTaskNum < iTaskRates.size(); cTaskNum++)
				//{
				//	if (string::StrCmp(&((iTaskRates[cTaskNum]).iTaskName), 
				//		&(iAdSchedule[adIndex].productName)))
				//	{
				//		iTaskRates[cTaskNum].iDemandModification += 
				//			iAdSchedule[adIndex].numImpressions;
				//		iIndividualDemandMods = true;
				//		iChannelDemandMods = true;
				//	}
				//}
			}
			else	// not TBB
			{
				// set demand mod for the specific product/channel pair
				pcn = iAdSchedule[adIndex].advertProdChanNum;
				iProductsAvailable[pcn].iNumPurchasedDemandMod += iAdSchedule[adIndex].numImpressions;
				iIndividualDemandMods = true;
				iChannelDemandMods = true;
			}
			break;
		}
	}	// end switch
}


// ------------------------------------------------------------------------------
// KMK 06/27/05 Respond to an event that modifies this segment's price disutility
// This will maintain the defined value permanently, or until another price disutility value comes in
// ------------------------------------------------------------------------------
void CMicroSegment::ProcessEventPriceDisutility(int adIndex)
{
	double	newPriceDisutility = iAdSchedule[adIndex].numImpressions;
	// set price disutility in the normal way
	ProcessPriceSensitivityParam(newPriceDisutility);
}

// ------------------------------------------------------------------------------
// Respond to an event that modifies shopping trip interval
// ------------------------------------------------------------------------------
void CMicroSegment::ProcessEventShoppingTripsAdType(int adIndex)
{
	int	pcn;

	// see what kind of demand modifier this is
	switch(iAdSchedule[adIndex].scope)
	{
		case kAdvertScope_AllProdsAllChannels:
			iCatShoppingTripFrequencyModifier += iAdSchedule[adIndex].numImpressions;
			break;

		case kAdvertScope_AllProdsOneChannel:
		case kAdvertScope_OneProdAllChannels:
		case kAdvertScope_OneProdOneChannel:
			WriteToSessionLog(kWarn_TripModificationError, -1, -1, 0);
			break;
	}	// end switch
}


// ------------------------------------------------------------------------------
// 
// ------------------------------------------------------------------------------
void CMicroSegment::ProcessDistributionAdType(int adIndex, int start)
{
	int	pcn;
	
	// reset to 100% distribution if there are no more distribution tasks
	//if (iAdSchedule[adIndex].daysRun == iAdSchedule[adIndex].daysDuration)
	//	iAdSchedule[adIndex].numImpressions = 1.0;

	// clear out previous distribution pointers in iProductsAvailable
	// this is not going to work here, because it clears out all of them each time one os set
	// we should instead try to do this at startsimstep, I think....
	//for (*prodChanNum = 0; *prodChanNum < iProductsAvailable.size(); (*prodChanNum)++)
	//	iProductsAvailable[*prodChanNum].iMyCurrDistNum = -1;

	// see what kind of demand modifier this is
	switch(iAdSchedule[adIndex].scope)
	{
		case kAdvertScope_AllProdsAllChannels:
		{
			// assign the given distribution parameter to all products & channels
			for (pcn = 0; pcn < iProductsAvailable.size(); pcn++)
			{
				iProductsAvailable[pcn].iPreUseDistributionPercent = iAdSchedule[adIndex].numImpressions;
				iProductsAvailable[pcn].iPostUseDistributionPercent = iAdSchedule[adIndex].costPerImpression;
				iProductsAvailable[pcn].iMyCurrDistNum = adIndex;
				iProductsAvailable[pcn].iNoDistSpecifiedYet = false;
			}
			break;
		}

		case kAdvertScope_AllProdsOneChannel:
		{
			// assign the given distribution parameter to all products in the specified channel
			for (pcn = 0; pcn < iProductsAvailable.size(); pcn++)
			{
				if (string::StrCmp(&((iProductsAvailable[pcn]).iChanName), &(iAdSchedule[adIndex].iChannelName)))
				{
					iProductsAvailable[pcn].iPreUseDistributionPercent = iAdSchedule[adIndex].numImpressions;
					iProductsAvailable[pcn].iPostUseDistributionPercent = iAdSchedule[adIndex].costPerImpression;
					iProductsAvailable[pcn].iMyCurrDistNum = adIndex;
					iProductsAvailable[pcn].iNoDistSpecifiedYet = false;
				}
			}
			break;
		}

		case kAdvertScope_OneProdAllChannels:
		{
			// assign the given distribution parameter to the specified product in all channels
			int pn = GetProdNumFromProdName(&(iAdSchedule[adIndex].productName));
			ProductTree::LeafNodeList prodList(pn);
			for(ProductTree::Iter iter = prodList.begin(); iter != prodList.end(); ++iter)
			{
				pn = *iter;
				for (int cn = 0; cn < iChannelsAvailable.size(); cn++)
				{
					pcn = GetProdChanNumFromProdAndChannelNum(pn, cn);
					if (pcn != -1)
					{
						iProductsAvailable[pcn].iPreUseDistributionPercent = iAdSchedule[adIndex].numImpressions;
						iProductsAvailable[pcn].iPostUseDistributionPercent = iAdSchedule[adIndex].costPerImpression;
						iProductsAvailable[pcn].iMyCurrDistNum = adIndex;
						iProductsAvailable[pcn].iNoDistSpecifiedYet = false;
					}
				}
			}
			break;
		}

		case kAdvertScope_OneProdOneChannel:
			{
				// assign the given distribution parameter to the specific product/channel pair
				pcn = iAdSchedule[adIndex].advertProdChanNum;
				int pn = GetProdNumFromProdChanNum(pcn);
				int cn = FastGetChannelNumFromProdChannelNum(pcn);
				ProductTree::LeafNodeList prodList(pn);
				for(ProductTree::Iter iter = prodList.begin(); iter != prodList.end(); ++iter)
				{
					pcn = GetProdChanNumFromProdAndChannelNum(pn, cn);
					if (pcn != -1)
					{
						iProductsAvailable[pcn].iPreUseDistributionPercent = iAdSchedule[adIndex].numImpressions;
						iProductsAvailable[pcn].iPostUseDistributionPercent = iAdSchedule[adIndex].costPerImpression;
						iProductsAvailable[pcn].iMyCurrDistNum = adIndex;
						iProductsAvailable[pcn].iNoDistSpecifiedYet = false;
					}
				}
				break;
			}
	}  // end switch
}


// ------------------------------------------------------------------------------
// 
// ------------------------------------------------------------------------------
//void CMicroSegment::ProcessDisplayAdType(int adIndex, int start, int displayType)
//{
//	int	pcn;
//
//	if (start)
//		iAdSchedule[adIndex].daysRun = 1;
//	else
//		iAdSchedule[adIndex].daysRun += 1;
//	
//	// see what kind of demand modifier this is
//	switch(iAdSchedule[adIndex].scope)
//	{
//		case kAdvertScope_AllProdsAllChannels:
//		{
//			// assign the given distribution parameter to all products & channels
//			for (pcn = 0; pcn < iProductsAvailable.size(); pcn++)
//			{
//				iProductsAvailable[pcn].iMyCurrDispNum[displayType] = adIndex;
//				iProductsAvailable[pcn].iNoDispSpecifiedYet = false;
//				iProductsAvailable[pcn].iTotalDisplayChanceThisTick += iAdSchedule[adIndex].numImpressions;
//			}
//			break;
//		}
//
//		case kAdvertScope_AllProdsOneChannel:
//		{
//			// assign the given distribution parameter to all products in the specified channel
//			for (pcn = 0; pcn < iProductsAvailable.size(); pcn++)
//			{
//				if (string::StrCmp(&((iProductsAvailable[pcn]).iChanName), &(iAdSchedule[adIndex].iChannelName)))
//				{
//					iProductsAvailable[pcn].iMyCurrDispNum[displayType] = adIndex;
//					iProductsAvailable[pcn].iNoDispSpecifiedYet = false;
//					iProductsAvailable[pcn].iTotalDisplayChanceThisTick += iAdSchedule[adIndex].numImpressions;
//				}
//			}
//			break;
//		}
//
//		case kAdvertScope_OneProdAllChannels:
//		{
//			// assign the given distribution parameter to the specified product in all channels
//			for (pcn = 0; pcn < iProductsAvailable.size(); pcn++)
//			{
//				if (string::StrCmp(&((iProductsAvailable[pcn]).iProdName), &(iAdSchedule[adIndex].productName)))
//				{
//					iProductsAvailable[pcn].iMyCurrDispNum[displayType] = adIndex;
//					iProductsAvailable[pcn].iNoDispSpecifiedYet = false;
//					iProductsAvailable[pcn].iTotalDisplayChanceThisTick += iAdSchedule[adIndex].numImpressions;
//				}
//			}
//			break;
//		}
//
//		case kAdvertScope_OneProdOneChannel:
//		{
//			// assign the given distribution parameter to the specific product/channel pair
//			pcn = iAdSchedule[adIndex].advertProdChanNum;
//			iProductsAvailable[pcn].iMyCurrDispNum[displayType] = adIndex;
//			iProductsAvailable[pcn].iTotalDisplayChanceThisTick += iAdSchedule[adIndex].numImpressions;
//			iProductsAvailable[pcn].iNoDispSpecifiedYet = false;
//			break;
//		}
//	}  // end switch
//}


// ------------------------------------------------------------------------------
// 
// ------------------------------------------------------------------------------
void CMicroSegment::CheckForUndefinedAdverts(void)
{
	int	adIndex;
	int	pcn;
	int	pn;
	int	chan;

	for (adIndex = 0; adIndex < iAdSchedule.size(); adIndex++)
	{
		GetProdAndChanForAdvert(adIndex, &pn, &chan, &pcn, NULL);
		if (pcn < 0)
		{
			int	pcN;
			int	newPCN;
			int	newProdNum;
			switch (iAdSchedule[adIndex].scope)
			{
				case kAdvertScope_AllProdsAllChannels:
					// we will ignore this case right now
					// the right hting to do would be to generate ads for ALL products
					break;
				case kAdvertScope_AllProdsOneChannel:
					// we will ignore this case right now
					// the right hting to do would be to generate ads for ALL products
					break;
				case kAdvertScope_OneProdAllChannels:
					// to make this work, we will assume it is the same as 
					// advertising the product through any channel is ti available...
					newPCN = -1;
					if (pn == -1)
					{
						for (pcN = 0; pcN < iProductsAvailable.size(); pcN++)
						{
							if (string::StrCmp(&((iProductsAvailable[pcN]).iProdName), 
								&(iAdSchedule[adIndex].productName)))
							{
								newPCN = pcN;
								break;
							}
						}
					}
					if (newPCN >= 0)
					{
							newProdNum = GetProdNumFromProdChanNum(newPCN);
							if (newProdNum >= 0)
								iAdSchedule[adIndex].advertProdNum = newProdNum;
					}
					break;
			}	// end switch 
		}  // end check for bad pcn
	}	// next ad
}


// ------------------------------------------------------------------------------
// We will scale the number of impressions by the ad strength
// ------------------------------------------------------------------------------
#define kMinNumberImpresionsPerDay 2
void CMicroSegment::GetImpressionsFromAd(int adIndex, double *totalImpressions, double *numImpressionsToday)
{
	if (iAdSchedule[adIndex].daysDuration > 0)
	{
		switch (iAdSchedule[adIndex].iAdvertType)
		{
			case kAdvertType_Coupon:	// BOGO Handled
			case kAdvertType_BOGO:
			case kAdvertType_Sample:
			{
				// impressions are computed differently for coupons
				// because we just look at redemption rate right now
				*totalImpressions = iAdSchedule[adIndex].numImpressions * (double) iPopulation->iConsumerList.size();
				// adjust to coupon penetration
				*totalImpressions *= iAdSchedule[adIndex].iRate;;
				// now we will account for the fact that coupons are not redemmed uniformly
				// across the period.   Rather, more are redeemed earlier than later.
				double	scaleFactor = (iAdSchedule[adIndex].daysDuration - iAdSchedule[adIndex].daysRun);
				/*double scaleFactor = 0;
				if(iAdSchedule[adIndex].daysRun == 1)
				{
					scaleFactor = 1;
				}*/
				if (scaleFactor < 0)
				{
					// TODO remove dialgoue and Add to log SSN 1/9/2006
					// string warnStr, w2str, w3str, w4str;
					// string::CopyFromCString("negative coupon scale factor, days duration =", &warnStr);
					//SINumToString(iAdSchedule[adIndex].daysDuration, &w2str);
					//string::CopyFromCString("days run =", &w3str);
					//SINumToString(iAdSchedule[adIndex].daysRun, &w4str);
					
				}
				scaleFactor /= (double) iAdSchedule[adIndex].daysDuration;
				// the constant multiplier here determines the slope of the dropoff.
				scaleFactor *= 2.0;
				*totalImpressions *= scaleFactor;
				break;
			}
			case kAdvertType_Event_units_purchased:
			case kAdvertType_Event_shopping_trips:
			case kAdvertType_MassMedia:
			{
				if (iAdSchedule[adIndex].isPercentage)
				{
					*totalImpressions = iAdSchedule[adIndex].numImpressions * 
						(double) iPopulation->iGroupSize;
				}
				else
					*totalImpressions = iAdSchedule[adIndex].numImpressions;
				break;
			}
			case kAdvertType_Distribution:
				// nothing to do for a distribution task
				break;
		}

		if (iAdSchedule[adIndex].daysDuration != 0)
		{
			switch (iAdSchedule[adIndex].iAdvertType)
			{
				case kAdvertType_Coupon:	// BOGO Handled
					*numImpressionsToday = *totalImpressions / (double) iAdSchedule[adIndex].daysDuration;
					//*numImpressionsToday = *totalImpressions;
					break;
				case kAdvertType_BOGO:
					*numImpressionsToday = *totalImpressions / (double) iAdSchedule[adIndex].daysDuration;
					//*numImpressionsToday = *totalImpressions;
					break;
				case kAdvertType_Sample:
					*numImpressionsToday = *totalImpressions / (double) iAdSchedule[adIndex].daysDuration;
					*numImpressionsToday *= iAdSchedule[adIndex].iRate; 
					break;
				case kAdvertType_Event_units_purchased:
				case kAdvertType_Event_shopping_trips:
				case kAdvertType_MassMedia:
					*numImpressionsToday = *totalImpressions / (double) iAdSchedule[adIndex].daysDuration;
					// scale down # impressions, since we treat a fractional strength as a 
					// fractional impression
					// but only for the awareness only model!
					break;
				case kAdvertType_Distribution:
					// nothing to do for a distribution task
					break;
			}
		}
		else
		{
			*numImpressionsToday = 0;
		}
	}
	else
	{
		*numImpressionsToday = 0;
	}

/*
	// we may have to deal with too few impresions for one day
	int scaleFactor = 1;
	while ((*numImpressionsToday < kMinNumberImpresionsPerDay) && 
			(scaleFactor < iAdSchedule[adIndex].daysDuration))
	{
		*numImpressionsToday *= 2;
		scaleFactor*= 2;
	}

	if (scaleFactor > 1)
	{
		// see if we do ad this tick
		//int ticknum = iCtr->GetLong(myNodePtr, kCntr_CurrentTime_Ticks);
		// only do impressions every scaleFactor days
		//if (ticknum % scaleFactor)
		if (iNumberDaysofSim % scaleFactor)
			*numImpressionsToday = 0;
	}
*/


	// correct for roundoff error
	if ((*numImpressionsToday < 100) && (*numImpressionsToday > 0))
	{
		int	trunactedVal = *numImpressionsToday;
		double	offset = *numImpressionsToday - trunactedVal;


		if (WillDo0to1(offset))
			trunactedVal += 1;
		*numImpressionsToday = trunactedVal;

		//offset *= 1000.0;
		//if (offset >= 1.0)
		//{
		//	*numImpressionsToday += ((double) LocalRand(offset))/1000.0;
		//}
	}

}

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	CMicroSegment::SendTransactionFileMessage(int value1)
{
	iSaveTransactionFile = !iSaveTransactionFile;
	if (iSaveTransactionFile == 0)
		return;

	// ask the timeline control for the names of all segments
	int	msgOut = iCtr->GetBroadcastMsg(myNodePtr);	
	iCtr->Set_MsgSqTyIn(msgOut, kMsgSequenceNoAck, kMsgNoAckMsg, kMarketSimMessage_CreateOrCloseTransactionFile);
	iCtr->Set_MsgParam(msgOut, kDataMsgParam_SendingNode, (int) myNodePtr);
	iCtr->Set_MsgParam(msgOut, kDataMsgParam_SendingNode + 1, value1);
	iCtr->RcvBroadcastMsg(myNodePtr, NULL, msgOut, kWeb_AllWebs);

	// Do my own
	CreateTransactionFile(value1);
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	CMicroSegment::RespondTo_CreateOrCloseTransactionFile(int inMessage)
{
	int value1 = iCtr->Get_MsgParam(inMessage, kDataMsgParam_SendingNode + 1);
	CreateTransactionFile(value1);
}

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_HereIsSegmentName(int inMessage)
{
	int	i;
	string *segmentName = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_SegmentName));

	if (segmentName)
	{
		if (segmentName->IsEmpty())
			return;
	
		// see if we already know about this segment
		for (i = 0; i < iSegmentNames.size(); i++)
			if (string::StrCmp(segmentName, &(iSegmentNames[i])))
				return;

		// make sure this segment is not me
		if (string::StrCmp(segmentName, &iNameString))
			return;

		// if we get here, we did not find the segment, and it is not this segment,
		// so Add it to the list
		string	newString;
		string::StrCpy(segmentName, &newString);
		iSegmentNames.push_back(newString);
		iSegmentPointers.push_back(NULL);	// we'll request this next.
	}
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_HereIsSegmentPointer(int inMessage)
{
	//iRespondingSegmentNodeID = 	iCtr->Get_MsgParam(inMessage, 
	//	kDataMsgParam_SendingNode);

	int	i;

	for (i = 0; i < iSegmentNames.size(); i++)
	{
		if (string::StrCmp(&iNameOfSegWhosePointerIsRequested, &(iSegmentNames[i])))
		{
			iSegmentPointers[i] = 
				(CMicroSegment *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ObjPtr));

			// set the pointer in our social network
			// who was this message from?



			return;
		}
	}
}


// VdM 7/25/01
// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_HereIsUserTaskSegInfo(int inMessage)
{
	//string taskName;
	//string segName;

	//int startDay;
	//int startMonth;
	//int startYear;
	//string timePeriod;

	//int	type;
	//int	longVal;
	//int	taskIndex;
	//int	foundIt = false;

	//// only accept messages for this segment
	//type = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskType);
	//if (type == kUserTaskParamType_seginfo)
	//{
	//	// only need segment name for task usage rate
	//	string::StrCpy((string *) iCtr->Get_MsgParam(inMessage,kMarketSimMessageParam_UserTaskSegName),&segName);
	//	if (!(string::StrCmp(&iNameString, &segName)))
	//		return;
	//}

	//string::StrCpy((string *) iCtr->Get_MsgParam(inMessage,kMarketSimMessageParam_UserTaskName),&taskName);
	//startDay = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskStartDay);
	//startMonth = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskStartMonth);
	//startYear = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskStartYear);

	//// look for the task
	//for (taskIndex =0; taskIndex < iTaskRates.size(); taskIndex++)
	//{
	//	if (string::StrCmp(&iTaskRates[taskIndex].iTaskName, &taskName))
	//	{
	//		// check for start and end dates
	//		if ((iTaskRates[taskIndex].iStartMonth == startMonth) && 
	//			(iTaskRates[taskIndex].iStartYear == startYear) && 
	//			(iTaskRates[taskIndex].iStartDay == startDay))
	//		{
	//			foundIt = true;
	//			break;
	//		}
	//	}
	//}

	//// if we did not find it, then Add it
	//if (!foundIt)
	//{
	//	AddUserTask(&taskName, startDay, startMonth, startYear, -1, -1, -1, -1, -1.0, -1.0, false);
	//	// look for it again
	//	foundIt = false;
	//	for (taskIndex =0; taskIndex < iTaskRates.size(); taskIndex++)
	//	{
	//		if (string::StrCmp(&iTaskRates[taskIndex].iTaskName, &taskName))
	//		{
	//			// check for start and end dates
	//			if ((iTaskRates[taskIndex].iStartMonth == startMonth) && 
	//				(iTaskRates[taskIndex].iStartYear == startYear) && 
	//				(iTaskRates[taskIndex].iStartDay == startDay))
	//			{
	//				foundIt = true;
	//				break;
	//			}
	//		}
	//	}
	//}

	//// we should always be able to find it...
	//if (foundIt)
	//{
	//	if (type == kUserTaskParamType_startend)
	//	{
	//		iTaskRates[taskIndex].iEndDay = 
	//			iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskEndDay);
	//		iTaskRates[taskIndex].iEndMonth = 
	//			iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskEndMonth);
	//		iTaskRates[taskIndex].iEndYear = 
	//			iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskEndYear);
	//		iTaskRates[taskIndex].iHaveScaled = false;
	//	}
	//	else if (type == kUserTaskParamType_timeinfo)
	//	{
	//		string::StrCpy((string *) iCtr->Get_MsgParam(inMessage,kMarketSimMessageParam_UserTaskTimePeriod),&timePeriod);
	//		char	tmpBytes[256];
	//		timePeriod.CopyToCString(tmpBytes);

	//		switch (tmpBytes[0])
	//		{
	//			case 'd':
	//			case 'D':
	//				iTaskRates[taskIndex].iUsageTimeUnit = 1.0;
	//				break;

	//			case 'w':
	//			case 'W':
	//				iTaskRates[taskIndex].iUsageTimeUnit = 7.0;
	//				break;

	//			case 'm':
	//			case 'M':
	//				iTaskRates[taskIndex].iUsageTimeUnit = 365.0/12.0;
	//				break;

	//			case 'y':
	//			case 'Y':
	//				iTaskRates[taskIndex].iUsageTimeUnit = 365.0;
	//				break;
	//		}
	//		// might need to scale usageRate
	//		//if ((iTaskRates[taskIndex].iTasksPerDay != -1.0)
	//		//	&& !(iTaskRates[taskIndex].iHaveScaled))
	//		//{
	//		//	iTaskRates[taskIndex].iTasksPerDay /= (double) iTaskRates[taskIndex].iUsageTimeUnit;
	//		//	iTaskRates[taskIndex].iHaveScaled = true;
	//		//}
	//	}
	//	else if (type == kUserTaskParamType_seginfo)
	//	{
	//		double segVal;
	//		// only need segment name for task usage rate
	//		longVal = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskSegValue);
	//		segVal = MSG_LONG_TO_FLOAT(longVal);
	//		iTaskRates[taskIndex].iTasksPerDay = segVal;
	//		// might need to scale to days
	//		if ((iTaskRates[taskIndex].iUsageTimeUnit != -1) &&
	//			!(iTaskRates[taskIndex].iHaveScaled))
	//		{
	//			iTaskRates[taskIndex].iTasksPerDay /= iTaskRates[taskIndex].iUsageTimeUnit;
	//			iTaskRates[taskIndex].iHaveScaled = true;
	//		}
	//	}
	//}
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::AddUserTask(string* taskName, int startDay, int startMonth, int startYear,
		int iEndMonth, int iEndYear, int iEndDay, int iDaysDuration, double iTasksPerDay,
		double iUsageTimeUnit, int iHaveScaled)
{
	//TaskPerformanceRate	tempTPR;

	//string::StrCpy(taskName, &tempTPR.iTaskName);
	//tempTPR.iStartMonth = startMonth;
	//tempTPR.iStartYear = startYear;
	//tempTPR.iStartDay = startDay;
	//tempTPR.iEndMonth = iEndMonth;
	//tempTPR.iEndYear = iEndYear;
	//tempTPR.iEndDay = iEndDay;
	//tempTPR.iDaysDuration = iDaysDuration;
	//tempTPR.iTasksPerDay = iTasksPerDay;
	//tempTPR.iUsageTimeUnit = iUsageTimeUnit;
	//tempTPR.iHaveScaled = iHaveScaled;
	//tempTPR.iHaveWarnedNoProds = false;
	//tempTPR.iTaskObjNum = -1;
	//tempTPR.iActive = false;
	//tempTPR.iDemandModification = 0.0;
	//tempTPR.iTempShopListTasksNeededFloat = 0.0;
	//tempTPR.iTempShopListProdToBuy = -1;
	//tempTPR.iTempTasksDone = 0.0;
	//tempTPR.iTempTasksDeferred = 0.0;

	//iTaskRates.push_back(tempTPR);

	//// when Adding user tasks, need to expand shopping list as well
	//iPopulation->AddToShopList(iTaskRates.size());
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_HereIsUserTaskProdInfo(int inMessage)
{
	string taskName;
	string prodName;

	int startDay;
	int startMonth;
	int startYear;
	int endDay;
	int endMonth;
	int endYear;
	int dur;
	int min;
	int max;
	double prodVal;

	int	type;
	int	longVal;
	int	i;
	int	taskIndex;
	int	prodIndex;

	string::StrCpy((string *) iCtr->Get_MsgParam(inMessage,kMarketSimMessageParam_UserTaskName),&taskName);
	type = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskType);
	taskIndex = FindTaskByName(&taskName);

	if (taskIndex == -1)
		return;

	if (type == kUserTaskParamType_startend)
	{
		startDay = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskStartDay);
		startMonth = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskStartMonth);
		startYear = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskStartYear);
		endDay = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskEndDay);
		endMonth = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskEndMonth);
		endYear = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskEndYear);
		dur = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskDuration);
	/*	iProdTasks[taskIndex].iStartMonth = startMonth;
		iProdTasks[taskIndex].iStartYear = startYear;
		iProdTasks[taskIndex].iStartDay = startDay;
		iProdTasks[taskIndex].iEndMonth = endDay;
		iProdTasks[taskIndex].iEndYear = endMonth;
		iProdTasks[taskIndex].iEndDay = endDay;
		iProdTasks[taskIndex].iDaysDuration = endYear;*/
	}
	else if (type == kUserTaskParamType_minmax)
	{
		max = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskMaxValue);
		min = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskMinValue);
	/*	iProdTasks[taskIndex].iSuitabilityScaleMin = min;
		iProdTasks[taskIndex].iSuitabilityScaleMax = max;*/
	}
	else if (type == kUserTaskParamType_suitability)
	{
		string::StrCpy((string *) iCtr->Get_MsgParam(inMessage,kMarketSimMessageParam_UserTaskProdName),&prodName);

		longVal = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskSuitablityVal);
		prodVal = MSG_LONG_TO_FLOAT(longVal);
		prodIndex = FindTaskProdByName(taskIndex, &prodName);
		if (prodIndex == -1)
			return;
//		iProdTasks[taskIndex].iSuitability[prodIndex] = prodVal;
	}
	else if (type == kUserTaskParamType_sku)
	{
		double	fraction;
		string::StrCpy((string *) iCtr->Get_MsgParam(inMessage,kMarketSimMessageParam_UserTaskProdName),&prodName);

		longVal = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskPreSKUVal);
		prodVal = MSG_LONG_TO_FLOAT(longVal);		
		prodIndex = FindTaskProdByName(taskIndex, &prodName);
		if (prodIndex == -1)
			return;
//		iProdTasks[taskIndex].iUsesPerSkuPre[prodIndex] = prodVal;
		fraction = 1.0 / prodVal;
//		iProdTasks[taskIndex].iFractionPerUsePre[prodIndex] = fraction;

		longVal = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_UserTaskPostSKUVal);
		prodVal = MSG_LONG_TO_FLOAT(longVal);	
		if (prodVal == 0)
		{
//			iProdTasks[taskIndex].iUsesPerSkuPost[prodIndex] = iProdTasks[taskIndex].iUsesPerSkuPre[prodIndex];
	//		iProdTasks[taskIndex].iFractionPerUsePost[prodIndex] = iProdTasks[taskIndex].iFractionPerUsePre[prodIndex];
		}
		else
		{
//			iProdTasks[taskIndex].iUsesPerSkuPost[prodIndex] = prodVal;
			fraction = 1.0 / prodVal;
//			iProdTasks[taskIndex].iFractionPerUsePost[prodIndex] = fraction;
		}
	}
}


// ------------------------------------------------------------------------------
// Try to find the task.  If I can't, then create it
// ------------------------------------------------------------------------------
int CMicroSegment::FindTaskByName(string* taskName)
{
	if (taskName->IsEmpty())
	{
		return -1;
	}

	int taskIndex;
	int foundIt = false;

	// look for the task
	//for (taskIndex =0; taskIndex < iProdTasks.size(); taskIndex++)
	//{
	//	if (string::StrCmp(&iProdTasks[taskIndex].iTaskName, taskName))
	//	{
	//		foundIt = true;
	//		break;
	//	}
	//}

	// if we did not find it, then Add it
	if (!foundIt)
	{
		//AddProdTask(taskName, -1, -1, -1, -1, -1, -1, -1, -1, -1);
		//// look for it again
		//foundIt = false;
		//for (taskIndex =0; taskIndex < iProdTasks.size(); taskIndex++)
		//{
		//	if (string::StrCmp(&iProdTasks[taskIndex].iTaskName, taskName))
		//	{
		//		foundIt = true;
		//		break;
		//	}
		//}
	}

	//if (foundIt)
	//	return taskIndex;
	//else
		return -1;
}


// ------------------------------------------------------------------------------
// Try to find a product the task.  If I can't, then create it
// ------------------------------------------------------------------------------
int CMicroSegment::FindTaskProdByName(int taskIndex, string* prodName)
{
	//int prodIndex;
	//int foundIt = false;

	//// look for the task
	////for (prodIndex =0; prodIndex < iProdTasks[taskIndex].iProductName.size(); prodIndex++)
	////{
	////	if (string::StrCmp(&(iProdTasks[taskIndex].iProductName[prodIndex]), prodName))
	////	{
	////		foundIt = true;
	////		break;
	////	}
	////}

	//// if we did not find it, then Add it
	//if (!foundIt)
	//{
	//	string	newString;
	//	double		aFloat = -1.0;
	//	double		anotherFloat = -1.0;
	//	double		yetAnotherFloat = -1.0;
	//	int		aLong = -1;
	//	double		float3 = -1.0;
	//	double		float4 = -1.0;

	//	string::StrCpy(prodName, &newString);
	//	iProdTasks[taskIndex].iProductName.push_back(newString);
	//	iProdTasks[taskIndex].iSuitability.push_back(aFloat);
	//	iProdTasks[taskIndex].iUsesPerSkuPre.push_back(anotherFloat);
	//	iProdTasks[taskIndex].iUsesPerSkuPost.push_back(yetAnotherFloat);
	//	iProdTasks[taskIndex].iFractionPerUsePre.push_back(float3);
	//	iProdTasks[taskIndex].iFractionPerUsePost.push_back(float4);
	//	iProdTasks[taskIndex].iProdIndex.push_back(aLong);

	//	// look for it again
	//	foundIt = false;
	//	for (prodIndex =0; prodIndex < iProdTasks[taskIndex].iProductName.size(); prodIndex++)
	//	{
	//		if (string::StrCmp(&(iProdTasks[taskIndex].iProductName[prodIndex]), prodName))
	//		{
	//			foundIt = true;
	//			break;
	//		}
	//	}
	//}

	//// look up the product number, and Add that to the list too

	//if (foundIt)
	//	return prodIndex;
	//else
		return -1;
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void CMicroSegment::AddProdTask(string* taskName, int	startMonth, int startYear,
		int startDay, int endMonth, int endYear, int endDay, int daysDuration,
		double suitabilityScaleMin, double suitabilityScaleMax)
{
	//TaskObj	tempTask;

	//string::StrCpy(taskName, &tempTask.iTaskName);
	//
	//// these will be Added as products are received
	//// CArray <double, double> iSuitability;
	////  CArray <double, double> iUsesPerSku;
	//// CArray <string, string> iProductName;
	//tempTask.iStartMonth = -1;
	//tempTask.iStartYear = -1;
	//tempTask.iStartDay = -1;
	//tempTask.iEndMonth = -1;
	//tempTask.iEndYear = -1;
	//tempTask.iEndDay = -1;
	//tempTask.iDaysDuration = -1;
	//tempTask.iSuitabilityScaleMin = -1;
	//tempTask.iSuitabilityScaleMax = -1;
	//tempTask.iAllProductsSuitable = false;
	// iProdTasks.push_back(tempTask);
}

// ------------------------------------------------------------------------------
// VdM 9/19/01
// ------------------------------------------------------------------------------
void CMicroSegment::GetUserTasks(void)
{
	int outgoingMessage = iCtr->GetBroadcastMsg(myNodePtr);

	iCtr->Set_MsgSqTyIn(outgoingMessage, kMsgSequenceNoAck, kMsgNoAckMsg,
							kMarketSimMessage_SendMeAllUserTaskInfo);
							
	iCtr->Set_MsgParam(outgoingMessage, kDataMsgParam_SendingNode, myNodePtr);
	iCtr->RcvBroadcastMsg(myNodePtr, NULL, outgoingMessage, kWeb_MyWeb);
}


// ------------------------------------------------------------------------------
// VdM 4/22/02
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_HereIsSharePenetration(int inMessage)
{
	double	percent;
	double	units;
	int	i;
	int	pN;
	string* prodName = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ProductName));
	int type = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ParamType);

	int tmpLong = iCtr->Get_MsgParam(inMessage,kMarketSimMessageParam_ParamValue);
	percent = MSG_LONG_TO_FLOAT(tmpLong);

	if ((percent < 0.0) && (type != kSharePenetrationParamType_persuasion))
		percent = 0.0;
	else if (percent > 100.0)
		percent = 100.0;

	percent *= 0.01;	// convert all incoming percents to 0 to 1.0

	units = percent * ((double) iPopulation->iGroupSize);

	if (units == 0)
		return;

	// look for the product in iTempShareInfo
	pN = -1;
	for (i = 0; i < iTempShareInfo.size(); i++)
	{
		if (string::StrCmp(&(iTempShareInfo[i].prodName), prodName))
		{
			pN = i;
			break;
		}
	}
	// if it is not found, Add an element to the array
	if (pN == -1)
	{
		TempShareInfo shareInfo;
		string::StrCpy(prodName, &(shareInfo.prodName));
		shareInfo.iSharePercent = 0.0;
		shareInfo.initialUnitsToSellUnscaled = 0.0;
		shareInfo.iPenetrationPct = 0.0;
		shareInfo.iPenetrationUnits = 0.0;
		shareInfo.iBrandAwarenessPct = 0.0;
		shareInfo.iBrandAwarenessUnits = 0.0;
		shareInfo.iInitialPersuasion = 0.0;
		iTempShareInfo.push_back(shareInfo);
		// look for it again to get the proper index
		for (i = 0; i < iTempShareInfo.size(); i++)
		{
			if (string::StrCmp(&(iTempShareInfo[i].prodName), prodName))
			{
				pN = i;
				break;
			}
		}
	}
	if (pN != -1)
	{
		// store the data away
		switch (type) 
		{
			case kSharePenetrationParamType_initialshare:
				iTempShareInfo[pN].iSharePercent = percent;
				iTempShareInfo[pN].initialUnitsToSellUnscaled = units;
				break;

			case kSharePenetrationParamType_penetration:
				iTempShareInfo[pN].iPenetrationPct = percent;
				iTempShareInfo[pN].iPenetrationUnits = units;
				break;

			case kSharePenetrationParamType_brandawareness:
				iTempShareInfo[pN].iBrandAwarenessPct = percent;
				iTempShareInfo[pN].iBrandAwarenessUnits = units;
				break;

			case kSharePenetrationParamType_persuasion:
				iTempShareInfo[pN].iInitialPersuasion = percent * 100.0;	// persuasion is not a percent
				break;
		}
	}
}


// ------------------------------------------------------------------------------
// 
// ------------------------------------------------------------------------------
void CMicroSegment::RespondTo_HereIsProductMatrixInfo(int inMessage)
{
	string* have = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ProductMatrixHave));
	string* want = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ProductMatrixWant));
	string* relationship = (string *)(iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_ProductMatrixValue));
	
	// if the relationship is irellevant, then the message can be ignored
	string	notRelevantStr;
	string	prereqStr;
	string	incompatStr;
	int		pn;
	int		foundIt;

	string::CopyFromCString("not relevant", &notRelevantStr);
	if (string::StrCmpNoCase(&notRelevantStr, relationship))
		return;

	// if we are still here, then a relationsip is defined;

	// for every product, need a Carray or prerequisite products, expands with each unique HereIsProductMatrixInfo message
	// for every product, need a Carray of incompatable products, expands with each unique HereIsProductMatrixInfo message
	// there are realy two arays for each:  One with product names, and another with product numbers.   The numbers are calculated each tick
	// we need to be careful not to have two products of the same name

	// Prerequisite means that the "want" product requireds to  the "have" product
	// the have product goes in the want product's iPrereqName array
	string::CopyFromCString("prerequisite", &prereqStr);
	string::CopyFromCString("incompatible", &incompatStr);
	if (string::StrCmpNoCase(relationship, &prereqStr))
	{
		iHavePrereqs = true;
		// find the want product
		foundIt = false;
		for (pn = 0; pn < iProducts.size(); pn++)
		{
			if (string::StrCmpNoCase(want, &iProducts[pn].iName))
			{
				foundIt = true;
				break;	// exit with pn of found product
			}
		}	//next product
		if (!foundIt)
				AddAProduct(want);
		string	newString;
		int		newNum;
		string::StrCpy(have, &newString);
		iProducts[pn].iPrereqName.push_back(newString);
		newNum = -1;
		iProducts[pn].iPrereqNum.push_back(newNum);
		iProducts[pn].iHavePrepreqs = true;


	}	// end check for prerequisite
/*
	// Incompatable means that the "want" product will not work with the "have" product
	// each product goes in the other product's incompatable list
	else if (string::StrCmpNoCase(relationship,&incompatStr))
	{
		iHaveIncompats = true;
		// Add have to want
		foundIt = false;
		for (pn = 0; pn < iProducts.size(); pn++)
		{
			if (string::StrCmpNoCase(want, &iProducts[pn].iName))
			{
				foundIt = true;
				break;	// exit with pn of found product
			}
		}	//next product
		if (!foundIt)
				AddAProduct(want);
		string	newString1;
		int		newNum1;
		string::StrCpy(have, &newString1);
		iProducts[pn].iIncompatName.push_back(newString1);
		newNum1 = -1;
		iProducts[pn].iIncompatNum.push_back(newNum1);
		iProducts[pn].iHaveIncompats = true;

		// Add want to have
		foundIt = false;
		for (pn = 0; pn < iProducts.size(); pn++)
		{
			if (string::StrCmpNoCase(have, &iProducts[pn].iName))
			{
				foundIt = true;
				break;	// exit with pn of found product
			}
		}	//next product
		if (!foundIt)
				AddAProduct(have);
		string	newString2;
		int		newNum2;
		string::StrCpy(want, &newString2);
		iProducts[pn].iIncompatName.push_back(newString2);
		newNum2 = -1;
		iProducts[pn].iIncompatNum.push_back(newNum2);
		iProducts[pn].iHaveIncompats = true;
	}	// end check for incompatible
*/
}

// ------------------------------------------------------------------------------
// Once we have read a product in, we need to Add the starting share, penetration
// and brand awareness info that came from SSIO
// ------------------------------------------------------------------------------
void CMicroSegment::InsertStartingShares(int prodNum)
{
	int	i;

	for (i=0; i<iTempShareInfo.size(); i++)
	{
		if (string::StrCmp(&(iTempShareInfo[i].prodName), &(iProducts[prodNum].iName)))
		{
			// starting share
			if (iTempShareInfo[i].iSharePercent != 0.0)
			{
				iProducts[prodNum].iSharePercent = iTempShareInfo[i].iSharePercent;
				//iProducts[prodNum].initialUnitsToSellScaled = iTempShareInfo[i].initialUnitsToSellUnscaled;
				//iProducts[prodNum].initialUnitsToSellScaled  /= iPopulation->iPopScaleFactor;
				iSetInitialPurchasePercentages = false;
				//iProducts[prodNum].initialAwarenessType = kColType_Share;
			}
			else
			{
				iProducts[prodNum].iSharePercent = 0.0;
				iProducts[prodNum].initialUnitsToSellScaled = 0;
			}

			// penetration
			iProducts[prodNum].iPenetrationPct = iTempShareInfo[i].iPenetrationPct;
			iProducts[prodNum].iPenetrationUnits = iTempShareInfo[i].iPenetrationUnits;

			// brand awareness not mutually exclusive with starting share  
			// KMK 1/21/04 simplified following code
			iProducts[prodNum].iBrandAwarenessPct = iTempShareInfo[i].iBrandAwarenessPct;

			// persuasion
			iProducts[prodNum].iInitialPersuasion = iTempShareInfo[i].iInitialPersuasion;
		}
	}
}

void CMicroSegment::RespondTo_HereIsSocNetValue(int inMessage)
{
	int toNode = iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_toNode);

	CMicroSegment* toSeg = 0;

	// find the pointer to the other segment (maybe its me!
	if (toNode > 0)
		toSeg = (CMicroSegment*) iCtr->GetLong(toNode, kCntr_MyPtr);

	NetworkParamsRecordset*	networkParams;

	networkParams = (NetworkParamsRecordset*)iCtr->Get_MsgParam(inMessage, kMarketSimMessageParam_networkParams);

	
	SocialNetwork* socNet = new SocialNetwork(this, toSeg, networkParams);

	if (socialNetworks == 0)
	{
		// create container
		socialNetworks = new vector<SocialNetwork*>;
	}

	socialNetworks->push_back(socNet);
}