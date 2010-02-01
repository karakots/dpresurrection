#include "Channel.h"

// ------------------------------------------------------------------------------
//
// ------------------------------------------------------------------------------
void	Channel::InitializeChannel(void)
{
	iChanName.Empty();
	iChannelID = -1;
	iPctChanceChosen = 0.0;
	iCumPctChanceChosen = 0.0;
}