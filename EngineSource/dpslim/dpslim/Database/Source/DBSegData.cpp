// ----------------------
//
// Created 3/8/2004
// Vicki de Mey, DecisionPower, Inc.
//
// ----------------------


#include "DBSegData.h"
#include "DBSimQueue.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

SegmentData::SegmentData()
{
	m_runID = -1;
	m_segmentID = 1;
  	m_productID = 1;
	m_channelID = 1;

	m_date = CTime();

	m_percent_aware_sku_cum = 0.0;
	m_persuasion_sku = 0.0;
	m_GRPs_SKU_tick = 0.0;
	m_promoprice = 0.0;
	m_unpromoprice = 0.0;
	m_sku_dollar_purchased_tick = 0.0;
	m_percent_preuse_distribution_sku = 0.0;
	m_percent_on_display_sku = 0.0;
	m_percent_sku_at_promo_price = 0.0;
	m_num_units_unpromo = 0.0;
	m_num_units_promo = 0.0;
	m_num_units_display = 0.0;
	m_display_price = 0.0;
	m_percent_at_display_price = 0.0;
	m_eq_units = 0.0;
	m_volume = 0.0;

	m_num_adds_sku = 0;
	m_num_drop_sku = 0;
	m_num_coupon_redemptions = 0;
	m_num_units_bought_on_coupon = 0;
	m_num_sku_bought = 0;
	m_num_sku_triers = 0;
	m_num_sku_repeaters = 0;
	m_num_sku_repeater_trips_cum = 0;
	num_trips = 0;
}

void SegDataRecordset::CopyFromSegmentData(const SegmentData& segData)
{
	m_runID = segData.m_runID;
	m_segmentID =segData.m_segmentID;
  	m_productID = segData.m_productID;
	m_channelID = segData.m_channelID;

	m_date = segData.m_date;

	m_percent_aware_sku_cum = segData.m_percent_aware_sku_cum;
	m_persuasion_sku = segData.m_persuasion_sku;
	m_GRPs_SKU_tick = segData.m_GRPs_SKU_tick;
	m_promoprice = segData.m_promoprice;
	m_unpromoprice = segData.m_unpromoprice;
	m_sku_dollar_purchased_tick = segData.m_sku_dollar_purchased_tick;
	m_percent_preuse_distribution_sku = segData.m_percent_preuse_distribution_sku;
	m_percent_on_display_sku = segData.m_percent_on_display_sku;
	m_percent_sku_at_promo_price = segData.m_percent_sku_at_promo_price;
	m_num_units_unpromo = segData.m_num_units_unpromo;
	m_num_units_promo = segData.m_num_units_promo;
	m_num_units_display = segData.m_num_units_display;
	m_display_price = segData.m_display_price;
	m_percent_at_display_price = segData.m_percent_at_display_price;
	m_eq_units = segData.m_eq_units;
	m_volume = segData.m_volume;

	m_num_adds_sku = segData.m_num_adds_sku;
	m_num_drop_sku = segData.m_num_drop_sku;
	m_num_coupon_redemptions = segData.m_num_coupon_redemptions;
	m_num_units_bought_on_coupon = segData.m_num_units_bought_on_coupon;
	m_num_sku_bought = segData.m_num_sku_bought;
	m_num_sku_triers = segData.m_num_sku_triers;
	m_num_sku_repeaters = segData.m_num_sku_repeaters;
	m_num_sku_repeater_trips_cum = segData.m_num_sku_repeater_trips_cum;
	num_trips = segData.num_trips;
}
/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(SegDataRecordset, CRecordset)

SegDataRecordset::SegDataRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(SegDataRecordset)

	m_runID = -1;
	m_segmentID = 1;
  	m_productID = 1;
	m_channelID = 1;

	m_date = CTime();

	m_percent_aware_sku_cum = 0.0;
	m_persuasion_sku = 0.0;
	m_GRPs_SKU_tick = 0.0;
	m_promoprice = 0.0;
	m_unpromoprice = 0.0;
	m_sku_dollar_purchased_tick = 0.0;
	m_percent_preuse_distribution_sku = 0.0;
	m_percent_on_display_sku = 0.0;
	m_percent_sku_at_promo_price = 0.0;
	m_num_sku_bought = 0.0;
	m_num_units_unpromo = 0.0;
	m_num_units_promo = 0.0;
	m_num_units_display = 0.0;
	m_display_price = 0.0;
	m_percent_at_display_price = 0.0;
	m_eq_units = 0.0;
	m_volume = 0.0;

	m_num_adds_sku = 0;
	m_num_drop_sku = 0;
	m_num_coupon_redemptions = 0;
	m_num_units_bought_on_coupon = 0;
	m_num_sku_triers = 0;
	m_num_sku_repeaters = 0;
	m_num_sku_repeater_trips_cum = 0;
	num_trips = 0;

	m_nFields = 30;
	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString SegDataRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString SegDataRecordset::GetDefaultSQL()
{
	// return getQuery("results_sts");

	// string::CopyToCString(&iTableName, byteName);

	CString tmp("SELECT run_id, segment_id,product_id,channel_id,calendar_date,percent_aware_sku_cum,persuasion_sku,GRPs_SKU_tick,promoprice,unpromoprice,sku_dollar_purchased_tick,percent_preuse_distribution_sku,percent_on_display_sku,percent_sku_at_promo_price,num_adds_sku,num_drop_sku,num_coupon_redemptions,num_units_bought_on_coupon,num_sku_bought,num_sku_triers,num_sku_repeaters,num_sku_repeater_trips_cum, num_trips, num_units_unpromo, num_units_promo, num_units_display, display_price, percent_at_display_price, eq_units, volume FROM results_std");
	tmp += SimQueueRecordset::RunQuery;
	
	return tmp;
}

void SegDataRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(SegDataRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Long(pFX, _T("[run_id]"),m_runID);
	RFX_Long(pFX, _T("[segment_id]"),m_segmentID);
	RFX_Long(pFX, _T("[product_id]"),m_productID);
	RFX_Long(pFX, _T("[channel_id]"),m_channelID);

	RFX_Date(pFX, _T("[calendar_date]"),m_date);

	RFX_Single(pFX, _T("[percent_aware_sku_cum]"),m_percent_aware_sku_cum);
	RFX_Single(pFX, _T("[persuasion_sku]"),m_persuasion_sku);
	RFX_Single(pFX, _T("[GRPs_SKU_tick]"),m_GRPs_SKU_tick);
	RFX_Single(pFX, _T("[promoprice]"),m_promoprice);
	RFX_Single(pFX, _T("[unpromoprice]"),m_unpromoprice);
	RFX_Single(pFX, _T("[sku_dollar_purchased_tick]"),m_sku_dollar_purchased_tick);
	RFX_Single(pFX, _T("[num_sku_bought]"),m_num_sku_bought);
	RFX_Single(pFX, _T("[percent_preuse_distribution_sku]"),m_percent_preuse_distribution_sku);
	RFX_Single(pFX, _T("[percent_on_display_sku]"),m_percent_on_display_sku);
	RFX_Single(pFX, _T("[percent_sku_at_promo_price]"),m_percent_sku_at_promo_price);
	RFX_Single(pFX, _T("[num_units_unpromo]"),m_num_units_unpromo);
	RFX_Single(pFX, _T("[num_units_promo]"),m_num_units_promo);
	RFX_Single(pFX, _T("[num_units_display]"),m_num_units_display);
	RFX_Single(pFX, _T("[display_price]"),m_display_price);
	RFX_Single(pFX, _T("[percent_at_display_price]"),m_percent_at_display_price);
	RFX_Single(pFX, _T("[eq_units]"),m_eq_units);
	RFX_Single(pFX, _T("[volume]"),m_volume);

	RFX_Long(pFX, _T("[num_adds_sku]"),m_num_adds_sku);
	RFX_Long(pFX, _T("[num_drop_sku]"),m_num_drop_sku);
	RFX_Long(pFX, _T("[num_coupon_redemptions]"),m_num_coupon_redemptions);
	RFX_Long(pFX, _T("[num_units_bought_on_coupon]"),m_num_units_bought_on_coupon);
	RFX_Long(pFX, _T("[num_sku_triers]"),m_num_sku_triers);
	RFX_Long(pFX, _T("[num_sku_repeaters]"),m_num_sku_repeaters);
	RFX_Long(pFX, _T("[num_sku_repeater_trips_cum]"),m_num_sku_repeater_trips_cum);
	RFX_Long(pFX, _T("[num_trips]"), num_trips);

	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);

}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void SegDataRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void SegDataRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int SegDataRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
{
    // Override the default behavior of the Open member function. We want to
    //  ensure that the record set executes on the SQL Server default
    //  (blocking) statement type.
    nOpenType = CRecordset::snapshot; //forwardOnly;
    dwOptions = CRecordset::appendOnly | optimizeBulkAdd | CRecordset::executeDirect; // allow writing in bulk
	// dwOptions = CRecordset::none | skipDeletedRecords;

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
void SegDataRecordset::DeleteAll(void) 
{
	;
}

