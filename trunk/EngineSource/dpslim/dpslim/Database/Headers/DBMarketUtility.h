#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 2/20/2006
// Isaac Noble, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// DistDisplay recordset

class MarketUtilityRecordset : public CRecordset
{
public:
	MarketUtilityRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(MarketUtilityRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(MarketUtilityRecordset, CRecordset)
  	long	m_ProductID;
	long	m_ChannelID;
	long	m_SegmentID;
	float	m_Percent_Dist;
	float	m_Awareness;
	float	m_Persuasion;
	float	m_Utility;
	CTime	m_StartDate;
	CTime	m_EndDate;
	//}}AFX_FIELD


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(MarketUtilityRecordset)
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
