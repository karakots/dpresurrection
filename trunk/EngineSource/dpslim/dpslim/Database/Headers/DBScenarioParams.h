#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 10/25/2004
// Vicki de Mey, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// ScenarioParams recordset

class OptimizationParamsRecordset : public CRecordset
{
public:
	OptimizationParamsRecordset(CDatabase* pDatabase = NULL, int modelID = 0, int scenarioID = 0);
	DECLARE_DYNAMIC(OptimizationParamsRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(OptimizationParamsRecordset, CRecordset)
 	long	m_ModelID;
	long	m_ScenarioID;
	CString	m_Component_name;
	CString	m_Param;
	float	m_Lower;
	float	m_Upper;
	int		m_Leader;
	int		m_Slave;
	//}}AFX_FIELD

	// instance variables
	int i_modelID;
	int i_scenarioID;

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(OptimizationParamsRecordset)
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
