// ----------------------
//
// Created 11/3/2004
// Vicki de Mey, DecisionPower, Inc.
//
// ----------------------


#include "DBScenarioOutputSummary.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(OptimizationOutSumRecordset, CRecordset)

OptimizationOutSumRecordset::OptimizationOutSumRecordset(CDatabase* pdb, int modelID, int scenarioID)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(OptimizationOutSumRecordset)
 	m_ModelID = 0;
	m_ScenarioID = 0;
	m_TrialID = 0;
	m_RunID = 0;
	m_OptimizeFor = 0;
	m_RunValue = 0.0;
	m_Component_name = _T("");
	m_Param = _T("");
	m_ParamValue = 0.0;
	m_nFields = 9;
	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;

	// scenario info
	i_modelID = modelID;
	i_scenarioID = scenarioID;
	i_trialID = -1;
	i_runID = -1;
}

CString OptimizationOutSumRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString OptimizationOutSumRecordset::GetDefaultSQL()
{
	CString str;
	str.Format(_T("SELECT model_id,scenario_id, trial_id,run_id,optimize_for_id,run_value,component_name,parameter,param_value FROM optimization_output_summary WHERE model_id = %d AND scenario_id = %d"), i_modelID, i_scenarioID);	
	
	return str;
}

void OptimizationOutSumRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(OptimizationOutSumRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Long(pFX, _T("[model_id]"), m_ModelID);
	RFX_Long(pFX, _T("[scenario_id]"), m_ScenarioID);
	RFX_Long(pFX, _T("[trial_id]"), m_TrialID);
	RFX_Long(pFX, _T("[run_id]"), m_RunID);
	RFX_Int(pFX, _T("[optimize_for_id]"), m_OptimizeFor);
	RFX_Single(pFX, _T("[run_value]"), m_RunValue);
	RFX_Text(pFX, _T("[component_name]"), m_Component_name);
	RFX_Text(pFX, _T("[parameter]"), m_Param);
	RFX_Single(pFX, _T("[param_value]"), m_ParamValue);

	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void OptimizationOutSumRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void OptimizationOutSumRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int OptimizationOutSumRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
{
    // Override the default behavior of the Open member function. We want to
    //  ensure that the record set executes on the SQL Server default
    //  (blocking) statement type.
    nOpenType = CRecordset::snapshot; //forwardOnly;
    //dwOptions = CRecordset::appendOnly; // allow writing in bulk
    dwOptions = CRecordset::none | skipDeletedRecords; // allow writing in bulk

	try {
		CRecordset::Open(nOpenType, GetDefaultSQL(), dwOptions);
		//CRecordset::Open(nOpenType, str, dwOptions);
	}
	catch (CException* e)
    {
        e->Delete();
		return FALSE;
    }

	return TRUE;
}

// ------------------------
void OptimizationOutSumRecordset::DeleteAll(void) 
{
	;
}
