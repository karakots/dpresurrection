#pragma once
#define _CRT_SECURE_NO_WARNINGS

class Function
{
private:
	//function variables
public:
	Function();
	virtual double Value(double) = 0;
	virtual double Derivative(double) = 0;
	virtual double Integrate(double, double) = 0;
};

class ConstantFunction : public Function
{
private:
	double constant;
public:
	ConstantFunction(double);
	ConstantFunction();
	double Value(double);
	double Derivative(double);
	double Integrate(double, double);
};

class StepFunction : public Function
{
private:
	double left;
	double right;
	double step;
public:
	StepFunction(double, double, double);
	StepFunction();
	double Value(double);
	double Derivative(double);
	double Integrate(double, double);
};

class SumFunction : public Function
{
private:
	Function* func_one;
	Function* func_two;
public:
	SumFunction(Function*, Function*);
	SumFunction();
	double Value(double);
	double Derivative(double);
	double Integrate(double, double);
};