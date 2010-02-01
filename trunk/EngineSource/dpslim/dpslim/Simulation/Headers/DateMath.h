#pragma once
#define _CRT_SECURE_NO_WARNINGS

#include "stdafx.h"

using namespace std;

class MyTime : public CTime
{
public:
	MyTime(int year, int month, int day, int x, int y, int z, int flag = 0) : CTime(year, month, day, x, y, z, flag) {};
	MyTime(CTime a_time) : CTime(a_time) {};
	MyTime() : CTime() {};
	bool operator==(const MyTime &) const;
	bool operator==(const CTime &) const;
	bool operator<(const MyTime &) const;
	bool operator<(const CTime &) const;
	bool operator>(const MyTime &) const;
	bool operator>(const CTime &) const;
	bool operator<=(const MyTime &) const;
	bool operator<=(const CTime &) const;
	bool operator>=(const MyTime &) const;
	bool operator>=(const CTime &) const;
};