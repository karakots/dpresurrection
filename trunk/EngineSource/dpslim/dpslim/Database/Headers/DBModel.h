#pragma once
#define _CRT_SECURE_NO_WARNINGS
// ----------------------
//
// Created 11/20/200
// Vicki de Mey, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes
#include <string>

using namespace std;

/////////////////////////////////////////////////////////////////////////////
// Brand recordset

class ModelRecordset : public CRecordset
{
public:
	ModelRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(ModelRecordset)

	int access_time;
	double scale_factor;
	int reset_panel_data_day;

// Field/Param Data
	//{{AFX_FIELD(BrandRecordset, CRecordset)

 	long	m_ModelID;
	CString	m_ModelName;
	long	m_ModelType;

	// model start and end
	CTime	start_date;
	CTime	end_date;

	CString app_code;

	// defaults

	int	task_based;
	int	profit_loss;
	int	product_extensions;
	int	product_dependency;
	int	segment_growth;
	int	consumer_budget;
	int	periodic_price;
	int	promoted_price;
	int	distribution;
	int	display;
	int	market_utility;
	int	social_network;
	int	attribute_pre_and_post;

	// checkpointing
	CString	checkpoint_file;
	CTime	checkpoint_date;
	int	checkpoint_valid;
	double	checkpoint_scale_factor;
	int	using_checkpoint;
	int	writing_checkpoint;
	long pop_size;


	//}}AFX_FIELD


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(BrandRecordset)
	public:
	virtual CString GetDefaultConnect();    // Default connection string
	virtual CString GetDefaultSQL();    // Default SQL for Recordset
	virtual void DoFieldExchange(CFieldExchange* pFX);  // RFX support
	virtual int Open(unsigned int nOpenType = snapshot, LPCTSTR lpszSql = NULL, DWORD dwOptions = none);
	//}}AFX_VIRTUAL

// Implementation

	static char ModelQuery[1024];
	static char MarketPlanQuery[1024];
	static char ExtFactQuery[1024];


#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif
};
