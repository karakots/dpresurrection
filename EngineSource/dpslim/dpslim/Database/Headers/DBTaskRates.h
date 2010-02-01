#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 11/24/2003
// Vicki de Mey, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// Task rates recordset

class TaskRatesRecordset : public CRecordset
{
public:
	TaskRatesRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(TaskRatesRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(TaskRatesRecordset, CRecordset)
  	long	m_SegmentID;
	long	m_TaskID;
	CTime	m_StartDate;
	CTime	m_EndDate;
	CString	m_TimePeriod;
	float	m_Rate;
	//}}AFX_FIELD


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(TaskRatesRecordset)
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
