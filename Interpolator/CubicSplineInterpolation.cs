using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using SysLinearEqSolver;

namespace Interpolator
{
    public class CubicSplineInterpolation : InterpolationStrategy
    {
        public Func<double, double> Interpolate(List<System.Windows.Point> points)
        {
            //sort the input list in ascending order with respect to X-values
            points.Sort((p1, p2) => p1.X.CompareTo(p2.X));

            // n - number of points minus 1
            int n = points.Count-1;
            double[] x = (from point in points select point.X).ToArray();
            double[] y = (from point in points select point.Y).ToArray();
            double[] h = new double[n];
            for (int i = 0; i < n; i++)
            {
                h[i] = x[i + 1] - x[i];
            }

            double[,] coefficients = new double[n - 1, n - 1];
            double[] r = new double[n - 1]; //right-hand side of equations

            //fill the tridiagonal matrix with coefficients and the right-hand side vector
            for (int i = 0; i < n - 1; i++)
            {
                coefficients[i, i] = 2 * (h[i] + h[i + 1]); //diagonal
                if (i + 1 < n - 1)
                {
                    coefficients[i + 1, i] = h[i + 1];          //lower diagonal
                    coefficients[i, i + 1] = h[i + 1];          //upper diagonal
                }
                r[i] = 6.0 / h[i+1] * (y[i + 1 +1] - y[i+1]) - 6.0 / h[i - 1 + 1] * (y[i+1] - y[i - 1 +1]); //the right-hand side
            }

            //solve the system using Jordan-Gauss method
            LinearEquationsSystem system = new LinearEquationsSystem(coefficients, r);
            system.Solver = new GaussJordanSolver();
            system.Solve();
            double [] __y = new double[system.Solver.Results.Length+2]; //y with two primes (y'')
            Array.Copy(system.Solver.Results, 0, __y, 1, system.Solver.Results.Length); //__y[0] and __y[last] are empty yet 
            __y[0] = __y[__y.Length - 1] = 0;

            //find the unknown cubic polynomial coefficients
            double[] a = new double[n];
            double[] b = new double[n];
            double[] c = new double[n];
            double[] d = new double[n];

            for (int i = 0; i < n; i++)
            {
                a[i] = 1.0 / (6.0 * h[i]) * (__y[i + 1] - __y[i]);
                b[i] = 0.5 * __y[i];
                c[i] = 1.0 / h[i] * (y[i + 1] - y[i]) - h[i] / 6.0 * (__y[i + 1] + 2 * __y[i]);
                d[i] = y[i];
            }

            Func<double,double>[] s = new Func<double,double>[n]; //resulting cubic polynomials
            for (int i = 0; i < n; i++)
            {
                double ai = a[i], bi = b[i], ci = c[i], di = d[i], xi = x[i];
                s[i] = xarg =>
                {
                    double z = xarg-xi;
                    return ai * Math.Pow(z, 3) + bi * z * z + ci * z + di;
                };
            }

            //form the resulting spline function
            Func<double,double> spline = delegate(double xarg){
                //find polynomial, responsible for that xarg x-value using binary search
                int from = 0;
                int to = x.Length-1;
                int p=0;
                while (to - from > 1)
                {
                    p = (to + from) / 2;
                    if (xarg < x[p])
                        to = p;
                    else from = p;
                }
                //now p - is an index of needed cubic polynomial in array 's'
                p = (to + from) / 2;
                return s[p](xarg);
            };

            return spline;
        }
    }
}
