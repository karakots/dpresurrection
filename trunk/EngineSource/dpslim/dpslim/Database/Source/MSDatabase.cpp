// MSDatabase.cpp
//
// Database accessing routines using MFC classes for ODBC
//
// Copyright 2003 by  DecisionPower
//
// author: Vicki de Mey
// creation data: 9/18/2003
//
//

#ifndef __MSDATABASE__
#include "MSDatabase.h"
#endif

#define MAXORDERROWS    35
#define MAXDETAILROWS   25

#include "DBParameter.h"
#include "DBProductTree.h"
#include "DBScenario.h"
#include "Math.h"
#include <fstream>

static int allID = -1;
static int UndefID = -1;

// using namespace MrktSimDb;

// ---------------------------------------------------------------------------
//	 constructor
// ---------------------------------------------------------------------------

MSDatabase::MSDatabase()
{
	m_ModelInfo = NULL;
	m_Channel = NULL;
	m_ConsumerPrefs = NULL;
	m_DistDisplay = NULL;
	m_InitalConds = NULL;
	m_MassMedia = NULL;
	m_MarketUtility = NULL;
	m_ProdAttr = NULL;
	m_ProdAttrVal = NULL;
	m_ProdChan = NULL;
    m_Product = NULL;
	m_SegChan = NULL;
	m_Segment = NULL;
	m_ProductEvent = NULL;
	m_TaskEvent = NULL;
	m_Task = NULL;
	m_TaskProd = NULL;
	m_TaskRates = NULL;
	m_ProdMatrix = NULL;
	m_ProductTree = NULL;

	// VdM 10/25/04
	m_OptimizationParams = NULL;
	m_OptimizationPlan = NULL;

	// VdM 11/3/04
	m_OptimizationOutputSummary = NULL;

	// VdM 12/6/04
	theOutputResults = NULL;

	// SSN 1/11/05
	m_SimQueue = NULL;

	// SSN 4/15/05
	m_Simulation = NULL;

	// SSN 4/26/2005
	m_SocNetwork = NULL;
	m_NetworkParams = NULL;

	// SSN 7/4/2005
	m_SimLog = NULL;

	// SSN 1/5/2007
	m_product_size = NULL;

	comma = ",";
	slash = "/";

	iResultsTableName.clear();
	iOptResultsTableName.clear();

	iConnectString = "Not connected";

	// default value
	iModelID = UndefID;
	iRunID = UndefID;
	iScenarioID = -1;

	iStartTime = GetTickCount();

	warningsLogged = false;

	dayLastWrite = 0;
}

int MSDatabase::Open(const char* fname, int run_id)
{
	const int error = -1;
	const int configOK = 1;

//	Database* db = new Database();

	int rtn = 0;

	// get connection info from the file dbconnct in the folder where the .EXE is found
	//LPCTSTR lpszConnectString;
	string writeTable;
	char connect[1024];
	fstream in_file;
	fstream out_file;
	int	err = 0;
	string	dialogString;

	//string::CopyFromCString(appPath, &connectFileWithPath);
	//string::CopyFromCString("\\dbconnect", &connectFile);
	//string::StrCat(&connectFileWithPath, &connectFile, FALSE);


	int createFile = false;

	in_file.open(fname, fstream::in);

	if (in_file.is_open()) 
	{
		in_file.getline(connect, 1024);	
		in_file.close();
	}
	else 
	{
		// use dialog to establish connection
		strcpy(connect, "");
		createFile = true;
	}

	// results table names

	iResultsTableName = "results_std";
	iOptResultsTableName = "optimization_results_std";

	// parse connect string

	iConnectString = connect;

	// connect with string

    try
    {
		//rtn = iTheCurrentDB.Open( _T( "Test" ), FALSE, FALSE, _T( "ODBC;UID=Vicki" ), FALSE);

		// CDatabase::useCursorLib so we can do snapshots of the DB
		//iTheCurrentDB.OpenEx( _T( "DSN=Test;UID=Vicki;Trusted_Connection=yes" ), FALSE); // CDatabase::useCursorLib);
		//rtn = iTheCurrentDB.OpenEx( _T( "DSN=Test;UID=Vicki" ), CDatabase::useCursorLib);
		//rtn = iTheCurrentDB.OpenEx( _T( "DSN=Test;UID=Vicki" ), FALSE);
		//rtn = iTheCurrentDB.OpenEx(connect, FALSE);
		//rtn = iTheCurrentDB.OpenEx(connect);

		rtn = iTheCurrentDB.OpenEx(CString(connect), CDatabase::useCursorLib);

		int length = iTheCurrentDB.GetConnect().GetLength();
		
		iTheCurrentDB.SetQueryTimeout(0);

		if (!rtn || !iTheCurrentDB.IsOpen())
        {
            return error;
        }

		if(createFile)
		{
			out_file.open(fname, fstream::out);
			if (out_file.is_open()) 
			{
				// remove the first 5 characters from string
				// connection string for the database includes a ODBC; at the front that
				// is not understood by OpenEx. odd but true

				CString connection_string = iTheCurrentDB.GetConnect().Right(length - 5);
				const char* out_string = connection_string.GetString();
				
				out_file << out_string << endl;
				out_file.close();
			}
		}

		rtn = writeDataDB.OpenEx(iTheCurrentDB.GetConnect().Right(length - 5), CDatabase::useCursorLib);

		if (!rtn || !writeDataDB.IsOpen())
        {
            return error;
        }

    }
    catch (CException* e)
    {
        e->Delete();
        return error;
    }

	int b = iTheCurrentDB.CanUpdate();

	if(run_id == -2)
	{
		return configOK;
	}

	if (SelectModel(run_id))
	{
		iTheCurrentDB.BeginTrans();
		ApplyParameters();

		PreloadAllRecordsets();

		RestoreValues();

		iTheCurrentDB.CommitTrans();
	}
	else
	{
		return error;
	}

	InitOutput();

	// ok
	return 0;
}

// ---------------------------------------------------------------------------
//	 destructor
// ---------------------------------------------------------------------------

MSDatabase::~MSDatabase()
{
	 if (m_ModelInfo != NULL)
	{
		m_ModelInfo->Close();
        delete m_ModelInfo;
	}

	if (m_SimQueue != NULL)
	{
		m_SimQueue->Close();
        delete m_SimQueue;
	}

	if (m_Simulation != NULL)
	{
		m_Simulation->Close();
        delete m_Simulation;
	}

	  // VdM 12/6/04
    if (theOutputResults != NULL)
        delete theOutputResults;

	if (m_SimLog != NULL)
	{
		m_SimLog->Close();
		delete m_SimLog;
	}

	FreeAllRecordsets();

	
	if (iTheCurrentDB.IsOpen())
		iTheCurrentDB.Close();
}

void MSDatabase::FreeAllRecordsets()
{
	if (m_Channel != NULL)
	{
		m_Channel->Close();
        delete m_Channel;
		m_Channel = 0;
	}
	
    if (m_ConsumerPrefs != NULL)
	{
		m_ConsumerPrefs->Close();
        delete m_ConsumerPrefs;
		m_ConsumerPrefs = 0;
	}
	
    if (m_DistDisplay != NULL)
	{
		m_DistDisplay->Close();
        delete m_DistDisplay;
		m_DistDisplay = 0;
	}
	
    if (m_InitalConds != NULL)
	{
		m_InitalConds->Close();
        delete m_InitalConds;
		m_InitalConds = 0;
	}
	
    if (m_MassMedia != NULL)
	{
		m_MassMedia->Close();
        delete m_MassMedia;
		m_MassMedia = 0;
	}

	if (m_MarketUtility != NULL)
	{
		m_MarketUtility->Close();
        delete m_MarketUtility;
		m_MarketUtility = 0;
	}
	
    if (m_ProdAttr != NULL)
	{
		m_ProdAttr->Close();
        delete m_ProdAttr;
		m_ProdAttr = 0;
	}
	
    if (m_ProdAttrVal != NULL)
	{
		m_ProdAttrVal->Close();
        delete m_ProdAttrVal;
		m_ProdAttrVal = 0;
	}
	
    if (m_ProdChan != NULL)
	{
		m_ProdChan->Close();
        delete m_ProdChan;
		m_ProdChan = 0;
	}

    if (m_Product != NULL)
	{
		m_Product->Close();
        delete m_Product;
		m_Product = 0;
	}
	
    if (m_SegChan != NULL)
	{
		m_SegChan->Close();
        delete m_SegChan;
		m_SegChan = 0;
	}
	
    if (m_Segment != NULL)
	{
		m_Segment->Close();
        delete m_Segment;
		m_Segment = 0;
	}
	
    if (m_ProductEvent != NULL)
	{
		m_ProductEvent->Close();
        delete m_ProductEvent;
		m_ProductEvent = 0;
	}

	if (m_TaskEvent != NULL)
	{
		m_TaskEvent->Close();
        delete m_TaskEvent;
		m_TaskEvent = 0;
	}
	
    if (m_Task != NULL)
	{
		m_Task->Close();
        delete m_Task;
		m_Task = 0;
	}
	
    if (m_TaskProd != NULL)
	{
		m_TaskProd->Close();
        delete m_TaskProd;
		m_TaskProd = 0;
	}
	
    if (m_TaskRates != NULL)
	{
		m_TaskRates->Close();
        delete m_TaskRates;
		m_TaskRates = 0;
	}
	
   if (m_ProdMatrix != NULL)
   {
	   m_ProdMatrix->Close();
	   delete m_ProdMatrix;
	   m_ProdMatrix = 0;
   }

    if (m_ProductTree != NULL)
   {
	   m_ProductTree->Close();
	   delete m_ProductTree;
	   m_ProductTree = 0;
   }

 
   
	// VdM 10/25/04
   if (m_OptimizationParams != NULL)
   {
	   m_OptimizationParams->Close();
	   delete m_OptimizationParams;
	   m_OptimizationParams = 0;
   }

   if (m_OptimizationPlan != NULL)
   {
	   m_OptimizationPlan->Close();
	   delete m_OptimizationPlan;
	   m_OptimizationPlan = 0;
   }

	// VdM 11/3/04
   if (m_OptimizationOutputSummary != NULL)
   {
	   m_OptimizationOutputSummary->Close();
	   delete m_OptimizationOutputSummary;
	   m_OptimizationOutputSummary = 0;
   }

 


	// SSN 4/26/2005
	if (m_SocNetwork != NULL)
	{
		m_SocNetwork->Close();
		delete m_SocNetwork;
		m_SocNetwork = 0;
	}

	// SSN 4/26/2005
	if (m_NetworkParams != NULL)
	{
		m_NetworkParams->Close();
		delete m_NetworkParams;
		m_NetworkParams = 0;
	}

	// SSN 1/5/2007
	if (m_product_size != NULL)
   { 
	   m_product_size->Close();
	   delete m_product_size;
	   m_product_size = 0;
   }

}

int MSDatabase::SelectModel(int run_id)
{
	int simulationID = UndefID;
	int scenarioID = UndefID;

	// reset the SimQueue and ModelInfo Object
	if (m_SimQueue != 0)
	{
		m_SimQueue->Close();
		delete m_SimQueue;
		m_SimQueue = 0;
	}

	if (m_ModelInfo != 0)
	{
		m_ModelInfo->Close();
		delete m_ModelInfo;
		m_ModelInfo = 0;
	}

	sprintf(&(ModelRecordset::ModelQuery[0]),  " ");
	sprintf(&(ModelRecordset::MarketPlanQuery[0]), " ");
	sprintf(&(SimQueueRecordset::RunQuery[0]), " ");



	m_SimQueue = new SimQueueRecordset(&iTheCurrentDB);
	m_SimQueue->Open();

	// there are no models in database
	if ( m_SimQueue->IsEOF())
	{
		m_SimQueue->Close();
		return false;
	}

	// check if current model is valid
	/*if (iModelID != UndefID )
	{
	m_SimQueue->SetAbsolutePosition(1);
	while (!m_SimQueue->IsEOF())
	{	
	// check if iModelID is a valid model
	if( m_SimQueue->m_ModelID == iModelID)
	break;

	m_SimQueue->MoveNext();
	}

	// could not find run
	if (m_SimQueue->IsEOF())
	{
	iModelID = UndefID;
	iRunID = UndefID;
	}
	else
	{
	// make sure we are looking at the correct run
	iRunID = m_SimQueue->m_RunID;
	}
	}
	else
	{*/
	// looking for first pending model on simulation queue
	// order is determined by num in queue
	iRunID = UndefID;
	if(run_id >= 0)
	{
		m_SimQueue->SetAbsolutePosition(1);
		while (!m_SimQueue->IsEOF())
		{	
			if( m_SimQueue->m_Status == 0 && m_SimQueue->m_RunID == run_id)
			{
				iModelID = m_SimQueue->m_ModelID;
				iRunID = m_SimQueue->m_RunID;
				simulationID = m_SimQueue->m_SimulationID;
			}

			m_SimQueue->MoveNext();
		}
	}

	if(iRunID == UndefID)
	{
		int num = -1;
		m_SimQueue->SetAbsolutePosition(1);
		while (!m_SimQueue->IsEOF())
		{	
			if( m_SimQueue->m_Status == 0)
			{
				if(num == -1 || m_SimQueue->m_Num < num)
				{
					iModelID = m_SimQueue->m_ModelID;
					iRunID = m_SimQueue->m_RunID;
					simulationID = m_SimQueue->m_SimulationID;
					num = m_SimQueue->m_Num;
				}
			}

			m_SimQueue->MoveNext();
		}
	}
	//}

	// could not find model
	if (iModelID == UndefID)
		return false;
	
	// reset sim queue to this run
	m_SimQueue->Close();
	delete m_SimQueue;

	// construct query string to be used for selects
	sprintf(&(ModelRecordset::ModelQuery[0]), " WHERE model_id = %d ", iModelID);
	
	sprintf(&(SimQueueRecordset::RunQuery[0]),  " WHERE run_id = %d ", iRunID);

	sprintf(&(SimulationRecordset::SimulationQuery[0]),  " WHERE id  = %d ", simulationID);
	
	ParameterRecordset::SetScenario(simulationID);

	// try it out
	m_SimQueue = new SimQueueRecordset(&iTheCurrentDB);
	m_SimQueue->Open();

	// this is a bug
	ASSERT(!m_SimQueue->IsEOF());

	// read in model info
	m_ModelInfo = new ModelRecordset(&iTheCurrentDB);
	m_ModelInfo->Open();

	if (m_ModelInfo->IsEOF())
		return false;

	m_ModelInfo->MoveFirst();

	// get scenario information
	m_Simulation = new SimulationRecordset(&iTheCurrentDB);
	m_Simulation->Open();

	// something amiss with scenario?
	// we can still run the sim but...
	if ( m_Simulation->IsEOF())
	{
		SimLogRecordset* log = NewLogEntry();

		log->message = "Error opening simulation";

		log->Update();

		m_Simulation->Close();
		m_Simulation = 0;
	}
	else
	{
		// change start and end date of Model
		m_Simulation->MoveFirst();

		// this is a test
		// always start mode at start
		// m_ModelInfo->start_date = m_Scenario->start_date;
		m_ModelInfo->end_date = m_Simulation->end_date;
		m_ModelInfo->access_time = m_Simulation->access_time;
		scenarioID = m_Simulation->scenario_id;

		sprintf(&(ScenarioRecordset::ScenarioQuery[0]), " WHERE scenario_id  = %d ", scenarioID);

		sprintf(&(ModelRecordset::MarketPlanQuery[0]), " WHERE market_plan.model_id = %d AND market_plan_id = id AND id IN (SELECT child_id FROM market_plan_tree WHERE parent_id IN (SELECT market_plan_id FROM scenario_market_plan WHERE scenario_id = %d)) ORDER BY record_id",
												   iModelID, scenarioID);
		sprintf(&(ModelRecordset::ExtFactQuery[0]), " WHERE market_plan.model_id = %d AND market_plan_id = id AND id IN (SELECT market_plan_id from scenario_market_plan where scenario_id = %d)", 
											   iModelID, scenarioID);
	
	
		int file_valid = m_ModelInfo->checkpoint_valid;  //TBD check to make sure file exists
		int time_valid = (m_Simulation->start_date >= m_ModelInfo->checkpoint_date);

		if(fabs(m_Simulation->scale_factor - m_ModelInfo->checkpoint_scale_factor) < 0.005)
		{
			m_ModelInfo->scale_factor = m_ModelInfo->checkpoint_scale_factor;
		}
		else
		{
			m_ModelInfo->scale_factor = m_Simulation->scale_factor;
			file_valid = false;
		}
		if(file_valid && time_valid)
		{
			m_ModelInfo->using_checkpoint = true;
			m_ModelInfo->start_date = m_ModelInfo->checkpoint_date;

			CString checkPointDate = m_ModelInfo->checkpoint_date.Format("'%m/%d/%Y'");

			// only load data significant to checkpoint
			sprintf(&(ScenarioRecordset::ScenarioQuery[0]), " WHERE end_date >= %s AND scenario_id  = %d ", checkPointDate, scenarioID);
			sprintf(&(ModelRecordset::MarketPlanQuery[0]), " WHERE market_utility.end_date >= %s AND market_plan.model_id = %d AND market_plan_id = id AND id IN (SELECT child_id FROM market_plan_tree WHERE parent_id IN (SELECT market_plan_id FROM scenario_market_plan WHERE scenario_id = %d)) ORDER BY record_id",
												   checkPointDate, iModelID, scenarioID);

			sprintf(&(ModelRecordset::ExtFactQuery[0]), " WHERE product_event.end_date >= %s AND market_plan.model_id = %d AND market_plan_id = id AND id IN (SELECT market_plan_id from scenario_market_plan where scenario_id = %d)", 
											   checkPointDate, iModelID, scenarioID);

		}

		if(m_Simulation->type == 150)
		{
			m_ModelInfo->writing_checkpoint = true;
		}

		if(m_Simulation->reset_panel_state)
		{
			CTime modelStartDate = CTime(m_ModelInfo->start_date.GetYear(),m_ModelInfo->start_date.GetMonth(), m_ModelInfo->start_date.GetDay(),0,0,0,0);
			CTime simStartDate = CTime(m_Simulation->start_date.GetYear(),m_Simulation->start_date.GetMonth(), m_Simulation->start_date.GetDay(),0,0,0,0);

			CTimeSpan diffStart = simStartDate - modelStartDate;
			m_ModelInfo->reset_panel_data_day = (int)diffStart.GetDays();
		}
		else
		{
			m_ModelInfo->reset_panel_data_day = -1;
		}
	}


	SimStatus("Connected to Database");

	return true;
}

// return the sim seed from the sim queue
short MSDatabase::SimSeed()
{
	return m_SimQueue->seed;
}

void MSDatabase::SimStatus(const string message)
{
	if (m_SimQueue == 0)
		return;

	try
	{
		m_SimQueue->Edit();
	}
	catch(CException* e)
	{
		// some problem with this record
		e->Delete();
		return;
	}

	// this means that the simulation is now running
	m_SimQueue->m_Status = 1;
	m_SimQueue->current_status = message.c_str();
	m_SimQueue->elapsed_time = GetTickCount() - iStartTime;

	// ------ Update - write to data source
	try
	{
		m_SimQueue->Update();
	}
	catch (CException* e)
	{
		e->Delete();
		return;
	}
}

void MSDatabase::SimStatus(CTime date, const string message)
{
	if (m_SimQueue == 0)
		return;

	try
	{
		m_SimQueue->Edit();
	}
	catch(CException* e)
	{
		// some problem with this record
		e->Delete();
		return;
	}

	// this means that the simulation is now running
	m_SimQueue->m_Status = 1;
	m_SimQueue->current_status = message.c_str();
	m_SimQueue->elapsed_time = GetTickCount() - iStartTime;
	m_SimQueue->current_date = date;

	// ------ Update - write to data source
	try
	{
		m_SimQueue->Update();
	}
	catch (CException* e)
	{
		e->Delete();
		return;
	}
}


SimLogRecordset* MSDatabase::NewLogEntry()
{
	if (m_SimQueue == 0)
		return 0;

	if (m_SimLog == NULL)
	{
		if (!iTheCurrentDB.IsOpen())
			return 0;

		m_SimLog = new SimLogRecordset(&iTheCurrentDB);

		if (m_SimLog->IsOpen())
			m_SimLog->Requery();
		else
			m_SimLog->Open();
	}

	// create new log entry
	m_SimLog->AddNew();

	m_SimLog->run_id = iRunID;

	warningsLogged = true;

	return m_SimLog;
}


// update database to show simulation is starting
int MSDatabase::SimStart()
{
	// FreeAllRecordsets();

	// set the run time
	if (m_SimQueue == 0)
		return false;

	try
	{
		m_SimQueue->Edit();
	}
	catch(CException* e)
	{
		// some problem with this record
		e->Delete();
		return false;
	}

	// log when the sim starts
	m_SimQueue->run_time =  CTime::GetCurrentTime();

	// ------ Update - write to data source
	try
	{
		m_SimQueue->Update();
	}
	catch (CException* e)
	{
		e->Delete();
		return false;
	}

	SimStatus("running");

	// TEST
	m_ModelInfo->start_date = m_Simulation->start_date;

	return true;
}

// update database to show simulation is done
int MSDatabase::SimDone()
{
	// flush the buffer
	theOutputResults->WriteSegData();

	// write to log
	theOutputResults->WriteTransactionLog(m_ModelInfo->m_ModelName);

	if (m_SimQueue == 0)
		return false;


	try
	{
		m_SimQueue->Edit();
	}
	catch(CException* e)
	{
		// some problem with this record
		e->Delete();
		return false;
	}


	// this means that the simulation is now done
	m_SimQueue->m_Status = 2;
	m_SimQueue->elapsed_time = GetTickCount() - iStartTime;

	if (warningsLogged)
		m_SimQueue->current_status = _T("done (warnings logged)");
	else
		m_SimQueue->current_status = _T("done");



	// ------ Update - write to data source
	try
	{
		m_SimQueue->Update();
	}
	catch (CException* e)
	{
		e->Delete();
		return false;
	}

	m_SimQueue->Close();
	delete m_SimQueue;
	m_SimQueue = 0;

	return true;
}

SegmentData* MSDatabase::NewSegData()
{
	if (m_SimQueue == 0)
		return 0;

	SegmentData* m_SegData = theOutputResults->NewSegData();
	m_SegData->m_runID = iRunID;
	return m_SegData;
}

void MSDatabase::WriteSegData()
{
	if (m_SimQueue == 0)
		return;

	CTime currentDate = theOutputResults->CurrentDate();



	// theOutputResults->WriteSegData();

	// update simulation time
	m_SimQueue->Edit();
	m_SimQueue->elapsed_time = GetTickCount() - iStartTime;
	m_SimQueue->current_date = currentDate;
	m_SimQueue->Update();

	
	// we do want to write to the database once in awhile
	//CTimeSpan span = currentDate - m_ModelInfo->start_date;

	//int daysSinceStart =  span.GetDays();

	//// write out about every 6 months
	//if ( daysSinceStart - dayLastWrite > 180 )
	//{
	//	dayLastWrite = daysSinceStart;
	//}
}

// SSN 1/15/2005
// for those items we are not using
// we check the model info before loading in the data
// no sense wasting precious time and memory
void MSDatabase::PreloadAllRecordsets(void)
{
	string	dialogString;

	// Product
	if (m_Product == NULL)
		m_Product = new ProductRecordset(&iTheCurrentDB);

	if (m_Product->IsOpen())
		m_Product->Requery();
	else
		m_Product->Open();

	// Channel
	if (m_Channel == NULL)
		m_Channel = new ChannelRecordset(&iTheCurrentDB);

	if (m_Channel->IsOpen())
		m_Channel->Requery();
	else
		m_Channel->Open();

	// consumer preds
	if (m_ConsumerPrefs == NULL)
		m_ConsumerPrefs = new ConsumerPrefsRecordset(&iTheCurrentDB);

	if (m_ConsumerPrefs->IsOpen())
		m_ConsumerPrefs->Requery();
	else
		m_ConsumerPrefs->Open();

	if (m_ModelInfo->distribution || m_ModelInfo->display)
	{
		// distribution and display
		if (m_DistDisplay == NULL)
			m_DistDisplay = new DistDisplayRecordset(&iTheCurrentDB);

		if (m_DistDisplay->IsOpen())
			m_DistDisplay->Requery();
		else
			m_DistDisplay->Open();
	}

	// initial conds
	if (m_InitalConds == NULL)
		m_InitalConds = new InitialCondsRecordset(&iTheCurrentDB);

	if (m_InitalConds->IsOpen())
		m_InitalConds->Requery();
	else
		m_InitalConds->Open();

	// mass media
	if (m_MassMedia == NULL)
		m_MassMedia = new MassMediaRecordset(&iTheCurrentDB);

	if (m_MassMedia->IsOpen())
		m_MassMedia->Requery();
	else
		m_MassMedia->Open();

	// market utility
	if (m_MarketUtility == NULL)
		m_MarketUtility = new MarketUtilityRecordset(&iTheCurrentDB);

	if (m_MarketUtility->IsOpen())
		m_MarketUtility->Requery();
	else
		m_MarketUtility->Open();

	// Product attr 
	if (m_ProdAttr == NULL)
		m_ProdAttr = new ProdAttrRecordset(&iTheCurrentDB);

	if (m_ProdAttr->IsOpen())
		m_ProdAttr->Requery();
	else
		m_ProdAttr->Open();

	// Product attr value
	if (m_ProdAttrVal == NULL)
		m_ProdAttrVal = new ProdAttrValRecordset(&iTheCurrentDB);

	if (m_ProdAttrVal->IsOpen())
		m_ProdAttrVal->Requery();
	else
		m_ProdAttrVal->Open();

	// Product channel
	if (m_ProdChan == NULL)
		m_ProdChan = new ProdChanRecordset(&iTheCurrentDB);

	if (m_ProdChan->IsOpen())
		m_ProdChan->Requery();
	else
		m_ProdChan->Open();

	// seg channel
	if (m_SegChan == NULL)
		m_SegChan = new SegChanRecordset(&iTheCurrentDB);

	if (m_SegChan->IsOpen())
		m_SegChan->Requery();
	else
		m_SegChan->Open();

	// segment
	if (m_Segment == NULL)
		m_Segment = new SegmentRecordset(&iTheCurrentDB);

	if (m_Segment->IsOpen())
		m_Segment->Requery();
	else
		m_Segment->Open();

	// product events
	if (m_ProductEvent == NULL)
		m_ProductEvent = new ProductEventRecordset(&iTheCurrentDB);

	if (m_ProductEvent->IsOpen())
		m_ProductEvent->Requery();
	else
		m_ProductEvent->Open();

	// product tree
	if (m_ProductTree == NULL)
		m_ProductTree = new ProductTreeRecordset(&iTheCurrentDB);

	if (m_ProductTree->IsOpen())
		m_ProductTree->Requery();
	else
		m_ProductTree->Open();

	// product size
	if (m_product_size == NULL)
		m_product_size = new ProductSizeRecordset(&iTheCurrentDB);

	if (m_product_size->IsOpen())
		m_product_size->Requery();
	else
		m_product_size->Open();


	// task
	// need tasks even if model is not task based
	// TODO need to investigate task_based option
	// SSN 3/31/2005

	// SSN 6/2/2006
	// put back test - can now run without tasks
	if (m_ModelInfo->task_based)
	{
		// task events
		if (m_TaskEvent == NULL)
			m_TaskEvent = new TaskEventRecordset(&iTheCurrentDB);

		if (m_TaskEvent->IsOpen())
			m_TaskEvent->Requery();
		else
			m_TaskEvent->Open();
	
		if (m_Task == NULL)
			m_Task = new TaskRecordset(&iTheCurrentDB);

	
		if (m_Task->IsOpen())
			m_Task->Requery();
		else
			m_Task->Open();

		// task prod
		if (m_TaskProd == NULL)
			m_TaskProd = new TaskProdRecordset(&iTheCurrentDB);

		if (m_TaskProd->IsOpen())
			m_TaskProd->Requery();
		else
			m_TaskProd->Open();

		// task rates
		if (m_TaskRates == NULL)
			m_TaskRates = new TaskRatesRecordset(&iTheCurrentDB);

		if (m_TaskRates->IsOpen())
			m_TaskRates->Requery();
		else
			m_TaskRates->Open();
	}

	if (m_ModelInfo->product_dependency)
	{
		// product matrix
		if (m_ProdMatrix == NULL)
			m_ProdMatrix = new ProductMatrixRecordset(&iTheCurrentDB);

		if (m_ProdMatrix->IsOpen())
			m_ProdMatrix->Requery();
		else
			m_ProdMatrix->Open();
	}

	if (m_ModelInfo->social_network)
	{
		// social network
		if (m_SocNetwork == NULL)
			m_SocNetwork = new SocNetworkRecordset(&iTheCurrentDB);

		if (m_SocNetwork->IsOpen())
			m_SocNetwork->Requery();
		else
			m_SocNetwork->Open();

		// network parameters
		if (m_NetworkParams == NULL)
			m_NetworkParams = new NetworkParamsRecordset(&iTheCurrentDB);

		if (m_NetworkParams->IsOpen())
			m_NetworkParams->Requery();
		else
			m_NetworkParams->Open();
	}
}

void MSDatabase::InitOutput()
{
	if (theOutputResults == 0 )
	{
		theOutputResults = new OutputResults(&iTheCurrentDB);
	}
}


void MSDatabase::GetProductName(int productID, string* productName)
{
	if ( productID == allID)
	{
		productName = new string("all");
		return;
	}

	else 
	{
		char* temp = new char[256];
		sprintf(temp, "%d", productID);
		productName = new string(temp);
	}
}

void MSDatabase::GetChannelName(int channelID, string* channelName)
{
	if ( channelID == allID)
	{
		channelName = new string("all");
	}
	else
	{
		char* temp = new char[256];
		sprintf(temp, "%d", channelID);
		channelName = new string(temp);
	}
}

void MSDatabase::GetSegmentName(int segmentID, string* segmentName)
{

	if ( segmentID == allID)
	{
		segmentName = new string("all");
	}
	else
	{
		char* temp = new char[256];
		sprintf(temp, "%d", segmentID);
		segmentName = new string(temp);
	}
}

void MSDatabase::GetProdAttrName(int prodAttrID, string* prodAttrName, double& tau)
{
	tau = 0;

	if ( prodAttrID == allID)
	{
		prodAttrName = new string("all");
	}
	else
	{
		char* temp = new char[256];
		sprintf(temp, "%d", prodAttrID);
		prodAttrName = new string(temp);
	}
}

// apply parameters to database
void MSDatabase::ApplyParameters()
{
	ParameterRecordset parameter(&iTheCurrentDB);

	if(!parameter.Open())
	{
		return;
	}

	if (!parameter.IsEOF())
	{
		SimStatus("Updating parameter values");

		parameter.MoveFirst();

		while (!parameter.IsEOF())
		{
			parameter.SetDBValues();
			parameter.MoveNext();
		}
	}

	parameter.Close();
}

// restore database to origianl values
void MSDatabase::RestoreValues()
{
	ParameterRecordset parameter(&iTheCurrentDB);

	if(!parameter.Open())
	{
		return;
	}

	if (!parameter.IsEOF())
	{
		SimStatus("Updating parameter values");

		parameter.MoveFirst();

		while (!parameter.IsEOF())
		{
			parameter.Restore();
			parameter.MoveNext();
		}
	}

	parameter.Close();
}