#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 11/20/200
// Vicki de Mey, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// MassMedia recordset

class MassMediaRecordset : public CRecordset
{
public:
	MassMediaRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(MassMediaRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(MassMediaRecordset, CRecordset)
  	long	m_ProductID;
	long	m_ChannelID;
	long	m_SegmentID;
	CString	m_Type;
	float	m_GValue;
	float	m_HValue;
	float	m_IValue;
	float	m_Awareness;
	float	m_Persuasion;
	CTime	m_StartDate;
	CTime	m_EndDate;
	long	m_Duration;
	//}}AFX_FIELD


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(MassMediaRecordset)
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
