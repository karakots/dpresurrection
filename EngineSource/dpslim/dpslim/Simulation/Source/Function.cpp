
#include "Function.h"

Function::Function()
{
}

ConstantFunction::ConstantFunction(double value) : Function()
{
	constant = value;
}

ConstantFunction::ConstantFunction() : Function()
{
	constant = 0;
}

double ConstantFunction::Derivative(double point)
{
	return 0;
}

double ConstantFunction::Value(double point)
{
	return constant;
}

double ConstantFunction::Integrate(double start, double end)
{
	return (end-start)*constant;
}

StepFunction::StepFunction(double left_value, double step_loc, double right_value) : Function()
{
	left = left_value;
	step = step_loc;
	right = right_value;
}

StepFunction::StepFunction() : Function()
{
	left = 1;
	step = 1;
	right = -1;
}

double StepFunction::Derivative(double point)
{
	return 0;
}

double StepFunction::Value(double point)
{
	if(point < step)
	{
		return left;
	}
	return right;
}

double StepFunction::Integrate(double start, double end)
{
	double value = 0;
	if(start < step)
	{
		if(end <= step)
		{
			value =  left*(end-start);
		}
		else
		{
			value = left*(step - start);
		}
	}
	if(end > step)
	{
		if(start < step)
		{
			value += right*(end-step);
		}
		else
		{
			value += right*(end - start);
		}
	}
	return value;
}

SumFunction::SumFunction(Function* one, Function* two)
{
	func_one = one;
	func_two = two;
}

SumFunction::SumFunction()
{
	func_one = 0;
	func_two = 0;
}

double SumFunction::Value(double point)
{
	return (func_one->Value(point) + func_two->Value(point));
}

double SumFunction::Integrate(double start, double end)
{
	return (func_one->Integrate(start,end) + func_two->Integrate(start,end));
}

double SumFunction::Derivative(double point)
{
	return (func_one->Derivative(point) + func_two->Derivative(point));
}