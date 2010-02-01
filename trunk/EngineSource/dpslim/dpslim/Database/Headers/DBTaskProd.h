#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 11/24/200
// Vicki de Mey, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// Task product recordset

class TaskProdRecordset : public CRecordset
{
public:
	TaskProdRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(TaskProdRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(TaskProdRecordset, CRecordset)
  	long	m_ProductID;
	long	m_TaskID;

	float	m_PreUseSKU;
	float	m_PostUseSKU;
	float	m_Suitability;
	//}}AFX_FIELD


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(TaskProdRecordset)
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
