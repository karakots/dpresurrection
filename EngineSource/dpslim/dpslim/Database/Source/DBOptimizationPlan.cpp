// ----------------------
//
// Created 11/18/2004
// Vicki de Mey, DecisionPower, Inc.
//
// ----------------------


#include "DBOptimizationPlan.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(OptimizationPlanRecordset, CRecordset)

OptimizationPlanRecordset::OptimizationPlanRecordset(CDatabase* pdb, int modelID)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(OptimizationPlanRecordset)
	m_ScenarioID = 0;
	m_OptimizeFor = 0;
	m_NumSteps = 0;
	m_ModeExecID = 0;
	m_ExploreModeID = 0;

	m_nFields = 5;
	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;

	// scenario info
	i_modelID = modelID;
}

CString OptimizationPlanRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString OptimizationPlanRecordset::GetDefaultSQL()
{
	return _T("SELECT scenario_id,optimize_for,num_steps,mode_exec_id,explore_mode_id FROM optimization_plan WHERE is_active = 1");
}

void OptimizationPlanRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(OptimizationPlanRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Long(pFX, _T("[scenario_id]"), m_ScenarioID);
	RFX_Int(pFX, _T("[optimize_for]"), m_OptimizeFor);
	RFX_Int(pFX, _T("[num_steps]"), m_NumSteps);	
	RFX_Int(pFX, _T("[mode_exec_id]"), m_ModeExecID);
	RFX_Int(pFX, _T("[explore_mode_id]"), m_ExploreModeID);

	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void OptimizationPlanRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void OptimizationPlanRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int OptimizationPlanRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
{
    // Override the default behavior of the Open member function. We want to
    //  ensure that the record set executes on the SQL Server default
    //  (blocking) statement type.
    nOpenType = CRecordset::snapshot; //forwardOnly;
    dwOptions = CRecordset::readOnly;

	//CString str;
	//str.Format(_T("SELECT scenario_id,optimize_for,num_steps,mode_exec_id,explore_mode_id FROM optimization_plan WHERE scenario_id = %d"), i_scenarioID);	

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
