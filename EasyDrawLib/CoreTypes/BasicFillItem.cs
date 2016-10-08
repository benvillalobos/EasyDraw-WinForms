using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace EasyDrawLib
{
    internal class BasicFillItem : GraphicsItem
    {
        public Action<Brush, int, int, int, int> FillAction { get; set; }

        public BasicFillItem(Color drawColor, Point gridLocation, Size size, Action<Brush, int, int, int, int> fillAction)
            : base(drawColor, 0f, gridLocation, size)
        {
            FillAction = fillAction;
        }

        public override void Draw(int gridSize)
        {
            FillAction(new SolidBrush(DrawColor), (GridLocation.X - Origin.X) * gridSize, (GridLocation.Y - Origin.Y) * gridSize, Size.Width * gridSize, Size.Height * gridSize);
        }

    }
}
