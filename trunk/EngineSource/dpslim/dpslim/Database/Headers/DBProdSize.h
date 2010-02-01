#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 1/5/2007
// Steve Noble, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// SegChan recordset

class ProductSizeRecordset : public CRecordset
{
public:
	ProductSizeRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(ProductSizeRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(SegChanRecordset, CRecordset)
	long	product_id;
    long	channel_id;
    float	prod_size;

	//}}AFX_FIELD


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(SegChanRecordset)
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
