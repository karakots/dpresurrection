// ----------------------
//
// Created 11/20/2003
// Vicki de Mey, DecisionPower, Inc.
//
// ----------------------


#include "DBProdAttrVal.h"
#include "DBModel.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(ProdAttrValRecordset, CRecordset)

ProdAttrValRecordset::ProdAttrValRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(ProdAttrValRecordset)

	m_ProductID = 1;
	m_ProductAttrID = 1;
	m_StartDate = CTime();
	m_PreValue = 0.0;
	m_PostValue = 0.0;
	has_attribute = false;
	m_nFields = 6;
	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString ProdAttrValRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString ProdAttrValRecordset::GetDefaultSQL()
{
	CString tmp("SELECT product_id,product_attribute_id,start_date, pre_attribute_value, post_attribute_value, has_attribute FROM product_attribute_value ");
	tmp += ModelRecordset::ModelQuery;

	return tmp;
}

void ProdAttrValRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(ProdAttrValRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Long(pFX, _T("[product_id]"), m_ProductID);
	RFX_Long(pFX, _T("[product_attribute_id]"), m_ProductAttrID);
	RFX_Date(pFX, _T("[start_date]"), m_StartDate);
	RFX_Single(pFX, _T("[pre_attribute_value]"), m_PreValue);
	RFX_Single(pFX, _T("[post_attribute_value]"), m_PostValue);
	RFX_Bool(pFX, _T("[has_attribute]"), has_attribute);
	//}}AFX_FIELD_MAP
	
    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void ProdAttrValRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void ProdAttrValRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int ProdAttrValRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
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
