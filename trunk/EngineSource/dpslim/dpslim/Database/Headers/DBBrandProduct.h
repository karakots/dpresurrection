#pragma once
#define _CRT_SECURE_NO_WARNINGS

// ----------------------
//
// Created 11/20/200
// Vicki de Mey, DecisionPower, Inc.
//
// ----------------------


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// BrandProductRecordset recordset

class BrandProductRecordset : public CRecordset
{
public:
	BrandProductRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(BrandProductRecordset)

// Field/Param Data
	//{{AFX_FIELD(BrandProductRecordset, CRecordset)

 	long	m_ProductID;
 	long	m_BrandID;
	CString m_Type;
	CString m_ProductGroup;
	CString m_RelatedGroup;
	CString m_PercentRelation;
	float	m_Cost;
 	float	m_InitialDislikeProb;
 	float	m_RepeatLikeProb;
	CString	m_Color;
	float	m_base_price;
	float	m_eq_units;

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
