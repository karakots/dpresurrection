// Math Utility.h

#pragma once

#using <mscorlib.dll>

using namespace System;
using namespace System::Collections::Generic;

namespace MathUtility {

	public ref class Solver
	{
	public: 

	
		delegate double Saturation(double persuasion, int numDerives);

		static double tol = 0.00001;
		static int max_iter = 100;

		static List<double>^ Solve(List< List<double>^ >^ S, List< List<double>^ >^ R, List<double>^ W, List< List< List<double>^ >^ >^ A);

		static List<double>^ Solve_Sequential(List< List<double>^ >^ S, List< List<double>^ >^ R, List<double>^ W, List< List< List<double>^ >^ >^ A);

		static List<double>^ SolveWithSaturation(Saturation^ fcn, List<List<double>^ >^ S, List< List<double>^ >^ R, List<double>^ W, List<List<double>^>^ totalVal, List< List< List<double>^ >^ >^ A);

	private:

		static List< List<double>^ >^ UpdateShare(List< List<double>^ >^ S, List< List< List<double>^ >^ >^ A, int i, double d_pref);

		static List< List<double>^ >^ UpdateShare(List< List<double>^ >^ S, List< List< List<double>^ >^ >^ A, List< double >^ DP);

		static double Evaluate(List< List<double>^ >^ S, List< List<double>^ >^ R, List<double>^ W, List< List< List<double>^ >^ >^ A, int i, double d_pref);

		static double Derivative( List< List<double>^ >^ S, List< List<double>^ >^ R, List<double>^ W, List< List< List<double>^ >^ >^ A, int i, double pref);

		static List<double>^ Evaluate(List< List<double>^ >^ S, List< List<double>^ >^ R, List<double>^ W, List< List< List<double>^ >^ >^ A);

		static List< List<double>^ >^ Jacobian(List< List<double>^ >^ S, List< List<double>^ >^ R, List<double>^ W, List< List< List<double>^ >^ >^ A);

		static double Sum_of_Squares(List<double>^ F);

		static List<double>^ Vector_Multiply(List<double>^ V, double a);

		static List<double>^ Vector_Add(List<double>^ V1, List<double>^ V2);

		static List<double>^ Gradient(List<double>^ F, List< List<double>^ >^ J);

		static double AttributeSqrNorm(List<double>^ W, List< List< List<double>^ >^ >^ A);

		static List< List<double>^ >^ JacobianWithSaturation(Saturation^ fcn, List< List<double>^ >^ S, List< List<double>^ >^ R, List<double>^ W, 
													  List< List<double>^ >^ totalVal, List< List< List<double>^ >^ >^ A);

		static List< List< double >^ >^ UpdateShareWithSaturation(Saturation^ fcn, List< List< double >^ >^ S, List< List<double>^ >^ totalVal, List< List< List< double >^ >^ >^ A, List< double >^ DP);
	};
}
