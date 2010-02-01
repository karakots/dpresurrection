// ----------------------
//
// Created 11/20/2003
// Vicki de Mey, DecisionPower, Inc.
//
// ----------------------


#include "DBProdChan.h"
#include "DBScenario.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(ProdChanRecordset, CRecordset)

ProdChanRecordset::ProdChanRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(ProdChanRecordset)

	m_ProductID = 1;
	m_ChannelID = 1;
	m_Markup = 0.0;
	m_Price = 0.0;
	m_SkusAtPrice = 0.0;
	m_PeriodicPrice = 0.0;
	m_HowOften = _T("");
	m_PType = _T("");
	m_StartDate = CTime();
	m_EndDate = CTime();
	m_Duration = 0;
	m_nFields = 11;
	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString ProdChanRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString ProdChanRecordset::GetDefaultSQL()
{
	CString tmp("SELECT  product_id, channel_id, parm2 * markup as markup, parm1 * price as price, parm4 * percent_SKU_in_dist as percent_SKU_in_dist, parm3 * periodic_price as periodic_price, how_often, ptype, start_date, end_date,duration FROM scenario_product_channel ");
	tmp += ScenarioRecordset::ScenarioQuery;

	return tmp;
}

void ProdChanRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(ProdChanRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Long(pFX, _T("[product_id]"), m_ProductID);
	RFX_Long(pFX, _T("[channel_id]"), m_ChannelID);
	RFX_Single(pFX, _T("[markup]"), m_Markup);
	RFX_Single(pFX, _T("[price]"), m_Price);
	RFX_Single(pFX, _T("[percent_SKU_in_dist]"), m_SkusAtPrice);
	RFX_Single(pFX, _T("[periodic_price]"), m_PeriodicPrice);
	RFX_Text(pFX, _T("[how_often]"), m_HowOften);
	RFX_Text(pFX, _T("[ptype]"), m_PType);
	RFX_Date(pFX, _T("[start_date]"), m_StartDate);
	RFX_Date(pFX, _T("[end_date]"), m_EndDate);
	RFX_Long(pFX, _T("[duration]"), m_Duration);

	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void ProdChanRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void ProdChanRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int ProdChanRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
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
