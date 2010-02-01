#include "Coupon.h"

Coupon::Coupon(double reach, double redemption, double awareness, double persuasion, int segment, int product, int channel, CTime& start, CTime& end) : TimeSeriesData(segment, product, channel, start, end)
{
	this->span = (end - start).GetDays();
	this->end = end;
	this->reach = reach;
	this->redemption = redemption;
	this->awareness = awareness;
	this->persuasion = persuasion;
}

Coupon::Coupon(MassMediaRecordset* record)
{
	SetSegment(record->m_SegmentID);
	SetProduct(record->m_ProductID);
	SetChannel(record->m_ChannelID);
	SetStartDate(record->m_StartDate);
	SetEndDate(record->m_EndDate + CTimeSpan(1,0,0,0));

	this->span = (record->m_EndDate - record->m_StartDate).GetDays();
	this->end = record->m_EndDate;
	this->reach = record->m_GValue/100;
	this->redemption = record->m_IValue/100;
	this->awareness = record->m_Awareness;
	this->persuasion = record->m_Persuasion;
}

void Coupon::ModifySegment(CMicroSegment* segment, CTime & date)
{
	if(GetStartDate() == date)
	{
		segment->AddCouponDrop(this);
	}
	
	if(GetEndDate() == date)
	{
		segment->RemoveCouponDrop(this);
	}
}

bool Coupon::ActiveOnDate(CTime &date)
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

bool Coupon::DeactiveOnDate(CTime &date)
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

double Coupon::GetSpan()
{
	return span;
}

const CTime & Coupon::GetEndDate()
{
	return end;
}

double Coupon::GetRedemption()
{
	return redemption;
}

double Coupon::GetReach()
{
	return reach;
}

double Coupon::GetAwareness()
{
	return awareness;
}

double Coupon::GetPersuasion()
{
	return persuasion;
}