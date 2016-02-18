using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Interpolator
{
    class Program
    {
        static void Main(string[] args)
        {
            //usage example ----------------
            List<Point> points = new List<Point>();
            points.Add(new Point(-4, 16));
            points.Add(new Point(-3, 9));
            points.Add(new Point(-2, 4));
            points.Add(new Point(0, 0));
            points.Add(new Point(2, 4));
            points.Add(new Point(3, 9));
            points.Add(new Point(4, 16));

            //points.Add(new Point(0, 0));
            //points.Add(new Point(1, 1));
            //points.Add(new Point(2, 0));
            //points.Add(new Point(3, 1));
            TabulatedValuesList list = new TabulatedValuesList(points);
            Func<double,double> f = list.Interpolate(new CubicSplineInterpolation());

            //Console.WriteLine("f(-3)={0}", f(-3)); //should be near 9
            Console.WriteLine("f(-4)={0}", f(4)); //should be near 16
            //.....and so on.....
            //---------end of example-------
            
        }
    }
}
