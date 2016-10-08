using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace EasyDrawLib
{
    /// <summary>
    /// A line that can be drawn
    /// </summary>
    public class Line : GraphicsItem
    {
        private Point _point2;
        /// <summary>
        /// The second point to connect
        /// </summary>
        public Point Point2
        {
            get { return _point2; }
            set { _point2 = value; }
        }
        /// <summary>
        /// The left side of the line
        /// </summary>
        public override int Left
        {
            get
            {
                return Point1.X < Point2.X ? Point1.X : Point2.X;
            }
        }
        /// <summary>
        /// The top of the line
        /// </summary>
        public override int Top
        {
            get
            {
                return Point1.Y < Point2.Y ? Point1.Y : Point2.Y;
            }
        }
        /// <summary>
        /// The width of the line
        /// </summary>
        public override int Width
        {
            get
            {
                return Math.Abs(base.Width);
            }
            set
            {
                base.Width = value;
            }
        }
        /// <summary>
        /// The height of the line
        /// </summary>
        public override int Height
        {
            get
            {
                return Math.Abs(base.Height);
            }
            set
            {
                base.Height = value;
            }
        }


        
        /// <summary>
        /// The first point to connect
        /// </summary>
        public Point Point1
        {
            get
            {
                return GridLocation;
            }
            set
            {
                GridLocation = value;
            }
        }
        /// <summary>
        /// The x coordinate of the first point of this line.
        /// </summary>
        public override int X
        {
            get
            {
                return base.X;
            }
            set
            {
                base.X = value;
                _point2.X = base.X + Size.Width;
            }
        }

        /// <summary>
        /// The Y coordinate of the first point of the line.
        /// </summary>
        public override int Y
        {
            get
            {
                return base.Y;
            }
            set
            {
                base.Y = value;
                _point2.Y = base.Y + Size.Height;
            }
        }

        /// <summary>
        /// The function to be used to draw with
        /// </summary>
        public Action<Pen, Point, Point> DrawAction { get; set; }
        /// <summary>
        /// The line to be drawn
        /// </summary>
        /// <param name="DrawColor">The color to draw with</param>
        /// <param name="PenThickness">The thickness of pen to use when drawing</param>
        /// <param name="point1">The first point to connect</param>
        /// <param name="point2">The second point to connect</param>
        /// <param name="drawAction">The function to use for drawing this</param>
        public Line(Color DrawColor, float PenThickness, Point point1, Point point2, Action<Pen, Point, Point> drawAction)
            : base(DrawColor, PenThickness, point1, new Size((point2.X - point1.X), (point2.Y - point1.Y)))//Subtracting point2/2 to offset the center as origin
        {
            // Pass-through constructor for drawing lines.
            _point2 = point2;
            DrawAction = drawAction;
        }
        /// <summary>
        /// Draws the line relative to the current gridsize
        /// </summary>
        /// <param name="gridSize"></param>
        public override void Draw(int gridSize)
        {
            DrawAction(new Pen(DrawColor, PenThickness), new Point(GridLocation.X * gridSize, GridLocation.Y * gridSize), new Point(Point2.X * gridSize, Point2.Y * gridSize));
        }
    }
}
