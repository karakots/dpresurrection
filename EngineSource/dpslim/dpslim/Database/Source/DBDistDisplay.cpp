// ----------------------
//
// Created 11/20/2003
// Vicki de Mey, DecisionPower, Inc.
//
// ----------------------


#include "DBDistDisplay.h"
#include "DBScenario.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(DistDisplayRecordset, CRecordset)

DistDisplayRecordset::DistDisplayRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(DistDisplayRecordset)

	m_ProductID = 1;
	m_ChannelID = 1;
	m_Type = _T("");
	m_FValue = 0.0;
	m_GValue = 0.0;
	m_Awareness = 0.0;
	m_Persuasion = 0.0;
	m_StartDate = CTime();
	m_EndDate = CTime();
	m_Duration = 0;

	m_nFields = 10;
	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString DistDisplayRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString DistDisplayRecordset::GetDefaultSQL()
{
	CString tmp("SELECT  product_id,  channel_id, media_type, parm3 * attr_value_F as attr_value_F, parm4 * attr_value_G as attr_value_G, parm1 * message_awareness_probability as message_awareness_probability, parm2 * message_persuation_probability as message_persuation_probability, start_date, end_date, duration FROM scenario_dist_display");
	tmp += ScenarioRecordset::ScenarioQuery;

	return tmp;
}

void DistDisplayRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(DistDisplayRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Long(pFX, _T("[product_id]"), m_ProductID);
	RFX_Long(pFX, _T("[channel_id]"), m_ChannelID);
	RFX_Text(pFX, _T("[media_type]"), m_Type);
	RFX_Single(pFX, _T("[attr_value_F]"), m_FValue);
	RFX_Single(pFX, _T("[attr_value_G]"), m_GValue);
	RFX_Single(pFX, _T("[message_awareness_probability]"), m_Awareness);
	RFX_Single(pFX, _T("[message_persuation_probability]"), m_Persuasion);
	RFX_Date(pFX, _T("[start_date]"), m_StartDate);
	RFX_Date(pFX, _T("[end_date]"), m_EndDate);
	RFX_Long(pFX, _T("[duration]"), m_Duration);


	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void DistDisplayRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void DistDisplayRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int DistDisplayRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
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
