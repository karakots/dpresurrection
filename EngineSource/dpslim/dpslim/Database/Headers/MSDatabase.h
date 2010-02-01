#pragma once
#define _CRT_SECURE_NO_WARNINGS

// Database accessing routines using MFC classes for ODBC
//
// Copyright 2003 by  DecisionPower
//
// author: Vicki de Mey
// creation data: 9/18/2003
//
// 

#define __MSDATABASE__

#include <afxdb.h>			// MFC ODBC database classes
#include <stdafx.h>

#include <string>

#include "DBModel.h"
#include "DBBrand.h"
#include "DBChannel.h"
#include "DBConsumerPrefs.h"
#include "DBDistDisplay.h"
#include "DBInitialConds.h"
#include "DBMassMedia.h"
#include "DBMarketUtility.h"
#include "DBProdAttr.h"
#include "DBProdAttrVal.h"
#include "DBProdChan.h"
#include "DBProduct.h"
#include "DBProductTree.h"
#include "DBSegChan.h"
#include "DBSegment.h"

#include "DBTask.h"
#include "DBTaskProd.h"
#include "DBTaskRates.h"
#include "DBProdMatrix.h"
#include "DBScenarioParams.h"
#include "DBScenarioOutputSummary.h"
#include "DBOptimizationPlan.h"
#include "DBProductEvent.h"
#include "DBTaskEvent.h"
#include "DBSimQueue.h"
#include "DBSimulation.h"
#include "DBSocNetwork.h"
#include "DBNetworkParams.h"
#include "DBSimLog.h"
#include "DBProdSize.h"

// forward declare
class ParameterRecordset;
class ProductTreeRecordset;

//#include "custrecord.h"
//#include "employeerecord.h"

// VdM 12/6/04
// wrapper for out results tables
#ifndef __OUTPUTRESULTS__
#include "OutputResults.h"
#endif

using namespace std;


enum {
	kText = 1,
	kNumber,
	kFloat
};

class MSDatabase
{
	public:
// default preferences
		CDatabase						iTheCurrentDB;
		CDatabase						writeDataDB;

		// read only
		ModelRecordset*					m_ModelInfo;
		ChannelRecordset*				m_Channel;
		ConsumerPrefsRecordset*			m_ConsumerPrefs;
		DistDisplayRecordset*			m_DistDisplay;
		InitialCondsRecordset*			m_InitalConds;
		MassMediaRecordset*				m_MassMedia;
		MarketUtilityRecordset*			m_MarketUtility;
		ProdAttrRecordset*				m_ProdAttr;
		ProdAttrValRecordset*			m_ProdAttrVal;
		ProdChanRecordset*				m_ProdChan;
		ProductRecordset*				m_Product;
		SegChanRecordset*				m_SegChan;
		SegmentRecordset*				m_Segment;
		ProductEventRecordset*			m_ProductEvent;
		TaskEventRecordset*				m_TaskEvent;
		TaskRecordset*					m_Task;
		TaskProdRecordset*				m_TaskProd;
		TaskRatesRecordset*				m_TaskRates;
		ProductMatrixRecordset*			m_ProdMatrix;
		ProductTreeRecordset*			m_ProductTree;
		ProductSizeRecordset*			m_product_size;

		// VdM 10/25/04
		OptimizationParamsRecordset*	m_OptimizationParams;
		OptimizationPlanRecordset*		m_OptimizationPlan;

		// VdM 11/3/04
		OptimizationOutSumRecordset*	m_OptimizationOutputSummary;

		// VdM 12/6/04
		OutputResults*					theOutputResults;

		// SSN 1/11/2005
		SimQueueRecordset*				m_SimQueue;

		// SSN 4/15/2005
		SimulationRecordset*			m_Simulation;

		// SSN 4/26/2005
		SocNetworkRecordset*			m_SocNetwork;

		// SSN 4/26/2005
		NetworkParamsRecordset*			m_NetworkParams;
		
		SimLogRecordset*				m_SimLog;

		string						comma;
		string						slash;

		string						iConnectString;
		string						iResultsTableName;
		string						iOptResultsTableName;

		long							iModelID;
		long							iRunID;
		long							iScenarioID;
		long							iStartTime;

		int							warningsLogged;

		int								dayLastWrite;

		void ApplyParameters();
		void RestoreValues();

		void	PreloadAllRecordsets(void);
		void	FreeAllRecordsets(void);

	public:
// methods
		// create & destroy
				MSDatabase(void);
				~MSDatabase(void);

		int	Open(const char* fname, int run_id);

	
		void	InitOutput(void);
		void	GetConnectInfo(string* str) { iConnectString =  *str; } 

		// read
		int	SelectModel(int run_id);

		int	Get(int paramID, int inTherecordNum);

		void	Get(int paramID, int inTheRecordNum, string* outString);
		void	GetBrandName(int brandID, string* brandName);

		void	GetProductName(int productID, string* productName);

		void	GetChannelName(int channelID, string* channelName);

		void	GetSegmentName(int segmentID, string* segmentName);

		void	GetTaskName(int taskID, string* taskName);
		void	GetProdAttrName(int prodAttrID, string* prodAttrName, double& tau);

		// write
		// void	SetResults(string* inString, int optResults, int trialNum, int runNum);

		void	AddToString(string* outputString, CString str, int doComma);
		void	AddToString(string* outputString, int val, int doComma);
		void	AddToString(string* outputString, float val, int doComma);
		void	AddToString(string* outputString, CTime val, int doComma);

		// VdM 10/25/04
		// Scenarios

	/*	void	SelectOptimizationParams(int modelID, int scenarioID);
		void	SelectOptimizationPlan(int modelID);
		void	SelectOptimizationOutputSummary(int modelID, int scenarioID);
	*/	//void	SetOptimizationOutputSummary(string* inString);
		int	GetOptimizationParam(int paramNum);

		int	SimStart();
		int	SimDone();

		void	SimStatus(const string message);
		void	SimStatus(CTime date, const string message);

		SegmentData* NewSegData();
		void WriteSegData();

		SimLogRecordset* NewLogEntry();

		short SimSeed();
};



