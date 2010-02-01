#pragma once

// ----------------------
//
// Created 1/12/2007
// Isaac Noble, DecisionPower, Inc.
//
// ----------------------


#include "DBSimulation.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

char SimulationRecordset::SimulationQuery[256] = "";

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(SimulationRecordset, CRecordset)

SimulationRecordset::SimulationRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(BrandRecordset)
	scenario_id = 0;
	start_date = CTime();
	end_date = CTime();
	type = 0;
	access_time = 1;
	scale_factor = 1;
	reset_panel_state = false;

	// this nnumber must needs match how many above
	m_nFields = 7;

	//}}AFX_FIELD_INIT

	m_nDefaultType = CRecordset::snapshot;
}

CString SimulationRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString SimulationRecordset::GetDefaultSQL()
{
	CString tmp("SELECT start_date, end_date, type, access_time, scale_factor, scenario_id, reset_panel_state FROM simulation ");
	tmp += SimulationQuery;

	return tmp;
}

void SimulationRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(BrandRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);
	RFX_Date(pFX, _T("[start_date]"), start_date);
	RFX_Date(pFX, _T("[end_date]"), end_date);
	RFX_Long(pFX, _T("[type]"), type);
	RFX_Long(pFX, _T("[access_time]"), access_time);
	RFX_Double(pFX, _T("[scale_factor]"), scale_factor);
	RFX_Long(pFX, _T("[scenario_id]"), scenario_id);
	RFX_Bool(pFX, _T("[reset_panel_state]"), reset_panel_state);
	
	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void SimulationRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void SimulationRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int SimulationRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
{
    // Override the default behavior of the Open member function. We want to
    //  ensure that the record set executes on the SQL Server default
    //  (blocking) statement type.
    nOpenType = CRecordset::snapshot; //forwardOnly;
    dwOptions = CRecordset::none;

	try {
		CRecordset::Open(nOpenType, GetDefaultSQL(), dwOptions);
	}
	catch (CException* e)
    {
        e->Delete();
		return FALSE;
    }

	return TRUE;
}
