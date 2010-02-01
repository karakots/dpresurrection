#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 11/3/2004
// Vicki de Mey, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// Scenario Ourput summary recordset

class OptimizationOutSumRecordset : public CRecordset
{
public:
	OptimizationOutSumRecordset(CDatabase* pdb, int modelID = 0, int scenarioID = 0);
	DECLARE_DYNAMIC(OptimizationOutSumRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(OptimizationOutSumRecordset, CRecordset)
 	long	m_ModelID;
	long	m_ScenarioID;
	long	m_TrialID;
	long	m_RunID;
	int		m_OptimizeFor;
	float	m_RunValue;
	CString	m_Component_name;
	CString	m_Param;
	float	m_ParamValue;
	//}}AFX_FIELD

	// instance variables
	int i_modelID;
	int i_scenarioID;
	int i_trialID;
	int i_runID;

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(OptimizationOutSumRecordset)
	public:
	virtual CString GetDefaultConnect();    // Default connection string
	virtual CString GetDefaultSQL();    // Default SQL for Recordset
	virtual void DoFieldExchange(CFieldExchange* pFX);  // RFX support
	virtual BOOL Open(UINT nOpenType = snapshot, LPCTSTR lpszSql = NULL, DWORD dwOptions = none);
	//}}AFX_VIRTUAL

// Implementation
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

	void DeleteAll(void);

};
