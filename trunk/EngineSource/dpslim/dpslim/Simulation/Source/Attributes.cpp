#include "Attributes.h"

ProductAttribute::ProductAttribute(double pre_use, double post_use, int attribute_id, int segment, int product, int channel, CTime& start, CTime& end) : TimeSeriesData(segment, product, channel, start, end)
{
	this->pre_use = pre_use;
	this->post_use = post_use;
	this->attribute_id = attribute_id;
}

ProductAttribute::ProductAttribute(ProdAttrValRecordset* record, bool use_post_use)
{
	SetProduct(record->m_ProductID);
	SetStartDate(record->m_StartDate);
	SetEndDate(record->m_StartDate);
	
	pre_use = record->m_PreValue;
	if(use_post_use)
	{
		post_use = record->m_PostValue;
	}
	else
	{
		post_use = pre_use;
	}
	attribute_id = record->m_ProductAttrID;
}

void ProductAttribute::ModifySegment(CMicroSegment* segment, CTime & date)
{
	segment->UpdateProductAttribute(this);
}

bool ProductAttribute::ActiveOnDate(CTime& date)
{
	if(date == GetStartDate())
	{
		return true;
	}
	else
	{
		return false;
	}
}

bool ProductAttribute::DeactiveOnDate(CTime& date)
{
	return false;
}

int ProductAttribute::GetAttribute()
{
	return attribute_id;
}

double ProductAttribute::GetPreUse()
{
	return pre_use;
}

double ProductAttribute::GetPostUse()
{
	return post_use;
}

ConsumerPref::ConsumerPref(double pre_use, double post_use, double price_sensitivity, int attribute_id, int segment, int product, int channel, CTime& start, CTime& end) : TimeSeriesData(segment, product, channel, start, end)
{
	this->pre_use = pre_use;
	this->post_use = post_use;
	this->attribute_id = attribute_id;
	this->price_sensitivity = price_sensitivity;
}

ConsumerPref::ConsumerPref(ConsumerPrefsRecordset* record, bool use_post_use)
{
	SetSegment(record->m_SegmentID);
	SetStartDate(record->m_StartDate);
	SetEndDate(record->m_StartDate);
	
	pre_use = record->m_PreValue;
	if(use_post_use)
	{
		post_use = record->m_PostValue;
	}
	else
	{
		post_use = pre_use;
	}
	price_sensitivity = record->m_PriceSensitivity;
	attribute_id = record->m_ProdAttrID;
}

void ConsumerPref::ModifySegment(CMicroSegment* segment, CTime & date)
{
	segment->UpdateConsumerPreference(this);
}

bool ConsumerPref::ActiveOnDate(CTime& date)
{
	if(date == GetStartDate())
	{
		return true;
	}
	else
	{
		return false;
	}
}

bool ConsumerPref::DeactiveOnDate(CTime& date)
{
	return false;
}

int ConsumerPref::GetAttribute()
{
	return attribute_id;
}

double ConsumerPref::GetPreUse()
{
	return pre_use;
}

double ConsumerPref::GetPostUse()
{
	return post_use;
}

double ConsumerPref::GetPriceSensitivity()
{
	return price_sensitivity;
}

void ConsumerPref::ApplyTau(double tau)
{
	pre_use += tau;
	post_use += tau;
}

Attribute::Attribute(ProdAttrRecordset* record)
{
	id = record->m_ProductAttrID;
	tau = record->m_CustTau;
	type = record->m_Type;
	initial_awareness = record->m_InitialAwareness/100;
}

int Attribute::GetAttribute()
{
	return id;
}

int Attribute::GetType()
{
	return type;
}

double Attribute::GetInitialAwareness()
{
	return initial_awareness;
}

double Attribute::GetTau()
{
	return tau;
}

DynamicAttribute::DynamicAttribute(ProductAttribute *attribute, int awareness)
{
	prod_id = attribute->GetProduct();
	attr_id = attribute->GetAttribute();
	post_use = attribute->GetPostUse();
	pre_use = attribute->GetPreUse();
	aware = awareness;
}

DynamicAttribute::DynamicAttribute(ID product_id, ID attribute_id, double post_use_value, double pre_use_value, int is_aware)
{
	prod_id = product_id;
	attr_id = attribute_id;
	post_use = post_use_value;
	pre_use = pre_use_value;
	aware = is_aware;
}

ID DynamicAttribute::GetAttributeID()
{
	return attr_id;
}

ID DynamicAttribute::GetProductID()
{
	return prod_id;
}

int DynamicAttribute::Aware()
{
	return aware;
}

double DynamicAttribute::PostUse()
{
	return post_use;
}

double DynamicAttribute::PreUse()
{
	return pre_use;
}

void DynamicAttribute::MakeAware()
{
	aware = 1;
}

void DynamicAttribute::Forget()
{
	aware = 0;
}

void DynamicAttribute::SetPostUse(int value)
{
	post_use = value;
}

void DynamicAttribute::SetPreUse(int value)
{
	pre_use = value;
}