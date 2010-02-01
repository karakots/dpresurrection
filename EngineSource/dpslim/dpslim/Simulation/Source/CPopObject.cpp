// CPopObject.cpp
//
// Copyright 1998	Salamander Interactive
//
// Multiagent population object
// Created by Ken Karakotsios 10/26/98
#include "PopObject.h"

#include "Consumer.h"
#include "DBModel.h"
#include "Product.h"
#include "Pantry.h"


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
PopObject::PopObject(void)
{
	iNumProdMsgsAlloc = 0;
	iProdsEverBoughtAlloc = 0; 

	iAwarenessDecayRatePreUse = 1;
	iAwarenessDecayRatePostUse = 0;
	iPersuasionDecayRatePreUse = 1;
	iPersuasionDecayRatePostUse = 1;
	iPopScaleFactor = 1.0;
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	PopObject::CreateProdsEverBought(int num_products)
{
	for(size_t i = 0; i < iConsumerList.size(); ++i)
	{
		iConsumerList[i]->productsBought.clear();
		iConsumerList[i]->productsBought = vector<unsigned int>(num_products, 0);
	}
}

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	PopObject::CreatePersuasion(int num_products)
{
	for(size_t i = 0; i < iConsumerList.size(); ++i)
	{
		iConsumerList[i]->productPersuasion.clear();
		iConsumerList[i]->productPersuasion = vector<double>(num_products, 0.0);
	}
}


void	PopObject::CreateAwareness(int num_products)
{
	for(size_t i = 0; i < iConsumerList.size(); ++i)
	{
		iConsumerList[i]->productAwareness.clear();
		iConsumerList[i]->productAwareness = vector<int>(num_products, -1);
	}
}

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	PopObject::CreateProductTries(int num_products)
{
	for(size_t i = 0; i < iConsumerList.size(); ++i)
	{
		iConsumerList[i]->productTries.clear();
		iConsumerList[i]->productTries = vector<int>(num_products, -1);
	}
}

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	PopObject::CreateDynamicAttributes(int num_products)
{
	for(size_t i = 0; i < iConsumerList.size(); ++i)
	{
		iConsumerList[i]->dynamic_attributes.clear();
		iConsumerList[i]->dynamic_attributes = vector<map<int, DynamicAttribute*>>(num_products);
		for(Index pn = 0; pn < iConsumerList[i]->dynamic_attributes.size(); pn++)
		{
			iConsumerList[i]->dynamic_attributes[pn] = map<int, DynamicAttribute*>();
		}
	}
}

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	PopObject::ResetProdsEverBought()
{
	for(size_t i = 0; i < iConsumerList.size(); ++i)
	{
		for(size_t ii = 0; ii < iConsumerList[i]->productsBought.size(); ++ii)
		{
			iConsumerList[i]->productsBought[ii] = 0;
		}
	}
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void PopObject::ResetPersuasion()
{
	for(size_t i = 0; i < iConsumerList.size(); ++i)
	{
		for(size_t ii = 0; ii < iConsumerList[i]->productPersuasion.size(); ++ii)
		{
			iConsumerList[i]->productPersuasion[ii] = 0.0;
		}
	}
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void PopObject::ResetAwareness()
{
	for(size_t i = 0; i < iConsumerList.size(); ++i)
	{
		for(size_t ii = 0; ii < iConsumerList[i]->productAwareness.size(); ++ii)
		{
			iConsumerList[i]->productAwareness[ii] = -1;
		}
	}
}

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void PopObject::ResetProductTries()
{
	for(size_t i = 0; i < iConsumerList.size(); ++i)
	{
		for(size_t ii = 0; ii < iConsumerList[i]->productTries.size(); ++ii)
		{
			iConsumerList[i]->productTries[ii] = -1;
		}
	}
}

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void PopObject::ResetDynamicAttributes()
{
	map<int, DynamicAttribute*>::iterator iter;
	for(size_t i = 0; i < iConsumerList.size(); ++i)
	{
		for(Index pn = 0; pn < iConsumerList[i]->dynamic_attributes.size(); pn++)
		{
			for(iter = iConsumerList[i]->dynamic_attributes[pn].begin(); iter != iConsumerList[i]->dynamic_attributes[pn].end(); iter++)
			{
				delete iter->second;
			}
			iConsumerList[i]->dynamic_attributes[pn].clear();
		}
	}
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	PopObject::DestroyConsumerPopulation(void)
{
	for(size_t guy = 0; guy < iConsumerList.size(); ++guy)
	{
		delete iConsumerList[guy]->pantry;
		delete iConsumerList[guy]->coupons;
		for(size_t task = 0; task < iConsumerList[guy]->iTasks.size(); ++task)
		{
			delete iConsumerList[guy]->iTasks[task];
		}
		for(Index pn = 0; pn < iConsumerList[guy]->dynamic_attributes.size(); pn++)
		{
			for(map<int, DynamicAttribute*>::iterator iter = iConsumerList[guy]->dynamic_attributes[pn].begin(); iter != iConsumerList[guy]->dynamic_attributes[pn].end(); iter++)
			{
				delete iter->second;
			}
			iConsumerList[guy]->dynamic_attributes[pn].clear();
		}
		iConsumerList[guy]->dynamic_attributes.clear();
		iConsumerList[guy]->iTasks.clear();
		iConsumerList[guy]->productTries.clear();
		iConsumerList[guy]->productAwareness.clear();
		iConsumerList[guy]->productPersuasion.clear();
		iConsumerList[guy]->productsBought.clear();
		delete iConsumerList[guy];
	}

	iConsumerList.clear();
}


// ------------------------------------------------------------------------------
//	
// ------------------------------------------------------------------------------
void PopObject::CreatePopulaionCore(int compress, int choiceModel,
									CMicroSegment* theSegment, int numProdChans, int numProds, 
									int numFeatures, double useThisScaleFactor, int numConsumerTasks, int repurchaseModel)
{
	DestroyConsumerPopulation();
	this->iPopScaleFactor = 100/theSegment->iModelInfo->scale_factor;
	iConsumerList.resize(iGroupSize);

	for (size_t guyNum = 0; guyNum < iConsumerList.size(); guyNum++)
	{
		iConsumerList[guyNum] = new Consumer();
	}

	//initialize each consumer

	for (size_t guyNum = 0; guyNum < iConsumerList.size(); guyNum++)
	{
		Consumer* aGuy = iConsumerList[guyNum];
		aGuy->segment = theSegment;
		aGuy->choiceModel = theSegment->GetChoiceModel();
		aGuy->repurchaseModel = theSegment->GetRepurchaseModel();
		aGuy->id = (int)guyNum;
		aGuy->coupons = new set< int >();
		aGuy->pantry = new Pantry(theSegment);
		aGuy->pantry->AddBin();
		aGuy->pantry->SetActivationEnergy(-13.0);
		aGuy->iShoppingChance = 0.142857;
		aGuy->iInStoreSwitchFactor = 1;
		aGuy->iTasks = vector< PantryTask* >();
		aGuy->iTasks.push_back(0);
		aGuy->iPreferredProduct = kNoProduct;
	}

	CreateProductTries(numProds);
	CreateAwareness(numProds);
	CreatePersuasion(numProds);
	CreateProdsEverBought(numProds);
	CreateDynamicAttributes(numProds);
}

/*void PopObject::updateConsumerReferences()
{
	int pnConsBase;
	for (size_t guyNum = 0; guyNum < iConsumerList.size(); guyNum++)
		{
			Consumer* aGuy = iConsumerList[guyNum];
			aGuy->numProducts = (int)aGuy->segment->iProducts.size();
			pnConsBase = (int)guyNum * aGuy->numProducts ;			
			// begging to be a structure
			aGuy->productAwareness = &(iProdAwareness[pnConsBase]);
			aGuy->productPersuasion = &(iProdPersuasion[pnConsBase]);
			aGuy->productTries = &(iProdTriers[pnConsBase]);
			aGuy->productsBought = &(iProdsEverBought[pnConsBase]);
		}
}*/

void PopObject::WriteToFile(string file)
{
	ofstream outfile;
	outfile.open(file.c_str(),ifstream::out);

	outfile << iConsumerList.size() << " ";

	outfile << iConsumerList[0]->segment->iProducts.size() << " ";

	outfile << iConsumerList[0]->segment->iProductsAvailable.size() << " ";

	outfile << iConsumerList[0]->segment->iNumDays << " ";

	for(Pn_iter iter = iConsumerList[0]->segment->iProducts.begin(); iter != iConsumerList[0]->segment->iProducts.end(); ++iter)
	{
		outfile << (*iter)->iNumEverTriedProduct << " ";
	}

	for(Pn_iter iter = iConsumerList[0]->segment->iProducts.begin(); iter != iConsumerList[0]->segment->iProducts.end(); ++iter)
	{
		outfile << (*iter)->iRepeatPurchasersCumulative << " ";
	}

	for(Pn_iter iter = iConsumerList[0]->segment->iProducts.begin(); iter != iConsumerList[0]->segment->iProducts.end(); ++iter)
	{
		outfile << (*iter)->iTotalRepeatPurchaseOccasionsCumulative << " ";
	}

	outfile << endl;

	iConsumerList[0]->segment->WriteAttributes(outfile);

	outfile << endl;

	for(size_t i = 0; i < this->iConsumerList.size(); i++)
	{
		iConsumerList[i]->WriteToFile(outfile);
	}
}

int PopObject::ReadFromFile(string file)
{
	int consumer_size = -1;
	int product_size = -1;
	int prod_chan_size = -1;

	ifstream infile;
	const char* fname = file.c_str();
	infile.open (file.c_str(), ifstream::in);

	infile >> consumer_size;

	if(iConsumerList.size() != consumer_size)
	{
		return false;
	}

	infile >> product_size;

	if(iConsumerList[0]->segment->iProducts.size() != product_size)
	{
		return false;
	}

	infile >> prod_chan_size;

	if(iConsumerList[0]->segment->iProductsAvailable.size() != prod_chan_size)
	{
		return false;
	}

	infile >> iConsumerList[0]->segment->iNumDays;

	for(Pn_iter iter = iConsumerList[0]->segment->iProducts.begin(); iter != iConsumerList[0]->segment->iProducts.end(); ++iter)
	{
		infile >> (*iter)->iNumEverTriedProduct;
	}

	for(Pn_iter iter = iConsumerList[0]->segment->iProducts.begin(); iter != iConsumerList[0]->segment->iProducts.end(); ++iter)
	{
		infile >> (*iter)->iRepeatPurchasersCumulative;
	}

	for(Pn_iter iter = iConsumerList[0]->segment->iProducts.begin(); iter != iConsumerList[0]->segment->iProducts.end(); ++iter)
	{
		infile >> (*iter)->iTotalRepeatPurchaseOccasionsCumulative;
	}

	if(iConsumerList[0]->segment->iModelInfo->reset_panel_data_day > 0)
	{
		for(Pn_iter iter = iConsumerList[0]->segment->iProducts.begin(); iter != iConsumerList[0]->segment->iProducts.end(); ++iter)
		{		
			(*iter)->iNumEverTriedProduct = 0;
			(*iter)->iRepeatPurchasersCumulative = 0;
			(*iter)->iTotalRepeatPurchaseOccasionsCumulative = 0;
		}
	}

	iConsumerList[0]->segment->ReadAttributes(infile);

	for(Pn_iter iter = iConsumerList[0]->segment->iProducts.begin(); iter != iConsumerList[0]->segment->iProducts.end(); ++iter)
	{
		(*iter)->iNumAwareSofar = 0;
		(*iter)->iTotalPersuasion = 0;
	}

	iConsumerList[0]->segment->iTotalMessages = 0;

	for(size_t i = 0; i < this->iConsumerList.size(); i++)
	{
		iConsumerList[i]->ReadFromFile(infile);
	}

	return true;
}