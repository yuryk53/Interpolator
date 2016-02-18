using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Interpolator
{
    public interface InterpolationStrategy
    {
        /// The Interpolate func. interpolates a function by its tabulated point values (x,y)
        /// <returns>Function, that satisfies all interpolation conditions</returns>
        Func<double, double> Interpolate(List<Point> points);
    }
}
