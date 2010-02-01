#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 9/1/2004
// Steve Noble, DecisionPower, Inc.
//
// ----------------------


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// ProductMatrix recordset

class ProductMatrixRecordset : public CRecordset
{
public:
	ProductMatrixRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(ProductMatrixRecordset)

// Field/Param Data
	//{{AFX_FIELD(BrandProductRecordset, CRecordset)

 	long	m_HaveProductID;
	long	m_WantProductID;
	CString m_Value;


	//}}AFX_FIELD


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(BrandProductRecordset)
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