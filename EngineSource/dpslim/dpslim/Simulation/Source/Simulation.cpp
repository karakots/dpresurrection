// simulation.cpp : This is the main simulation code
// This file and the companion header file, define the main architecture of the
// new engine.
//
// Copyright 2007 by  DecisionPower
//
// author: Isaac Noble
// creation data: 9/01/2007


#include "Simulation.h"

#include "MSDatabase.h"
#include "DBSegment.h"
#include "Channel.h"
#include "Product.h"
#include "ProductTree.h"
#include "Display.h"
#include "Media.h"
#include "Coupon.h"
#include "ExternalFactor.h"
#include "MarketUtility.h"
#include "Price.h"
#include "Distribution.h"
#include "Attributes.h"
#include "SocialNetwork.h"
#include "InitialCondition.h"
#include "TimeController.h"


// ------------------------------------------------------------------------------
// The simulation must be linked to a database.  Currently the database is expected to be open.
// This could be modified to pass a connection file to the simulation engine
// ------------------------------------------------------------------------------
Simulation::Simulation(MSDatabase* database)
{
	//Link to the database
	this->database = database;
	isNimo = (database->m_ModelInfo->app_code.CompareNoCase("MSNIMOAPP") == 0);

	//The new time controller.  This will store the time series data, such as market data and attributes
	time_controller = new TimeController(database->m_ModelInfo->start_date, database->m_ModelInfo->end_date);
	start_date = CTime(database->m_ModelInfo->start_date.GetYear(), database->m_ModelInfo->start_date.GetMonth(), database->m_ModelInfo->start_date.GetDay(), 0, 0, 0, 0);
	end_date = CTime(database->m_Simulation->end_date.GetYear(),database->m_Simulation->end_date.GetMonth(),database->m_Simulation->end_date.GetDay(), 0, 0, 0, 0);

	//Initialize the in memory database
	segments = map<int, CMicroSegment*>();
	channels = map<int, Channel*>();
	products = map<int, Product*>();
	social_networks = vector<SocialNetwork*>();
	product_tree = vector<pair<int,int>>();
	product_size = map<pair<int, int>, double>();
	prerequistes = map<pair<int, int>, double>();
	segment_channel_choice = map<int, map<int, double>>();
	initial_conditions = map<int, map<int , InitialCondition*>>();

	mass_media = vector<Media*>();
	coupons = vector<Coupon*>();
	distribution = vector<Distribution*>();
	//displays = vector<Display*>();
	price = vector<Price*>();
	market_utility = vector<MarketUtility*>();
	external_factors = vector<ExternalFactor*>();
	product_attributes = vector<ProductAttribute*>();
	consumer_prefs = vector<ConsumerPref*>();
	attributes = map<int, Attribute*>();
}

// ------------------------------------------------------------------------------
// Run the simulation
// ------------------------------------------------------------------------------
void Simulation::Run()
{
	int day = 0;
	CTime date = CTime(start_date.GetYear(),start_date.GetMonth(),start_date.GetDay(), 0, 0, 0, 0);
	CTime end = CTime(end_date.GetYear(),end_date.GetMonth(),end_date.GetDay(), 0, 0, 0, 0);
	vector<TimeSeriesData*>* active_data;
	map<int, CMicroSegment*>::const_iterator iter;

	for(iter = segments.begin(); iter != segments.end(); ++iter)
	{
		iter->second->Reset();
	}
	database->SimStart();
	while(date <= end)
	{
		CMicroSegment::Current_Time = CTime(date.GetYear(),date.GetMonth(),date.GetDay(), 0, 0, 0, 0);
		active_data = time_controller->DeactiveOnDate(day);
		for(size_t i = 0; i < active_data->size(); ++i)
		{
			if(active_data->at(i)->GetSegment() == -1)
			{
				for(iter = segments.begin(); iter != segments.end(); ++iter)
				{
					active_data->at(i)->ModifySegment(iter->second, date);
				}
			}
			else
			{
				active_data->at(i)->ModifySegment(segments[active_data->at(i)->GetSegment()], date);
			}
		}
		active_data = time_controller->ActiveOnDate(day);
		for(size_t i = 0; i < active_data->size(); ++i)
		{
			if(active_data->at(i)->GetSegment() == -1)
			{
				for(iter = segments.begin(); iter != segments.end(); ++iter)
				{
					active_data->at(i)->ModifySegment(iter->second, date);
				}
			}
			else
			{
				active_data->at(i)->ModifySegment(segments[active_data->at(i)->GetSegment()], date);
			}
		}
		for(iter = segments.begin(); iter != segments.end(); ++iter)
		{
			iter->second->SimStep();
		}
		if(TimeToWrite(date))
		{
			for(iter = segments.begin(); iter != segments.end(); ++iter)
			{
				iter->second->DBOutput(database);
			}
			database->WriteSegData();
		}
		date = date + CTimeSpan(1,0,0,0);
		day++;
	}
	database->SimDone();
}

// ------------------------------------------------------------------------------
// Initialize the simulation
// ------------------------------------------------------------------------------
void Simulation::Initialize()
{
	//Load the segments with the products and channels
	map<int, CMicroSegment*>::const_iterator iter;
	for(iter = segments.begin(); iter != segments.end(); ++iter)
	{
		iter->second->LoadDataFromDatabase(	products, 
											channels, 
											segment_channel_choice[iter->second->iSegmentID],
											product_tree,
											attributes,
											product_size,
											prerequistes,
											initial_conditions[iter->second->iSegmentID]);
		iter->second->Initialize();
	}

	for(int i = 0; i < mass_media.size(); ++i)
	{
		time_controller->AddData(mass_media[i]);
	}

	for(int i = 0; i < coupons.size(); ++i)
	{
		time_controller->AddData(coupons[i]);
	}

	for(int i = 0; i < distribution.size(); ++i)
	{
		time_controller->AddData(distribution[i]);
	}

	for(int i = 0; i < price.size(); ++i)
	{
		time_controller->AddData(price[i]);
	}

	for(int i = 0; i < market_utility.size(); ++i)
	{
		time_controller->AddData(market_utility[i]);
	}

	for(int i = 0; i < external_factors.size(); ++i)
	{
		time_controller->AddData(external_factors[i]);
	}

	for(int i = 0; i < product_attributes.size(); ++i)
	{
		time_controller->AddData(product_attributes[i]);
	}

	if(isNimo)
	{
		apply_nimo_taus();
	}

	for(int i = 0; i < consumer_prefs.size(); ++i)
	{
		time_controller->AddData(consumer_prefs[i]);
	}
}

// ------------------------------------------------------------------------------
// Check if it time to write out to databse
// ------------------------------------------------------------------------------
bool Simulation::TimeToWrite(CTime & date)
{
	if(date <= start_date)
	{
		return false;
	}
	//Redundant check for v23 Regression...
	if(date <= start_date || date >= end_date)
	{
		return true;
	}
	if(iModelInfo->access_time % 30 == 0)
	{
		int num_months = iModelInfo->access_time/30;
		if((date.GetMonth() - last_write.GetMonth() + 12) % 12 == num_months)
		{
			return true;
		}
	}
	CTimeSpan span = date - start_date;
	if((span.GetDays()+1) % iModelInfo->access_time == 0)
	{
		return true;
	}

	return false;
}
// ------------------------------------------------------------------------------
// Run the simulation
// ------------------------------------------------------------------------------
void Simulation::reset_values()
{
}


// ------------------------------------------------------------------------------
// Reads the databse into memory
// ------------------------------------------------------------------------------
void Simulation::LoadFromDB()
{
	//Set the sim seed
	int seed = database->SimSeed();
	// reset system random number generater
	RandomInitialise(seed, seed+109);
	
	if (database->m_Segment->IsOpen())
	{
		database->m_Segment->Requery();
	}
	else
	{
		database->m_Segment->Open();
	}

	//Set ModelInfo
	iModelInfo = database->m_ModelInfo;

	//Read the segments in
	database->m_Segment->MoveFirst();
	while(!database->m_Segment->IsEOF())
	{
		segments[database->m_Segment->m_SegmentID] =  new CMicroSegment();
		segments[database->m_Segment->m_SegmentID]->ReadSegmentAndModel(database->m_Segment, database->m_ModelInfo, isNimo);
		database->m_Segment->MoveNext();
	}

	//Now read in all the static data
	read_products();
	read_channels();
	read_segment_channels();
	read_attributes();
	read_product_attributes(iModelInfo->attribute_pre_and_post);
	read_segment_attributes(iModelInfo->attribute_pre_and_post);
	read_product_tree(products.size());
	read_product_sizes();
	read_prerequistes();
	read_social_networks();
	read_initial_conditions();

	//Now read in all the time series data
	read_media_and_coupons();
	read_distribution_and_display();
	read_price();
	if(iModelInfo->market_utility)
	{
		read_market_utility();
	}
	read_external_factors();

}

// ------------------------------------------------------------------------------
// All of the following are utility functions for reading in the various recordsets
// ------------------------------------------------------------------------------

void Simulation::read_channels()
{
	if (database->m_Channel->IsOpen())
	{
		database->m_Channel->Requery();
	}
	else
	{
		database->m_Channel->Open();
	}
	if(database->m_Channel->IsEOF())
	{
		return;
	}
	database->m_Channel->MoveFirst();
	while(!database->m_Channel->IsEOF())
	{
		channels[database->m_Channel->m_ChannelID] = new Channel();
		channels[database->m_Channel->m_ChannelID]->iChanName = database->m_Channel->m_ChannelName;
		channels[database->m_Channel->m_ChannelID]->iChannelID = database->m_Channel->m_ChannelID;
		database->m_Channel->MoveNext();
	}
}

void Simulation::read_products()
{
	if (database->m_Product->IsOpen())
	{
		database->m_Product->Requery();
	}
	else
	{
		database->m_Product->Open();
	}
	if(database->m_Product->IsEOF())
	{
		return;
	}
	database->m_Product->MoveFirst();
	while(!database->m_Product->IsEOF())
	{
		products[database->m_Product->m_ProductID] = new Product();
		products[database->m_Product->m_ProductID]->iProdName = database->m_Product->m_ProductName;
		products[database->m_Product->m_ProductID]->iProductID = database->m_Product->m_ProductID;
		products[database->m_Product->m_ProductID]->iDropRateDecay = database->m_Product->m_RepeatLikeProb/100;
		products[database->m_Product->m_ProductID]->iInitialDropRate = database->m_Product->m_InitialDislikeProb/100;
		products[database->m_Product->m_ProductID]->iEq_units = database->m_Product->m_eq_units;
		products[database->m_Product->m_ProductID]->iBase_price = database->m_Product->m_base_price;
		database->m_Product->MoveNext();
	}
}

void Simulation::read_segment_channels()
{
	if (database->m_SegChan->IsOpen())
	{
		database->m_SegChan->Requery();
	}
	else
	{
		database->m_SegChan->Open();
	}
	if(database->m_SegChan->IsEOF())
	{
		return;
	}
	database->m_SegChan->MoveFirst();
	while(!database->m_SegChan->IsEOF())
	{
		int segment_id = database->m_SegChan->m_SegmentID;
		int channel_id = database->m_SegChan->m_ChannelID;
		segment_channel_choice[segment_id][channel_id] = database->m_SegChan->m_ProbOfChoice/100;
		database->m_SegChan->MoveNext();
	}
}

void Simulation::read_product_tree(int num_prods)
{
	if (database->m_ProductTree->IsOpen())
	{
		database->m_ProductTree->Requery();
	}
	else
	{
		database->m_ProductTree->Open();
	}
	if(database->m_ProductTree->IsEOF())
	{
		return;
	}
	database->m_ProductTree->MoveFirst();
	while(!database->m_ProductTree->IsEOF())
	{
		int parentID = database->m_ProductTree->parentID;
		int childID = database->m_ProductTree->childID;
		product_tree.push_back(pair<int,int>(parentID, childID));
		database->m_ProductTree->MoveNext();
	}
}

void Simulation::read_product_sizes()
{
	if (database->m_product_size->IsOpen())
	{
		database->m_product_size->Requery();
	}
	else
	{
		database->m_product_size->Open();
	}
	if(database->m_product_size->IsEOF())
	{
		return;
	}
	database->m_product_size->MoveFirst();
	while(!database->m_product_size->IsEOF())
	{
		int product_id = database->m_product_size->product_id;
		int channel_id = database->m_product_size->channel_id;
		double size = database->m_product_size->prod_size;
		pair<int, int> key = pair<int, int>(product_id, channel_id);
		product_size[key] = size;
		database->m_product_size->MoveNext();
	}
}

void Simulation::read_prerequistes()
{
	if(!database->m_ProdMatrix)
	{
		database->m_ProdMatrix = new ProductMatrixRecordset(&database->iTheCurrentDB);
	}

	if (database->m_ProdMatrix->IsOpen())
	{
		database->m_ProdMatrix->Requery();
	}
	else
	{
		database->m_ProdMatrix->Open();
	}
	if(database->m_ProdMatrix->IsEOF())
	{
		return;
	}
	database->m_ProdMatrix->MoveFirst();
	while(!database->m_ProdMatrix->IsEOF())
	{
		int parent_id = database->m_ProdMatrix->m_HaveProductID;
		int child_id = database->m_ProdMatrix->m_WantProductID;
		CString value = database->m_ProdMatrix->m_Value;
		pair<int, int> key = pair<int, int>(parent_id, child_id);
		if(value.CompareNoCase("Prerequisite") == 0)
		{
			prerequistes[key] = 1;
		}
		else if(value.CompareNoCase("Incompatible") == 0)
		{
			prerequistes[key] = -1;
		}
		database->m_ProdMatrix->MoveNext();
	}
}

void Simulation::read_social_networks()
{
	if(!database->m_SocNetwork)
	{
		database->m_SocNetwork = new SocNetworkRecordset(&database->iTheCurrentDB);
	}

	if (database->m_SocNetwork->IsOpen())
	{
		database->m_SocNetwork->Requery();
	}
	else
	{
		database->m_SocNetwork->Open();
	}
	if(database->m_SocNetwork->IsEOF())
	{
		return;
	}
	NetworkParamsRecordset* network_parms = 0;
	if(database->m_SocNetwork->IsEOF())
	{
		return;
	}
	database->m_SocNetwork->MoveFirst();
	while(!database->m_SocNetwork->IsEOF())
	{
		network_parms = 0;
		if(database->m_NetworkParams->IsEOF())
		{
			return;
		}
		database->m_NetworkParams->MoveFirst();
		while(!database->m_NetworkParams->IsEOF())
		{
			if(database->m_NetworkParams->id == database->m_SocNetwork->network_param)
			{
				network_parms = database->m_NetworkParams;
			}
		}
		if(network_parms == 0)
		{
			database->m_SocNetwork->MoveNext();
			continue;
		}
		social_networks.push_back(new SocialNetwork(segments[database->m_SocNetwork->from_segment], segments[database->m_SocNetwork->to_segment], network_parms));
		database->m_SocNetwork->MoveNext();
	}
}

void Simulation::read_initial_conditions()
{
	if (database->m_InitalConds->IsOpen())
	{
		database->m_InitalConds->Requery();
	}
	else
	{
		database->m_InitalConds->Open();
	}
	if(database->m_InitalConds->IsEOF())
	{
		return;
	}
	database->m_InitalConds->MoveFirst();
	while(!database->m_InitalConds->IsEOF())
	{
		int segment_id = database->m_InitalConds->m_SegmentID;
		int product_id = database->m_InitalConds->m_ProductID;
		initial_conditions[segment_id][product_id] = new InitialCondition(database->m_InitalConds);
		database->m_InitalConds->MoveNext();
	}
}

void Simulation::read_media_and_coupons()
{
	if (database->m_MassMedia->IsOpen())
	{
		database->m_MassMedia->Requery();
	}
	else
	{
		database->m_MassMedia->Open();
	}
	if(database->m_MassMedia->IsEOF())
	{
		return;
	}
	database->m_MassMedia->MoveFirst();
	while(!database->m_MassMedia->IsEOF())
	{
		if(database->m_MassMedia->m_Type.CompareNoCase(CString("V")) == 0)
		{
			mass_media.push_back(new Media(database->m_MassMedia));
		}
		else
		{
			coupons.push_back(new Coupon(database->m_MassMedia));
		}
		database->m_MassMedia->MoveNext();
	}
}

void Simulation::read_distribution_and_display()
{
	if (database->m_DistDisplay->IsOpen())
	{
		database->m_DistDisplay->Requery();
	}
	else
	{
		database->m_DistDisplay->Open();
	}
	if(database->m_DistDisplay->IsEOF())
	{
		return;
	}
	database->m_DistDisplay->MoveFirst();
	while(!database->m_DistDisplay->IsEOF())
	{
		if(database->m_DistDisplay->m_Type.CompareNoCase(CString("D")) == 0)
		{
			distribution.push_back(new Distribution(database->m_DistDisplay));
		}
		else
		{
			market_utility.push_back(new MarketUtility(database->m_DistDisplay));
		}
		database->m_DistDisplay->MoveNext();
	}
}

void Simulation::read_market_utility()
{
	if (database->m_MarketUtility->IsOpen())
	{
		database->m_MarketUtility->Requery();
	}
	else
	{
		database->m_MarketUtility->Open();
	}
	if(database->m_MarketUtility->IsEOF())
	{
		return;
	}
	database->m_MarketUtility->MoveFirst();
	while(!database->m_MarketUtility->IsEOF())
	{
		market_utility.push_back(new MarketUtility(database->m_MarketUtility));
		database->m_MarketUtility->MoveNext();
	}
}

void Simulation::read_price()
{
	if (database->m_ProdChan->IsOpen())
	{
		database->m_ProdChan->Requery();
	}
	else
	{
		database->m_ProdChan->Open();
	}
	if(database->m_ProdChan->IsEOF())
	{
		return;
	}
	database->m_ProdChan->MoveFirst();
	while(!database->m_ProdChan->IsEOF())
	{
		price.push_back(new Price(database->m_ProdChan));
		database->m_ProdChan->MoveNext();
	}
}

void Simulation::read_external_factors()
{
	if (database->m_ProductEvent->IsOpen())
	{
		database->m_ProductEvent->Requery();
	}
	else
	{
		database->m_ProductEvent->Open();
	}
	if(database->m_ProductEvent->IsEOF())
	{
		return;
	}
	database->m_ProductEvent->MoveFirst();
	while(!database->m_ProductEvent->IsEOF())
	{
		external_factors.push_back(new ExternalFactor(database->m_ProductEvent));
		database->m_ProductEvent->MoveNext();
	}
}

void Simulation::read_product_attributes(bool use_post_use)
{
	if (database->m_ProdAttrVal->IsOpen())
	{
		database->m_ProdAttrVal->Requery();
	}
	else
	{
		database->m_ProdAttrVal->Open();
	}
	if(database->m_ProdAttrVal->IsEOF())
	{
		return;
	}
	database->m_ProdAttrVal->MoveFirst();
	while(!database->m_ProdAttrVal->IsEOF())
	{
		product_attributes.push_back(new ProductAttribute(database->m_ProdAttrVal, use_post_use));
		database->m_ProdAttrVal->MoveNext();
	}
}

void Simulation::read_segment_attributes(bool use_post_use)
{
	if (database->m_ConsumerPrefs->IsOpen())
	{
		database->m_ConsumerPrefs->Requery();
	}
	else
	{
		database->m_ConsumerPrefs->Open();
	}
	if(database->m_ConsumerPrefs->IsEOF())
	{
		return;
	}
	database->m_ConsumerPrefs->MoveFirst();
	while(!database->m_ConsumerPrefs->IsEOF())
	{
		consumer_prefs.push_back(new ConsumerPref(database->m_ConsumerPrefs, use_post_use));
		database->m_ConsumerPrefs->MoveNext();
	}
}

void Simulation::read_attributes()
{
	if (database->m_ProdAttr->IsOpen())
	{
		database->m_ProdAttr->Requery();
	}
	else
	{
		database->m_ProdAttr->Open();
	}
	if(database->m_ProdAttr->IsEOF())
	{
		return;
	}
	database->m_ProdAttr->MoveFirst();
	while(!database->m_ProdAttr->IsEOF())
	{
		int product_attribute_id = database->m_ProdAttr->m_ProductAttrID;
		attributes[product_attribute_id] = new Attribute(database->m_ProdAttr);
		database->m_ProdAttr->MoveNext();
	}
}

Simulation::~Simulation()
{
	delete time_controller;
	product_tree.clear();
	product_size.clear();
	prerequistes.clear();

	for(int i = 0; i < mass_media.size(); ++i)
	{
		delete mass_media[i];
	}
	mass_media.clear();

	for(int i = 0; i < coupons.size(); ++i)
	{
		delete coupons[i];
	}
	coupons.clear();

	for(int i = 0; i < distribution.size(); ++i)
	{
		delete distribution[i];
	}
	distribution.clear();

	for(int i = 0; i < price.size(); ++i)
	{
		delete price[i];
	}
	price.clear();

	for(int i = 0; i < market_utility.size(); ++i)
	{
		delete market_utility[i];
	}
	market_utility.clear();

	for(int i = 0; i < external_factors.size(); ++i)
	{
		delete external_factors[i];
	}
	external_factors.clear();

	for(int i = 0; i < product_attributes.size(); ++i)
	{
		delete product_attributes[i];
	}
	product_attributes.clear();

	for(int i = 0; i < consumer_prefs.size(); ++i)
	{
		delete consumer_prefs[i];
	}
	consumer_prefs.clear();

	for(int i = 0; i < social_networks.size(); ++i)
	{
		delete social_networks[i];
	}
	social_networks.clear();

	for(map<int, map<int, double>>::iterator iter = segment_channel_choice.begin(); iter != segment_channel_choice.end(); ++iter)
	{
		iter->second.clear();
	}
	segment_channel_choice.clear();

	for(map<int, map<int, InitialCondition*>>::iterator iter = initial_conditions.begin(); iter != initial_conditions.end(); ++iter)
	{
		for(map<int, InitialCondition*>::iterator iiter = iter->second.begin(); iiter != iter->second.end(); ++iiter)
		{
			delete iiter->second;
		}
		iter->second.clear();
	}
	initial_conditions.clear();

	for(map<int, Product*>::iterator iter = products.begin(); iter != products.end(); ++iter)
	{
		delete iter->second;
	}
	products.clear();

	for(map<int, Channel*>::iterator iter = channels.begin(); iter != channels.end(); ++iter)
	{
		delete iter->second;
	}
	channels.clear();

	for(map<int, CMicroSegment*>::iterator iter = segments.begin(); iter != segments.end(); ++iter)
	{
		delete iter->second;
	}
	segments.clear();
}

// ------------------------------------------------------------------------------
// Special NIMO functions
// ------------------------------------------------------------------------------
void Simulation::apply_nimo_taus()
{
	for(int i = 0; i < consumer_prefs.size(); i++)
	{
		consumer_prefs[i]->ApplyTau(attributes[consumer_prefs[i]->GetAttribute()]->GetTau());
	}
}