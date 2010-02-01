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

class NetworkParamsRecordset : public CRecordset
{
public:
	NetworkParamsRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(NetworkParamsRecordset)

// Field/Param Data
	//{{AFX_FIELD(BrandRecordset, CRecordset)
	long id;
	int type;
	float persuasion_pre_use;
	float persuasion_post_use;
	float awareness_weight;
	float num_contacts;
	float prob_of_talking_pre_use;
	float prob_of_talking_post_use;
	int  use_local;
	float  percent_talking;
	float  neg_persuasion_reject;
	float neg_persuasion_pre_use;
	float neg_persuasion_post_use;

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
