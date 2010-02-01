#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 11/20/200
// Vicki de Mey, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// ConsumerPrefs recordset

class ConsumerPrefsRecordset : public CRecordset
{
public:
	ConsumerPrefsRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(ConsumerPrefsRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(ConsumerPrefsRecordset, CRecordset)
 	long	m_SegmentID;
	long	m_ProdAttrID;
	CTime	m_StartDate;
	float	m_PreValue;
	float	m_PostValue;
	float	m_PriceSensitivity;
	//}}AFX_FIELD


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(ConsumerPrefsRecordset)
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
