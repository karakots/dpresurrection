#include "stdafx.h"

class	Channel
{
public:
	CString		iChanName;
	int			iChannelIndex;
	int			iChannelID;
	double		iPctChanceChosen;
	double		iCumPctChanceChosen;

	Channel() {;}
	Channel(const Channel* x)
	{
		iChanName = x->iChanName;
		iChannelIndex = x->iChannelIndex;
		iChannelID = x->iChannelID;
		iPctChanceChosen = x->iPctChanceChosen;
		iCumPctChanceChosen = x->iCumPctChanceChosen;
	}

	void		InitializeChannel(void);
};
