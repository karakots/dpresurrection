#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 4/26/2005
// Steve Noble, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// Brand recordset

class SocNetworkRecordset : public CRecordset
{
public:
	SocNetworkRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(SocNetworkRecordset)

// Field/Param Data
	//{{AFX_FIELD(BrandRecordset, CRecordset)
	long from_segment;
	long to_segment;
	long network_param;

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
