// Consumer.cpp
//
// Copyright 2005	DecisionPower
//
// Consumer Agent Object
// Created by Ken Karakotsios 10/26/98
#include "Consumer.h"
#include "RepurchaseModel.h"
#include "ProductTree.h"
#include "DBModel.h"
#include "Product.h"
#include "Attributes.h"
#include "ChoiceModel.h"

// ------------------------------------------------------------------------------
//	Everything should be allocated and reset in the newPopulation
//	by the time we get to here.
// ------------------------------------------------------------------------------
void	Consumer::CopyTo(Consumer* destGuy)
{
	destGuy->iRepurchaseProb =			iRepurchaseProb;
	destGuy->iProductLastBought =		iProductLastBought;
	destGuy->iBoughtThisTick =			iBoughtThisTick;
	destGuy->iRecommend =				iRecommend;
	destGuy->iPreferredChannel =		iPreferredChannel;
	destGuy->iPreferredProduct =		iPreferredProduct;		// consumers preferred brand
	destGuy->iShoppingChance =			iShoppingChance;
	destGuy->iDaysSinceLastShopping =	iDaysSinceLastShopping;
	destGuy->iInStoreSwitchFactor =		iInStoreSwitchFactor;
}





// ------------------------------------------------------------------------------
// 
// ------------------------------------------------------------------------------
double	Consumer::CountAllMessagesForThisProduct(Index pn)
{
	return productPersuasion[pn];
}


// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
int	Consumer::HasNegMsgs(Index pn)
{
	if(segment->iProdTree->IsLeaf(pn))
	{
		return productPersuasion[pn] < 0.0;	// OK
	}
	else
	{
		ProductTree::LeafNodeList leafs(pn);
		ProductTree::Iter iter;
		for(iter = leafs.begin(); iter != leafs.end(); ++iter)
		{
			pn = *iter;
			if(productPersuasion[pn] > 0.0)
			{
				return true;
			}
		}
		return false;
	}
}
// ------------------------------------------------------------------------------
// 
// ------------------------------------------------------------------------------
void	Consumer::CountMyAvailableProdMsgs()
{
	if (segment->iMessageSensitivity == 0)
		return;

	switch (choiceModel->iChoiceModel)
	{
	case kModel_EmergentLogit:
	case kModel_Bass:
	case kModel_CrossingTheChasm:
	case kModel_Emergent:
	case kModel_Linear:
		iTotalMessages = 0.0;
		return;
		break;
	case kModel_LinearSOV:
		break; 
	case kModel_General:	// OK
		{
			switch (choiceModel->iGCMf2_PersuasionValComp)
			{
			case kGCMf2_ShareOfVoice:
				break;
			case kGCMf2_Absolute:
			case kGCMf2_squareRoot:
			case kGCMf2_base10log:
				iTotalMessages = 0.0;
				return;
				break;
			} // end choiceModel->iGCMf2_PersuasionValComp switch
			break;
		}
	}	// end choiceModel->iChoiceModel switch

	
	int	i;
	double sum;
	for (i = 0; i < productPersuasion.size(); i++)
	{
		// It would seem that absolute number persuasion units makes no sense, 
		// so we'll go with net persuasion in both numerator and denominator
		sum = productPersuasion[i];	// OK

		if (sum < 0.0)
		{
			sum = 0.0;
		}
		iTotalMessages += sum;
	}
}



// ------------------------------------------------------------------------------
// Return true if the consumer has ever tried this product
// keeping this in a method so that I canchange it later.  Curent implementation
// is a test.  It only allows 16 products, because storage is statically allocated.
//
// 4/2/01 am replacing use of iTriedProduct with iProdsEverBought.  So, now I can 
// have more than 16 products.  Also, 8 bits will be stored.  For starters, we'll
// continue to keep a count of uses in the first two bits.
// ------------------------------------------------------------------------------
int	Consumer::NumTimesTriedProduct(Index pn)
{
	return productsBought[pn] & kProdTriesMask;
}

int	Consumer::EverTriedProduct(Index pn, int doSibs)
{
	int count;

	count = productsBought[pn] & kProdTriesMask;
	if (count > 0)
		return true;

	if(doSibs && (productsBought[pn] & kTriedSib))
	{
		return true;
	}

	return false;
}

int	Consumer::Aware(Index pn) 
{
	if(productAwareness[pn] == -1)
	{
		// not aware
		return false;
	}
	else if(productAwareness[pn] == segment->iNumDays)
	{
		// have just become aware - no time to lose awareness
		return true;
	}
	else
	{
		// maybe have lost awareness
		long span = segment->iNumDays - productAwareness[pn];
		double aware_prob = 1.00000 - AwareProb(pn);
		long double prob = pow(aware_prob,span);
		if(WillDo0to1(prob))
		{
			// since we did not lose awareness for this product
			// we have "checked" it for the siblings
			// otherwise the more siblings the more you lose
			ProductTree::List* sibs = ProductTree::theTree->Siblings(pn);
			if (segment->iShareAwarenessWithSibs && sibs)
			{
				for( ProductTree::Iter iter = sibs->begin(); iter != sibs->end(); ++iter)
				{
					int sibPn = *iter;
					if(productAwareness[sibPn] != -1)
					{
						productAwareness[sibPn] = segment->iNumDays;

						LosePersuasion(sibPn, span);
					}

					
				}
			}
			else
			{
				// just this one product
				productAwareness[pn] = segment->iNumDays;

				// lose perusasion regradless
				LosePersuasion(pn, span);
			}

		

			return true;
		}
		else
		{
			// turn off awareness of all siblings
			ProductTree::List* sibs = ProductTree::theTree->Siblings(pn);
			if (segment->iShareAwarenessWithSibs && sibs)
			{
				for( ProductTree::Iter iter = sibs->begin(); iter != sibs->end(); ++iter)
				{
					int sibPn = *iter;
					
					if(productAwareness[sibPn] != -1)
					{
						productAwareness[sibPn] = -1;

						for(da_iter iter = dynamic_attributes[sibPn].begin(); iter != dynamic_attributes[sibPn].end(); ++iter)
						{
							iter->second->Forget();
						}

						segment->iProducts[sibPn]->iNumAwareSofar -= 1;
						if (segment->iProducts[sibPn]->iNumAwareSofar < 0)
						{
							segment->iProducts[sibPn]->iNumAwareSofar = 0;
						}

						// lose awareness - lose persuasion
						segment->iProducts[sibPn]->iTotalPersuasion -= productPersuasion[sibPn];
						productPersuasion[sibPn] = 0.0;
					}
				}
				
			}
			else
			{
				productAwareness[pn] = -1;

				for(da_iter iter = dynamic_attributes[pn].begin(); iter != dynamic_attributes[pn].end(); ++iter)
				{
					iter->second->Forget();
				}

				segment->iProducts[pn]->iNumAwareSofar -= 1;
				if (segment->iProducts[pn]->iNumAwareSofar < 0)
				{
					segment->iProducts[pn]->iNumAwareSofar = 0;
				}

				segment->iProducts[pn]->iTotalPersuasion -= productPersuasion[pn];
				productPersuasion[pn] = 0.0;
			}

			return false;
		}
	} 
}

void Consumer::LosePersuasion(Index pn, int span)
{
	double persuasionVal = productPersuasion[pn];
	if (persuasionVal != 0)
	{
		double	locPersuasionDecayRate = PersuasionLoss(pn);
		double	persuasionLoss = persuasionVal;
		persuasionVal *= pow(1.0 - locPersuasionDecayRate, span);
		if(persuasionVal*persuasionVal < 0.0001)
		{
			persuasionVal = 0;
		}
		persuasionLoss -= persuasionVal;
		segment->iProducts[pn]->iTotalPersuasion -= persuasionLoss;	// KMK 8/22/05
		productPersuasion[pn] = persuasionVal;
	}
}

//-------------------------------------------------------------------------------
//
//-------------------------------------------------------------------------------
int Consumer::AlreadyHasCoupon(int CouponIndex)
{
	if(coupons->find(CouponIndex) != coupons->end())
	{
		return true;
	}
	return false;
}

double Consumer::DynamicScore(Pcn pcn, int post_use)
{
	double utility = 0.0;
	map<int, DynamicAttribute*>::const_iterator iter;
	for(iter = dynamic_attributes[pcn.pn].begin(); iter != dynamic_attributes[pcn.pn].end(); ++iter)
	{
		if(iter->second->Aware())
		{
			utility += segment->AttributeScore(iter->first, iter->second, post_use);
		}
	}
	return utility;
}

// ------------------------------------------------------------------------------
// Return true if the consumer has ever tried this product
// keeping this in a method so that I can change it later.  Curent implementation
// is a test.  It only allows 16 products, because storage is statically allocated.
//
// 4/2/01 am replacing use of iTriedProduct with iProdsEverBought.  So, now I can 
// have more than 16 products.  Also, 8 bits will be stored.  For starters, we'll
// continue to keep a count of uses in the first two bits.
// ------------------------------------------------------------------------------
int	Consumer::ConsumerTriedAndRejectedProduct(Index pn)
{
	Index lpn;
	unsigned int rejected;
	if(segment->iProdTree->IsLeaf(pn))
	{
		if(productsBought[pn] & kRejectedBit)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	else
	{
		ProductTree::LeafNodeList leafs(pn);
		ProductTree::Iter iter;
		for(iter = leafs.begin(); iter != leafs.end(); ++iter)
		{
			lpn = *iter;
			rejected = productsBought[lpn] & kRejectedBit;
			if(!rejected)
			{
				return false;
			}
		}
		return true;
	}
}

void Consumer::WriteToFile(ofstream & file)
{
	for(int i = 0; i < this->productsBought.size(); i++)
	{
		file << this->productsBought[i] << " ";
	}

	//panel data
	for(int i = 0; i < this->productTries.size(); i++)
	{
		file << (int)this->productTries[i] << " ";
	}

	//product persuasion
	for(int i = 0; i < this->productPersuasion.size(); i++)
	{
		file << (double)this->productPersuasion[i] << " ";
	}

	//product awareness
	for(int i = 0; i < this->productAwareness.size(); i++)
	{
		file << (int)this->productAwareness[i] << " ";
	}

	//dynamic attributes
	for(int i = 0; i < this->dynamic_attributes.size(); i++)
	{
		for(da_iter iter = dynamic_attributes[i].begin(); iter != dynamic_attributes[i].end(); ++iter)
		{
			file << (int)segment->iProducts[i]->iProductID << " ";
			file << (int)iter->first << " ";
			file << (int)iter->second->Aware() << " ";
			file << (double)iter->second->PreUse() << " ";
			file << (double)iter->second->PostUse() << " ";
		}
	}

	file << (int)(-1) << " ";

	file << (double)this->iRepurchaseProb << " ";
	file << (unsigned int)this->iBoughtThisTick << " ";
	file << (unsigned int)this->iRecommend << " ";
	file << (unsigned int)this->iPreferredChannel << " ";
	file << (unsigned int)this->iProductLastBought << " ";

	file << endl;
}

void Consumer::ReadFromFile(ifstream & file)
{
	for(int i = 0; i < this->productsBought.size(); i++)
	{
		file >> this->productsBought[i];
	}

	for(int i = 0; i < this->productTries.size(); i++)
	{
		file >> this->productTries[i];
		if(segment->iModelInfo->reset_panel_data_day >= 0)
		{
			this->productTries[i] = -1;
		}
	}

	//product persuasion
	for(int i = 0; i < this->productPersuasion.size(); i++)
	{
		file >> this->productPersuasion[i];
		double persuasion = this->productPersuasion[i];
		int numPersuasionUnitsToAdd = (int)persuasion;

		persuasion -= numPersuasionUnitsToAdd;

		if (persuasion > 0.0)
		{
			if (WillDo0to1(persuasion))
			{
				numPersuasionUnitsToAdd += 1;
			}
		}
		else if (persuasion < 0.0)
		{
			if (WillDo0to1(0.0 - persuasion))
			{
				numPersuasionUnitsToAdd -= 1;
			}
		}

		segment->iProducts[i]->iTotalPersuasion += numPersuasionUnitsToAdd;
		segment->iTotalMessages += abs(numPersuasionUnitsToAdd);

	}

	//product awareness
	for(int i = 0; i < this->productAwareness.size(); i++)
	{
		file >> this->productAwareness[i];

		if(this->productAwareness[i] != -1)
		{
			// Moving POPULATION->iPopScaleFactor to segment writer
			//PRODUCT[i].iNumAwareSofar += POPULATION->iPopScaleFactor;
			segment->iProducts[i]->iNumAwareSofar += 1;
		}
	}

	int prod_id;
	int attr_id;
	int pn;
	int attr_awareness;
	double attr_pre_use;
	double attr_post_use;
	file >> prod_id;
	while(prod_id != -1)
	{
		file >> attr_id;
		file >> attr_awareness;
		file >> attr_pre_use;
		file >> attr_post_use;
		pn = segment->ProdIndex(prod_id);
		dynamic_attributes[pn][attr_id] = new DynamicAttribute(prod_id, attr_id, attr_post_use, attr_pre_use, attr_awareness);
		file >> prod_id;
	}

	file >> this->iRepurchaseProb;
	file >> this->iBoughtThisTick;
	file >> this->iRecommend;
	file >> this->iPreferredChannel;
	file >> this->iProductLastBought;
}


