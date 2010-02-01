#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 01/11/2005
// Vicki de Mey, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// Brand recordset

class ScenarioRecordset : public CRecordset
{
public:
	ScenarioRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(ScenarioRecordset)

// Field/Param Data
	//{{AFX_FIELD(BrandRecordset, CRecordset)

	CTime	start_date;
	CTime	end_date;

	long	type;

	long	access_time;

	double	scale_factor;

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

	static char ScenarioQuery[256];

#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif
};
