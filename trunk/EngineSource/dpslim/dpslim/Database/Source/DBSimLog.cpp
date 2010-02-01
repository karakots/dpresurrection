#pragma once

// ----------------------
//
// Created 1/11/2005
// Steve Noble, DecisionPower, Inc.
//
// ----------------------


#include "DBSimLog.h"
#include "DBSimQueue.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(SimLogRecordset, CRecordset)

SimLogRecordset::SimLogRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(BrandRecordset)
	run_id = 1;
	calendar_date = CTime();
	product_id = -1;
	segment_id = -1;
	channel_id = -1;
	comp_id = 0;
	message_id = -1;
	message = _T("");

	// this nnumber must needs match how many above
	m_nFields = 8;

	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString SimLogRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString SimLogRecordset::GetDefaultSQL()
{
	CString tmp("SELECT run_id, calendar_date, product_id, segment_id, channel_id, comp_id, message_id, message FROM run_log");
	tmp += SimQueueRecordset::RunQuery;
	return tmp;
}

void SimLogRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(BrandRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);
	RFX_Long(pFX, _T("[run_id]"), run_id);
	RFX_Date(pFX, _T("[calendar_date]"), calendar_date);
	RFX_Long(pFX, _T("[product_id]"), product_id);
	RFX_Long(pFX, _T("[segment_id]"), segment_id);
	RFX_Long(pFX, _T("[channel_id]"), channel_id);
	RFX_Long(pFX, _T("[comp_id]"), comp_id);
	RFX_Long(pFX, _T("[message_id]"), message_id);
	RFX_Text(pFX, _T("[message]"), message);
	
	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void SimLogRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void SimLogRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int SimLogRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
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
