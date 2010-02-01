#pragma once
// ----------------------
//
// Created 3/8/2004
// Vicki de Mey, DecisionPower, Inc.
//


#include <afxdb.h>			// MFC ODBC database classes


/////////////////////////////////////////////////////////////////////////////
// MassMedia recordset

class OldSegDataRecordset : public CRecordset
{
public:
	OldSegDataRecordset(CDatabase* pDatabase = NULL);
	DECLARE_DYNAMIC(OldSegDataRecordset)
	
// Field/Param Data
	//{{AFX_FIELD(OldSegDataRecordset, CRecordset)
	long	m_SegmentID;
  	long	m_ProductID;
	long	m_ChannelID;

	CTime	m_Date;

	float	m_Price;
	float	m_PercentAware;
	float	m_AvgMsgPerPerson;
	float	m_PercentAwareBrand;
	
	long	m_NumBought;
	long	m_NumAdds;
	long	m_NumDrops;
	long	m_NumEverTried;
	long	m_NumFirstTimeBuyers;
	long	m_NumCouponRedempt;
	long	m_NumUnitsCoupon;
	long	m_CouponsToBeRedempt;
	long	m_NumEverTriedBrand;
	long	m_FirstTimeBrand;
	long	m_NumErrorCount;

	//}}AFX_FIELD

	string	iTableName;


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(SegDataRecordset)
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

	void DeleteAll(void);
	void InitTableName(string* tbl);
};
