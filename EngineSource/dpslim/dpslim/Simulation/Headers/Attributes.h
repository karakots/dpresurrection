// Attribute.h : Classes to handle attributes.  Currently the microsegment has had
// all attribute stuff ripped from it, so that is can be redifined.  The class used
// by the microsegment should be compatable with dynamic attributes and time series
// attributes and preferences.  This part of the architecture is not well defined
// at this point.
//
// Copyright 2007 by  DecisionPower
//
// author: Isaac Noble
// creation data: 9/01/2007

#include "stdafx.h"

#include "TimeSeriesData.h"


// ------------------------------------------------------------------------------
// Class for handling the time series attributes values.
// ------------------------------------------------------------------------------
class ProductAttribute : public TimeSeriesData
{
public:
	ProductAttribute(ProdAttrValRecordset*, bool);
	ProductAttribute(double, double, int, int, int, int, CTime&, CTime&);
	void ModifySegment(CMicroSegment*, CTime&);
	bool ActiveOnDate(CTime&);
	bool DeactiveOnDate(CTime&);
	int GetAttribute();
	double GetPreUse();
	double GetPostUse();
private:
	double pre_use;
	double post_use;
	int attribute_id;
};

// ------------------------------------------------------------------------------
// Class for handling the time series attributes preferences
// ------------------------------------------------------------------------------
class ConsumerPref : public TimeSeriesData
{
public:
	ConsumerPref(ConsumerPrefsRecordset*, bool);
	ConsumerPref(double, double, double, int, int, int, int, CTime&, CTime&);
	void ModifySegment(CMicroSegment*, CTime&);
	bool ActiveOnDate(CTime&);
	bool DeactiveOnDate(CTime&);
	int GetAttribute();
	double GetPreUse();
	double GetPostUse();
	double GetPriceSensitivity();
	void ApplyTau(double);
private:
	double pre_use;
	double post_use;
	double price_sensitivity;
	int attribute_id;
};

class Attribute
{
public:
	Attribute(ProdAttrRecordset*);
	int GetAttribute();
	int GetType();
	double GetInitialAwareness();
	double GetTau();
private:
	int id;
	int type;
	double tau;
	double initial_awareness;
};

class DynamicAttribute
{
public:
	DynamicAttribute(ProductAttribute*, int);
	DynamicAttribute(ID, ID, double, double, int);
	ID GetProductID();
	ID GetAttributeID();
	double PostUse();
	double PreUse();
	int Aware();
	void MakeAware();
	void Forget();
	void SetPostUse(int);
	void SetPreUse(int);
private:
	double post_use;
	double pre_use;
	int aware;
	ID prod_id;
	ID attr_id;
};