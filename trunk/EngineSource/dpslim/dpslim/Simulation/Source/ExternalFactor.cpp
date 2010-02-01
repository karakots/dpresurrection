#include "ExternalFactor.h"

ExternalFactor::ExternalFactor(double demand, int segment, int product, int channel, CTime& start, CTime& end) : TimeSeriesData(segment, product, channel, start, end)
{
	this->demand = demand;
}

ExternalFactor::ExternalFactor(ProductEventRecordset * record)
{
	SetSegment(record->m_SegmentID);
	SetProduct(record->m_ProductID);
	SetChannel(record->m_ChannelID);
	SetStartDate(CTime(record->m_StartDate.GetYear(), record->m_StartDate.GetMonth(), record->m_StartDate.GetDay(), 0,0,0,0));
	SetEndDate(CTime(record->m_EndDate.GetYear(), record->m_EndDate.GetMonth(), record->m_EndDate.GetDay(),0,0,0,0) + CTimeSpan(1,0,0,0));

	this->demand = record->m_DemandMod/100;
}


void ExternalFactor::ModifySegment(CMicroSegment* segment, CTime & date)
{
	if(GetStartDate() == date)
	{
		segment->AddExternalFactor(this);
	}

	if(GetEndDate() == date)
	{
		segment->RemoveExternalFactor(this);
	}
}

bool ExternalFactor::ActiveOnDate(CTime &date)
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

bool ExternalFactor::DeactiveOnDate(CTime &date)
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

double ExternalFactor::GetDemand()
{
	return demand;
}