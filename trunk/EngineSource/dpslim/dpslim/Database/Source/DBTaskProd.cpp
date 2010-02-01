// ----------------------
//
// Created 11/20/2003
// Vicki de Mey, DecisionPower, Inc.
//
// ----------------------


#include "DBTaskProd.h"
#include "DBModel.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(TaskProdRecordset, CRecordset)

TaskProdRecordset::TaskProdRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(TaskProdRecordset)

	m_ProductID = 1;
	m_TaskID = 0;
	m_PreUseSKU = 0.0;
	m_PostUseSKU = 0.0;
	m_Suitability = 0.0;
	m_nFields = 5;
	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString TaskProdRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString TaskProdRecordset::GetDefaultSQL()
{
	CString tmp("SELECT product_id, task_id, pre_use_upsku, post_use_upsku, suitability FROM task_product_fact ");
	tmp += ModelRecordset::ModelQuery;

	return tmp;
}


void TaskProdRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(TaskProdRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Long(pFX, _T("[product_id]"), m_ProductID);
	RFX_Long(pFX, _T("[task_id]"), m_TaskID);
	RFX_Single(pFX, _T("[pre_use_upsku]"), m_PreUseSKU);
	RFX_Single(pFX, _T("[post_use_upsku]"), m_PostUseSKU);
	RFX_Single(pFX, _T("[suitability]"), m_Suitability);
	

	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void TaskProdRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void TaskProdRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int TaskProdRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
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
