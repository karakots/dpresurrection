using System;
using System.Collections.Generic;
using System.Text;

namespace Mapack
{
    public class Solver
    {
        public static double tol = 0.0001;
        public static int max_iter = 1000;
        public static List<double> Solve_Sequential(List<List<double>> S, List<List<double>> R, List<double> W, List<List<List<double>>> A)
        {
            
            List<double> DP = new List<double>();
            double F = 0.0;
            double J = 0.0;
            double d_pref = 0.0;
            double g0 = 0.0;
            double g1 = 0.0;
            double g2 = 0.0;
            double g3 = 0.0;
            double a0 = 0.0;
            // double a1 = 0.0;
            double a2 = 0.0;
            double a3 = 0.0;
            double h1 = 0.0;
            double h2 = 0.0;
            double h3 = 0.0;
            double Z0 = 0.0;

            for (int i = 0; i < A[0][0].Count; ++i)
            {
                DP.Add(0.0);
            }
          
            double key = 0.0;
            SortedDictionary<double, int> preference_order = new SortedDictionary<double, int>();
            for (int i = 0; i < A[0][0].Count; i++)
            {
                F = Evaluate(S, R, W, A, i, d_pref);
                J = Derivative(S, R, W, A, i, d_pref);
                Z0 = 2 * F * J;
                key = Z0;
                while(preference_order.ContainsKey(key))
                {
                    key -= 0.00000001;

                }
                preference_order.Add(key , i);
            }

            //Start by looping over attributes
            foreach (int i in preference_order.Values)
            {
                d_pref = 0.0;
                //Gradient Decent loop for attribute i
                for (int iter = 0; iter < max_iter; iter++)
                {
                    F = Evaluate(S, R, W, A, i, d_pref);
                    J = Derivative(S, R, W, A, i, d_pref);
                    Z0 = 2 * F * J;
                    if (Math.Abs(Z0) < tol)
                    {
                        break;
                    }
                    Z0 = Z0 / Math.Abs(Z0);
                    g1 = F * F;
                    // a1 = 0;
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

                    if (Math.Abs(g0 - g1) < tol*tol)
                    {
                        break;
                    }
                }

                //QAD catch for bad d_pref values...
                if (Math.Abs(d_pref) < 10)
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

        public static List<List<double>> UpdateShare(List<List<double>> S, List<List<List<double>>> A, int i, double d_pref)
        {
            List<List<double>> new_share = new List<List<double>>();
            double total = 0.0;
            double share = 0.0;
            for (int n = 0; n < S.Count; n++)
            {
                total = 0.0;
                new_share.Add(new List<double>());
                for (int k = 0; k < S[n].Count; k++)
                {
                    share = S[n][k] * Math.Exp(A[n][k][i] * d_pref);
                    new_share[n].Add(share);
                    total += share;
                }

                if (total > 0.0)
                {
                    for (int k = 0; k < S[n].Count; k++)
                    {
                        new_share[n][k] = new_share[n][k] / total;
                    }
                }
            }

            return new_share;
        }

        public static List<double> Solve(List<List<double>> S, List<List<double>> R, List<double> W, List<List<List<double>>> A)
        {
            List<double> F;
            List<List<double>> J;
            List<double> Z;
            List<double> DP = new List<double>();
            double g0 = 0.0;
            double g1 = 0.0;
            double g2 = 0.0;
            double g3 = 0.0;
            double a0 = 0.0;
            // double a1 = 0.0;
            double a2 = 0.0;
            double a3 = 0.0;
            double h1 = 0.0;
            double h2 = 0.0;
            double h3 = 0.0;
            double Z0 = 0.0;

            for (int i = 0; i < A[0][0].Count; ++i)
            {
                DP.Add(0.0);
            }

            for (int i = 0; i < max_iter; ++i)
            {
                F = Evaluate(S, R, W, A, DP);
                J = Jacobian(S, R, W, A, DP);
                Z = Gradient(F, J);
                Z0 = Math.Sqrt(Sum_of_Squares(Z));
                if (Z0 < tol)
                {
                    return DP;
                }
                g1 = Sum_of_Squares(F);
                Z = Vector_Multiply(Z, 1 / Z0);
                // a1 = 0;
                a3 = 1;
                g3 = Sum_of_Squares(Evaluate(S, R, W, A, Vector_Add(DP, Vector_Multiply(Z, -a3))));
                while (g3 >= g1)
                {
                    a3 = a3 / 2;
                    if (a3 < tol)
                    {
                        return DP;
                    }
                    g3 = Sum_of_Squares(Evaluate(S, R, W, A, Vector_Add(DP, Vector_Multiply(Z, -a3))));
                }
                a2 = a3 / 2;
                g2 = Sum_of_Squares(Evaluate(S, R, W, A, Vector_Add(DP, Vector_Multiply(Z, -a2))));
                h1 = (g2 - g1) / a2;
                h2 = (g3 - g2) / (a3 - a2);
                h3 = (h2 - h1) / a3;
                a0 = 0.5 * (a2 - h1) / h3;
                g0 = Sum_of_Squares(Evaluate(S, R, W, A, Vector_Add(DP, Vector_Multiply(Z, -a0))));
                if (g3 < g0)
                {
                    a0 = a3;
                    g0 = g3;
                }
                DP = Vector_Add(DP, Vector_Multiply(Z, -a0));
                if (Math.Abs(g0 - g1) < tol)
                {
                    return DP;
                }
            }
            return DP;
        }

        static double Evaluate(List<List<double>> S, List<List<double>> R, List<double> W, List<List<List<double>>> A, int i, double d_pref)
        {
            double value = 0.0;
            double normalizer = 0.0;
            double exponent = 0.0;
            for (int n = 0; n < S.Count; ++n)
            {

                normalizer = 0.0;
                for (int k = 0; k < S[0].Count; ++k)
                {
                    exponent = A[n][k][i] * d_pref;
                    exponent = Math.Exp(exponent);
                    if (Double.IsInfinity(exponent))
                    {
                        return Double.MaxValue;
                    }
                    normalizer += S[n][k] * exponent;
                }
                if (normalizer <= 0.0 || Double.IsNaN(normalizer))
                {
                    continue;
                }
                normalizer = Math.Log(normalizer);

                for (int k = 0; k < S[0].Count; ++k)
                {
                    exponent = A[n][k][i] * d_pref;
                    exponent -= normalizer;
                    exponent = Math.Exp(exponent);
                    value += A[n][k][i] * W[n] * (exponent * S[n][k] - R[n][k]);
                }
            }
            return value;
        }

        static double Derivative(List<List<double>> S, List<List<double>> R, List<double> W, List<List<List<double>>> A, int i, double pref)
        {
            double value = 0.0;
            double exponent = 0.0;
            double normalizer = 0.0;
            double d_normalizer = 0.0;

            for (int n = 0; n < S.Count; ++n)
            {
                normalizer = 0.0;
                d_normalizer = 0.0;
                for (int k = 0; k < S[0].Count; ++k)
                {
                    exponent = A[n][k][i] * pref;
                    exponent = Math.Exp(exponent);
                    if (Double.IsInfinity(exponent))
                    {
                        return Double.MaxValue;
                    }
                    normalizer += S[n][k] * exponent;
                    d_normalizer += A[n][k][i] * S[n][k] * exponent;
                }
                if (normalizer <= 0.0 || Double.IsNaN(normalizer))
                {
                    continue;
                }

                d_normalizer = d_normalizer / normalizer;
                normalizer = Math.Log(normalizer);

                for (int k = 0; k < S[0].Count; ++k)
                {
                    exponent = A[n][k][i] * pref;
                    exponent -= normalizer;
                    exponent = Math.Exp(exponent);
                    value += A[n][k][i] * W[n] * exponent * (A[n][k][i] - d_normalizer) * S[n][k];
                }
            }
            return value;
        }

        static List<double> Evaluate(List<List<double>> S, List<List<double>> R, List<double> W, List<List<List<double>>> A, List<double> DP)
        {
            List<double> vec_value = new List<double>();
            double value = 0.0;
            double exponent = 0.0;
            double normalizer = 0.0;

            for (int i = 0; i < DP.Count; ++i)
            {
                value = 0.0;
                for (int n = 0; n < S.Count; ++n)
                {
                    normalizer = 0.0;
                    for (int k = 0; k < S[0].Count; ++k)
                    {
                        exponent = 0.0;
                        for (int l = 0; l < A[0][0].Count; ++l)
                        {
                            exponent += A[n][k][l] * DP[l];
                        }
                        exponent = Math.Exp(exponent);
                        normalizer += S[n][k] * exponent;
                    }
                    if (normalizer <= 0.0)
                    {
                        continue;
                    }
                    normalizer = Math.Log(normalizer);

                    for (int k = 0; k < S[0].Count; ++k)
                    {
                        exponent = 0.0;
                        for (int l = 0; l < A[0][0].Count; ++l)
                        {
                            exponent += A[n][k][l] * DP[l];
                        }
                        exponent -= normalizer;
                        exponent = Math.Exp(exponent);
                        value += A[n][k][i] * W[n] * (exponent * S[n][k] - R[n][k]);
                    }
                }
                vec_value.Add(value);
            }
            return vec_value;
        }

        static List<List<double>> Jacobian(List<List<double>> S, List<List<double>> R, List<double> W, List<List<List<double>>> A, List<double> DP)
        {
            List<List<double>> jacobian = new List<List<double>>();
            double value = 0.0;
            double exponent = 0.0;
            double normalizer = 0.0;
            double d_normalizer = 0.0;

            for (int i = 0; i < DP.Count; ++i)
            {
                jacobian.Add(new List<double>());
                for (int j = 0; j < DP.Count; ++j)
                {
                    value = 0.0;
                    for (int n = 0; n < S.Count; ++n)
                    {
                        normalizer = 0.0;
                        d_normalizer = 0.0;
                        for (int k = 0; k < S[0].Count; ++k)
                        {
                            exponent = 0.0;
                            for (int l = 0; l < A[0][0].Count; ++l)
                            {
                                exponent += A[n][k][l] * DP[l];
                            }
                            exponent = Math.Exp(exponent);
                            normalizer += S[n][k] * exponent;
                            d_normalizer += A[n][k][j] * S[n][k] * exponent;
                        }
                        if (normalizer <= 0.0)
                        {
                            continue;
                        }
                        d_normalizer = d_normalizer / normalizer;
                        normalizer = Math.Log(normalizer);

                        for (int k = 0; k < S[0].Count; ++k)
                        {
                            exponent = 0.0;
                            for (int l = 0; l < A[0][0].Count; ++l)
                            {
                                exponent += A[n][k][l] * DP[l];
                            }
                            exponent -= normalizer;
                            exponent = Math.Exp(exponent);
                            value += A[n][k][i] * W[n] * exponent * (A[n][k][j] - d_normalizer) * S[n][k];
                        }
                    }
                    jacobian[i].Add(value);
                }
            }
            return jacobian;
        }

        static double Sum_of_Squares(List<double> F)
        {
            double value = 0.0;
            for (int i = 0; i < F.Count; ++i)
            {
                value += F[i] * F[i];
            }
            return value;
        }

        static List<double> Vector_Multiply(List<double> V, double a)
        {
            List<double> ret_val = new List<double>();
            for (int i = 0; i < V.Count; ++i)
            {
                ret_val.Add(V[i] * a);
            }
            return ret_val;
        }

        static List<double> Vector_Add(List<double> V1, List<double> V2)
        {
            List<double> ret_val = new List<double>();
            for (int i = 0; i < V1.Count; ++i)
            {
                ret_val.Add(V1[i] + V2[i]);
            }
            return ret_val;
        }

        static List<double> Gradient(List<double> F, List<List<double>> J)
        {
            List<double> G = new List<double>();
            double value = 0.0;
            for (int i = 0; i < J.Count; ++i)
            {
                value = 0.0;
                for (int j = 0; j < F.Count; ++j)
                {
                    value += J[i][j] * F[j];
                }
                G.Add(value);
            }
            return G;
        }
    }
}