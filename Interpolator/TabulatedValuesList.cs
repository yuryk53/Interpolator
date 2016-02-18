using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Interpolator
{
    [Serializable]
    public class TabulatedValuesList
    {
        private List<Point> _points = new List<Point>();

        Point this[int index]
        {
            get
            {
                return _points[index];
            }
            set
            {
                _points[index] = value;
            }
        }

        public TabulatedValuesList(List<Point> points)
        {
            this._points = points;
        }

        public TabulatedValuesList() { }

        public Func<double,double> Interpolate(InterpolationStrategy interpStrategy)
        {
            return interpStrategy.Interpolate(this._points);
        }

        public void Add(Point p)
        {
            this._points.Add(p);
        }
    }
}
