// DateMath.cpp
//
// Utility functions for doing some date math
// Created by Ken Karakotsios 8/2/97

#include "DateMath.h"

bool MyTime::operator ==(const MyTime &a_time) const
{
	if(a_time.GetDay() == GetDay() && a_time.GetMonth() == GetMonth() && a_time.GetYear() == GetYear())
	{
		return true;
	}

	return false;
}

bool MyTime::operator ==(const CTime &a_time) const
{
	if(a_time.GetDay() == GetDay() && a_time.GetMonth() == GetMonth() && a_time.GetYear() == GetYear())
	{
		return true;
	}

	return false;
}

bool MyTime::operator <(const MyTime &a_time) const
{
	if(GetYear() < a_time.GetYear())
	{
		return true;
	}
	if(GetYear() == a_time.GetYear() && GetMonth() < a_time.GetMonth())
	{
		return true;
	}
	if(GetYear() == a_time.GetYear() && GetMonth() == a_time.GetMonth() && GetDay() < a_time.GetDay())
	{
		return true;
	}

	return false;
}

bool MyTime::operator <(const CTime &a_time) const
{
	if(GetYear() < a_time.GetYear())
	{
		return true;
	}
	if(GetYear() == a_time.GetYear() && GetMonth() < a_time.GetMonth())
	{
		return true;
	}
	if(GetYear() == a_time.GetYear() && GetMonth() == a_time.GetMonth() && GetDay() < a_time.GetDay())
	{
		return true;
	}

	return false;
}

bool MyTime::operator >(const MyTime &a_time) const
{
	if(GetYear() > a_time.GetYear())
	{
		return true;
	}
	if(GetYear() == a_time.GetYear() && GetMonth() > a_time.GetMonth())
	{
		return true;
	}
	if(GetYear() == a_time.GetYear() && GetMonth() == a_time.GetMonth() && GetDay() > a_time.GetDay())
	{
		return true;
	}

	return false;
}

bool MyTime::operator >(const CTime &a_time) const
{
	if(GetYear() > a_time.GetYear())
	{
		return true;
	}
	if(GetYear() == a_time.GetYear() && GetMonth() > a_time.GetMonth())
	{
		return true;
	}
	if(GetYear() == a_time.GetYear() && GetMonth() == a_time.GetMonth() && GetDay() > a_time.GetDay())
	{
		return true;
	}

	return false;
}

bool MyTime::operator <=(const MyTime &a_time) const
{
	if(GetYear() <= a_time.GetYear())
	{
		return true;
	}
	if(GetYear() == a_time.GetYear() && GetMonth() <= a_time.GetMonth())
	{
		return true;
	}
	if(GetYear() == a_time.GetYear() && GetMonth() == a_time.GetMonth() && GetDay() <= a_time.GetDay())
	{
		return true;
	}

	return false;
}

bool MyTime::operator <=(const CTime &a_time) const
{
	if(GetYear() < a_time.GetYear())
	{
		return true;
	}
	if(GetYear() == a_time.GetYear() && GetMonth() < a_time.GetMonth())
	{
		return true;
	}
	if(GetYear() == a_time.GetYear() && GetMonth() == a_time.GetMonth() && GetDay() <= a_time.GetDay())
	{
		return true;
	}

	return false;
}

bool MyTime::operator >=(const MyTime &a_time) const
{
	if(GetYear() > a_time.GetYear())
	{
		return true;
	}
	if(GetYear() == a_time.GetYear() && GetMonth() > a_time.GetMonth())
	{
		return true;
	}
	if(GetYear() == a_time.GetYear() && GetMonth() == a_time.GetMonth() && GetDay() >= a_time.GetDay())
	{
		return true;
	}

	return false;
}

bool MyTime::operator >=(const CTime &a_time) const
{
	if(GetYear() >= a_time.GetYear())
	{
		return true;
	}
	if(GetYear() == a_time.GetYear() && GetMonth() >= a_time.GetMonth())
	{
		return true;
	}
	if(GetYear() == a_time.GetYear() && GetMonth() == a_time.GetMonth() && GetDay() >= a_time.GetDay())
	{
		return true;
	}

	return false;
}

