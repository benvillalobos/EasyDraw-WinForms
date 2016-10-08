using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace EasyDrawLib
{
    /// <summary>
    /// A triangle that can be drawn
    /// </summary>
    public class Triangle : Polygon
    {
        /// <summary>
        /// The first point to connect
        /// </summary>
        public Point Point1
        {
            get
            {
                return _points[0];
            }
            set
            {
                _points[0] = value;
            }
        }
        /// <summary>
        /// The second point to connect
        /// </summary>
        public Point Point2
        {
            get
            {
                return _points[1];
            }
            set
            {
                _points[0] = value;
            }
        }
        /// <summary>
        /// The third point to connect
        /// </summary>
        public Point Point3
        {
            get
            {
                return _points[2];
            }
            set
            {
                _points[0] = value;
            }
        }
        /// <summary>
        /// A triangle that can be drawn
        /// </summary>
        /// <param name="DrawColor">The color to draw with</param>
        /// <param name="PenThickness">The pen thickness to use when drawing</param>
        /// <param name="fill">Whether to fill it in or not</param>
        /// <param name="points">The points to connect, only taking into account the first 3</param>
         public Triangle(Color DrawColor, float PenThickness, bool fill, Point[] points)
            : base(DrawColor, PenThickness, fill, points)
        {
            
        }
    }
}
