// ----------------------
//
// Created 10/25/2004
// Vicki de Mey, DecisionPower, Inc.
//
// ----------------------


#include "DBScenarioParams.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(OptimizationParamsRecordset, CRecordset)

OptimizationParamsRecordset::OptimizationParamsRecordset(CDatabase* pdb, int modelID, int scenarioID)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(OptimizationParamsRecordset)
 	m_ModelID = 0;
	m_ScenarioID = 0;
	m_Component_name = _T("");
	m_Param = _T("");
	m_Lower = 0.0;
	m_Upper = 0.0;
	m_Leader = 0;
	m_Slave = 0;

	m_nFields = 8;
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
}

CString OptimizationParamsRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString OptimizationParamsRecordset::GetDefaultSQL()
{
	return _T("SELECT model_id,scenario_id,component_name,parameter,lower,uppper,leader,slave FROM optimization_params");
}

void OptimizationParamsRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(OptimizationParamsRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Long(pFX, _T("[model_id]"), m_ModelID);
	RFX_Long(pFX, _T("[scenario_id]"), m_ScenarioID);
	RFX_Text(pFX, _T("[component_name]"), m_Component_name);
	RFX_Text(pFX, _T("[parameter]"), m_Param);
	RFX_Single(pFX, _T("[lower]"), m_Lower);
	RFX_Single(pFX, _T("[upper]"), m_Upper);
	RFX_Int(pFX, _T("[leader]"), m_Leader);
	RFX_Int(pFX, _T("[slave]"), m_Slave);

	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void OptimizationParamsRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void OptimizationParamsRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int OptimizationParamsRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
{
    // Override the default behavior of the Open member function. We want to
    //  ensure that the record set executes on the SQL Server default
    //  (blocking) statement type.
    nOpenType = CRecordset::snapshot; //forwardOnly;
    dwOptions = CRecordset::readOnly;

	CString str;
	str.Format(_T("SELECT model_id, scenario_id, component_name,parameter,lower,upper,leader,slave FROM optimization_params WHERE model_id = %d AND scenario_id = %d"), i_modelID, i_scenarioID);	

	try {
		//CRecordset::Open(nOpenType, GetDefaultSQL(), dwOptions);
		CRecordset::Open(nOpenType, str, dwOptions);
	}
	catch (CException* e)
    {
        e->Delete();
		return FALSE;
    }

	return TRUE;
}
