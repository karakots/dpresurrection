#include "stdafx.h"

class CMicroSegment;
class Consumer;
class Pantry;

class	PopObject
{
public:
	int		iNumProdMsgsAlloc;
	int		iProdsEverBoughtAlloc;
	int		iGroupSize;
	vector< Consumer* >	iConsumerList;
	double		iPopScaleFactor;

	PopObject();
	void	CreatePopulaionCore(int compress, int choiceModel,
			CMicroSegment* theSegment, int numProdChans, int numProducts, 
			int numFeatures, double useThisScaleFactor, int numConsumerTasks, int repurchaseModel);
	void	DestroyConsumerPopulation(void);
	void	CalcPopScaleFactor(int compress, int choiceModel,
			CMicroSegment* theSegment, double useThis);
	void	DeleteProdsEverBought(void);
	
	
	void	AddProduct(int newNumProducts, int repurchaseModel);

	void	DeleteProdMessages(void);
	
	
	void	AddProductToMessages(int newNumProducts);
	
	
	
	
	void	DeleteProdsInStock(void);
	void	ResetProdsInStock(int* ProdsInStock);
	double iAwarenessDecayRatePreUse;
	double iAwarenessDecayRatePostUse;
	double iPersuasionDecayRatePreUse;
	double iPersuasionDecayRatePostUse;
	void 	updateConsumerReferences();
	void	WriteToFile(string file);
	int		ReadFromFile(string file);

	void	CreateProductTries(int num_products);
	void	CreateAwareness(int num_products);
	void	CreatePersuasion(int num_products);
	void	CreateProdsEverBought(int num_products);
	void	CreateDynamicAttributes(int num_products);

	void	ResetDynamicAttributes();
	void	ResetProductTries();
	void	ResetAwareness();
	void	ResetPersuasion();
	void	ResetProdsEverBought();
};
