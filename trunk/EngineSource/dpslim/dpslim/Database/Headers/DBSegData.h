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

class SegmentData
{
public:

	SegmentData();

	long	m_runID;
	long	m_segmentID;
  	long	m_productID;
	long	m_channelID;

	CTime	m_date;

	float	m_percent_aware_sku_cum;
	float	m_persuasion_sku;
	float	m_GRPs_SKU_tick;
	float	m_promoprice;
	float	m_unpromoprice;
	float	m_sku_dollar_purchased_tick;
	float	m_percent_preuse_distribution_sku;
	float	m_percent_on_display_sku;
	float	m_percent_sku_at_promo_price;
	float	m_num_sku_bought;
	float	m_num_units_unpromo;
	float	m_num_units_promo;
	float	m_num_units_display;
	float	m_display_price;
	float	m_percent_at_display_price;
	float	m_eq_units;
	float	m_volume;

	long	m_num_adds_sku;
	long	m_num_drop_sku;
	long	m_num_coupon_redemptions;
	long	m_num_units_bought_on_coupon;
	long	m_num_sku_triers;
	long	m_num_sku_repeaters;
	long	m_num_sku_repeater_trips_cum;
	long	num_trips;
};

class SegDataRecordset : public CRecordset
{
public:
	SegDataRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(SegDataRecordset)

	void CopyFromSegmentData(const SegmentData&);

	long	m_runID;
	long	m_segmentID;
  	long	m_productID;
	long	m_channelID;

	CTime	m_date;

	float	m_percent_aware_sku_cum;
	float	m_persuasion_sku;
	float	m_GRPs_SKU_tick;
	float	m_promoprice;
	float	m_unpromoprice;
	float	m_sku_dollar_purchased_tick;
	float	m_percent_preuse_distribution_sku;
	float	m_percent_on_display_sku;
	float	m_percent_sku_at_promo_price;
	float	m_num_sku_bought;
	float	m_num_units_unpromo;
	float	m_num_units_promo;
	float	m_num_units_display;
	float	m_display_price;
	float	m_percent_at_display_price;
	float	m_eq_units;
	float	m_volume;

	long	m_num_adds_sku;
	long	m_num_drop_sku;
	long	m_num_coupon_redemptions;
	long	m_num_units_bought_on_coupon;
	long	m_num_sku_triers;
	long	m_num_sku_repeaters;
	long	m_num_sku_repeater_trips_cum;
	long	num_trips;

// Field/Param Data
	//{{AFX_FIELD(SegDataRecordset, CRecordset)


	//}}AFX_FIELD

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(SegDataRecordset)
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
