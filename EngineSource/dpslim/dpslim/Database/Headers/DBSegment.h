#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 11/25/200
// Vicki de Mey, DecisionPower, Inc.
//
#include <afxdb.h>			// MFC ODBC database classes

/////////////////////////////////////////////////////////////////////////////
// Segment recordset

class SegmentRecordset : public CRecordset
{
public:
	SegmentRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(SegmentRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(SegmentRecordset, CRecordset)
 	long	m_SegmentID;
	CString m_SegmentModel;
	CString	m_SegmentName;
	CString m_color;
	CString m_show_current_share_pie_chart;

	CString m_show_cmulative_share_chart;
	double m_segment_size;
	float m_growth_rate;
	CString m_growth_rate_people_percent; //
	CString m_growth_rate_month_year; //
	CString m_compress_population;
	long m_variability;

	float m_price_disutility; //
	float m_attribute_sensitivity;
	float m_persuasion_scaling;
	float m_display_utility;
	float m_display_utility_scaling_factor;
	float m_max_display_hits_per_trip;
	float m_inertia;
	CString m_repurchase;

	CString m_repurchase_model;
	float m_gamma_location_parameter_a;
	float m_gamma_shape_parameter_k;
	float m_repurchase_period_frequency;
	float m_repurchase_frequency_variation;
	CString m_repurchase_timescale;
	float m_avg_max_units_purch;
	float m_shopping_trip_interval;

	long m_category_penetration;
	long m_category_rejection;
	long m_num_initial_buyers;
	float m_initial_buying_period;
	CString m_seed_with_repurchasers;
	CString m_use_budget;
	float m_budget;

	CString m_budget_period;
	CString m_save_unspent;
	float m_initial_savings;
	CString m_social_network_model;
	float m_num_close_contacts;
	float m_prob_talking_close_contact_pre;
	float m_prob_talking_close_contact_post;

	CString m_use_local;
	float m_num_distant_contacts;
	float m_prob_talk_distant_contact_pre;
	float m_prob_talk_distant_contact_post;
	float m_awareness_weight_personal_message;
	float m_pre_persuasion_prob;
	float m_post_persuasion_prob;

	float m_units_desired_trigger;
	CString m_awareness_model;
	float m_awareness_threshold;
	float m_awareness_decay_rate_pre;
	float m_awareness_decay_rate_post;
	float m_persuasion_decay_rate_pre;
	float m_persuasion_decay_rate_post;
	CString m_persuasion_decay_method;
	
	

	CString m_product_choice_model;
	CString m_persuasion_score;
	CString m_persuasion_value_computation; //
	CString m_persuasion_contribution_overall_score;
	CString m_utility_score;
	CString m_combination_part_utilities; //

	CString m_price_contribution_overall_score;
	CString m_price_score;
	CString m_price_value;
	float m_reference_price;
	CString m_choice_prob; //
	CString m_inertia_model;
	CString m_error_term; //
	float	m_loyalty;


	//}}AFX_FIELD

	long	m_error_term_user_value;


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(SegmentRecordset)
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

};
