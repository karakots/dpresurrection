#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 11/20/200
// Vicki de Mey, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// ProdAttr recordset

class ProdAttrRecordset : public CRecordset
{
public:
	ProdAttrRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(ProdAttrRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(ProdAttrRecordset, CRecordset)
 	long	m_ProductAttrID;
	CString	m_ProductAttrName;
	float	m_UtilityMin;
	float	m_UtilityMax;
	float	m_PreferenceMin;
	float	m_PreferenceMax;
	float	m_CustTau;
	long	m_Type;
	float	m_InitialAwareness;
	//}}AFX_FIELD


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(ProdAttrRecordset)
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
