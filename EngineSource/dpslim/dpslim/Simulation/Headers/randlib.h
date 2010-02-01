#pragma once
#define _CRT_SECURE_NO_WARNINGS
#define __RANDLIB__

#pragma once

#include <stdio.h>
#include <math.h>
#include <stdlib.h>
#include <vector>
using namespace std;

#define ABS(x) ((x) >= 0 ? (x) : -(x))
//#define min(a,b) ((a) <= (b) ? (a) : (b))
//#define max(a,b) ((a) >= (b) ? (a) : (b))


long GetIntegerFromStatFloat(double inValue);
long GetDivIntFromStatFloat(double inValue, double percent);
long LocalRand(long inVal);
bool WillDo0to1(double);
double LocalRandUniform();
long GetIntegerFromStatFloatWithRV(double inValue, double rv);


double	GetPosGaussDivNum(double mean, double stddev);
void   RandomInitialise(int,int);
double RandomUniform(void);
double RandomGaussian(double,double);
int    RandomInt(int,int);
double RandomDouble(double,double);
int		RandomPoisson(double rate);

/* Prototypes for all user accessible RANDLIB routines */

extern void advnst(long k);
extern double genbet(double aa,double bb);
extern double genchi(double df);
extern double genexp(double av);
extern double genf(double dfn, double dfd);
extern double gengam(double a,double r);
extern void genmn(double *parm,double *x,double *work);
extern void genmul(long n,double *p,long ncat,long *ix);
extern double gennch(double df,double xnonc);
extern double gennf(double dfn, double dfd, double xnonc);
extern double gennor(double av,double sd);
extern void genprm(long *iarray,int larray);
extern double genunf(double low,double high);
extern void getsd(long *iseed1,long *iseed2);
extern void gscgn(long getset,long *g);
extern long ignbin(long n,double pp);
extern long ignnbn(long n,double p);
extern long ignlgi(void);
extern long ignpoi(double mu);
extern long ignuin(long low,long high);
extern void initgn(long isdtyp);
extern long mltmod(long a,long s,long m);
extern void phrtsd(char* phrase,long* seed1,long* seed2);
extern double ranf(void);
extern void setall(long iseed1,long iseed2);
extern void setant(long qvalue);
extern void setgmn(double *meanv,double *covm,long p,double *parm);
extern void setsd(long iseed1,long iseed2);
extern double sexpo(void);
extern double sgamma(double a);
extern double snorm(void);

// usage
// Bucket bucket;
// bucket.Reset(1000);
// while(!bucket.Empty()) 
// {
//	int val = bucket.Draw();
// ...
// or you can use a static bucket
// as the bucket can be reused
// static Bucket bucket;
// bucket.Reset(1000);
// while(!bucket.Empty()) 
// {
//	int val = bucket.Draw();
// ...

class Bucket {
public:
	
	Bucket() : mySize(0) {}

	// reset the bucket to be full
	void Reset(int n);

	// draw -without replacement
	int Draw();
	
	// is the bucket empty?
	inline int Empty() { return numDraws >= mySize; }

private:
	vector<int>	drawList;
	int numDraws;
	int mySize;
};