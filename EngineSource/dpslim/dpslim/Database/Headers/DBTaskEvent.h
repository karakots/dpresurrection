#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 12/15/2004
// Steve Noble, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// Special events recordset

class TaskEventRecordset : public CRecordset
{
public:
	TaskEventRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(TaskEventRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(SpecialEventRecordset, CRecordset)

	long	m_SegmentID;
	long	m_TaskID; 
	CTime	m_StartDate;
	CTime	m_EndDate;
	float	m_DemandMod;
	//}}AFX_FIELD


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(SpecialEventRecordset)
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
