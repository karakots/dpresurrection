
// ----------------------
//
// Created 3/8/2004
// Vicki de Mey, DecisionPower, Inc.
//
// ----------------------


#include "DBOptSegData.h"
#include "DBModel.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(OptSegDataRecordset, CRecordset)

OptSegDataRecordset::OptSegDataRecordset(CDatabase* pdb, int scenarioID)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(SegDataRecordset)
	m_scenarioID = -1;
	m_trialID = -1;
	m_runID = -1;

	m_modelID = -1;
	m_segmentID = 1;
  	m_productID = 1;
	m_channelID = 1;

	m_date = CTime();

	m_percent_aware_sku_cum = 0.0;
	m_percent_aware_brand_cum = 0.0;
	m_persuasion_sku = 0.0;
	m_avg_brand_transaction_size_dollars = 0.0;
	m_brand_dollars_purchased_tick = 0.0;
	m_GRPs_SKU_tick = 0.0;
	m_percent_distribution_brand = 0.0;
	m_promoprice = 0.0;
	m_unpromoprice = 0.0;
	m_avg_sku_transaction_dollars = 0.0;
	m_sku_dollar_purchased_tick = 0.0;
	m_percent_preuse_distribution_sku = 0.0;
	m_percent_on_display_sku = 0.0;
	m_percent_sku_at_promo_price = 0.0;

	m_num_adds_sku = 0;
	m_num_drop_sku = 0;
	m_num_adds_brand = 0;
	m_num_drop_brand = 0;
	m_num_brand_triers_cum = 0;
	m_num_brand_repeaters_cum = 0;
	m_num_brand_repeater_trips_cum = 0;
	m_num_coupon_redemptions = 0;
	m_num_units_bought_on_coupon = 0;
	m_num_sku_bought = 0;
	m_num_sku_triers = 0;
	m_num_sku_repeaters = 0;
	m_num_sku_repeater_trips_cum = 0;

	m_nFields = 35;
	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
	iScenarioID = scenarioID;
}

CString OptSegDataRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString OptSegDataRecordset::GetDefaultSQL()
{
	CString tmp("SELECT model_id, scenario_id, trial_id, run_id, segment_id,product_id,channel_id,calendar_date,percent_aware_sku_cum,percent_aware_brand_cum,persuasion_sku,avg_brand_transaction_size_dollars,brand_dollars_purchased_tick,GRPs_SKU_tick,percent_distribution_brand,promoprice,unpromoprice,avg_sku_transaction_dollars,sku_dollar_purchased_tick,percent_preuse_distribution_sku,percent_on_display_sku,percent_sku_at_promo_price,num_adds_sku,num_drop_sku,num_adds_brand,num_drop_brand,num_brand_triers_cum,num_brand_repeaters_cum,num_brand_repeater_trips_cum,num_coupon_redemptions,num_units_bought_on_coupon,num_sku_bought,num_sku_triers,num_sku_repeaters,num_sku_repeater_trips_cum FROM optimization_results_std ");
	//tmp += ModelRecordset::ModelQuery;
	CString str;
	str.Format(_T("WHERE scenario_id = %d"), iScenarioID);	

	tmp += str;
	
	return tmp;
}

void OptSegDataRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(SegDataRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Long(pFX, _T("[model_id]"), m_modelID);
	RFX_Long(pFX, _T("[scenario_id]"), m_scenarioID);
	RFX_Long(pFX, _T("[trial_id]"), m_trialID);
	RFX_Long(pFX, _T("[run_id]"), m_runID);
	RFX_Long(pFX, _T("[segment_id]"), m_segmentID);
	RFX_Long(pFX, _T("[product_id]"), m_productID);
	RFX_Long(pFX, _T("[channel_id]"), m_channelID);

	RFX_Date(pFX, _T("[calendar_date]"), m_date);

	RFX_Single(pFX, _T("[percent_aware_sku_cum]"), m_percent_aware_sku_cum);
	RFX_Single(pFX, _T("[percent_aware_brand_cum]"), m_percent_aware_brand_cum);
	RFX_Single(pFX, _T("[persuasion_sku]"), m_persuasion_sku);
	RFX_Single(pFX, _T("[avg_brand_transaction_size_dollars]"), m_avg_brand_transaction_size_dollars);
	RFX_Single(pFX, _T("[brand_dollars_purchased_tick]"), m_brand_dollars_purchased_tick);
	RFX_Single(pFX, _T("[GRPs_SKU_tick]"), m_GRPs_SKU_tick);
	RFX_Single(pFX, _T("[percent_distribution_brand]"), m_percent_distribution_brand);
	RFX_Single(pFX, _T("[promoprice]"), m_promoprice);
	RFX_Single(pFX, _T("[unpromoprice]"), m_unpromoprice);
	RFX_Single(pFX, _T("[avg_sku_transaction_dollars]"), m_avg_sku_transaction_dollars);
	RFX_Single(pFX, _T("[sku_dollar_purchased_tick]"), m_sku_dollar_purchased_tick);
	RFX_Single(pFX, _T("[percent_preuse_distribution_sku]"), m_percent_preuse_distribution_sku);
	RFX_Single(pFX, _T("[percent_on_display_sku]"), m_percent_on_display_sku);
	RFX_Single(pFX, _T("[percent_sku_at_promo_price]"), m_percent_sku_at_promo_price);

	RFX_Long(pFX, _T("[num_adds_sku]"), m_num_adds_sku);
	RFX_Long(pFX, _T("[num_drop_sku]"), m_num_drop_sku);
	RFX_Long(pFX, _T("[num_adds_brand]"), m_num_adds_brand);
	RFX_Long(pFX, _T("[num_drop_brand]"), m_num_drop_brand);
	RFX_Long(pFX, _T("[num_brand_triers_cum]"), m_num_brand_triers_cum);
	RFX_Long(pFX, _T("[num_brand_repeaters_cum]"), m_num_brand_repeaters_cum);
	RFX_Long(pFX, _T("[num_brand_repeater_trips_cum]"), m_num_brand_repeater_trips_cum);
	RFX_Long(pFX, _T("[num_coupon_redemptions]"), m_num_coupon_redemptions);
	RFX_Long(pFX, _T("[num_units_bought_on_coupon]"), m_num_units_bought_on_coupon);
	RFX_Long(pFX, _T("[num_sku_bought]"), m_num_sku_bought);
	RFX_Long(pFX, _T("[num_sku_triers]"), m_num_sku_triers);
	RFX_Long(pFX, _T("[num_sku_repeaters]"), m_num_sku_repeaters);
	RFX_Long(pFX, _T("[num_sku_repeater_trips_cum]"), m_num_sku_repeater_trips_cum);

	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);

}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void OptSegDataRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void OptSegDataRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int OptSegDataRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
{
    // Override the default behavior of the Open member function. We want to
    //  ensure that the record set executes on the SQL Server default
    //  (blocking) statement type.
    nOpenType = CRecordset::snapshot; //forwardOnly;
    //dwOptions = CRecordset::appendOnly | optimizeBulkAdd; // allow writing in bulk
	dwOptions = CRecordset::none | skipDeletedRecords; 

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
void OptSegDataRecordset::DeleteAll(void) 
{
	;
}

