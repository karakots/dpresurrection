// This is the main DLL file.

#include "stdafx.h"

#include "Math Utility.h"

using namespace MathUtility;

double Solver::AttributeSqrNorm(List<double>^ W, List< List< List<double>^ >^ >^ A)
{
	double sqrNorm = 0.0;

	for (int n = 0; n < A->Count; ++n)
	{
		for (int k = 0; k < A[n]->Count; ++k)
		{
			for (int i = 0; i < A[n][k]->Count; ++i)
			{
				sqrNorm += A[n][k][i] * A[n][k][i] * W[n];
			}
		}
	}

	return sqrNorm;
}
List<double>^ Solver::SolveWithSaturation(Saturation^ fcn, List< List<double>^ >^ S, List< List<double>^ >^ R, List<double>^ W,  List<List<double>^>^ totalVal, List< List< List<double>^ >^ >^ A)
{
	List<double>^ F;
	List< List<double>^ >^ J;
	List<double>^ Z;
	List<double>^ DP = gcnew List<double>();
	List< List<double>^ >^ temp_share;
	double g0 = 0.0;
	double g1 = 0.0;
	double g2 = 0.0;
	double g3 = 0.0;
	double a0 = 0.0;
	double a1 = 0.0;
	double a2 = 0.0;
	double a3 = 0.0;
	double h1 = 0.0;
	double h2 = 0.0;
	double h3 = 0.0;
	double Z0 = 0.0;


	if (A->Count == 0 || A[0]->Count == 0)
	{
		return DP;
	}
		

	for (int i = 0; i < A[0][0]->Count; ++i)
	{
		DP->Add(0.0);
	}

	double attrSqrNorm = AttributeSqrNorm(W, A);

	int sat_max_iter = max_iter/10;
	double sat_tol = 0.01;

	for (int i = 0; i < max_iter; ++i)
	{
		F = Evaluate(S, R, W, A);

		g1 = Sum_of_Squares(F);


		if (g1 < sat_tol * sat_tol * attrSqrNorm) {
			return DP;
		}

		J = JacobianWithSaturation(fcn, S, R, W, totalVal, A);
		Z = Gradient(F, J);
		Z0 = Math::Sqrt(Sum_of_Squares(Z));

		// Z0 is not our step but it we do not want to divide by zero
		if (Z0 < 1.e-24 )
		{
			return DP;
		}

		Z = Vector_Multiply(Z, 1 / Z0);
		a1 = 0;
		a3 = 1;
		temp_share = UpdateShareWithSaturation(fcn, S, totalVal, A, Vector_Multiply(Z, -a3));
		g3 = Sum_of_Squares(Evaluate(temp_share, R, W, A));
		while (g3 >= g1)
		{
			a3 = a3 / 2;
			if (a3 < sat_tol)
			{
				return DP;
			}
			temp_share = UpdateShareWithSaturation(fcn, S, totalVal, A, Vector_Multiply(Z, -a3));
			g3 = Sum_of_Squares(Evaluate(temp_share, R, W, A));
		}

		a2 = a3 / 2;
		temp_share = UpdateShareWithSaturation(fcn, S, totalVal, A, Vector_Multiply(Z, -a2));
		g2 = Sum_of_Squares(Evaluate(temp_share, R, W, A));

		// ready to calcualte a0 and g0
		// test if bendy up or bendy down
		if ( g3 + g1 > 2*g2)
		{
			a0 = (a3/4) * (g3 - 4*g2 + 3*g1) / (g3 + g1 - 2*g2);

			if (a0 > 2)
			{
				a0 = 2;
			}

			temp_share = UpdateShareWithSaturation(fcn, S, totalVal, A, Vector_Multiply(Z, -a0));
			g0 = Sum_of_Squares(Evaluate(temp_share, R, W, A));

			if (g3 < g0)
			{
				a0 = a3;
				g0 = g3;
			}
		}
		else
		{
			a0 = a3;
			g0 = g3;
		}

		if ( g2 < g0 )
		{
			a0 = a2;
			g0 = g2;
		}

		if (Math::Abs(g0 - g1) < sat_tol * sat_tol * attrSqrNorm)
		{
			return Vector_Add(DP, Vector_Multiply(Z, -a0));
		}

		// we have our update
		List<double>^ delta = Vector_Multiply(Z, -a0);

		S = UpdateShareWithSaturation(fcn, S, totalVal, A, delta);


		// update total values
		for (int n = 0; n < S->Count; n++)
		{
			for (int k = 0; k < S[n]->Count; k++)
			{
				double total = 0.0;
				for(int i = 0; i < DP->Count; i++)
				{
					total += A[n][k][i] * delta[i];
				}

				totalVal[n][k] += total;
			}
		}

		DP = Vector_Add(DP, delta);
	}
	return DP;
}

List<double>^ Solver::Solve(List< List<double>^ >^ S, List< List<double>^ >^ R, List<double>^ W, List< List< List<double>^ >^ >^ A)
{
	List<double>^ F;
	List< List<double>^ >^ J;
	List<double>^ Z;
	List<double>^ DP = gcnew List<double>();
	List< List<double>^ >^ temp_share;
	double g0 = 0.0;
	double g1 = 0.0;
	double g2 = 0.0;
	double g3 = 0.0;
	double a0 = 0.0;
	double a1 = 0.0;
	double a2 = 0.0;
	double a3 = 0.0;
	double h1 = 0.0;
	double h2 = 0.0;
	double h3 = 0.0;
	double Z0 = 0.0;

	if (A->Count == 0 || A[0]->Count == 0)
	{
		return DP;
	}

	for (int i = 0; i < A[0][0]->Count; ++i)
	{
		DP->Add(0.0);
	}

	double attrSqrNorm = AttributeSqrNorm(W, A);

	for (int i = 0; i < max_iter; ++i)
	{
		F = Evaluate(S, R, W, A);

		
		g1 = Sum_of_Squares(F);

		if (g1 < tol * tol * attrSqrNorm) {
			return DP;
		}

		J = Jacobian(S, R, W, A);
		Z = Gradient(F, J);
		Z0 = Math::Sqrt(Sum_of_Squares(Z));
		if (Z0 < 1.e-24 )
		{
			return DP;
		}
		g1 = Sum_of_Squares(F);
		Z = Vector_Multiply(Z, 1 / Z0);
		a1 = 0;
		a3 = 1;
		temp_share = UpdateShare(S, A, Vector_Multiply(Z, -a3));
		g3 = Sum_of_Squares(Evaluate(temp_share, R, W, A));
		while (g3 >= g1)
		{
			a3 = a3 / 2;
			if (a3 < tol)
			{
				return DP;
			}
			temp_share = UpdateShare(S, A, Vector_Multiply(Z, -a3));
			g3 = Sum_of_Squares(Evaluate(temp_share, R, W, A));
		}
		a2 = a3 / 2;
		temp_share = UpdateShare(S, A, Vector_Multiply(Z, -a2));
		g2 = Sum_of_Squares(Evaluate(temp_share, R, W, A));
		
		// ready to calcualte a0 and g0
		// test if bendy up or bendy down
		if ( g3 + g1 > 2*g2)
		{
			a0 = (a3/4) * (g3 - 4*g2 + 3*g1) / (g3 + g1 - 2*g2);

			if (a0 > 2)
			{
				a0 = 2;
			}

			temp_share = UpdateShare(S, A, Vector_Multiply(Z, -a0));
			g0 = Sum_of_Squares(Evaluate(temp_share, R, W, A));

			if (g3 < g0)
			{
				a0 = a3;
				g0 = g3;
			}
		}
		else
		{
			a0 = a3;
			g0 = g3;
		}

		if ( g2 < g0 )
		{
			a0 = a2;
			g0 = g2;
		}

		if (Math::Abs(g0 - g1) < tol * tol * attrSqrNorm)
		{
			return Vector_Add(DP, Vector_Multiply(Z, -a0));
		}
		S = UpdateShare(S, A, Vector_Multiply(Z, -a0));
		DP = Vector_Add(DP, Vector_Multiply(Z, -a0));
	}
	return DP;
}

List<double>^ Solver::Solve_Sequential(List< List<double>^ >^ S, List< List<double>^ >^ R, List<double>^ W, List< List< List<double>^ >^ >^ A)
{

	List<double>^ DP = gcnew List<double>();
	double F = 0.0;
	double J = 0.0;
	double d_pref = 0.0;
	double g0 = 0.0;
	double g1 = 0.0;
	double g2 = 0.0;
	double g3 = 0.0;
	double a0 = 0.0;
	double a1 = 0.0;
	double a2 = 0.0;
	double a3 = 0.0;
	double h1 = 0.0;
	double h2 = 0.0;
	double h3 = 0.0;
	double Z0 = 0.0;

	for (int i = 0; i < A[0][0]->Count; ++i)
	{
		DP->Add(0.0);
	}

	double key = 0.0;
	SortedDictionary<double, int>^ preference_order = gcnew SortedDictionary<double, int>();
	for (int i = 0; i < A[0][0]->Count; i++)
	{
		F = Evaluate(S, R, W, A, i, d_pref);
		J = Derivative(S, R, W, A, i, d_pref);
		Z0 = 2 * F * J;
		key = Z0;
		while(preference_order->ContainsKey(key))
		{
			key -= 0.00000001;

		}
		preference_order->Add(key , i);
	}

	//Start by looping over attributes
	for each(int i in preference_order->Values)
	{
		d_pref = 0.0;
		//Gradient Decent loop for attribute i
		for (int iter = 0; iter < max_iter; iter++)
		{
			F = Evaluate(S, R, W, A, i, d_pref);
			J = Derivative(S, R, W, A, i, d_pref);
			Z0 = 2 * F * J;
			if (Math::Abs(Z0) < tol)
			{
				break;
			}
			Z0 = Z0 / Math::Abs(Z0);
			g1 = F * F;
			a1 = 0;
			a3 = 1;
			g3 = Evaluate(S, R, W, A, i, d_pref - a3*Z0);
			g3 = g3 * g3;
			while (g3 >= g1)
			{
				a3 = a3 / 2;
				if (a3 < tol)
				{
					break;
				}
				g3 = Evaluate(S, R, W, A, i, d_pref - a3 * Z0);
				g3 = g3 * g3;
			}
			a2 = a3 / 2;
			g2 = Evaluate(S, R, W, A, i, d_pref - a2 * Z0);
			g2 = g2 * g2;
			h1 = (g2 - g1) / a2;
			h2 = (g3 - g2) / (a3 - a2);
			h3 = (h2 - h1) / a3;

			//TODO: For some reason the a0 calc seems to be off in this solve...
			a0 = 0.5 * (a2 - h1) / h3;
			if (a0 >= a3 + 1)
			{
				a0 = a3 + 1;
			}
			if (a0 < 0)
			{
				a0 = 0;
			}

			g0 = Evaluate(S, R, W, A, i, d_pref - a0 * Z0);
			g0 = g0 * g0;
			if (g3 < g0)
			{
				a0 = a3;
				g0 = g3;
			}
			if (g2 < g0)
			{
				a0 = a2;
				g0 = g2;
			}
			d_pref = d_pref - a0 * Z0;

			if (Math::Abs(g0 - g1) < tol*tol)
			{
				break;
			}
		}

		//QAD catch for bad d_pref values...
		if (Math::Abs(d_pref) < 10)
		{
			S = UpdateShare(S, A, i, d_pref);
			DP[i] = d_pref;
		}
		else
		{
			DP[i] = 0;
		}
	}
	return DP;
}





List< List< double >^ >^ Solver::UpdateShareWithSaturation(Saturation^ fcn, List< List< double >^ >^ S, List< List<double>^ >^ totalVal, List< List< List< double >^ >^ >^ A, List< double >^ DP)
{
	List< List<double>^ >^ new_share = gcnew List< List<double>^ >();
	double exponent = 0.0;
	double total = 0.0;
	double share = 0.0;
	for (int n = 0; n < S->Count; n++)
	{
		total = 0.0;
		new_share->Add(gcnew List<double>());
		for (int k = 0; k < S[n]->Count; k++)
		{
			exponent = totalVal[n][k];
			for(int i = 0; i < DP->Count; i++)
			{
				exponent += A[n][k][i] * DP[i];
			}
			share = S[n][k] * Math::Exp(fcn(exponent,0) - fcn(totalVal[n][k],0));
			new_share[n]->Add(share);
			total += share;
		}

		if (total > 0.0)
		{
			for (int k = 0; k < S[n]->Count; k++)
			{
				new_share[n][k] = new_share[n][k] / total;
			}
		}
	}

	return new_share;
}

List< List< double >^ >^ Solver::UpdateShare(List< List< double >^ >^ S, List< List< List< double >^ >^ >^ A, List< double >^ DP)
{
	List< List<double>^ >^ new_share = gcnew List< List<double>^ >();
	double exponent = 0.0;
	double total = 0.0;
	double share = 0.0;
	for (int n = 0; n < S->Count; n++)
	{
		total = 0.0;
		new_share->Add(gcnew List<double>());
		for (int k = 0; k < S[n]->Count; k++)
		{
			exponent = 0.0;
			for(int i = 0; i < DP->Count; i++)
			{
				exponent += A[n][k][i] * DP[i];
			}
			share = S[n][k] * Math::Exp(exponent);
			new_share[n]->Add(share);
			total += share;
		}

		if (total > 0.0)
		{
			for (int k = 0; k < S[n]->Count; k++)
			{
				new_share[n][k] = new_share[n][k] / total;
			}
		}
	}

	return new_share;
}


List< List<double>^ >^ Solver::UpdateShare(List< List<double>^ >^ S, List< List< List<double>^ >^ >^ A, int i, double d_pref)
{
	List< List<double>^ >^ new_share = gcnew List< List<double>^ >();
	double total = 0.0;
	double share = 0.0;
	for (int n = 0; n < S->Count; n++)
	{
		total = 0.0;
		new_share->Add(gcnew List<double>());
		for (int k = 0; k < S[n]->Count; k++)
		{
			share = S[n][k] * Math::Exp(A[n][k][i] * d_pref);
			new_share[n]->Add(share);
			total += share;
		}

		if (total > 0.0)
		{
			for (int k = 0; k < S[n]->Count; k++)
			{
				new_share[n][k] = new_share[n][k] / total;
			}
		}
	}

	return new_share;
}


double Solver::Evaluate(List< List<double>^ >^ S, List< List<double>^ >^ R, List<double>^ W, List< List< List<double>^ >^ >^ A, int i, double d_pref)
{
	double value = 0.0;
	double normalizer = 0.0;
	double exponent = 0.0;
	for (int n = 0; n < S->Count; ++n)
	{

		normalizer = 0.0;
		for (int k = 0; k < S[0]->Count; ++k)
		{
			exponent = A[n][k][i] * d_pref;
			exponent = Math::Exp(exponent);
			if (Double::IsInfinity(exponent))
			{
				return Double::MaxValue;
			}
			normalizer += S[n][k] * exponent;
		}
		if (normalizer <= 0.0 || Double::IsNaN(normalizer))
		{
			continue;
		}
		normalizer = Math::Log(normalizer);

		for (int k = 0; k < S[0]->Count; ++k)
		{
			exponent = A[n][k][i] * d_pref;
			exponent -= normalizer;
			exponent = Math::Exp(exponent);
			value += A[n][k][i] * W[n] * (exponent * S[n][k] - R[n][k]);
		}
	}
	return value;
}

List<double>^ Solver::Evaluate(List< List<double>^ >^ S, List< List<double>^ >^ R, List<double>^ W, List< List< List<double>^ >^ >^ A)
{
	List<double>^ vec_value = gcnew List<double>();
	double value = 0.0;

	for (int i = 0; i < A[0][0]->Count; ++i)
	{
		value = 0.0;
		for (int n = 0; n < S->Count; ++n)
		{
			for (int k = 0; k < S[0]->Count; ++k)
			{
				value += A[n][k][i] * W[n] * (S[n][k] - R[n][k]);
				//double val0 = A[n][k][i];
				//double val1 =  W[n];
				//double val2 = S[n][k];
				//double val3 = R[n][k];

				//double val = val0 * val1 * (val2 - val3);

				//value += val;
			}
		}
		vec_value->Add(value);
	}
	return vec_value;
}


double Solver::Derivative( List< List<double>^ >^ S, List< List<double>^ >^ R, List<double>^ W, List< List< List<double>^ >^ >^ A, int i, double pref)
{
	double value = 0.0;
	double exponent = 0.0;
	double normalizer = 0.0;
	double d_normalizer = 0.0;

	for (int n = 0; n < S->Count; ++n)
	{
		normalizer = 0.0;
		d_normalizer = 0.0;
		for (int k = 0; k < S[0]->Count; ++k)
		{
			exponent = A[n][k][i] * pref;
			exponent = Math::Exp(exponent);
			if (Double::IsInfinity(exponent))
			{
				return Double::MaxValue;
			}
			normalizer += S[n][k] * exponent;
			d_normalizer += A[n][k][i] * S[n][k] * exponent;
		}
		if (normalizer <= 0.0 || Double::IsNaN(normalizer))
		{
			continue;
		}

		d_normalizer = d_normalizer / normalizer;
		normalizer = Math::Log(normalizer);

		for (int k = 0; k < S[0]->Count; ++k)
		{
			exponent = A[n][k][i] * pref;
			exponent -= normalizer;
			exponent = Math::Exp(exponent);
			value += A[n][k][i] * W[n] * exponent * (A[n][k][i] - d_normalizer) * S[n][k];
		}
	}
	return value;
}

List< List<double>^ >^ Solver::Jacobian(List< List<double>^ >^ S, List< List<double>^ >^ R, List<double>^ W, List< List< List<double>^ >^ >^ A)
{
	List< List<double>^ >^ jacobian = gcnew List< List<double>^ >();
	
	double value = 0.0;

	List< List<double>^ >^ meanAttr = gcnew List<List<double>^>();

	for (int n = 0; n < S->Count; ++n)
	{
		meanAttr->Add(gcnew List<double>());

		for (int i = 0; i < A[0][0]->Count; ++i)
		{
			value = 0.0;

			for (int k = 0; k < S[0]->Count; ++k)
			{
				value += A[n][k][i] * S[n][k];
			}

			meanAttr[n]->Add(value);
		}
	}

	for (int i = 0; i < A[0][0]->Count; ++i)
	{
		jacobian->Add(gcnew List<double>());
		for (int j = 0; j < A[0][0]->Count; ++j)
		{
			value = 0.0;
			for (int n = 0; n < S->Count; ++n)
			{
				for (int k = 0; k < S[0]->Count; ++k)
				{
					value += A[n][k][i] * W[n] * A[n][k][j] * S[n][k];
				}

				value -= W[n] * meanAttr[n][i] * meanAttr[n][j];
			}

			jacobian[i]->Add(value);
		}
	}
	return jacobian;
}


List< List<double>^ >^ Solver::JacobianWithSaturation(Saturation^ fcn,
													  List< List<double>^ >^ S, List< List<double>^ >^ R, List<double>^ W, 
													  List< List<double>^ >^ totalVal, List< List< List<double>^ >^ >^ A)
{
	

	List< List<double>^ >^ jacobian = gcnew List< List<double>^ >();
	double value = 0.0;

	double satMean = 0.0;

	List< List<double>^ >^ meanAttr = gcnew List<List<double>^>();
	List< List<double>^ >^ satMeanAttr = gcnew List<List<double>^>();

	for (int n = 0; n < S->Count; ++n)
	{
		meanAttr->Add(gcnew List<double>());
		satMeanAttr->Add(gcnew List<double>());

		for (int i = 0; i < A[0][0]->Count; ++i)
		{
			value = 0.0;
			satMean = 0.0;

			for (int k = 0; k < S[0]->Count; ++k)
			{
				value += A[n][k][i] * S[n][k];
				satMean += fcn( totalVal[n][k], 1) * A[n][k][i] * S[n][k];
			}

			meanAttr[n]->Add(value);
			satMeanAttr[n]->Add(satMean);
		}
	}

	for (int i = 0; i < A[0][0]->Count; ++i)
	{
		jacobian->Add(gcnew List<double>());
		for (int j = 0; j < A[0][0]->Count; ++j)
		{
			value = 0.0;
			for (int n = 0; n < S->Count; ++n)
			{
				for (int k = 0; k < S[0]->Count; ++k)
				{
					value += fcn( totalVal[n][k], 1) * A[n][k][i] * W[n] * A[n][k][j] * S[n][k];
				}

				value -= W[n] * meanAttr[n][j] * satMeanAttr[n][i];
			}

			jacobian[i]->Add(value);
		}
	}

	return jacobian;
}


List<double>^ Solver::Gradient(List<double>^ F, List< List<double>^ >^ J)
{
	List<double>^ G = gcnew List<double>();
	double value = 0.0;
	for (int i = 0; i < J->Count; ++i)
	{
		value = 0.0;
		for (int j = 0; j < F->Count; ++j)
		{
			value += J[i][j] * F[j];
		}
		G->Add(value);
	}
	return G;
}

double Solver::Sum_of_Squares(List<double>^ F)
{
	double value = 0.0;
	for (int i = 0; i < F->Count; ++i)
	{
		value += F[i] * F[i];
	}
	return value;
}

List<double>^ Solver::Vector_Multiply(List<double>^ V, double a)
{
	List<double>^ ret_val = gcnew List<double>();
	for (int i = 0; i < V->Count; ++i)
	{
		ret_val->Add(V[i] * a);
	}
	return ret_val;
}

List<double>^ Solver::Vector_Add(List<double>^ V1, List<double>^ V2)
{
	List<double>^ ret_val = gcnew List<double>();
	for (int i = 0; i < V1->Count; ++i)
	{
		ret_val->Add(V1[i] + V2[i]);
	}
	return ret_val;
}


