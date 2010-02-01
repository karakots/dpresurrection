// ----------------------
//
// Created 2/20/2006
// Isaac Noble, DecisionPower, Inc.
//
// ----------------------


#include "DBMarketUtility.h"
#include "DBModel.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(MarketUtilityRecordset, CRecordset)

MarketUtilityRecordset::MarketUtilityRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(MarketUtilityRecordset)

	m_ProductID = 1;
	m_ChannelID = 1;
	m_SegmentID = 1;
	m_Percent_Dist = 0.0;
	m_Awareness = 0.0;
	m_Persuasion = 0.0;
	m_Utility = 0.0;
	m_StartDate = CTime();
	m_EndDate = CTime();

	m_nFields = 9;
	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString MarketUtilityRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString MarketUtilityRecordset::GetDefaultSQL()
{
	CString tmp("SELECT market_utility.product_id, market_utility.channel_id, market_utility.segment_id, market_plan.parm4 * market_utility.percent_dist as percent_dist, market_plan.parm1 * market_utility.awareness as awareness, market_plan.parm2 * market_utility.persuasion as persuasion, market_plan.parm3 * market_utility.utility as utility, market_utility.start_date, market_utility.end_date FROM market_utility, market_plan ");
	tmp += ModelRecordset::MarketPlanQuery;

	return tmp;
}

void MarketUtilityRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(MarketUtilityRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Long(pFX, _T("[product_id]"), m_ProductID);
	RFX_Long(pFX, _T("[channel_id]"), m_ChannelID);
	RFX_Long(pFX, _T("[segment_id]"), m_SegmentID);
	RFX_Single(pFX, _T("[percent_dist]"), m_Percent_Dist);
	RFX_Single(pFX, _T("[awareness]"), m_Awareness);
	RFX_Single(pFX, _T("[persuasion]"), m_Persuasion);
	RFX_Single(pFX, _T("[utility]"), m_Utility);
	RFX_Date(pFX, _T("[start_date]"), m_StartDate);
	RFX_Date(pFX, _T("[end_date]"), m_EndDate);


	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void MarketUtilityRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void MarketUtilityRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int MarketUtilityRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
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
