#pragma once

// ----------------------
//
// Created 1/11/2005
// Steve Noble, DecisionPower, Inc.
//
// ----------------------


#include "DBSimQueue.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

char SimQueueRecordset::RunQuery[128] = "";

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(SimQueueRecordset, CRecordset)

SimQueueRecordset::SimQueueRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(BrandRecordset)
	m_RunID = 1;
	m_SimulationID = 1;
	m_ModelID = 1;
	m_Status = 1;
	m_Num = 0;
	current_status = _T("");
	elapsed_time = 0;
	current_date = CTime();
	run_time = CTime();
	seed = 1;

	// this nnumber must needs match how many above
	m_nFields = 10;

	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString SimQueueRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString SimQueueRecordset::GetDefaultSQL()
{
	CString tmp("SELECT run_id, sim_id, model_id, status, num, current_status, elapsed_time, [current_date], run_time, seed FROM sim_queue");
	tmp += RunQuery;

	return tmp;
}

void SimQueueRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(BrandRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);
	RFX_Long(pFX, _T("[run_id]"), m_RunID);
	RFX_Long(pFX, _T("[sim_id]"), m_SimulationID);
	RFX_Long(pFX, _T("[model_id]"), m_ModelID);
	RFX_Int(pFX, _T("[status]"), m_Status);
	RFX_Long(pFX, _T("[num]"), m_Num);
	RFX_Text(pFX, _T("[current_status]"), current_status);
	RFX_Long(pFX, _T("elapsed_time"), elapsed_time);
	RFX_Date(pFX, _T("[current_date]"), current_date);
	RFX_Date(pFX, _T("[run_time]"), run_time);
	RFX_Int(pFX, _T("[seed]"), seed);
	
	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void SimQueueRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void SimQueueRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int SimQueueRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
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
