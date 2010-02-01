#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 11/20/200
// Vicki de Mey, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// DistDisplay recordset

class DistDisplayRecordset : public CRecordset
{
public:
	DistDisplayRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(DistDisplayRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(DistDisplayRecordset, CRecordset)
  	long	m_ProductID;
	long	m_ChannelID;
	CString	m_Type;
	float	m_FValue;
	float	m_GValue;
	float	m_Awareness;
	float	m_Persuasion;
	CTime	m_StartDate;
	CTime	m_EndDate;
	long	m_Duration;
	//}}AFX_FIELD


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(DistDisplayRecordset)
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
