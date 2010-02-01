// ----------------------
//
// Created 11/20/2003
// Vicki de Mey, DecisionPower, Inc.
//
// ----------------------


#include "DBProdAttr.h"
#include "DBModel.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(ProdAttrRecordset, CRecordset)

ProdAttrRecordset::ProdAttrRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(ProdAttrRecordset)
	m_ProductAttrID = 1;
	m_ProductAttrName = _T("");
	m_UtilityMin = 0.0;
	m_UtilityMax = 0.0;
	m_PreferenceMin = 0.0;
	m_PreferenceMax = 0.0;
	m_CustTau = 0.0;
	m_Type = 0;
	m_InitialAwareness = 0;

	m_nFields = 9;
	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString ProdAttrRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString ProdAttrRecordset::GetDefaultSQL()
{
	CString tmp("SELECT product_attribute_id, product_attribute_name, utility_max, utility_min, cust_pref_max, cust_pref_min, cust_tau, type, initial_awareness FROM product_attribute ");
	tmp += ModelRecordset::ModelQuery;

	return tmp;
}

void ProdAttrRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(ProdAttrRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Long(pFX, _T("[product_attribute_id]"), m_ProductAttrID);
	RFX_Text(pFX, _T("[product_attribute_name]"), m_ProductAttrName);
	RFX_Single(pFX, _T("[utility_max]"), m_UtilityMax);
	RFX_Single(pFX, _T("[utility_min]"), m_UtilityMin);
	RFX_Single(pFX, _T("[cust_pref_max]"), m_PreferenceMax);
	RFX_Single(pFX, _T("[cust_pref_min]"), m_PreferenceMin);
	RFX_Single(pFX, _T("[cust_tau]"), m_CustTau);
	RFX_Long(pFX, _T("[type]"), m_Type);
	RFX_Single(pFX, _T("[intial_awarenss]"), m_InitialAwareness);


	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void ProdAttrRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void ProdAttrRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int ProdAttrRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
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
