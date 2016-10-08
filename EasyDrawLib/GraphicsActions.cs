using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace EasyDrawLib
{
    /// <summary>
    /// The class that contains all actions for sprites being drawn
    /// </summary>
    public class GraphicsActions
    {
        private Graphics _gfx;

        /// <summary>
        /// The graphics object this class is drawing with
        /// </summary>
        public Graphics GFX
        {
            get
            {
                return _gfx;
            }
            set
            {
                _gfx = value;
            }
        }
        /// <summary>
        /// Fills an ellipse
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void FillEllipse(Brush brush, int x, int y, int width, int height)
        {
            _gfx.FillEllipse(brush, x, y, width, height);
        }
        /// <summary>
        /// Draws an ellipse
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void DrawEllipse(Pen pen, int x, int y, int width, int height)
        {
            _gfx.DrawEllipse(pen, x, y, width, height);
        }
        /// <summary>
        /// Fills a rectangle
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void FillRectangle(Brush brush, int x, int y, int width, int height)
        {
            _gfx.FillRectangle(brush, x, y, width, height);
        }
        /// <summary>
        /// Draws a rectangle
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void DrawRectangle(Pen pen, int x, int y, int width, int height)
        {
            _gfx.DrawRectangle(pen, x, y, width, height);
        }
        /// <summary>
        /// Draws a line
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public void DrawLine(Pen pen, Point p1, Point p2)
        {
            _gfx.DrawLine(pen, p1, p2);
        }
        /// <summary>
        /// Fills a polygon
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="points"></param>
        public void FillPolygon(Brush brush, Point[] points)
        {
            _gfx.FillPolygon(brush, points);
        }
        /// <summary>
        /// Draws a polygon
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="points"></param>
        public void DrawPolygon(Pen pen, Point[] points)
        {
            _gfx.DrawPolygon(pen, points);
        }

        /// <summary>
        /// Initializes a class that contains all draw functions
        /// </summary>
        /// <param name="gfx"></param>
        public GraphicsActions(Graphics gfx)
        {
            _gfx = gfx;

        }

    }
}
