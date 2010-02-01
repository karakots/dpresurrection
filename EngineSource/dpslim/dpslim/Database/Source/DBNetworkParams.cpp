// ----------------------
//
// Created 4/26/2005
// Steve Noble, DecisionPower, Inc.
//
// ----------------------


#include "DBNetworkParams.h"
#include "DBModel.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(NetworkParamsRecordset, CRecordset)

NetworkParamsRecordset::NetworkParamsRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(BrandRecordset)
	id = 0;
	type = 0;
	persuasion_pre_use = 0.0;
	persuasion_post_use = 0.0;
	awareness_weight = 0.0;
	num_contacts = 0.0;
	prob_of_talking_pre_use = 0.0;
	prob_of_talking_post_use = 0.0;
	use_local = false;
	percent_talking = 0.0;
	neg_persuasion_reject = 0.0;
	neg_persuasion_pre_use = 0.0;
	neg_persuasion_post_use = 0.0;

	// this nnumber must needs match how many above
	m_nFields = 13;

	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString NetworkParamsRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString NetworkParamsRecordset::GetDefaultSQL()
{
	CString tmp("SELECT id, type, persuasion_pre_use,persuasion_post_use, awareness_weight,num_contacts, prob_of_talking_pre_use, prob_of_talking_post_use, use_local, percent_talking, neg_persuasion_reject, neg_persuasion_pre_use, neg_persuasion_post_use FROM network_parameter ");
	tmp += ModelRecordset::ModelQuery;

	return tmp;
}

void NetworkParamsRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(BrandRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Long(pFX, _T("[id]"), id);
	RFX_Int(pFX, _T("[type]"), type);
	RFX_Single(pFX, _T("[persuasion_pre_use]"), persuasion_pre_use);
	RFX_Single(pFX, _T("[persuasion_post_use]"), persuasion_post_use);
	RFX_Single(pFX, _T("[awareness_weight]"), awareness_weight);
	RFX_Single(pFX, _T("[num_contacts]"), num_contacts);
	RFX_Single(pFX, _T("[prob_of_talking_pre_use]"), prob_of_talking_pre_use);
	RFX_Single(pFX, _T("[prob_of_talking_post_use]"), prob_of_talking_post_use);
	RFX_Bool(pFX, _T("[use_local]"), use_local);
	RFX_Single(pFX, _T("[percent_talking]"), percent_talking);
	RFX_Single(pFX, _T("[neg_persuasion_reject]"), neg_persuasion_reject);
	RFX_Single(pFX, _T("[neg_persuasion_pre_use]"), neg_persuasion_pre_use);
	RFX_Single(pFX, _T("[neg_persuasion_post_use]"), neg_persuasion_post_use);
	
	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void NetworkParamsRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void NetworkParamsRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int NetworkParamsRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
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
