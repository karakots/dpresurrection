#include "InitialCondition.h"

InitialCondition::InitialCondition()
{
}

InitialCondition::InitialCondition(InitialCondsRecordset* record)
{
	segment_id = record->m_SegmentID;
	product_id = record->m_ProductID;

	awareness = record->m_BrandAwareness/100;
	persuasion = record->m_Persuasion;
	penetration = record->m_Penetration/100;
	share = record->m_InitialShare/100;
}

int InitialCondition::GetSegmentID()
{
	return segment_id;
}

int InitialCondition::GetProductID()
{
	return product_id;
}

double InitialCondition::GetAwareness()
{
	return awareness;
}

double InitialCondition::GetPenetration()
{
	return penetration;
}

double InitialCondition::GetPersuasion()
{
	return persuasion;
}

double InitialCondition::GetShare()
{
	return share;
}