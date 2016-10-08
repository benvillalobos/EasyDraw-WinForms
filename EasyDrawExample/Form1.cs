using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EasyDrawLib;

namespace EasyDrawExample
{
    public partial class Form1 : EasyDrawForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        int speedx = 1;
        int speedy = 1;

        int leftScore = 0;
        int rightScore = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            StartUpdating = true;
        }

        private void Form1_ReadyToDraw(object sender, EventArgs e)
        {
            UpdateTime = 10;
            DrawCircle(ClientSizeGrid.Width / 2, ClientSizeGrid.Height / 2, 5, Color.Red, "Circle1", true);

            DrawRectangle(ClientSizeGrid.Width - 2, 10, 3, 10, "RightPaddle", true);
            DrawRectangle(1, 10, 3, 10, "LeftPaddle", true);

            DrawLine(new Point(20, 20), new Point(10, 10), "Bob");

            DrawPolygon(Color.Purple, "Billy", true, new Point(5, 10), new Point(5, 12), new Point(1, 10), new Point(5, 15));
        }

        int bobSpeedX = 1;
        int bobSpeedY = 1;

        int billySpeedX = 1;
        int billySpeedY = -1;

        private void Form1_OnUpdate(object sender, EventArgs e)
        {
            if (IsKeyDown(Keys.Up) && !IsTouchingEdge(GetSprite("LeftPaddle"), ScreenBorders.Top))
            {
                GetSprite("LeftPaddle").Y--;
            }
            else if(IsKeyDown(Keys.Down) && !IsTouchingEdge(GetSprite("LeftPaddle"), ScreenBorders.Bottom))
            {
                GetSprite("LeftPaddle").Y++;
            }

            GetSprite("Circle1").X += speedx;
            GetSprite("Circle1").Y += speedy;

            GetSprite("Billy").X+=billySpeedX;
            GetSprite("Billy").Y+=billySpeedY;

            GetSprite("Billy").Width++;

            GetSprite("Bob").X += bobSpeedX;
            GetSprite("Bob").Y += bobSpeedY;

            if (IsTouchingEdge(GetSprite("Billy"), ScreenBorders.Left, ScreenBorders.Right))
            {
                billySpeedX *= -1;
            }
            if (IsTouchingEdge(GetSprite("Billy"), ScreenBorders.Top, ScreenBorders.Bottom))
            {
                billySpeedY *= -1;
            }

            if (IsTouchingEdge(GetSprite("Bob"), ScreenBorders.Left, ScreenBorders.Right))
            {
                bobSpeedX *= -1;
            }
            if (IsTouchingEdge(GetSprite("Bob"), ScreenBorders.Top, ScreenBorders.Bottom))
            {
                bobSpeedY *= -1;
            }
           

            if (GetSprite("Circle1").HitBox.IntersectsWith(GetSprite("LeftPaddle").HitBox) || GetSprite("Circle1").HitBox.IntersectsWith(GetSprite("RightPaddle").HitBox))
            {
                speedx *= -1;
            }
            if (IsTouchingEdge(GetSprite("Circle1"), ScreenBorders.Top, ScreenBorders.Bottom))
            {
                speedy *= -1;
            }
            if (IsTouchingEdge(GetSprite("Circle1"), ScreenBorders.Left, ScreenBorders.Right))
            {
                if (GetSprite("Circle1").X < ClientSizeGrid.Width / 2)
                {
                    rightScore++;
                }
                else
                {
                    leftScore++;
                }
                GetSprite("Circle1").X = ClientSizeGrid.Width / 2;
                GetSprite("Circle1").Y = ClientSizeGrid.Height / 2;
                speedx *= -1;
                scoreLabel.Text = string.Format("{0}   |   {1}", leftScore, rightScore);

                if (leftScore >= 5 || rightScore >= 5)
                {
                    StartUpdating = false;
                    somebodyWonLabel.Visible = true;
                }

            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void DrawBox_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void DrawBox_MouseMove(object sender, MouseEventArgs e)
        {
            GetSprite("RightPaddle").Y = ConvertToGridCoordinates(e.Y);
        }
    }
}
