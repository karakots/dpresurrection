#pragma once

// ----------------------
//
// Created 5/18/2005
// Steve Noble, DecisionPower, Inc.
//
// ----------------------


#include "DBParameter.h"
#include "DBModel.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


class SingleValueRecordset : public CRecordset
{
public:
	SingleValueRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(SingleValueRecordset)

// Field/Param Data
	//{{AFX_FIELD(BrandRecordset, CRecordset)
	CString	table_name;
	CString	col_name;
	CString	filter;
	float theValue;

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

#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif
};

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(ParameterRecordset, CRecordset)

char ParameterRecordset::ScenarioQuery[128] = "";

void ParameterRecordset::SetScenario(int scenarioID)
{
	sprintf(&(ScenarioQuery[0]), " scenario_parameter.sim_id  = %d ", scenarioID);
}

ParameterRecordset::ParameterRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(BrandRecordset)
	param_id = 0;
	table_name = _T("");
	col_name = _T("");
	filter = _T("");
	update_value = 0.0;
	orig_value = 0.0;

	// this nnumber must needs match how many above
	m_nFields = 6;

	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString ParameterRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString ParameterRecordset::GetDefaultSQL()
{
	CString tmp("SELECT ");
	
	tmp += "model_parameter.id as param_id, model_parameter.table_name AS table_name, model_parameter.col_name AS col_name, model_parameter.filter AS filter, ";
	tmp += " scenario_parameter.aValue as update_value, scenario_parameter.origValue as orig_value ";
	tmp += " FROM model_parameter, scenario_parameter ";
	tmp += " WHERE model_parameter.id = scenario_parameter.param_id AND ";
	tmp += ScenarioQuery;

	return tmp;
}

void ParameterRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(BrandRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Long(pFX, _T("[param_id]"), param_id);
	RFX_Text(pFX, _T("[table_name]"), table_name);
	RFX_Text(pFX, _T("[col_name]"), col_name);
	RFX_Text(pFX, _T("[filter]"), filter);
	RFX_Single(pFX, _T("[update_value]"), update_value);
	RFX_Single(pFX, _T("[orig_value]"), orig_value);
	
	
	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void ParameterRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void ParameterRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int ParameterRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
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

void ParameterRecordset::SetDBValues()
{
	// retrieve original value
	SingleValueRecordset tableUpdate(m_pDatabase);

	tableUpdate.table_name = table_name;
	tableUpdate.col_name = col_name;
	tableUpdate.filter = filter;

	tableUpdate.Open();
	tableUpdate.SetAbsolutePosition(1);

	char origValue[64];
	sprintf(&(origValue[0]), " %f ", tableUpdate.theValue);

	tableUpdate.Close();

	char parmID[64];
	sprintf(&(parmID[0]), " %d ", param_id);

	// store origianl value
	CString store = _T("UPDATE scenario_parameter set origValue = ");
	store += origValue;
	store += _T(" WHERE param_id = ");
	store += parmID;
	store += " AND ";
	store += ScenarioQuery;

	m_pDatabase->ExecuteSQL(store);

	char updateValue[64];
	sprintf(&(updateValue[0]), " %f ", update_value);

	// update table with new value
	CString updateString = "UPDATE ";
	updateString += table_name;
	updateString += " SET ";
	updateString += col_name;
	updateString += " = ";
	updateString += updateValue;
	updateString += " WHERE ";
	updateString += filter;

	m_pDatabase->ExecuteSQL(updateString);
}

void ParameterRecordset::Restore()
{
	char origValue[64];
	sprintf(&(origValue[0]), " %f ", orig_value);

	// update table with new value
	CString restoreString = "UPDATE ";
	restoreString += table_name;
	restoreString += " SET ";
	restoreString += col_name;
	restoreString += " = ";
	restoreString += origValue;
	restoreString += " WHERE ";
	restoreString += filter;

	m_pDatabase->ExecuteSQL(restoreString);
}


// Update a single value in the database

IMPLEMENT_DYNAMIC(SingleValueRecordset, CRecordset)

SingleValueRecordset::SingleValueRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(BrandRecordset)
	
	table_name = _T("");
	col_name = _T("");
	filter = _T("");
	theValue = 0.0;

	// this nnumber must needs match how many above
	m_nFields = 1;

	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString SingleValueRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString SingleValueRecordset::GetDefaultSQL()
{
	CString tmp("SELECT ");

	tmp += col_name;
	tmp += _T(" FROM ");
	tmp += table_name;
	tmp += _T(" WHERE ");
	tmp += filter;

	return tmp;
}

void SingleValueRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(BrandRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Single(pFX, col_name, theValue);
	
	
	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void SingleValueRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void SingleValueRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int SingleValueRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
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

