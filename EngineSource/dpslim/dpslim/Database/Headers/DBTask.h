#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 11/24/200
// Vicki de Mey, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// Task recordset

class TaskRecordset : public CRecordset
{
public:
	TaskRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(TaskRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(TaskRecordset, CRecordset)
 	long	m_TaskID;
	CString	m_TaskName;
	CTime	m_StartDate;
	CTime	m_EndDate;
	float	m_SuitMin;
	float	m_SuitMax;
	//}}AFX_FIELD


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(TaskRecordset)
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
