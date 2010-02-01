#include "Consumer.h"
#include "RepurchaseModel.h"
#include "ProductTree.h"
#include "Product.h"
#include "Attributes.h"
#include "ChoiceModel.h"
#include "Pantry.h"
#include "Channel.h"

//Static consideration set
vector< Pcn > Consumer::products_in_channel = vector< Pcn >();
vector< Pcn > Consumer::consideration_set = vector< Pcn >();
vector< Pcn > Consumer::shopping_cart = vector< Pcn >();

// ------------------------------------------------------------------------------
// Determines if the consumer is ready to shop again.  Currently consumers are 
// going to shop every week.  If the conumser is ready to shop then there pantry
// will be updated based on task use.
// ------------------------------------------------------------------------------
void Consumer::Shop()
{
	static int total = 0;
	static int pass = 0;

	total++;
	// ------------------------------------------------------------------------------
	// First determine if the consumer is ready to shop.  In the future channels may
	// have different shopping rates, so this may have to be combined with
	// SelectChannel().
	// ------------------------------------------------------------------------------
	if(!WillDo0to1(iShoppingChance))
	{
		iDaysSinceLastShopping++;
		return;
	}

	pass++;

	for(Pcn_iter iter = segment->iProductsAvailable.begin(); iter != segment->iProductsAvailable.end(); ++iter)
	{
		(*iter)->overallScore = 0;
	}

	//Perform Tasks
	PerformTasks();

	//Choose a channel to go shopping in
	SelectChannel();

	//Go Shopping
	GoShopping();

	//Reset days since last shopping trip.
	iDaysSinceLastShopping = 0;
	return;
}

// ------------------------------------------------------------------------------
// Selects a channel to shop in.  Chooses a random channel is consumer does not
// have a preferred channel or isn't loyal.  Modifies iPreferredChannel.
// ------------------------------------------------------------------------------
void Consumer::SelectChannel()
{	
	// occasionally consider switching Channels
	if (iPreferredChannel == kNoChannel || LocalRand(1000) >= (segment->iChannelLoyalty * 100.0))
	{
		ChooseAChannel();
	}

	return;
}

// ------------------------------------------------------------------------------
// Pick a channel at random from existing channels and sets iPreferredChannel.
// ------------------------------------------------------------------------------
void	Consumer::ChooseAChannel()
{
	double	randNum;
	size_t cn = 0;

	randNum = LocalRandUniform();

	for (Cn_iter iter = segment->iChannels.begin(); iter != segment->iChannels.end(); ++iter)
	{
		cn = (*iter)->iChannelIndex;
		if (((*iter)->iPctChanceChosen > 0.0) &&
			(randNum <= (*iter)->iCumPctChanceChosen))
		{
			iPreferredChannel = (unsigned int)cn;
			return;
		}
	}

	iPreferredChannel = (unsigned int)cn;
}

// ------------------------------------------------------------------------------
// Performs the all the tasks for this consumer.  Amount used is determined by
// the time since the last shopping trip. Modifies the consumers pantry.
// ------------------------------------------------------------------------------
void Consumer::PerformTasks()
{
	//Loop through all tasks
	for(size_t i = 0; i < iTasks.size(); i++)
	{
		//Update the pantry
		this->pantry->PerformTask(iTasks[i], iDaysSinceLastShopping);
	}
}

// ------------------------------------------------------------------------------
// Implements the main shopping loop.  After preprocessing, the consumer will
// continue to Add products to the shopping cart until no product is in the
// consideration set.  The consumer will then purchase all products int the
// shopping cart.
// ------------------------------------------------------------------------------
void Consumer::GoShopping()
{
	Pcn product_to_buy = Pcn(0,0);


	//Do Shopping Preprocessing
	DoShoppingPreprocessing();

	while(product_to_buy != Pcn(-1,-1))
	{
		//Build the consideration set
		consideration_set.clear();
		BuildConsiderationSet();

		//Select a product to buy
		product_to_buy = ChoosePreferredProduct();

		//Check if consumer made a purchase
		if(product_to_buy != Pcn(-1,-1))
		{
			//Update the cart with the product bought
			UpdateCart(product_to_buy);

			//Update preferred product
			//This should change in the future to reflect product heirarchy
			iPreferredProduct = product_to_buy.pn;

			//Consider changing brands
			ConsiderInStoreSwitching();
		}
	}

	//Checkout
	PurchaseProducts(false, false);
}

// ------------------------------------------------------------------------------
// Does all preprocessing for the main shopping loop.
// 1) Initializes all variables
// 2) Determines products in channel
// 3) Processes Displays
// 4) Computes initial utilities
// 5) Determines if consumer has a preferred brand
// ------------------------------------------------------------------------------
void Consumer::DoShoppingPreprocessing()
{
	// Initialize variables
	shopping_cart.clear();
	products_in_channel.clear();

	// Determine products in channel
	GetProductsInChannel();

	// Process displays
	// This sets msgWeightedScores for utility calc
	ProcessDisplays();

	// Compute initial product utilities
	ComputeProductUtilities();
	
	// See if consumer has a preferred brand
	ConsiderBrandChange();
}

// ------------------------------------------------------------------------------
// Adds all the products currently avaible in the current channel into the
// consideration set.  Initializes any necessary variables.
// ------------------------------------------------------------------------------
void Consumer::GetProductsInChannel()
{
	Index pn;
	Pcn pcn;
	
	for (Pcn_iter iter = segment->iProductsAvailable.begin(); iter != segment->iProductsAvailable.end(); ++iter)
	{
		pcn = segment->GetPcn(iter);
		pn = pcn.pn;
		//Initialize variables
		(*iter)->iUsingPriceType = kPriceTypeUnpromoted;

		// Check if this is the right channel
		if (!IsChannelOK(pcn))
		{
			// Nope go to next product
			continue;
		}

		// Make sure product isn't eliminated and it is active
		if ((*iter)->iActive)
		{
			// check if consumer has tried this product
			(*iter)->iHaveNotUsedProductBefore = !(EverTriedProduct(pn, true));
			
			// check of product is in stock (distribution) on this trip
			// random draw occurs here
			if (segment->IsProductInStock(pcn))
			{
				// Product has passed all the tests
				// Use random draw to set price of product
				segment->SetPrice(pcn);

				// Put product into consideration set
				products_in_channel.push_back(pcn);
				
			}	// end check for product available

		}	// end check for elimination and activity
	} // next prodChan
}

// ------------------------------------------------------------------------------
// Process the displays for all the products available in the channel
// ------------------------------------------------------------------------------
void Consumer::ProcessDisplays()
{
	//A bucket is needed because of awarness bleeding to siblings
	static Bucket bucket;


	int numDisplayHits = 0;
	Pcn pcn;
	int index;
	
	//Fill bucket
	bucket.Reset((int)products_in_channel.size());
	while( !bucket.Empty())
	{
		//get the index
		index = bucket.Draw();

		//get the pcn
		pcn = products_in_channel[index];

		//Reset Utility
		segment->iProductsAvailable[pcn]->msgWeightedScores = 0;

		//process the displays
		//this sets msgWeightedScores
		//Also updates numDisplayHits
		ProcessDisplays(pcn, numDisplayHits);
	}
}

// ------------------------------------------------------------------------------
// Computes the initial utilities for all the products in the channel
// ------------------------------------------------------------------------------
void Consumer::ComputeProductUtilities()
{
	Pcn pcn; 

	//Compute iTotalMessages for persuasion calculation
	//CountMyAvailableProdMsgs();

	//Loop through all products
	for(size_t i = 0; i < products_in_channel.size(); i++)
	{
		//Get pcn from list
		pcn = products_in_channel[i];

		//Compute all the utility components for the product channel
		ComputeProductScores(pcn);

		//Computes total utility for the product
		ComputeProductUtility(pcn);
	}
}

// ------------------------------------------------------------------------------
// Computes the all the utility components for the product channel record.
// ------------------------------------------------------------------------------
void Consumer::ComputeProductScores(Pcn pcn)
{
	//grab the product number
	Index pn = pcn.pn;
	
	//grab the persuasion
	double persuasion = productPersuasion[pn];

	//find the persuasion score based on the choice model CHECKME
	this->segment->iProductsAvailable[pcn]->persuasionScore = choiceModel->MsgContribution(persuasion, segment->iMessageSensitivity);

	//grab the price type
	int locPriceType = this->segment->iProductsAvailable[pcn]->iUsingPriceType;

	//find the feature score
	if (segment->iProductsAvailable[pcn]->iHaveNotUsedProductBefore)
	{
		this->segment->iProductsAvailable[pcn]->attributeScore = segment->iProducts[pn]->iPreUseFeatureScore + DynamicScore(pcn, !segment->iProductsAvailable[pcn]->iHaveNotUsedProductBefore);
		this->segment->iProductsAvailable[pcn]->priceScore = this->segment->iProductsAvailable[pcn]->iPreProdPriceScores[locPriceType];
	}
	else
	{
		this->segment->iProductsAvailable[pcn]->attributeScore = segment->iProducts[pn]->iPostUseFeatureScore + DynamicScore(pcn, !segment->iProductsAvailable[pcn]->iHaveNotUsedProductBefore);;
		this->segment->iProductsAvailable[pcn]->priceScore = this->segment->iProductsAvailable[pcn]->iPostProdPriceScores[locPriceType];
	}

	//get the pantry score
	this->segment->iProductsAvailable[pcn]->pantryScore = this->pantry->ComputeUtility(pcn);

	//Modify pantry score with activation energy
	this->segment->iProductsAvailable[pcn]->pantryScore += this->pantry->GetActivationEnergy();

	
	this->segment->iProductsAvailable[pcn]->errorScore = 0;
}

// ------------------------------------------------------------------------------
// Computes the total utility for a product based on various utility scores
// ------------------------------------------------------------------------------
void Consumer::ComputeProductUtility(Pcn pcn)
{
	double score = 0;
	score = this->segment->iProductsAvailable[pcn]->msgWeightedScores;
	score += this->segment->iProductsAvailable[pcn]->errorScore;
	score += this->segment->iProductsAvailable[pcn]->pantryScore;
	score += this->segment->iProductsAvailable[pcn]->priceScore;
	score += this->segment->iProductsAvailable[pcn]->attributeScore;
	score += this->segment->iProductsAvailable[pcn]->persuasionScore;
	this->segment->iProductsAvailable[pcn]->overallScore = score;
}

// ------------------------------------------------------------------------------
// Determines if the consumer will consider changing brands
// ------------------------------------------------------------------------------
void Consumer::ConsiderBrandChange()
{
	//Check if the consumer has a preferred brand
	if(this->iPreferredProduct != kNoProduct)
	{
		// use brand loyalty to drive change considerations
		// brand loyalty ranges from 0 to 10.  
		// 0 is always consider change, 10 is never consider change
		// 0 to 10 WTF is this.... ISN 2007
		double	churnFactor;
		churnFactor = LocalRand(1000);
		if (churnFactor >= (segment->iBrandLoyalty * 100.0))
		{
			this->iPreferredProduct = kNoProduct;
		}
	}
}

// ------------------------------------------------------------------------------
// Determines if the consumer will consider changing brands
// ------------------------------------------------------------------------------
void Consumer::ConsiderInStoreSwitching()
{
	float	switchFactor;
	switchFactor = RandomUniform();
	if (switchFactor > iInStoreSwitchFactor)
	{
		this->iPreferredProduct = kNoProduct;
	}
}

// ------------------------------------------------------------------------------
// Builds the consumers consideration set.  Checks all the products in the
// channel to see if consumer will consider buying them
// ------------------------------------------------------------------------------
void Consumer::BuildConsiderationSet()
{
	Pcn pcn;

	//Loop through all the products in the channel
	for(size_t i = 0; i < products_in_channel.size(); ++i)
	{
		pcn = products_in_channel[i];

		//Check is products meets the consumers constaints
		if(!CheckConstraints(pcn))
		{
			continue;
		}

		//Product meets all constaints, so Add it to the consideration set
		consideration_set.push_back(pcn);
	}
}

// ------------------------------------------------------------------------------
// Checks a product to see if it meets the constraints to be considered by the
// consumer:
// 1) Positive total utility
// 2) Consumer hasn't rejected product
// 3) Consumer has Prerequisites
// 4) Aware
// ------------------------------------------------------------------------------
int Consumer::CheckConstraints(Pcn pcn)
{
	Index pn = pcn.pn;

	//Check is total utility is positive
	if(segment->iProductsAvailable[pcn]->overallScore <= 0)
	{
		return false;
	}

	//Consumer tried and rejected product
	if (ConsumerTriedAndRejectedProduct(pn))
	{
		return false;
	}

	if(!HavePurchasedPrereqProduct(pn))
	{
		return false;
	}

	//Check awareness
	if (!Aware(pn))
	{
		return false;
	}

	return true;
}

// ------------------------------------------------------------------------------
// Has this consumer ever purchased anyting that would be a prerequisite to 
// the stated product?
// This may change...ISN 3/2007
// ------------------------------------------------------------------------------
int Consumer::HavePurchasedPrereqProduct(Index pn)
{	if(segment->iProducts[pn]->iPrerequisiteNum.size() == 0 && segment->iProducts[pn]->iInCompatibleNum.size() == 0)
	{
		return true;
	}

	int prerequites = true;
	int incompatible = false;
	if(segment->iProducts[pn]->iPrerequisiteNum.size() > 0)
	{
		prerequites = false;

		for (Index prn =  0; prn < segment->iProducts[pn]->iPrerequisiteNum.size();  prn++)
		{
			Index prProdNum = segment->iProducts[pn]->iPrerequisiteNum[prn];
			if(EverTriedProduct(prProdNum, false))
			{
				prerequites = true;
			}
		}
	}

	if (segment->iProducts[pn]->iInCompatibleNum.size() > 0)
	{
		incompatible = true;

		for (Index prn =  0; prn < segment->iProducts[pn]->iInCompatibleNum.size();  prn++)
		{
			Index prProdNum = segment->iProducts[pn]->iInCompatibleNum[prn];
			if(EverTriedProduct(prProdNum, false))
			{
				incompatible = false;
			}
		}
	}

	if (prerequites && !incompatible)
	{
		return true;
	}


	return false;

}

// ------------------------------------------------------------------------------
//  Choose a product from the consideration set.  Applies all choice math.
// ------------------------------------------------------------------------------
Pcn	Consumer::ChoosePreferredProduct()
{
	Pcn		pcn = Pcn(-1,-1);
	Index		pn = -1;
	double	scoreScaleFactor;
	double	aRandomNumber;
	double	score;
	vector< Pcn >::iterator iter;

	//See if purchase will be made...
	if(consideration_set.size() == 0)
	{
		return Pcn(-1,-1);
	}

	//See if consumer has a preferred product
	if(iPreferredProduct != kNoProduct)
	{
		//See if the preferred product is in the consideration set
		int preferred_product_in_set = false;
		for(iter = consideration_set.begin(); iter != consideration_set.end(); iter++)
		{
			pcn = *iter;
			pn = pcn.pn;
			if(ProductTree::theTree->isAncestor(iPreferredProduct, pn))
			{
				preferred_product_in_set = true;
				break;
			}
		}
		if(preferred_product_in_set)
		{
			vector<Pcn> new_set = vector<Pcn>();
			//Remove all other products from the consideration set
			for(iter = consideration_set.begin(); iter != consideration_set.end(); iter++)
			{
				pcn = *iter;
				pn = pcn.pn;

				if(ProductTree::theTree->isAncestor(iPreferredProduct, pn))
				{
					new_set.push_back(pcn);
				}
			}

			consideration_set.clear();

			for(iter = new_set.begin(); iter != new_set.end(); iter++)
			{
				consideration_set.push_back(*iter);
			}

		}
	}

	// scaled utility(p) = utility(p) / (product(all utilities))^(1/number of products)
	// hiho this is a non-standard special case for some strange reason...
	// should try testing without
	//ScaleScoresLogit();	// modifies msgWeightedScores

	// compute probability of choice, based on all known to this point
	// put into msgWeightedScores and make cumulative
	double scoreBound;
	scoreBound = GetLogitProbabilityFromUtility();

	// we could end up with scoreBound < 1 for valid reasons
	// in this case, we should scale the scores

	if (scoreBound <= 0)		// we found no products that we will consider
	{
		return Pcn(-1,-1);
	}

	scoreScaleFactor = 1.0 / scoreBound;

	// get a random number between 0 and sumMsgWeightedScores
	aRandomNumber = LocalRand((long)kMessageWeightScaleFactor);
	aRandomNumber /= kMessageWeightScaleFactor;

	// select a product randomly, weighted by score
	// products are sorted by score, with product 0 have a 0 offset for its score
	for (iter = consideration_set.begin(); iter != consideration_set.end(); ++iter)
	{

		pcn = *iter;
		score = (this->segment->iProductsAvailable[pcn]->overallScore) * scoreScaleFactor;
		if ((score > 0.0) && (aRandomNumber < score))
		{
			return pcn;
		}
	}

	return Pcn(-1,-1);
}

// ------------------------------------------------------------------------------
// Update consumers cart with a product.  Will also update the consumers pantry
// and recalculate the product utilites
// ------------------------------------------------------------------------------
void Consumer::UpdateCart(Pcn pcn)
{
	//Update the shopping cart
	shopping_cart.push_back(pcn);

	//Add product to pantry
	Index pn = pcn.pn;
	pantry->AddProduct(pcn);
	segment->iProductsAvailable[pcn]->iNotProcessed = true;

	//Update pantry score and total utility
	UpdatePantryScores();
}

// ------------------------------------------------------------------------------
// Updates the pantry score and total utility for every product in the channel
// ------------------------------------------------------------------------------
void Consumer::UpdatePantryScores()
{
	static int tick = 0;
	Pcn pcn;
	Index pn;
	vector< Pcn >::iterator iter;

	//Loop through all the products in the channel
	for (iter = products_in_channel.begin(); iter != products_in_channel.end(); ++iter)
	{
		pcn = *iter;
		pn = pcn.pn;

		//Update the pantry score
		this->segment->iProductsAvailable[pcn]->pantryScore = this->pantry->ComputeUtility(pcn);

		//Compute the new total utility
		ComputeProductUtility(pcn);
	}
}

// ------------------------------------------------------------------------------
// Computes the product utility without the pantry model
// ------------------------------------------------------------------------------
void Consumer::ComputeScoreNoPantry()
{
	static int tick = 0;
	Pcn pcn;
	Index pn;
	vector< Pcn >::iterator iter;

	//Loop through all the products in the channel
	for (iter = products_in_channel.begin(); iter != products_in_channel.end(); ++iter)
	{
		pcn = *iter;
		pn = pcn.pn;

		//Update the pantry score
		this->segment->iProductsAvailable[pcn]->pantryScore = 1000;

		//Compute the new total utility
		ComputeProductUtility(pcn);
	}
}



// ------------------------------------------------------------------------------
// Purchases all the products in the consumers shopping cart and updates metrics
// ------------------------------------------------------------------------------
void Consumer::PurchaseProducts(int isSample, int isInitialPurchases)
{
	Pcn pcn;
	Index pn;
	int switched;
	int priceType;

	vector< Pcn >::const_iterator iter;

	//Check for zero unit trip
	if(shopping_cart.size() == 0 && !isSample && !isInitialPurchases)
	{
		//Still need to record a shopping trip so....
		ComputeScoreNoPantry();

		//Build the consideration set
		consideration_set.clear();
		BuildConsiderationSet();

		//Find the product consumer would have bought
		Pcn best = ChoosePreferredProduct();

		if(best != Pcn(-1,-1))
		{
			//Record a trip
			segment->iProductsAvailable[best]->iNumTrips += 1;
		}

		return;
	}

	//Loop over all products in the shopping cart
	for(iter = shopping_cart.begin(); iter != shopping_cart.end(); iter++)
	{
		
		//Grab numbers off the product channel struct
		pcn = *iter;
		pn = pcn.pn;
		priceType = segment->iProductsAvailable[pcn]->iUsingPriceType;


		//Check for switches
		switched = CheckForSwitches(pn);

		//If not a sample update purchase metrics
		if (!isSample)
		{
			//Interesting that initial purchases are counted here
			segment->iProductsAvailable[pcn]->iAmountPurchasedThisTick[priceType] += 1;
			//Not an initial purchase so count it
			if (!isInitialPurchases)
			{
				segment->iProductsAvailable[pcn]->iTotalPurchaseOccasionsThisTick += 1;
			}
		}

		//Update num trips
		if (!isSample && !isInitialPurchases && segment->iProductsAvailable[pcn]->iNotProcessed)
		{
			segment->iProductsAvailable[pcn]->iNumTrips += 1;
			segment->iProductsAvailable[pcn]->iNotProcessed = false;
		}

		//Loop through ancestors to update metrics up product heirarchy
		int numTriesBeforeThisOne;

		//Get number of times consumer has tried the product
		numTriesBeforeThisOne = ConsumerIsTryingProduct(pn, isSample);

		//Possibly reject product
		if (!isInitialPurchases)
		{
			//WARNING This depends on the purchased product being last in the ancestor list
			SeeIfRepurchaserDrops(pn, numTriesBeforeThisOne);
		}

		//Become Aware of dynamic attibutes
		for(da_iter iter = dynamic_attributes[pn].begin(); iter != dynamic_attributes[pn].end(); ++iter)
		{
			iter->second->MakeAware();
		}

		iProductLastBought = pn;
	}

	
}

// ------------------------------------------------------------------------------
// If consumer is dissatisfied with product, then the producted is rejected.
// Since this can only happen after consumer has tried the product a consumer
// may have more rejected products in there shopping cart.
// ------------------------------------------------------------------------------
void	Consumer::SeeIfRepurchaserDrops(Index pn, int numTriesBeforeThisOne)
{
	//Consumer won't reject a product they have tried 3 or more times
	if (numTriesBeforeThisOne >= 3)
	{
		return;
	}

	double	pctDrop;

	//Find drop chance
	if (numTriesBeforeThisOne == 0)
	{
		pctDrop = segment->iProducts[pn]->iInitialDropRate;
	}
	else
	{
		pctDrop = 1.0 - segment->iProducts[pn]->iDropRateDecay;
	}
	
	//Optimization to avoid computing a random number
	if (pctDrop > 0.0)
	{
		if (WillDo0to1(pctDrop))
		{
			//Reject the product
			RejectProductNow(pn);
		}
	}
}


// ------------------------------------------------------------------------------
// Rejects the product and possibly all siblings
// ------------------------------------------------------------------------------
void	Consumer::RejectProductNow(Index pn)
{
	//Reject product
	productsBought[pn] |= kRejectedBit;

	// Find all siblings
	ProductTree::List* sibs = ProductTree::theTree->Siblings(pn);

	//If siblings and exist and should be rejected
	if (segment->iShareAwarenessWithSibs && sibs)
	{
		//Then reject all siblings
		for( ProductTree::Iter iter = sibs->begin(); iter != sibs->end(); ++iter)
		{
			int sibPn = *iter;
			productsBought[sibPn] |= kRejectedBit;
		}
	}
}