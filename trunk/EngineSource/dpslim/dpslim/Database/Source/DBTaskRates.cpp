// ----------------------
//
// Created 11/20/2003
// Vicki de Mey, DecisionPower, Inc.
//
// ----------------------


#include "DBTaskRates.h"
#include "DBModel.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(TaskRatesRecordset, CRecordset)

TaskRatesRecordset::TaskRatesRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(TaskRatesRecordset)

	m_SegmentID = 1;
	m_TaskID = 0;
	m_StartDate = CTime();
	m_EndDate = CTime();
	m_TimePeriod = _T("");
	m_Rate = 0.0;

	m_nFields = 6;
	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString TaskRatesRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString TaskRatesRecordset::GetDefaultSQL()
{
	CString tmp("SELECT segment_id, task_id, start_date, end_date, time_period, task_rate FROM task_rate_fact ");
	tmp += ModelRecordset::ModelQuery;

	return tmp;
}

void TaskRatesRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(TaskRatesRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Long(pFX, _T("[segment_id]"), m_SegmentID);
	RFX_Long(pFX, _T("[task_id]"), m_TaskID);
	RFX_Date(pFX, _T("[start_date]"), m_StartDate);
	RFX_Date(pFX, _T("[end_date]"), m_EndDate);
	RFX_Text(pFX, _T("[time_period]"), m_TimePeriod);
	RFX_Single(pFX, _T("[task_rate]"), m_Rate);


	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void TaskRatesRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void TaskRatesRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int TaskRatesRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
{
    // Override the default behavior of the Open member function. We want to
    //  ensure that the record set executes on the SQL Server default
    //  (blocking) statement type.
    nOpenType = CRecordset::snapshot; //forwardOnly;
    dwOptions = CRecordset::readOnly;

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
