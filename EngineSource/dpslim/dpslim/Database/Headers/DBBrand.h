#pragma once
#define _CRT_SECURE_NO_WARNINGS

// ----------------------
//
// Created 11/20/200
// Vicki de Mey, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// Brand recordset

class BrandRecordset : public CRecordset
{
public:
	BrandRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(BrandRecordset)

// Field/Param Data
	//{{AFX_FIELD(BrandRecordset, CRecordset)

 	long	m_BrandID;
	CString	m_BrandName;

	//}}AFX_FIELD


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(BrandRecordset)
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
