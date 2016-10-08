using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace EasyDrawLib
{
    internal class BasicDrawItem : GraphicsItem
    {
        public Action<Pen, int, int, int, int> DrawAction { get; set; }

        public BasicDrawItem(Color drawColor, float penThickness, Point gridLocation, Size size, Action<Pen, int, int, int, int> drawAction)
            : base(drawColor, penThickness, gridLocation, size)
        {
            DrawAction = drawAction;
        }

        public override void Draw(int gridSize)
        {
            DrawAction(new Pen(DrawColor, PenThickness), (GridLocation.X - Origin.X) * gridSize, (GridLocation.Y - Origin.Y) * gridSize, Size.Width * gridSize, Size.Height * gridSize);
        }
    }
}
