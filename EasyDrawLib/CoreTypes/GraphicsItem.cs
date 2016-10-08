using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace EasyDrawLib
{
    /// <summary>
    /// A generic sprite that can be drawn
    /// </summary>
    public abstract class GraphicsItem
    {
        /// <summary>
        /// The color to draw the sprite with
        /// </summary>
        public Color DrawColor { get; set; }
        /// <summary>
        /// The pen thickness to use when drawing
        /// </summary>
        public float PenThickness { get; set; }

        /// <summary>
        /// The sprite's visibility
        /// </summary>
        public bool Visible { get; set; }

        private Rectangle _hitBox;

        /// <summary>
        /// The hitbox for the sprite
        /// </summary>
        public Rectangle HitBox
        {
            get
            {
                _hitBox.X = Left;
                _hitBox.Y = Top;
                _hitBox.Width = Width;
                _hitBox.Height = Height;
                return _hitBox;
            }
            set
            {
                _hitBox = value;
            }
        }

        private Point _gridLocation;
        /// <summary>
        /// The location in grid coordinates
        /// </summary>
        public Point GridLocation
        {
            get
            {
                return _gridLocation;
            }
            set
            {
                _gridLocation = value;
            }
        }

        private Size _size;
        /// <summary>
        /// The size of the sprite
        /// </summary>
        public Size Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }
        /// <summary>
        /// The X position in grid coordinates
        /// </summary>
        public virtual int X
        {
            get
            {
                return _gridLocation.X;
            }
            set
            {
                _gridLocation.X = value;
            }
        }
        /// <summary>
        /// The Y position in grid coordinates
        /// </summary>
        public virtual int Y
        {
            get
            {
                return _gridLocation.Y;
            }
            set
            {
                _gridLocation.Y = value;
            }
        }
        /// <summary>
        /// The width of the sprite
        /// </summary>
        public virtual int Width
        {
            get { return _size.Width; }
            set
            {
                _size.Width = value;
               
            }
        }
        /// <summary>
        /// The height of the sprite
        /// </summary>
        public virtual int Height
        {
            get { return _size.Height; }
            set { _size.Height = value; }
        }
        /// <summary>
        /// Returns the right coordinate of the sprite accounting for center as origin.
        /// </summary>
        public virtual int Right
        {
            get { return Left + Width; }
        }
        /// <summary>
        /// Returns the left coordinate of the sprite accounting for center as origin.
        /// </summary>
        public virtual int Left
        {
            get
            {
                return _gridLocation.X - Origin.X;
            }
        }
        /// <summary>
        /// The top of the sprite
        /// </summary>
        public virtual int Top
        {
            get
            {
                return _gridLocation.Y - Origin.Y;
            }
        }
        /// <summary>
        /// The bottom of the sprite
        /// </summary>
        public virtual int Bottom
        {
            get
            {
                return Top + Height;
            }
        }

        private Point _origin;
        /// <summary>
        /// The origin of the sprite
        /// </summary>
        public Point Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }
        /// <summary>
        /// The name of the sprite
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="gridSize"></param>
        public abstract void Draw(int gridSize);
        /// <summary>
        /// A generic sprite that can be used to draw
        /// </summary>
        /// <param name="drawColor">The color to draw with</wparam>
        /// <param name="penThickness">The thickness of the pen to draw with</param>
        /// <param name="gridLocation">The location in grid coordinates</param>
        /// <param name="size">The size of the sprite</param>
        public GraphicsItem(Color drawColor, float penThickness, Point gridLocation, Size size)
        {
            DrawColor = drawColor;
            PenThickness = penThickness;
            GridLocation = gridLocation;
            Size = size;
            _origin = new Point(Size.Width / 2, Size.Height / 2);
            Visible = true;
        }
    }
}
