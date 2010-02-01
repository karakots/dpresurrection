#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 11/20/200
// Vicki de Mey, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// InitialConds recordset

class InitialCondsRecordset : public CRecordset
{
public:
	InitialCondsRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(InitialCondsRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(InitialCondsRecordset, CRecordset)
 	long	m_SegmentID;
	long	m_ProductID;
	float	m_InitialShare;
	float	m_Penetration;
	float	m_BrandAwareness;
	float	m_Persuasion;
	//}}AFX_FIELD


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(InitialCondsRecordset)
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
