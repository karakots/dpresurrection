#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 3/29/2006
// Steve Noble, DecisionPower, Inc.
//
// ----------------------


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// ProductMatrix recordset

class ProductTreeRecordset : public CRecordset
{
public:
	ProductTreeRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(ProductTreeRecordset)

// Field/Param Data
 	long	parentID;
	long	childID;



// Overrides
	// ClassWizard generated virtual function overrides
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