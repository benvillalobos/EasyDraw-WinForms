using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace EasyDrawLib
{
    /// <summary>
    /// A polygon that can be drawn
    /// </summary>
    public class Polygon : GraphicsItem
    {
        /// <summary>
        /// The function to draw with
        /// </summary>
        public Action<Pen, Point[]> DrawAction { get; set; }
        /// <summary>
        /// The function to fill with
        /// </summary>
        public Action<Brush, Point[]> FillAction { get; set; }
        /// <summary>
        /// A boolean determining whether to fill it in when drawing
        /// </summary>
        protected bool _fill;
        /// <summary>
        /// The points to connect when drawing
        /// </summary>
        protected Point[] _points;
        /// <summary>
        /// The points to connect when drawing
        /// </summary>
        public Point[] Points
        {
            get
            {
                return _points;
            }
        }
        /// <summary>
        /// The left of the polygon
        /// </summary>
        public override int Left
        {
            get
            {
                int furthestLeft = 0;
                for (int i = 0; i < _points.Length; i++)
                {
                    if (_points[furthestLeft].X > _points[i].X)
                    {
                        furthestLeft = i;
                    }
                }

                return _points[furthestLeft].X + this.X;
            }
        }
        /// <summary>
        /// The right of the polygon
        /// </summary>
        public override int Right
        {
            get
            {
                int furthestRight = 0;
                for (int i = 0; i < _points.Length; i++)
                {
                    if (_points[furthestRight].X < _points[i].X)
                    {
                        furthestRight = i;
                    }
                }

                return _points[furthestRight].X + this.X;
            }
        }
        /// <summary>
        /// The top of the polygon
        /// </summary>
        public override int Top
        {
            get
            {
                int furthestTop = 0;
                for (int i = 0; i < _points.Length; i++)
                {
                    if (_points[furthestTop].Y > _points[i].Y)
                    {
                        furthestTop = i;
                    }
                }
                return _points[furthestTop].Y + this.Y;
            }
        }
        /// <summary>
        /// The bottom of the polygon
        /// </summary>
        public override int Bottom
        {
            get
            {
                int furthestBottom = 0;
                for (int i = 0; i < _points.Length; i++)
                {
                    if (_points[furthestBottom].Y + this.Y < _points[i].Y + this.Y)
                    {
                        furthestBottom = i;
                    }
                }
                return _points[furthestBottom].Y + this.Y;
            }
        }
        /// <summary>
        /// The width of the polygon
        /// </summary>
        public override int Width
        {
            get
            {
                return Math.Abs(Right - Left);
            }
            set
            {
                base.Width = value;
            }
        }
        /// <summary>
        /// The height of the polygon
        /// </summary>
        public override int Height
        {
            get
            {
                return Math.Abs(Bottom - Top);
            }
            set
            {
                base.Height = value;
            }
        }
        /// <summary>
        /// The draw points relative to grid size
        /// </summary>
        protected Point[] _scaledDrawPoints;
        /// <summary>
        /// A polygon that can be drawn 
        /// </summary>
        /// <param name="DrawColor">The color to draw with</param>
        /// <param name="PenThickness">The pen thickness to use when drawing</param>
        /// <param name="fill">Whether to fill it in or not</param>
        /// <param name="points">The points to connect when drawing</param>
         public Polygon(Color DrawColor, float PenThickness, bool fill, params Point[] points)
            : base(DrawColor, PenThickness, Point.Empty, Size.Empty)
        {

            _points = points;
            _fill = fill;
            _scaledDrawPoints = new Point[_points.Length];

            int width = Right - Left;
            int height = Bottom - Top;
            base.Size = new Size(width, height);
        }
        
        /// <summary>
        /// Draws the polygon relative to gridsize
        /// </summary>
        /// <param name="gridSize">The gridsize to scale by</param>
         public override void Draw(int gridSize)
         {
             for(int i = 0; i < _points.Length; i++)
             {
                 _scaledDrawPoints[i] = new Point((_points[i].X + this.X) * gridSize, (_points[i].Y+this.Y) * gridSize);
             }
             if (_fill)
             {
                 FillAction(new SolidBrush(DrawColor), _scaledDrawPoints);
             }
             else
             {
                 DrawAction(new Pen(DrawColor, PenThickness), _scaledDrawPoints);
             }
         }
    }
}
