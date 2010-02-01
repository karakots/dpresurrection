#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 11/25/200
// Vicki de Mey, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// Segment Characteristics recordset

class SegCharsRecordset : public CRecordset
{
public:
	SegCharsRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(SegCharsRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(SegCharsRecordset, CRecordset)
 	long	m_SegmentID;
	CString	m_ProductName;
	//}}AFX_FIELD

	/*
Segment Model // m
Color
int Show Current Share Pie Chart
int Show Cumulative Share  Chart
long Size
Growth Rate
Growth Rate People or Percent // people, percent
Growth Rate Month or Year // month, year
int Compress Population
Variability // between 0 and 100
short Price Disutility
Attribute Sensitivity // between 0 and 10
float Persuasion Scaling
float Display Utility
float Display Utility Scaling Factor
Inertia // between 0 and 10
int Repurchase
Repurchase model // f,n,t,b,r
float Gamma Location Parameter (a)
float Gamma Shape Parameter (k)
Repurchase period or frequency // less than 365
Repurchase frequency variation
Repurchase timescale // days, weeks, months, years
Shopping Trip Interval
Category Penetration // between 0 and 100 inclusive
Category Rejection // between 0 and 100 inclusive
long # Initial buyers
short Initial buying period (days)
int Seed with repurchasers
int Use Budget
Budget ($)
Budget Period
int Save unspent money
Initial savings ($)
Social Network Model
Number of close contacts
Probability of talking to close contact pre-use
Probability of talking to close contact post-use
int Use Local
float Number of distant contacts // between 0 and 100
float Probability of talking to distant contact pre-use // between 0 and 1.0
float Probability of talking to distant contact post-use // between 0 and 1.0
Awareness Weight of a Personal Message
Pre-use Persuasion Probability
Post-use Persuasion Probability
float Units-Desired Trigger
Awareness Model // Awareness, Persuaion & Awareness
float Awareness Threshold
float Awareness Decay Rate
float Persuasion Decay Rate
float Average Max Units Purchased
float Max Display Hits Per Trip
Product Choice Model //  e, l, n, v, g
F1: Persuasion Score // *, ^
F2: Persuasion Value Computation // Share of voice, Absolute, square root, base 10 log
F3: Persuasion Contribution To Overall Score // +, -, *, /
F4: Utility Score // *, ^
F5: Combination of Part Utilities // Scaled Sum of Products, unscaled sum of products
F6: Price Contribution To Overall Score // +, -, *, /
F7: Price Score // *, ^
F8: Price Value // Absolute, Price/Use, relative, +reference
float Reference Price
F9: Choice Probability // Share of score, logit, scaled logit
F10: Inertia Model // Brand, SKU
F11: Error Term // None, User value, utility, score
short Error Term User Value

*/





// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(SegCharsRecordset)
	public:
	virtual CString GetDefaultConnect();    // Default connection string
	virtual CString GetDefaultSQL();    // Default SQL for Recordset
	virtual void DoFieldExchange(CFieldExchange* pFX);  // RFX support
	virtual int Open(unsigned int nOpenType = snapshot, LPCTSTR lpszSql = NULL, DWORD dwOptions = none);
	//}}AFX_VIRTUAL

// Implementation
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif
};
