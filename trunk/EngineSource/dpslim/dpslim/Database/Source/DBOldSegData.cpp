// ----------------------
//
// Created 3/8/2004
// Vicki de Mey, DecisionPower, Inc.
//
// ----------------------


#include "DBOldSegData.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(OldSegDataRecordset, CRecordset)

OldSegDataRecordset::OldSegDataRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(OldSegDataRecordset)

	m_SegmentID = 1;
  	m_ProductID = 1;
	m_ChannelID = 1;

	m_Date = CTime();

	m_Price = 0.0;
	m_PercentAware = 0.0;
	m_AvgMsgPerPerson = 0.0;
	m_PercentAwareBrand = 0.0;
	
	m_NumBought = 0;
	m_NumAdds = 0;
	m_NumDrops = 0;
	m_NumEverTried = 0;
	m_NumFirstTimeBuyers = 0;
	m_NumCouponRedempt = 0;
	m_NumUnitsCoupon = 0;
	m_CouponsToBeRedempt = 0;
	m_NumEverTriedBrand = 0;
	m_FirstTimeBrand = 0;
	m_NumErrorCount = 0;

	m_nFields = 19;
	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString OldSegDataRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString OldSegDataRecordset::GetDefaultSQL()
{
	char	byteName[255];

	string::CopyToCString(&iTableName, byteName);

	CString tmp("SELECT segment_id,product_id,channel_id,calendar_date,price,percent_aware,avg_msgs_per_pers,percent_aware_of_brand,num_bought,num_adds,num_drop,num_ever_tried,num_first_time_buyers,num_coupon_redemptions,num_units_coupon,coupons_tb_redeemed,num_ever_tried_brand_cum,num_one_time_brand_buy_this_tick,error_count FROM ");

	tmp += byteName;
	
	return tmp;
}

void OldSegDataRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(OldSegDataRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Long(pFX, _T("[segment_id]"), m_SegmentID);
	RFX_Long(pFX, _T("[product_id]"), m_ProductID);
	RFX_Long(pFX, _T("[channel_id]"), m_ChannelID);

	RFX_Date(pFX, _T("[calendar_date]"), m_Date);

	RFX_Single(pFX, _T("[price]"), m_Price);
	RFX_Single(pFX, _T("[percent_aware]"), m_PercentAware);
	RFX_Single(pFX, _T("[avg_msgs_per_pers]"), m_AvgMsgPerPerson);
	RFX_Single(pFX, _T("[percent_aware_of_brand]"), m_PercentAwareBrand);

	RFX_Long(pFX, _T("[num_bought]"), m_NumBought);
	RFX_Long(pFX, _T("[num_adds]"), m_NumAdds);
	RFX_Long(pFX, _T("[num_drop]"), m_NumDrops);
	RFX_Long(pFX, _T("[num_ever_tried]"), m_NumEverTried);
	RFX_Long(pFX, _T("[num_first_time_buyers]"), m_NumFirstTimeBuyers);
	RFX_Long(pFX, _T("[num_coupon_redemptions]"), m_NumCouponRedempt);
	RFX_Long(pFX, _T("[num_units_coupon]"), m_NumUnitsCoupon);
	RFX_Long(pFX, _T("[coupons_tb_redeemed]"), m_CouponsToBeRedempt);
	RFX_Long(pFX, _T("[num_ever_tried_brand_cum]"), m_NumEverTriedBrand);
	RFX_Long(pFX, _T("[num_one_time_brand_buy_this_tick]"), m_FirstTimeBrand);
	RFX_Long(pFX, _T("[error_count]"), m_NumErrorCount);

	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);

}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void OldSegDataRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void OldSegDataRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int OldSegDataRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
{
   // Override the default behavior of the Open member function. We want to
    //  ensure that the record set executes on the SQL Server default
    //  (blocking) statement type.
    nOpenType = CRecordset::snapshot; //forwardOnly;
    dwOptions = CRecordset::appendOnly | optimizeBulkAdd; //readOnly;

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

// ------------------------
void OldSegDataRecordset::DeleteAll(void) 
{
	;
}

void OldSegDataRecordset::InitTableName(string* tbl)
{
	string::StrCpy(tbl, &iTableName);
}
