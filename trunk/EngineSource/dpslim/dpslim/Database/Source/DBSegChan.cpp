// ----------------------
//
// Created 11/20/2003
// Vicki de Mey, DecisionPower, Inc.
//
// ----------------------


#include "DBSegChan.h"
#include "DBModel.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(SegChanRecordset, CRecordset)

SegChanRecordset::SegChanRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(SegChanRecordset)

	m_SegmentID = 1;
	m_ChannelID = 1;
	m_ProbOfChoice = 0.0;

	m_nFields = 3;
	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString SegChanRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString SegChanRecordset::GetDefaultSQL()
{
	CString tmp("SELECT segment_id,channel_id,probability_of_choice FROM segment_channel ");
	tmp += ModelRecordset::ModelQuery;

	return tmp;
}

void SegChanRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(SegChanRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Long(pFX, _T("[segment_id]"), m_SegmentID);
	RFX_Long(pFX, _T("[channel_id]"), m_ChannelID);
	RFX_Single(pFX, _T("[probability_of_choice]"), m_ProbOfChoice);

	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void SegChanRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void SegChanRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int SegChanRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
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
