#include "Media.h"

Media::Media(double grps, double awareness, double persuasion, int segment, int product, int channel, CTime& start, CTime& end) : TimeSeriesData(segment, product, channel, start, end)
{
	CTimeSpan span = GetEndDate() - GetStartDate();
	double days = span.GetDays();
	this->grps = grps / days;
	this->awareness = awareness;
	this->persuasion = persuasion;
}

Media::Media(MassMediaRecordset* record)
{
	SetSegment(record->m_SegmentID);
	SetProduct(record->m_ProductID);
	SetChannel(record->m_ChannelID);
	SetStartDate(CTime(record->m_StartDate.GetYear(), record->m_StartDate.GetMonth(), record->m_StartDate.GetDay(), 0,0,0,0));
	SetEndDate(CTime(record->m_EndDate.GetYear(), record->m_EndDate.GetMonth(), record->m_EndDate.GetDay(),0,0,0,0) + CTimeSpan(1,0,0,0));
	CTimeSpan span = GetEndDate() - GetStartDate();
	double days = span.GetDays();
	this->grps = record->m_GValue / days;
	this->awareness = record->m_Awareness;
	this->persuasion = record->m_Persuasion;
}

void Media::ModifySegment(CMicroSegment* segment, CTime & date)
{
	if(GetStartDate() == date)
	{
		segment->AddMassMedia(this);
	}

	if(GetEndDate() == date)
	{
		segment->RemoveMassMedia(this);
	}
}

bool Media::ActiveOnDate(CTime &date)
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

bool Media::DeactiveOnDate(CTime &date)
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

double Media::GRPs(CTime& date)
{
	return grps;
}

double Media::GetAwareness()
{
	return awareness;
}

double Media::GetPersuasion()
{
	return persuasion;
}