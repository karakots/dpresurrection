#pragma once
#define _CRT_SECURE_NO_WARNINGS
// PackSize.h
//
// Copyright 2008	DecisionPower
//
// PackSize


#include "stdafx.h"

class PackSize
{
public:
	static PackSize* GetPack(int);

	void Add(int,double);

	int GetSize();


private:
	static vector<PackSize*> Packs;

	vector<double> dist;
	// default constructor
	PackSize(int);

	// default destructor
	~PackSize();

	int id;


};