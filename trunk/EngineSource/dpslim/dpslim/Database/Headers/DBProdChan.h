#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 11/20/200
// Vicki de Mey, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// ProdChan recordset

class ProdChanRecordset : public CRecordset
{
public:
	ProdChanRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(ProdChanRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(ProdChanRecordset, CRecordset)
       long		m_ProductID;
       long		m_ChannelID;
       float	m_Markup;
       float	m_Price;
       float	m_PeriodicPrice;
       CString	m_HowOften;
       float	m_SkusAtPrice;
	   CString	m_PType;
       CTime	m_StartDate;
       CTime	m_EndDate;
       long		m_Duration;
	//}}AFX_FIELD


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(ProdChanRecordset)
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
