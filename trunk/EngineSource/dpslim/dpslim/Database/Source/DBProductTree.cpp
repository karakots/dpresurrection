// ----------------------
//
// Created 9/2/2004
// Steve Noble, DecisionPower, Inc.
//
// ----------------------


#include "DBProductTree.h"
#include "DBModel.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(ProductTreeRecordset, CRecordset)

ProductTreeRecordset::ProductTreeRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(BrandProductRecordset)

	parentID = 0;
	childID = 0;
	
	m_nFields = 2;
	
	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString ProductTreeRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString ProductTreeRecordset::GetDefaultSQL()
{
	CString tmp("SELECT parent_id, child_id FROM product_tree ");
	tmp += ModelRecordset::ModelQuery;

	return tmp;
}

void ProductTreeRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(BrandProductRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Long(pFX, _T("[parent_id]"), parentID);
	RFX_Long(pFX, _T("[child_id]"), childID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void ProductTreeRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void ProductTreeRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int ProductTreeRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
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