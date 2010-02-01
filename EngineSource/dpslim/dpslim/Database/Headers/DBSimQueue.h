#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 01/11/2005
// Vicki de Mey, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// Brand recordset

class SimQueueRecordset : public CRecordset
{
public:
	SimQueueRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(SimQueueRecordset)

// Field/Param Data
	//{{AFX_FIELD(BrandRecordset, CRecordset)

 	long	m_RunID;
	long	m_SimulationID;
	long	m_ModelID;
	int		m_Status;
	long	m_Num;
	CString current_status;
	long	elapsed_time;
	CTime	current_date;
	CTime	run_time;
	int		seed;
	

	//}}AFX_FIELD


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(BrandRecordset)
	public:
	virtual CString GetDefaultConnect();    // Default connection string
	virtual CString GetDefaultSQL();    // Default SQL for Recordset
	virtual void DoFieldExchange(CFieldExchange* pFX);  // RFX support
	virtual int Open(unsigned int nOpenType = snapshot, LPCTSTR lpszSql = NULL, DWORD dwOptions = none);
	//}}AFX_VIRTUAL

// Implementation

	static char RunQuery[128];

#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif
};
