#include "CMicroSegment.h"

#include "ProductTree.h"
#include "Display.h"
#include "Media.h"
#include "Coupon.h"
#include "MarketUtility.h"
#include "Price.h"
#include "Distribution.h"
#include "Attributes.h"
#include "ExternalFactor.h"
#include "Channel.h"
#include "Product.h"


void CMicroSegment::AddMassMedia(Media* mass_media)
{
	iMassMedia[(long)mass_media] = mass_media;
}

void CMicroSegment::RemoveMassMedia(Media* mass_media)
{
	iMassMedia.erase((long)mass_media);
}

void CMicroSegment::AddCouponDrop(Coupon* coupon)
{
	iCoupons[(long)coupon] = coupon;
}

void CMicroSegment::RemoveCouponDrop(Coupon* coupon)
{
	iCoupons.erase((long)coupon);
}

void CMicroSegment::AddMarketUtility(MarketUtility* market_utility)
{
	Pcn pcn;
	ProductTree::LeafNodeList leafs(ProdIndex(market_utility->GetProduct()));
	ProductTree::Iter iter;
	for(iter = leafs.begin(); iter != leafs.end(); ++iter)
	{
		if(market_utility->GetChannel() == -1)
		{
			for(Cn_iter cn_iter = iChannels.begin(); cn_iter != iChannels.end(); ++cn_iter)
			{
				pcn = Pcn(*iter, ChanIndex((*cn_iter)->iChannelID));
				iProductsAvailable[pcn]->iMarketUtility[(long)market_utility] = market_utility;
			}
		}
		else
		{
			pcn = Pcn(*iter, ChanIndex(market_utility->GetChannel()));
			iProductsAvailable[pcn]->iMarketUtility[(long)market_utility] = market_utility;
		}
	}
}

void CMicroSegment::RemoveMarketUtility(MarketUtility* market_utility)
{
	Pcn pcn;
	ProductTree::LeafNodeList leafs(ProdIndex(market_utility->GetProduct()));
	ProductTree::Iter iter;
	for(iter = leafs.begin(); iter != leafs.end(); ++iter)
	{
		if(market_utility->GetChannel() == -1)
		{
			for(Cn_iter cn_iter = iChannels.begin(); cn_iter != iChannels.end(); ++cn_iter)
			{
				pcn = Pcn(*iter, ChanIndex((*cn_iter)->iChannelID));
				iProductsAvailable[pcn]->iMarketUtility.erase((long)market_utility);
			}
		}
		else
		{
			pcn = Pcn(*iter, ChanIndex(market_utility->GetChannel()));
			iProductsAvailable[pcn]->iMarketUtility.erase((long)market_utility);
		}
	}
}

void CMicroSegment::AddExternalFactor(ExternalFactor* external_factor)
{
	iExternalFactor[(long)external_factor] = external_factor;
}

void CMicroSegment::RemoveExternalFactor(ExternalFactor* external_factor)
{
	iExternalFactor.erase((long)external_factor);
}

void CMicroSegment::UpdateConsumerPreference(ConsumerPref* consumer_pref)
{
	iAttributePreferences[consumer_pref->GetAttribute()] = consumer_pref;
	iNeedToCalcAttrScores = true;
}

void CMicroSegment::UpdateProductAttribute(ProductAttribute* product_attribute)
{
	int type = iAttributes[product_attribute->GetAttribute()]->GetType();
	if(type == 0)
	{
		(*iProductAttributes[product_attribute->GetProduct()])[product_attribute->GetAttribute()] = product_attribute;
	}
	else if(type == 1000)
	{
		AddDynamicAttribute(product_attribute);
	}
}

void CMicroSegment::UpdateDistribution(Distribution* distribution)
{
	Pcn pcn;
	if(distribution->GetChannel() == -1)
	{
		for(Cn_iter cn_iter = iChannels.begin(); cn_iter != iChannels.end(); ++cn_iter)
		{
			pcn = Pcn(ProdIndex(distribution->GetProduct()), ChanIndex((*cn_iter)->iChannelID));
			iProductsAvailable[pcn]->iPreUseDistributionPercent = distribution->GetPreUseDistribution(Current_Time) / 100;
			iProductsAvailable[pcn]->iPostUseDistributionPercent = distribution->GetPostUseDistribution(Current_Time) / 100;
			iProductsAvailable[pcn]->iDistributionAwareness = distribution->GetAwareness();
			iProductsAvailable[pcn]->iDistributionPersuasion = distribution->GetPersuasion();
		}
	}
	else
	{
		pcn = Pcn(ProdIndex(distribution->GetProduct()), ChanIndex(distribution->GetChannel()));
		iProductsAvailable[pcn]->iPreUseDistributionPercent = distribution->GetPreUseDistribution(Current_Time) / 100;
		iProductsAvailable[pcn]->iPostUseDistributionPercent = distribution->GetPostUseDistribution(Current_Time) / 100;
		iProductsAvailable[pcn]->iDistributionAwareness = distribution->GetAwareness();
		iProductsAvailable[pcn]->iDistributionPersuasion = distribution->GetPersuasion();
	}
}

void CMicroSegment::UpdatePrice(Price* price)
{
	Pcn pcn;
	if(price->GetChannel() == -1)
	{
		for(Cn_iter cn_iter = iChannels.begin(); cn_iter != iChannels.end(); ++cn_iter)
		{
			pcn = Pcn(ProdIndex(price->GetProduct()), ChanIndex((*cn_iter)->iChannelID));
			iProductsAvailable[pcn]->iPrice[price->GetType()] = price->GetPrice(Current_Time);
			iProductsAvailable[pcn]->iProbPrice[price->GetType()] = price->GetDistribution(Current_Time);
		}
	}
	else
	{
		pcn = Pcn(ProdIndex(price->GetProduct()), ChanIndex(price->GetChannel()));
		iProductsAvailable[pcn]->iPrice[price->GetType()] = price->GetPrice(Current_Time);
		iProductsAvailable[pcn]->iProbPrice[price->GetType()] = price->GetDistribution(Current_Time);
	}

	iNeedToCalcPriceScores = true;
}