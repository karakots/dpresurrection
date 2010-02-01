// ----------------------
//
// Created 11/20/2003
// Vicki de Mey, DecisionPower, Inc.
//
// ----------------------


#include "DBSegment.h"
#include "DBModel.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(SegmentRecordset, CRecordset)

SegmentRecordset::SegmentRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(SegmentRecordset)
	m_SegmentID = 1;
	m_SegmentModel = _T("");
	m_SegmentName = _T("");
	m_color = _T("");
	m_show_current_share_pie_chart = _T("");
	m_show_cmulative_share_chart = _T("");
	m_segment_size = 0;
	m_growth_rate = 0;
	m_growth_rate_people_percent = _T("");
	m_growth_rate_month_year = _T("");	// 10
	m_compress_population;
	m_variability = 0;
	m_price_disutility = 0;
	m_attribute_sensitivity = 0;
	m_persuasion_scaling = 0;
	m_display_utility = 0;
	m_display_utility_scaling_factor = 0;
	m_max_display_hits_per_trip = 0;
	m_inertia = 0;
	m_repurchase = _T("");				// 20
	m_repurchase_model = _T("");
	m_gamma_location_parameter_a = 0;
	m_gamma_shape_parameter_k = 0;
	m_repurchase_period_frequency = 0;
	m_repurchase_frequency_variation = 0;
	m_repurchase_timescale = _T("");
	m_avg_max_units_purch = 0;
	m_shopping_trip_interval = 0;
	m_category_penetration = 0;
	m_category_rejection = 0;			// 30
	m_num_initial_buyers = 0;
	m_initial_buying_period = 0;
	m_seed_with_repurchasers = _T("");
	m_use_budget = _T("");
	m_budget = 0;
	m_budget_period = _T("");
	m_save_unspent = _T("");
	m_initial_savings = 0;
	m_social_network_model = _T("");
	m_num_close_contacts = 0.0;			// 40
	m_prob_talking_close_contact_pre = 0;
	m_prob_talking_close_contact_post = 0;
	m_use_local = _T("");
	m_num_distant_contacts = 0;
	m_prob_talk_distant_contact_pre = 0;
	m_prob_talk_distant_contact_post = 0;
	m_awareness_weight_personal_message = 0;
	m_pre_persuasion_prob = 0;
	m_post_persuasion_prob = 0;
	m_units_desired_trigger = 0;		// 50
	m_awareness_model = _T("");
	m_awareness_threshold = 0;
	m_awareness_decay_rate_pre = 0;
	m_awareness_decay_rate_post = 0;
	m_persuasion_decay_rate_pre = 0;
	m_persuasion_decay_rate_post = 0;
	m_persuasion_decay_method = _T("");
	m_product_choice_model = _T("");
	m_persuasion_score = _T(""); 
	m_persuasion_value_computation = _T(""); // 60
	m_persuasion_contribution_overall_score = _T("");
	m_utility_score = _T(""); 
	m_combination_part_utilities = _T("");
	m_price_contribution_overall_score = _T("");
	m_price_score = _T("");
	m_price_value = _T("");
	m_reference_price = 0;
	m_choice_prob = _T("");
	m_inertia_model = _T("");
	m_error_term = _T("");					// 70
	m_loyalty = 0.0;

	m_nFields = 71;
	//}}AFX_FIELD_INIT


	// !!!!! will be adding m_error_term_user_value above  but temporarily here
	m_error_term_user_value = 1;



    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString SegmentRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString SegmentRecordset::GetDefaultSQL()
{
	// !!!!! will be adding m_error_term_user_value
	CString tmp("SELECT segment_id,segment_model,segment_name,color,show_current_share_pie_chart,show_cmulative_share_chart,segment_size,growth_rate,growth_rate_people_percent,growth_rate_month_year,compress_population,variability,price_disutility,attribute_sensitivity,persuasion_scaling,display_utility,display_utility_scaling_factor,max_display_hits_per_trip,inertia,repurchase,repurchase_model,gamma_location_parameter_a,gamma_shape_parameter_k,repurchase_period_frequency,repurchase_frequency_variation,repurchase_timescale,avg_max_units_purch,shopping_trip_interval,category_penetration,category_rejection,num_initial_buyers,initial_buying_period,seed_with_repurchasers,use_budget,budget,budget_period,save_unspent,initial_savings,social_network_model,num_close_contacts,prob_talking_close_contact_pre,prob_talking_close_contact_post,use_local,num_distant_contacts,prob_talk_distant_contact_pre,prob_talk_distant_contact_post,awareness_weight_personal_message,pre_persuasion_prob,post_persuasion_prob,units_desired_trigger,awareness_model,awareness_threshold,awareness_decay_rate_pre,awareness_decay_rate_post,persuasion_decay_rate_pre,persuasion_decay_rate_post,persuasion_decay_method,product_choice_model,persuasion_score,persuasion_value_computation,persuasion_contribution_overall_score,utility_score,combination_part_utilities,price_contribution_overall_score,price_score,price_value,reference_price,choice_prob,inertia_model,error_term, loyalty FROM segment");
	tmp += ModelRecordset::ModelQuery;
	return tmp;
}

void SegmentRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(SegmentRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);
	RFX_Long(pFX, _T("[segment_id]"), m_SegmentID);
	RFX_Text(pFX, _T("[segment_model]"), m_SegmentModel);
	RFX_Text(pFX, _T("[segment_name]"), m_SegmentName);
	RFX_Text(pFX, _T("[color]"),  m_color);
	RFX_Text(pFX, _T("[show_current_share_pie_chart]"),  m_show_current_share_pie_chart);
	RFX_Text(pFX, _T("[show_cmulative_share_chart]"),  m_show_cmulative_share_chart);
	RFX_Double(pFX, _T("[segment_size]"),  m_segment_size);
	RFX_Single(pFX, _T("[growth_rate]"), m_growth_rate); // float
	RFX_Text(pFX, _T("[growth_rate_people_percent]"),  m_growth_rate_people_percent);
	RFX_Text(pFX, _T("[growth_rate_month_year]"),  m_growth_rate_month_year);
	RFX_Text(pFX, _T("[compress_population]"),  m_compress_population);
	RFX_Long(pFX, _T("[variability]"),  m_variability);
	RFX_Single(pFX, _T("[price_disutility]"),  m_price_disutility);
	RFX_Single(pFX, _T("[attribute_sensitivity]"),  m_attribute_sensitivity);
	RFX_Single(pFX, _T("[persuasion_scaling]"),  m_persuasion_scaling);
	RFX_Single(pFX, _T("[display_utility]"),  m_display_utility);
	RFX_Single(pFX, _T("[display_utility_scaling_factor]"),  m_display_utility_scaling_factor);
	RFX_Single(pFX, _T("[max_display_hits_per_trip]"),  m_max_display_hits_per_trip);
	RFX_Single(pFX, _T("[inertia]"),  m_inertia);
	RFX_Text(pFX, _T("[repurchase]"),  m_repurchase);
	RFX_Text(pFX, _T("[repurchase_model]"),  m_repurchase_model);
	RFX_Single(pFX, _T("[gamma_location_parameter_a]"),  m_gamma_location_parameter_a);
	RFX_Single(pFX, _T("[gamma_shape_parameter_k]"),  m_gamma_shape_parameter_k);
	RFX_Single(pFX, _T("[repurchase_period_frequency]"),  m_repurchase_period_frequency);
	RFX_Single(pFX, _T("[repurchase_frequency_variation]"),  m_repurchase_frequency_variation);
	RFX_Text(pFX, _T("[repurchase_timescale]"),  m_repurchase_timescale);
	RFX_Single(pFX, _T("[avg_max_units_purch]"),  m_avg_max_units_purch);
	RFX_Single(pFX, _T("[shopping_trip_interval]"),  m_shopping_trip_interval);
	RFX_Long(pFX, _T("[category_penetration]"),  m_category_penetration);
	RFX_Long(pFX, _T("[category_rejection]"),  m_category_rejection);
	RFX_Long(pFX, _T("[num_initial_buyers]"),  m_num_initial_buyers);
	RFX_Single(pFX, _T("[initial_buying_period]"),  m_initial_buying_period);
	RFX_Text(pFX, _T("[seed_with_repurchasers]"),  m_seed_with_repurchasers);
	RFX_Text(pFX, _T("[use_budget]"),  m_use_budget);
	RFX_Single(pFX, _T("[budget]"),  m_budget);
	RFX_Text(pFX, _T("[budget_period]"),  m_budget_period);
	RFX_Text(pFX, _T("[save_unspent]"),  m_save_unspent);
	RFX_Single(pFX, _T("[initial_savings]"),  m_initial_savings);
	RFX_Text(pFX, _T("[social_network_model]"),  m_social_network_model);
	RFX_Single(pFX, _T("[num_close_contacts]"),  m_num_close_contacts);
	RFX_Single(pFX, _T("[prob_talking_close_contact_pre]"),  m_prob_talking_close_contact_pre);
	RFX_Single(pFX, _T("[prob_talking_close_contact_post]"),  m_prob_talking_close_contact_post);
	RFX_Text(pFX, _T("[use_local]"),  m_use_local);
	RFX_Single(pFX, _T("[num_distant_contacts]"),  m_num_distant_contacts);
	RFX_Single(pFX, _T("[prob_talk_distant_contact_pre]"),  m_prob_talk_distant_contact_pre);
	RFX_Single(pFX, _T("[prob_talk_distant_contact_post]"),  m_prob_talk_distant_contact_post);
	RFX_Single(pFX, _T("[awareness_weight_personal_message]"),  m_awareness_weight_personal_message);
	RFX_Single(pFX, _T("[pre_persuasion_prob]"),  m_pre_persuasion_prob);
	RFX_Single(pFX, _T("[post_persuasion_prob]"),  m_post_persuasion_prob);
	RFX_Single(pFX, _T("[units_desired_trigger]"),  m_units_desired_trigger);
	RFX_Text(pFX, _T("[awareness_model]"),  m_awareness_model);
	RFX_Single(pFX, _T("[awareness_threshold]"),  m_awareness_threshold);
	RFX_Single(pFX, _T("[awareness_decay_rate_pre]"),  m_awareness_decay_rate_pre);
	RFX_Single(pFX, _T("[awareness_decay_rate_post]"),  m_awareness_decay_rate_post);
	RFX_Single(pFX, _T("[persuasion_decay_rate_pre]"),  m_persuasion_decay_rate_pre);
	RFX_Single(pFX, _T("[persuasion_decay_rate_post]"),  m_persuasion_decay_rate_post);
	RFX_Text(pFX, _T("[persuasion_decay_method]"),  m_persuasion_decay_method);
	
	RFX_Text(pFX, _T("[product_choice_model]"),  m_product_choice_model);
	RFX_Text(pFX, _T("[persuasion_score]"),  m_persuasion_score);
	RFX_Text(pFX, _T("[persuasion_value_computation]"),  m_persuasion_value_computation);
	RFX_Text(pFX, _T("[persuasion_contribution_overall_score]"),  m_persuasion_contribution_overall_score);
	RFX_Text(pFX, _T("[utility_score]"),  m_utility_score);
	RFX_Text(pFX, _T("[combination_part_utilities]"),  m_combination_part_utilities); // 60
	RFX_Text(pFX, _T("[price_contribution_overall_score]"),  m_price_contribution_overall_score);
	RFX_Text(pFX, _T("[price_score]"),  m_price_score);
	RFX_Text(pFX, _T("[price_value]"),  m_price_value);
	RFX_Single(pFX, _T("[reference_price]"),  m_reference_price);
	RFX_Text(pFX, _T("[choice_prob]"),  m_choice_prob);
	RFX_Text(pFX, _T("[inertia_model]"),  m_inertia_model);
	RFX_Text(pFX, _T("[error_term]"),  m_error_term); 
	RFX_Single(pFX, _T("[loyalty]"),  m_loyalty); 

	//}}AFX_FIELD_MAP

	// !!!!! will be adding m_error_term_user_value	
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void SegmentRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void SegmentRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int SegmentRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
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
