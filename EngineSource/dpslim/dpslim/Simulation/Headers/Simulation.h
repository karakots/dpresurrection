// simulation.h : This is the main simulation header
// This file and the companion cpp file, define the main architecture of the
// new engine.
//
// Copyright 2007 by  DecisionPower
//
// author: Isaac Noble
// creation data: 9/01/2007

//Precompiled Header -- contains CMicroSegment.h and MSDatabase.h
#include "stdafx.h"

class CMicroSegment;
class MSDatabase;
class Product;
class Channel;
class SocialNetwork;
class ProductTree;

class TimeController;
class Media;
class Coupon;
class Price;
class Display;
class Distribution;
class ExternalFactor;
class MarketUtility;
class ProductAttribute;
class ConsumerPref;
class Attribute;



class Simulation
{
public:
	//Basic Constructore
	Simulation(MSDatabase*);
	~Simulation();

	//Runs the simulation
	void Run();

	//Initializes the simulation
	void Initialize();

	//Loads the simulation from the database
	void LoadFromDB();

	//Checks if it is time to write out to DB
	bool TimeToWrite(CTime&);

private:

	// Functions used to reset the simulation
	void process_initial_conditions();
	void reset_values();

	//Segment Reader Helpers
	void read_products();
	void read_channels();
	void read_segment_channels();
	void read_product_attributes(bool);
	void read_segment_attributes(bool);
	void read_attributes();
	void read_product_tree(int);
	void read_product_sizes();
	void read_prerequistes();
	void read_social_networks();
	void read_initial_conditions();

	//Market Data Reader Helpers
	void read_media_and_coupons();
	void read_distribution_and_display();
	void read_price();
	void read_market_utility();
	void read_external_factors();

	//Special NIMO functions
	void apply_nimo_taus();

	//NIMO APP
	int isNimo;

	// ------------------------------------------------------------------------------
	// Maps going from id to records.  This serves as an in memory version of the 
	// database.  These can then be used to reload the simulation without having to 
	// reread the database. This was chosen so that multiple runs can be performed 
	// with only a single database read.
	// ------------------------------------------------------------------------------
	//Static Data
	map<int, CMicroSegment*> segments;
	map<int, Product*> products;
	map<int, Channel*> channels;
	vector<pair<int, int>> product_tree;
	map<pair<int, int>, double> product_size;
	map<pair<int, int>, double> prerequistes;
	map<int, map<int, double>> segment_channel_choice;
	map<int, map<int , InitialCondition*>> initial_conditions;
	vector<SocialNetwork*> social_networks;

	//Time Series Data
	CTime start_date;
	CTime end_date;
	CTime last_write;
	TimeController* time_controller;
	vector<Media*> mass_media;
	vector<Coupon*> coupons;
	vector<Distribution*> distribution;
	//vector<Display*> displays;
	vector<Price*> price;
	vector<MarketUtility*> market_utility;
	vector<ExternalFactor*> external_factors;
	vector<ProductAttribute*> product_attributes;
	vector<ConsumerPref*> consumer_prefs;
	map<int, Attribute*> attributes;

	//Database
	MSDatabase* database;
	ModelRecordset* iModelInfo;
};