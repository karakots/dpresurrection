// dpslim.cpp : Defines the entry point for the console application.
// This is only temporary, and in no defines the main architecture.
//
// Copyright 2003 by  DecisionPower
//
// author: Isaac Noble
// creation data: 9/01/2007
//

#define _CRT_SECURE_NO_WARNINGS

#ifndef WINVER
#define WINVER 0x0501
#endif

//Standard Includes
#include <string>
#include <cstring>

//Precompiled Header -- contains CMicroSegment.h and MSDatabase.h
#include "stdafx.h"

#include "Simulation.h"

using namespace std;

CWinApp theApp = CWinApp("DPSLIM");

void ParseCommandLine(int num_args, char* arg_array[], char* &fname, int &run_id)
{

	for(int i = 1; i < num_args-1; i++)
	{
		string tmp = arg_array[i];
		if(tmp.compare("-f") == 0)
		{
			fname = arg_array[i+1];
		}
		if(tmp.compare("-s") == 0)
		{
			run_id = atoi(arg_array[i+1]);
		}
	}
}

void ParseCommandLine(string commandline, string &fname, int &run_id)
{
	if(commandline.length() <= 0)
	{
		return;
	}

	//Build array of command arguments
	//Should have function to break up string based on delimiter...
	vector<string> arg_array = vector<string>();
	int num_args = 0;
	int index = 0;
	int next = 0;
	while(commandline.find(" ",index) != string::npos)
	{
		next = commandline.find(" ",index);
		arg_array.push_back(commandline.substr(index, next-index));
		num_args++;
		index = next + 1;
	}
	//Get last command argument
	next = commandline.find(" ",index);
	arg_array.push_back(commandline.substr(index, next-index));
	num_args++;

	//Parse command arguments
	for(int i = 0; i < num_args-1; i++)
	{
		string tmp = arg_array[i];
		if(tmp.compare("-f") == 0)
		{
			fname = arg_array[i+1];
		}
		if(tmp.compare("-s") == 0)
		{
			run_id = atoi(arg_array[i+1].c_str());
		}
	}
}

//Windows Application Entry Point
int WINAPI  WinMain ( HINSTANCE  instance, HINSTANCE  previous, LPSTR
commandline, int  show )
{
	string fname = "";
	int	run_id = -1;

	ParseCommandLine(commandline, fname, run_id);

	fname = ".\\connect\\" + fname;

	//Create Database
	MSDatabase* database = new MSDatabase();
	int res = database->Open(fname.c_str(), run_id);

	if(res != 0)
	{
		return res;
	}

	//Set-Up Simulation
	Simulation* sim = new Simulation(database);

	sim->LoadFromDB();

	sim->Initialize();
	
	sim->Run();

	delete sim;
	delete database;

	CoUninitialize();
	return 0;
}

//Console Application Entry Point
int _tmain(int argc, TCHAR* argv[], TCHAR* envp[])
{
	//Windows trickery to get a handle for ODBC
	if(!AfxWinInit(GetModuleHandle(NULL), NULL, GetCommandLine(), SW_HIDE))
	{
		return -1;
	}

	//More windows trickery to hide the console window
	HWND hWnd = GetConsoleWindow();
	ShowWindow( hWnd, SW_HIDE );

	char* fname = "";
	int	run_id = -1;

	ParseCommandLine(argc, argv, fname, run_id);

	//Create Database
	MSDatabase* database = new MSDatabase();
	int i = database->Open(fname, run_id);
	if(i != 1)
	{
		return -1;
	}

	//Set-Up Simulation
	Simulation* sim = new Simulation(database);

	sim->LoadFromDB();

	sim->Initialize();
	
	sim->Run();

	delete sim;
	delete database;

	CoUninitialize();
	return 0;
}

