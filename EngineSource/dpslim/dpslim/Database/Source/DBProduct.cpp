// ----------------------
//
// Created 11/20/2003
// Vicki de Mey, DecisionPower, Inc.
//
// ----------------------


#include "DBProduct.h"
#include "DBModel.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 
IMPLEMENT_DYNAMIC(ProductRecordset, CRecordset)

ProductRecordset::ProductRecordset(CDatabase* pdb)
	: CRecordset(pdb)
{
	//{{AFX_FIELD_INIT(ProductRecordset)
	//m_modelID = -1;

	m_ProductID = 1;
	m_ProductName = _T("");
	m_BrandID = 1;
	m_Type = _T("");
	m_ProductGroup = _T("");
	m_RelatedGroup = _T("");
	m_PercentRelation = _T("");
	m_Cost = 0.0;
 	m_InitialDislikeProb = 0.0;
 	m_RepeatLikeProb = 0.0;
	m_Color = _T("");
	m_base_price = 0.0;
	m_eq_units = 0.0;

	m_nFields = 13;

	//}}AFX_FIELD_INIT

    // Changing default type ensures that blocking hstmt is used without
    //  the ODBC cursor library.

	//m_nDefaultType = snapshot;
	m_nDefaultType = CRecordset::snapshot;

    // Using bound parameters in place of MFC filter string modification.
    //m_nParams = 1;
}

CString ProductRecordset::GetDefaultConnect()
{
    // We don't allow the record set to connect on it's own. Dynamic
    //  creation from the view passes the document's database to
    //  the record set.
	//return _T("ODBC;DSN=Test");
	return _T("");
}

CString ProductRecordset::GetDefaultSQL()
{
	CString tmp("SELECT product_id, product_name, brand_id, type, product_group, related_group, percent_relation, cost, initial_dislike_probability, repeat_like_probability, color, base_price, eq_units  FROM product ");
	tmp += ModelRecordset::ModelQuery;

	return tmp;
}

void ProductRecordset::DoFieldExchange(CFieldExchange* pFX)
{
    // Create our own field exchange mechanism. We must do this for two
    //  reasons. First, MFC's class wizard cannot currently determine the data
    //  types of procedure columns. Second, we must create our own parameter
    //  data exchange lines to take advantage of a bound ODBC input parameter.
	
	//{{AFX_FIELD_MAP(ProductRecordset)
	pFX->SetFieldType(CFieldExchange::outputColumn);

	RFX_Long(pFX, _T("[product_id]"), m_ProductID);
	RFX_Text(pFX, _T("[product_name]"), m_ProductName);
	RFX_Long(pFX, _T("[brand_id]"), m_BrandID);

	RFX_Text(pFX, _T("[type]"), m_Type);
	RFX_Text(pFX, _T("[product_group]"), m_ProductGroup);
	RFX_Text(pFX, _T("[related_group]"), m_RelatedGroup);
	RFX_Text(pFX, _T("[percent_relation]"), m_PercentRelation);

	RFX_Single(pFX, _T("[cost]"), m_Cost);
	RFX_Single(pFX, _T("[initial_dislike_probability]"), m_InitialDislikeProb);
	RFX_Single(pFX, _T("[repeat_like_probability]"), m_RepeatLikeProb);
	RFX_Text(pFX, _T("[color]"), m_Color);
	RFX_Single(pFX, _T("[base_price]"), m_base_price);
	RFX_Single(pFX, _T("[eq_units]"), m_eq_units);


	//}}AFX_FIELD_MAP
	

    //pFX->SetFieldType(CFieldExchange::param);
    //RFX_Text(pFX, _T("@CustID"), m_strCustID);
}

/////////////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
void ProductRecordset::AssertValid() const
{
	CRecordset::AssertValid();
}

void ProductRecordset::Dump(CDumpContext& dc) const
{
	CRecordset::Dump(dc);
}
#endif //_DEBUG

int ProductRecordset::Open(unsigned int nOpenType, LPCTSTR lpszSql, DWORD dwOptions) 
{
    // Override the default behavior of the Open member function. We want to
    //  ensure that the record set executes on the SQL Server default
    //  (blocking) statement type.
    nOpenType = CRecordset::snapshot; //forwardOnly;;
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
