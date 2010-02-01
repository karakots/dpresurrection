#include "Distribution.h"

Distribution::Distribution(double pre_use_dist, double post_use_dist, double awareness, double persuasion, int segment, int product, int channel, CTime& start, CTime& end) : TimeSeriesData(segment, product, channel, start, end)
{
	this->post_use_dist = post_use_dist;
	this->pre_use_dist = pre_use_dist;
	this->awareness = awareness;
	this->persuasion = persuasion;
}

Distribution::Distribution(DistDisplayRecordset * record)
{
	SetProduct(record->m_ProductID);
	SetChannel(record->m_ChannelID);
	SetStartDate(CTime(record->m_StartDate.GetYear(), record->m_StartDate.GetMonth(), record->m_StartDate.GetDay(), 0,0,0,0));
	SetEndDate(CTime(record->m_EndDate.GetYear(), record->m_EndDate.GetMonth(), record->m_EndDate.GetDay(),0,0,0,0) + CTimeSpan(1,0,0,0));

	
	this->pre_use_dist = record->m_FValue;
	this->post_use_dist = record->m_GValue;
	this->awareness = record->m_Awareness;
	this->persuasion = record->m_Persuasion;
}

void Distribution::ModifySegment(CMicroSegment* segment, CTime & date)
{
	segment->UpdateDistribution(this);
}

bool Distribution::ActiveOnDate(CTime &date)
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

bool Distribution::DeactiveOnDate(CTime &date)
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

double Distribution::GetPreUseDistribution(CTime &date)
{
	if(date < GetStartDate() || date >= GetEndDate())
	{
		return 0;
	}
	return pre_use_dist;
}

double Distribution::GetPostUseDistribution(CTime &date)
{
	if(date < GetStartDate() || date >= GetEndDate())
	{
		return 0;
	}
	return post_use_dist;
}

double Distribution::GetAwareness()
{
	return awareness;
}

double Distribution::GetPersuasion()
{
	return persuasion;
}