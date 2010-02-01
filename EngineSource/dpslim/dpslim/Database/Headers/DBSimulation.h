#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 01/12/2007
// Isaac Noble, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes

class SimulationRecordset : public CRecordset
{
public:
	SimulationRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(SimulationRecordset)

	CTime	start_date;
	CTime	end_date;

	long	type;

	long	access_time;

	double	scale_factor;

	long	scenario_id;

	int	reset_panel_state;


	public:
	virtual CString GetDefaultConnect();    // Default connection string
	virtual CString GetDefaultSQL();    // Default SQL for Recordset
	virtual void DoFieldExchange(CFieldExchange* pFX);  // RFX support
	virtual int Open(unsigned int nOpenType = snapshot, LPCTSTR lpszSql = NULL, DWORD dwOptions = none);

	static char SimulationQuery[256];

#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif
};
