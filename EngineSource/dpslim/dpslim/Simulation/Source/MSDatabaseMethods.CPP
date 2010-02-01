#pragma once
#define _CRT_SECURE_NO_WARNINGS
// MSDatabaseMethods.cpp
//
// Copyright 2004 DecisionPower, Inc.
//
// Database calls for the microsegment
// Created by Vicki de Mey 4/16/04

#ifndef WINVER
#define WINVER 0x0501
#endif

#include "CMicroSegment.h"
#include "MSDatabase.h"

#include <stdlib.h>
#include <math.h>

#include "DBModel.h"
#include "DBProdAttr.h"
#include "DBProductTree.h"
#include "ProductTree.h"
#include "SegmentWriter.h"
#include "RepurchaseModel.h"
#include "DBProdSize.h"
#include "DBSimLog.h"
#include "Product.h"
#include "Channel.h"
#include "InitialCondition.h"
#include "PopObject.h"
#include "ChoiceModel.h"

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	CMicroSegment::write_purchase_info_to_file(void)
{
	
	for(Pcn_iter iter = iProductsAvailable.begin(); iter != iProductsAvailable.end(); iter++)
	{
		accumlate_data(GetPcn(iter));
	}
}

// ------------------------------------------------------------------------------
//	Write to Database - writes directly to m_SegData structure directly
// SSN 3/29/2005
// ------------------------------------------------------------------------------
void	CMicroSegment::accumlate_data(Pcn pcn)
{	
		SegmentDataAcc* currData;
		if (iSegmentMap.find(pcn) == iSegmentMap.end())
		{
			iSegmentMap[pcn] = new SegmentDataAcc(iModelInfo->start_date);
		}
		currData = iSegmentMap[pcn];

		currData->AddInfo(this, pcn);
}



void	CMicroSegment::DBOutput(MSDatabase* database)
{
	for(Pcn_iter iter = iProductsAvailable.begin(); iter != iProductsAvailable.end(); iter++)
	{
		Pcn pcn = GetPcn(iter);
		SegmentDataAcc* currData = iSegmentMap[pcn];
		currData->getInfo(database->NewSegData());
		currData->reset(Current_Time);
	}
}

void CMicroSegment::ReadSegmentAndModel(SegmentRecordset* segment, ModelRecordset* model, bool isNimo)
{
	iModelInfo = model;
	iSegmentID = segment->m_SegmentID;
	iNameString = segment->m_SegmentName;

	//NIMO BRANCH
	if(isNimo)
	{
		repurchaseModel->type = kRepurchaseModel_TaskBased_NBDrate;
		iShareAwarenessWithSibs = true;
		iRejectSibs = true;
		iUseACVDistribution = false;
		iDisplayUtility = segment->m_display_utility;
	}
	else
	{
		repurchaseModel->type = kRepurchaseModel_NBD;
		iShareAwarenessWithSibs = false;
		iRejectSibs = false;
		iUseACVDistribution = true;
		iDisplayUtility = 0.0;
	}
	//Repurchase Model (NIMO and MS)
	repurchaseModel->iRepurchase = true;
	repurchaseModel->iRepurchaseFrequencyDuration = segment->m_repurchase_period_frequency;
	repurchaseModel->iNBDstddev = segment->m_repurchase_frequency_variation;
	repurchaseModel->iGammaA = segment->m_gamma_location_parameter_a;
	repurchaseModel->iGammaK = segment->m_gamma_shape_parameter_k;
	repurchaseModel->iAverageMaxUnits = segment->m_avg_max_units_purch;

	//Choice Model	
	if(segment->m_price_value.CompareNoCase("Relative") == 0)
	{
		choiceModel->iGCMf8_PriceValueSource = kGCMf8_RelativePrice;
	}
	else if(segment->m_price_value.CompareNoCase("Base") == 0)
	{
		choiceModel->iGCMf8_PriceValueSource = kGCMf8_ReferencePrice;
	}
	else
	{
		choiceModel->iGCMf8_PriceValueSource = kGCMf8_AbsolutePrice;
	}

	if(segment->m_persuasion_value_computation.CompareNoCase("Share of Voice") == 0)
	{
		choiceModel->iGCMf2_PersuasionValComp = kGCMf2_ShareOfVoice;
	}
	else if(segment->m_persuasion_value_computation.CompareNoCase("Exponential") == 0)
	{
		choiceModel->iGCMf2_PersuasionValComp = kGCMf2_squareRoot;
	}
	else if(segment->m_persuasion_value_computation.CompareNoCase("Base 10 Log") == 0)
	{
		choiceModel->iGCMf2_PersuasionValComp = kGCMf2_base10log;
	}
	else
	{
		choiceModel->iGCMf2_PersuasionValComp = kGCMf2_Absolute;
	}
	choiceModel->iGCM_ErrorTermUserValue = segment->m_units_desired_trigger;
	

	//Choice Math Sensitivities
	iPriceSensitivity = segment->m_price_disutility;
	iMessageSensitivity = segment->m_persuasion_scaling;

	//Loyalties
	iChannelLoyalty = segment->m_loyalty;
	iBrandLoyalty = segment->m_inertia;

	//Message Model
	iPersuasionDecayRatePostUse = segment->m_persuasion_decay_rate_post;
	iPersuasionDecayRatePreUse = segment->m_persuasion_decay_rate_pre;
	iAwarenessDecayRatePostUse = segment->m_awareness_decay_rate_post;
	iAwarenessDecayRatePreUse = segment->m_awareness_decay_rate_pre;

	iDiversityPercent = segment->m_variability;
	iMaxDisplayHits = (int)segment->m_max_display_hits_per_trip;
	long segment_size = (segment->m_segment_size*model->pop_size*model->scale_factor)/10000;
	// cannot have zero consumers
	if (segment_size < 1)
	{
		segment_size = 1;
	}
	iPopulation->iGroupSize = segment_size;
}

void CMicroSegment::LoadDataFromDatabase(	map<int, Product*> & products, 
											map<int, Channel*> & channels, 
											map<int, double> & channel_choice,
											vector<pair<int,int>> & tree,
											map<int, Attribute*> & attributes,
											map<pair<int, int>, double> & product_sizes,
											map<pair<int, int>, double> & prerequistes,
											map<int, InitialCondition*> & initial_conditions)
{
	read_product_attributes(attributes);
	read_product_and_channels(products, channels, channel_choice);
	read_product_tree(tree);
	read_product_size(product_sizes);
	read_product_dependencies(prerequistes);
	read_initial_share_and_penetration(initial_conditions);
}

void CMicroSegment::read_product_and_channels(map<int, Product*> & products, map<int, Channel*> & channels, map<int, double> & channel_choice)
{
	iProducts.clear();
	iChannels.clear();

	map<int, Product*>::const_iterator p_iter;
	map<int, Channel*>::const_iterator c_iter;
	map<int, double>::const_iterator cc_iter;

	int product_index = 0;
	for(p_iter = products.begin(); p_iter != products.end(); ++p_iter)
	{
		iProducts.push_back(new Product(p_iter->second));
		iProducts[product_index]->iProductIndex = product_index;
		product_index++;
	}

	int channel_index = 0;
	for(c_iter = channels.begin(); c_iter != channels.end(); ++c_iter)
	{
		iChannels.push_back(new Channel(c_iter->second));
		iChannels[channel_index]->iChannelIndex = channel_index;
		channel_index++;
	}

	pcn_key::NumProducts = iProducts.size();

	build_product_channel_list();

	for(cc_iter = channel_choice.begin(); cc_iter != channel_choice.end(); ++cc_iter)
	{
		iChannels[ChanIndex(cc_iter->first)]->iPctChanceChosen = cc_iter->second;
	}

	compute_cumulative_channel_percents();

}


void	CMicroSegment::read_product_tree(vector<pair<ID,ID>> & tree)
{
	if(ProductTree::theTree->LeafNodes()->size() == 0)
	{
		ProductTree::theTree->SetSize(iProducts.size());
		for(size_t i = 0; i < tree.size(); ++i)
		{
			ID parent_id = tree[i].first;
			ID child_id = tree[i].second;
			Index p_pn = ProdIndex(parent_id);
			Index c_pn = ProdIndex(child_id);
			ProductTree::theTree->SetParent(p_pn, c_pn);
		}
	}
	iProdTree = ProductTree::theTree;
}

void	CMicroSegment::read_product_dependencies(map<pair<int, int>, double> & prerequistes)
{

	//First initial the prerequiste array for each product
	for(Pn_iter pn_iter = iProducts.begin(); pn_iter != iProducts.end(); ++pn_iter)
	{
		(*pn_iter)->iPrerequisiteNum = vector<Index>();
		(*pn_iter)->iInCompatibleNum = vector<Index>();
	}

	map<pair<int, int>, double>::const_iterator iter;

	int parent;
	int child;

	for(iter = prerequistes.begin(); iter != prerequistes.end(); ++iter)
	{
		parent = ProdIndex(iter->first.first);
		child = ProdIndex(iter->first.second);
		
		if(iter->second > 0)
		{
			iProducts[child]->iPrerequisiteNum.push_back(parent);
		}
		if(iter->second < 0)
		{
			iProducts[child]->iInCompatibleNum.push_back(parent);
		}
	}
}

void	CMicroSegment::read_product_size(map<pair<int, int>, double> & product_sizes)
{
	map<pair<int, int>, double>::const_iterator iter;

	for(iter = product_sizes.begin(); iter != product_sizes.end(); ++iter)
	{
		Pcn pcn = Pcn(ProdIndex(iter->first.first), ChanIndex(iter->first.second));
		iProductsAvailable[pcn]->iSize = iter->second;
	}
}

void	CMicroSegment::read_product_attributes(map<int, Attribute*> & attributes)
{
	iAttributes.insert(attributes.begin(), attributes.end());
}

void	CMicroSegment::read_initial_share_and_penetration(map<int, InitialCondition*> & initial_conditions)
{
	int ID;
	for(Pn_iter pn_iter = iProducts.begin(); pn_iter != iProducts.end(); ++pn_iter)
	{
		ID = (*pn_iter)->iProductID;
		if(initial_conditions.find(ID) != initial_conditions.end())
		{
			(*pn_iter)->iInitialSharePct = initial_conditions[ID]->GetShare();

			(*pn_iter)->iInitialPenetrationPct = initial_conditions[ID]->GetPenetration();

			(*pn_iter)->iInitialAwareness = initial_conditions[ID]->GetAwareness();

			(*pn_iter)->iInitialPersuasion = initial_conditions[ID]->GetPersuasion();
		}
	}
}