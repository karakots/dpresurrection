#pragma once
#define _CRT_SECURE_NO_WARNINGS
// OutputResults.h
//
// wrapper class for the different out tables in the DB
// standard output
// scenario/optimization output
//
// Copyright 2004 by  DecisionPower
//
// author: Vicki de Mey
// creation data: 12/6/2004
//
// 

#define __OUTPUTRESULTS__

#include <afxdb.h>			// MFC ODBC database classes

#include "DBSegData.h"
#include "DBOptSegData.h"

#include "atltime.h"


class OutputResults
{
private:
	CDatabase* myDb;

	long trans_time;
	int num_trans;
	CTime startTime;

public:
// default preferences
		SegDataRecordset*				m_SegData;
		
public:
// methods
	// create & destroy
	OutputResults(CDatabase*);
	~OutputResults();



	SegmentData* NewSegData();
	void WriteSegData();

	CTime& CurrentDate();

	void WriteTransactionLog(CString modelName);
};



