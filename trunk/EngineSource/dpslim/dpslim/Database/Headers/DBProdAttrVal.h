#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 11/20/200
// Vicki de Mey, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// ProdAttrVal recordset

class ProdAttrValRecordset : public CRecordset
{
public:
	ProdAttrValRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(ProdAttrValRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(ProdAttrValRecordset, CRecordset)
       long		m_ProductID;
       long		m_ProductAttrID;
       CTime	m_StartDate;
       float	m_PreValue;
       float	m_PostValue;
	   int		has_attribute;
	//}}AFX_FIELD


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(ProdAttrValRecordset)
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
