#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 4/18/2007
// Steve Noble, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// PackSize

class PackSizeRecordset : public CRecordset
{
public:
	PackSizeRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(PackSizeRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(SegChanRecordset, CRecordset)
	long	pack_size_id;
    long	size;
    float	dist;

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
