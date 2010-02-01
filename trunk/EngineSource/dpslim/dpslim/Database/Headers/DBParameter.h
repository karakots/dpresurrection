#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 4/26/2005
// Steve Noble, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

// parameters for adjusting single values in database

class ParameterRecordset : public CRecordset
{
public:
	ParameterRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(ParameterRecordset)

// Field/Param Data
	//{{AFX_FIELD(BrandRecordset, CRecordset)
	long param_id;
	CString	table_name;
	CString	col_name;
	CString	filter;
	float update_value;
	float orig_value;

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

	void SetDBValues();
	void Restore();

	static char ScenarioQuery[128];

	static void SetScenario(int scenarioID);

#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif
};
