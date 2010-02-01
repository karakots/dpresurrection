#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 10/25/2004
// Vicki de Mey, DecisionPower, Inc.
//
/*
TABLE [optimization_plan]
	[scenario_id] [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	[optimize_for] [tinyint] NOT NULL,
	[num_steps] [int] NOT NULL,
	[mode_exec_id] [tinyint] NOT NULL,
	[explore_mode_id] [tinyint]
*/

#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// ScenarioParams recordset

class OptimizationPlanRecordset : public CRecordset
{
public:
	OptimizationPlanRecordset(CDatabase* pDatabase = NULL, int modelID = 0);
	DECLARE_DYNAMIC(OptimizationPlanRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(OptimizationPlanRecordset, CRecordset)
	long	m_ScenarioID;
	int		m_OptimizeFor;
	int		m_NumSteps;
	int		m_ModeExecID;
	int		m_ExploreModeID;
	//}}AFX_FIELD

	// instance variables
	int i_modelID;
	//int i_scenarioID;

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(OptimizationPlanRecordset)
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
