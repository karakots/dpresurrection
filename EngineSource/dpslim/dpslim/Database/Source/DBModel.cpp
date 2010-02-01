#pragma once

// ----------------------
//
// Created 11/20/200
// Vicki de Mey, DecisionPower, Inc.
//
// ----------------------


#include "DBModel.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

char ModelRecordset::ModelQuery[1024] = "";
char ModelRecordset::MarketPlanQuery[1024] = "";
char ModelRecordset::ExtFactQuery[1024] = "";


/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(ModelRecordset, CRecordset)

ModelRecordset::ModelRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{

	access_time = 1;
	scale_factor = 1;
	reset_panel_data_day = 0;
	//{{AFX_FIELD_INIT(BrandRecordset)
	m_ModelID = 1;
	m_ModelName = _T("");
	m_ModelType = 0;

	// model start and end
	start_date = CTime();
	end_date = CTime();

	// application code

	app_code = _T("");

	// defaults

	task_based = false;
	profit_loss = false;
	product_extensions = false;
	product_dependency = false;
	segment_growth = false;
	consumer_budget = false;
	periodic_price = false;
	promoted_price = false;
	distribution = false;
	display = false;
	market_utility = false;
	social_network = false;
	attribute_pre_and_post = false;

	// checkpointing
	checkpoint_file = "";
	checkpoint_date = CTime();
	checkpoint_valid = false;
	checkpoint_scale_factor = 0.0;
	using_checkpoint = false;
	writing_checkpoint = false;
	pop_size = 0;

	m_nFields = 24;
	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString ModelRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString ModelRecordset::GetDefaultSQL()
{
	CString tmp("SELECT model_id, model_name, model_type, start_date, end_date, app_code, task_based, profit_loss, product_extensions, product_dependency, segment_growth, consumer_budget, periodic_price, promoted_price, distribution, display, market_utility, social_network, attribute_pre_and_post, checkpoint_file, checkpoint_date, checkpoint_valid, checkpoint_scale_factor, pop_size FROM model_info ");
	tmp += ModelQuery;

	return tmp;
}

void ModelRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(BrandRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);
	RFX_Long(pFX, _T("[model_id]"), m_ModelID);
	RFX_Text(pFX, _T("[model_name]"), m_ModelName);
	RFX_Long(pFX, _T("[model_type]"), m_ModelType);

	// start and end dates
	RFX_Date(pFX, _T("[start_date]"), start_date);
	RFX_Date(pFX, _T("[end_date]"), end_date);

	// special application code
	RFX_Text(pFX, _T("[app_code]"), app_code);



	// defaults

	RFX_Bool(pFX, _T("[task_based]"), task_based);
	RFX_Bool(pFX, _T("[profit_loss]"), profit_loss);
	RFX_Bool(pFX, _T("[product_extensions]"), product_extensions);
	RFX_Bool(pFX, _T("[product_dependency]"), product_dependency);
	RFX_Bool(pFX, _T("[segment_growth]"), segment_growth);
	RFX_Bool(pFX, _T("[consumer_budget]"), consumer_budget);
	RFX_Bool(pFX, _T("[periodic_price]"), periodic_price);
	RFX_Bool(pFX, _T("[promoted_price]"), promoted_price);
	RFX_Bool(pFX, _T("[distribution]"), distribution);
	RFX_Bool(pFX, _T("[display]"), display);
	RFX_Bool(pFX, _T("[market_utility]"), market_utility);
	RFX_Bool(pFX, _T("[social_network]"), social_network);
	RFX_Bool(pFX, _T("[attribute_pre_and_post]"), attribute_pre_and_post);

	RFX_Text(pFX, _T("[checkpoint_file]"), checkpoint_file);
	RFX_Date(pFX, _T("[checkpoint_date]"), checkpoint_date);
	RFX_Bool(pFX, _T("[checkpoint_valid]"), checkpoint_valid);
	RFX_Double(pFX, _T("[checkpoint_scale_factor]"), checkpoint_scale_factor);
	RFX_Long(pFX, _T("[pop_size]"), pop_size);


	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void ModelRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void ModelRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int ModelRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
{
    // Override the default behavior of the Open member function. We want to
    //  ensure that the record set executes on the SQL Server default
    //  (blocking) statement type.
    nOpenType = CRecordset::snapshot; //forwardOnly;
    dwOptions = CRecordset::none;

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
