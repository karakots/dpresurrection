#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 3/8/2004
// Vicki de Mey, DecisionPower, Inc.
//

#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// Results recordset

class OptSegDataRecordset : public CRecordset
{
public:
	OptSegDataRecordset(CDatabase* pDatabase = NULL, int scenarioID = -1);
	DECLARE_DYNAMIC(OptSegDataRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(OptSegDataRecordset, CRecordset)

	long	m_scenarioID;
	long	m_trialID;
	long	m_runID;

	long	m_modelID;
	long	m_segmentID;
  	long	m_productID;
	long	m_channelID;

	CTime	m_date;

	float	m_percent_aware_sku_cum;
	float	m_percent_aware_brand_cum;
	float	m_persuasion_sku;
	float	m_avg_brand_transaction_size_dollars;
	float	m_brand_dollars_purchased_tick;
	float	m_GRPs_SKU_tick;
	float	m_percent_distribution_brand;
	float	m_promoprice;
	float	m_unpromoprice;
	float	m_avg_sku_transaction_dollars;
	float	m_sku_dollar_purchased_tick;
	float	m_percent_preuse_distribution_sku;
	float	m_percent_on_display_sku;
	float	m_percent_sku_at_promo_price;

	long	m_num_adds_sku;
	long	m_num_drop_sku;
	long	m_num_adds_brand;
	long	m_num_drop_brand;
	long	m_num_brand_triers_cum;
	long	m_num_brand_repeaters_cum;
	long	m_num_brand_repeater_trips_cum;
	long	m_num_coupon_redemptions;
	long	m_num_units_bought_on_coupon;
	long	m_num_sku_bought;
	long	m_num_sku_triers;
	long	m_num_sku_repeaters;
	long	m_num_sku_repeater_trips_cum;

	//}}AFX_FIELD

	int iScenarioID;

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(OptSegDataRecordset)
	public:
	virtual CString GetDefaultConnect();    // Default connection string
	virtual CString GetDefaultSQL();    // Default SQL for Recordset
	virtual void DoFieldExchange(CFieldExchange* pFX);  // RFX support
	virtual int Open(unsigned int nOpenType = snapshot, LPCTSTR lpszSql = NULL, DWORD dwOptions = none);
	//}}AFX_VIRTUAL

// Implementation
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

	void DeleteAll(void);
};
