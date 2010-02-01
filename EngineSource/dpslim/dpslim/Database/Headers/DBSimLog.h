#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 01/11/2005
// Vicki de Mey, DecisionPower, Inc.
//

#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// Sim Log recordset

class SimLogRecordset : public CRecordset
{
public:
	SimLogRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(SimLogRecordset)

// Field/Param Data
	//{{AFX_FIELD(BrandRecordset, CRecordset)

 	long	run_id;
	long	product_id;
	long	segment_id;
	long	channel_id;
	long	comp_id;
	long	message_id;
	CString message;
	CTime	calendar_date;
	
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
