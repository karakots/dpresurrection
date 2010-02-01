using System;
using System.Collections.Generic;
using System.Text;

namespace Mapack
{
    public class Newton
    {

        /// <summary>
        /// applies a Newton-Raphston iteration step
        /// solves diff + sigma * step = 0
        /// step = -sigma.inverse * diff
        /// sigma must be symetric
        /// </summary>
        public void Solve(double[] diff, double[][] sigma, double tol, double stepSize, out double[] step)
        {
            Matrix Sigma = new Matrix(sigma);
            Matrix Diff = new Matrix(diff.Length, 1);
            step = new double[diff.Length];

            for (int ii = 0; ii < diff.Length; ++ii)
            {
                Diff[ii, 0] = -diff[ii];
            }

            if (!Sigma.Square ||
                Sigma.Rows != Diff.Rows)
            {
                throw new ArgumentException("Matrix dimensions are not valid.");
            }

             // Matrix I = A * A.Inverse;

            // we compute the eigen value decomposition of sigma
            EigenvalueDecomposition Eigen = new EigenvalueDecomposition(Sigma);


            // this is the difference vector in eigen coordinates
            Matrix eDiff = Eigen.EigenvectorMatrix * Diff;

            double[] eVals = Eigen.RealEigenvalues;

            for(int ii = 0; ii < eVals.Length; ++ii)
            {
                if( eVals[ ii ] > tol)
                {
                // do not step to infinity and beyond
                //if (Math.Abs(eDiff[ii, 0]) < Math.Abs(stepSize * eVals[ii]))
                //{
                    eDiff[ii, 0] /= eVals[ii]; 
                }
                else
                {
                    // we are not going to move along this line
                    eDiff[ii, 0] = 0;
                }
            }
           
            Matrix Step = Eigen.EigenvectorMatrix.Transpose() * eDiff;

            double max = 0.0;
            for (int ii = 0; ii < step.Length; ++ii)
            {
                step[ii] = Step[ii, 0];

                if (Math.Abs(step[ii]) > max)
                {
                    max = Math.Abs(step[ii]);
                }
            }

            if( max > stepSize ) {
                for( int ii = 0; ii < step.Length; ++ii ) {
                    step[ ii ] /= (max / stepSize);
                }
            }
        }

        /// <summary>
        /// computes gradient of square of dff
        /// step = - diff*sigma
        /// </summary>
        /// <param name="diff"></param>
        /// <param name="sigma"></param>
        /// <param name="tol"></param>
        /// <param name="stepSize"></param>
        /// <param name="step"></param>
        public void Gradient( double[] diff, double[][] sigma,  out double[] step ) {
            Matrix Sigma = new Matrix( sigma );
            Matrix Diff = new Matrix( diff.Length, 1 );

            step = new double[ diff.Length ];

            for( int ii = 0; ii < diff.Length; ++ii ) {
                Diff[ ii, 0 ] = -diff[ ii ];
            }

            Matrix Step = Sigma * Diff;

            for( int ii = 0; ii < step.Length; ++ii ) {
                step[ ii ] = Step[ ii, 0 ];
            }
        }
    }
}
