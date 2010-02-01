#include "Display.h"

Display::Display() : TimeSeriesData()
{
	this->distribution = 0;
	this->awareness = 0;
	this->persuasion = 0;
}

Display::Display(double distribution, double awareness, double persuasion, int segment, int product, int channel, CTime& start, CTime& end) : TimeSeriesData(segment, product, channel, start, end)
{
	this->distribution = distribution;
	this->awareness = awareness;
	this->persuasion = persuasion;
}

Display::Display(DistDisplayRecordset * record)
{
	SetProduct(record->m_ProductID);
	SetChannel(record->m_ChannelID);
	SetStartDate(CTime(record->m_StartDate.GetYear(), record->m_StartDate.GetMonth(), record->m_StartDate.GetDay(), 0,0,0,0));
	SetEndDate(CTime(record->m_EndDate.GetYear(), record->m_EndDate.GetMonth(), record->m_EndDate.GetDay(),0,0,0,0) + CTimeSpan(1,0,0,0));

	this->distribution = record->m_GValue;
	this->awareness = record->m_Awareness;
	this->persuasion = record->m_Persuasion;
}

void Display::ModifySegment(CMicroSegment* segment, CTime & date)
{
}

bool Display::ActiveOnDate(CTime &date)
{
	if(date >= GetStartDate())
	{
		return true;
	}
	else
	{
		return false;
	}
}

bool Display::DeactiveOnDate(CTime &date)
{
	if(date <= GetEndDate())
	{
		return true;
	}
	else
	{
		return false;
	}
}

double Display::GetAwareness()
{
	return awareness;
}

double Display::GetPersuasion()
{
	return persuasion;
}

double Display::GetDistribution()
{
	return distribution;
}

void Display::SetAwareness(double awareness)
{
	this->awareness = awareness;
}

void Display::SetPersuasion(double persuasion)
{
	this->persuasion = persuasion;
}

void Display::SetDistribution(double distribution)
{
	this->distribution = distribution;
}