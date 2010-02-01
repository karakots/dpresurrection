#include "Price.h"

Price::Price(double distribution, double price, CString type, int segment, int product, int channel, CTime& start, CTime& end) : TimeSeriesData(segment, product, channel, start, end)
{
	this->distribution = distribution;
	this->price = price;
	this->type = GetType(type);
}

Price::Price(ProdChanRecordset * record)
{
	SetProduct(record->m_ProductID);
	SetChannel(record->m_ChannelID);
	SetStartDate(CTime(record->m_StartDate.GetYear(), record->m_StartDate.GetMonth(), record->m_StartDate.GetDay(), 0,0,0,0));
	SetEndDate(CTime(record->m_EndDate.GetYear(), record->m_EndDate.GetMonth(), record->m_EndDate.GetDay(),0,0,0,0) + CTimeSpan(1,0,0,0));

	this->distribution = record->m_SkusAtPrice;
	this->price = record->m_Price;
	this->type = GetType(record->m_PType);
}

void Price::ModifySegment(CMicroSegment* segment, CTime & date)
{
	segment->UpdatePrice(this);
}

int Price::GetType(CString type)
{
	if(type.CompareNoCase(CString("promoted")) == 0)
	{
		return kPriceTypePromoted;
	}
	else if(type.CompareNoCase(CString("BOGO")) == 0)
	{
		return kPriceTypeBOGO;
	}
	else if(type.CompareNoCase(CString("Z")) == 0)
	{
		return kPriceTypeDisplayAbsolute;
	}
	else
	{
		return kPriceTypeUnpromoted;
	}
}

bool Price::ActiveOnDate(CTime &date)
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

bool Price::DeactiveOnDate(CTime &date)
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

bool Price::Deactivate(CTime & date)
{
	if(date < GetStartDate() || date > GetEndDate())
	{
		return true;
	}

	return false;
}

int Price::GetType()
{
	return type;
}

double Price::GetPrice(CTime & date)
{
	if(date < GetStartDate() || date > GetEndDate())
	{
		return 0;
	}

	return price;
}

double Price::GetDistribution(CTime & date)
{
	if(date < GetStartDate() || date > GetEndDate())
	{
		return 0;
	}

	return distribution/100;
}