/// OutputResults.h
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

#ifndef __OUTPUTRESULTS__
#include "OutputResults.h"
#endif

#include <iostream>
#include <fstream>
#include <time.h>

using namespace std;

// ---------------------------------------------------------------------------
//	 constructor
// ---------------------------------------------------------------------------

OutputResults::OutputResults(CDatabase* db)
{
	m_SegData = NULL;
	myDb = db;
	trans_time = 0;
	num_trans = 0;

	startTime = CTime::GetCurrentTime();
}

// ---------------------------------------------------------------------------
//	 destructor
// ---------------------------------------------------------------------------

OutputResults::~OutputResults()
{


	if (m_SegData != NULL)
	{
		delete m_SegData;
	}
}

#define BUFFER_SIZE 10000
static SegmentData segmentData[BUFFER_SIZE];

 static int index = 0;
SegmentData* OutputResults::NewSegData()
{
	SegmentData* rval = 0;

	if (index == BUFFER_SIZE)
	{
		WriteSegData();
		index = 0;
	}

	rval = &(segmentData[index]);
	index++;
	
	return rval;
}

void OutputResults::WriteSegData()
{
	long start,end;
	num_trans++;
	
	if (m_SegData == NULL)
	{
		m_SegData = new SegDataRecordset(myDb);
		m_SegData->Open();
	}

	start = GetTickCount();
	myDb->BeginTrans();

	int ii;
	for(ii = 0; ii < index; ii++)
	{
		m_SegData->AddNew();
		m_SegData->CopyFromSegmentData(segmentData[ii]);

		m_SegData->Update();
	}

	
	myDb->CommitTrans();
	end = GetTickCount();
	trans_time += end - start;

}

CTime& OutputResults::CurrentDate()
{
	if (index > 0)
		return segmentData[index - 1].m_date;


	return segmentData[0].m_date;
}

void OutputResults::WriteTransactionLog(CString modelName)
{
	CTime endTime = CTime::GetCurrentTime();
	CTimeSpan span = endTime - startTime;

	ofstream outfile;
	outfile.open("Total_Transaction_Times.txt",ifstream::app);

	//time_t rawtime;
	//time ( &rawtime );

	double secs = ((double)trans_time)/((double)1000);

	// write out start date and model Name 
	outfile << startTime.Format("%a %b %d %H:%M:%S %Y") << "       Model: " << modelName << endl;

	// write out total processing time
	outfile	<< "Total Processing Time: ";

	// this should never happen - but if it does we will want to know!
	if (span.GetDays() > 0)
	{
		outfile << span.Format( "%D days" );
	}

	if (span.GetHours() > 0)
	{
		outfile << span.Format( "%H hours" );
	}

	outfile << span.Format( "%M minutes %S seconds" ) << endl;
	
	// write out total transaction time and number of transactions
	outfile <<  "Total Transaction Time: " << secs << " seconds" << endl;
	outfile << "Number of Transactions: " << num_trans << endl;
	
	// write out end time and date of simulation
	outfile << "Simulation ended at: " << endTime.Format( "%H:%M:%S" ) << endl;
	outfile << endl;

	outfile.close();
}