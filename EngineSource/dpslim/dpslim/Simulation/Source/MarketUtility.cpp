//
// MarketUtility.cpp
// Author Isaac Noble (initial creation)
// Purpose:	Datastructure to hold

// Change History - most recent on top
// Who	When		Comment

//
// ISN	2/22/2006	Initial Creation

#include "MarketUtility.h"

// Default Constuctor
MarketUtility::MarketUtility()
{
	this->distribution = 0;
	this->awareness = 0;
	this->persuasion = 0;
	this->utility = 0;
}

// Creates a MarketUtility based on a MarketUtiliy recordset
MarketUtility::MarketUtility(MarketUtilityRecordset* record)
{
	SetProduct(record->m_ProductID);
	SetChannel(record->m_ChannelID);
	SetStartDate(CTime(record->m_StartDate.GetYear(), record->m_StartDate.GetMonth(), record->m_StartDate.GetDay(), 0,0,0,0));
	SetEndDate(CTime(record->m_EndDate.GetYear(), record->m_EndDate.GetMonth(), record->m_EndDate.GetDay(),0,0,0,0) + CTimeSpan(1,0,0,0));

	this->distribution = record->m_Percent_Dist/100;
	this->awareness = record->m_Awareness;
	this->persuasion = record->m_Persuasion;
	this->utility = record->m_Utility;
	this->display = 0;
}

// Creates a MarketUtility based on a Display recordset
MarketUtility::MarketUtility(DistDisplayRecordset * record)
{
	SetProduct(record->m_ProductID);
	SetChannel(record->m_ChannelID);
	SetStartDate(CTime(record->m_StartDate.GetYear(), record->m_StartDate.GetMonth(), record->m_StartDate.GetDay(), 0,0,0,0));
	SetEndDate(CTime(record->m_EndDate.GetYear(), record->m_EndDate.GetMonth(), record->m_EndDate.GetDay(),0,0,0,0) + CTimeSpan(1,0,0,0));

	this->distribution = record->m_FValue/100;
	this->awareness = record->m_Awareness;
	this->persuasion = record->m_Persuasion;
	this->utility = 0;
	this->display = 1;
}

double MarketUtility::GetUtility()
{
	return utility;
}

bool MarketUtility::ActiveOnDate(CTime &date)
{
	if(GetStartDate() == date)
	{
		return true;
	}
	else
	{
		return false;
	}
}

bool MarketUtility::DeactiveOnDate(CTime &date)
{
	if(GetEndDate() == date)
	{
		return true;
	}
	else
	{
		return false;
	}
}

void MarketUtility::ModifySegment(CMicroSegment* segment, CTime & date)
{
	if(GetStartDate() == date)
	{
		segment->AddMarketUtility(this);
	}
	
	if(GetEndDate() == date)
	{
		segment->RemoveMarketUtility(this);
	}
}

double MarketUtility::GetAwareness()
{
	return awareness;
}

double MarketUtility::GetPersuasion()
{
	return persuasion;
}

double MarketUtility::GetDistribution()
{
	return distribution;
}

int MarketUtility::IsDisplay()
{
	return display;
}

