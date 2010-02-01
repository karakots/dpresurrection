// ----------------------
//
// Created 12/15/2004
// Steven Noble, DecisionPower, Inc.
//
// ----------------------


#include "DBProductEvent.h"
#include "DBModel.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(ProductEventRecordset, CRecordset)

ProductEventRecordset::ProductEventRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(ProductEventRecordset)

	m_SegmentID = 1;
	m_ChannelID = 1;
	m_ProductID = 1;
	m_StartDate = CTime();
	m_EndDate = CTime();
	m_DemandMod = 0.0;

	m_nFields = 6;
	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString ProductEventRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString ProductEventRecordset::GetDefaultSQL()
{
	CString tmp("SELECT product_event.segment_id,product_event.channel_id,product_event.product_id,product_event.start_date,product_event.end_date, parm1 * demand_modification FROM product_event, market_plan ");
	tmp += ModelRecordset::ExtFactQuery;

	return tmp;
}

void ProductEventRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(ProductEventRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Long(pFX, _T("[segment_id]"), m_SegmentID);
	RFX_Long(pFX, _T("[channel_id]"), m_ChannelID);
	RFX_Long(pFX, _T("[product_id]"), m_ProductID);
	
	RFX_Date(pFX, _T("[start_date]"), m_StartDate);
	RFX_Date(pFX, _T("[end_date]"), m_EndDate);
	RFX_Single(pFX, _T("[demand_modification]"), m_DemandMod);
	

	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void ProductEventRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void ProductEventRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int ProductEventRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
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
