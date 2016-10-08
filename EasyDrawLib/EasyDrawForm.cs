using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EasyDrawLib;

namespace EasyDrawLib
{
    public partial class EasyDrawForm : Form
    {

        public bool UpdateCoordinates { get; set; }
        /// <summary>
        /// The main canvas everything but the grid is drawn on
        /// </summary>
        protected Bitmap _canvas;
        /// <summary>
        /// The main graphics object that draws onto the canvas
        /// </summary>
        protected GraphicsActions _mainGfx;

        private Bitmap _grid;
        private Graphics _gridGfx;

        private Bitmap _formImage;

        /// <summary>
        /// Determines whether to show the grid or not
        /// </summary>
        protected bool _showGrid;
        /// <summary>
        /// The size of each grid square
        /// </summary>
        protected int _gridSize = 10;

        private bool _isRefreshNeeded;
        /// <summary>
        /// A boolean determining whether to redraw the main canvas
        /// </summary>
        protected bool _isCanvasUpdateNeeded;
        /// <summary>
        /// Gets current mouse position in grid coordinates
        /// </summary>
        public Point GridPosition { get; set; }

        /// <summary>
        /// An event that runs when the form is ready to draw
        /// </summary>
        public event EventHandler ReadyToDraw;

        /// <summary>
        /// An event that runs when the form updates itself
        /// </summary>
        public event EventHandler OnUpdate;

        /// <summary>
        /// The default color to draw everything with
        /// </summary>
        public Color DrawColor { get; set; }
        /// <summary>
        /// The pen thickness to use when drawing
        /// </summary>
        public float PenThickness { get; set; }

        /// <summary>
        /// Set the size of the grid. Default is 10
        /// </summary>
        [Browsable(true), DefaultValue(10)]
        public int GridSize
        {
            get { return _gridSize; }

            set
            {
                if (value > 0 && value != _gridSize)
                {
                    _gridSize = value;
                    prepareGrid();

                    updateCanvas(true);
                }
            }
        }

        /// <summary>
        /// Show or hide the drawing grid for this EasyDrawForm
        /// </summary>
        [Browsable(true)]
        public bool ShowGrid
        {
            get { return _showGrid; }

            set
            {
                _showGrid = value;
                _isRefreshNeeded = true;

                this.Invalidate(true);
            }
        }
        /// <summary>
        /// How often the form updates
        /// </summary>
        public int UpdateTime
        {
            get
            {
                return updateTimer.Interval;
            }
            set
            {
                updateTimer.Interval = value;
            }
        }
        /// <summary>
        /// A boolean determining whether to start the update timer.
        /// </summary>
        public bool StartUpdating
        {
            get
            {
                return updateTimer.Enabled;
            }
            set
            {
                updateTimer.Enabled = value;
            }
        }

        /// <summary>
        /// The collection of all graphics items being drawn
        /// </summary>
        protected GraphicsItemsCollection _graphicsItems;

        /// <summary>
        /// The collection of all things drawn.
        /// </summary>
        public GraphicsItemsCollection Sprites
        {
            get
            {
                return _graphicsItems;
            }
        }
        /// <summary>
        /// The size of the cliensize converted to grid coordinates.
        /// </summary>
        public Size ClientSizeGrid
        {
            get
            {
                return new Size(DrawBox.ClientRectangle.Width / (_gridSize), DrawBox.ClientRectangle.Height / (_gridSize));
            }
        }

        Dictionary<int, bool> pressedKeys = new Dictionary<int, bool>();

        /// <summary>
        /// A form to make it easier to draw things.
        /// </summary>
        public EasyDrawForm()
        {
            InitializeComponent();
            
            //drawBox.Size = ClientSize;
            _graphicsItems = new GraphicsItemsCollection();

            _canvas = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            _mainGfx = new GraphicsActions(Graphics.FromImage(_canvas));

            _grid = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            _gridGfx = Graphics.FromImage(_grid);

            prepareGrid();

            _isRefreshNeeded = true;

            //Grid position tracking 
            this.MouseMove += new MouseEventHandler(EasyDrawForm_MouseMove);

            //Drawing defaults
            DrawColor = Color.Black;
            PenThickness = 1.0f;

            saveDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            saveDialog.Filter = "PNG (*.png) |*png|JPG (*.jpg)|*.jpg";

            
            DrawBox.Size = ClientSize;
            UpdateCoordinates = true;
            Keys[] allKeys = (Keys[])Enum.GetValues(typeof(Keys));

            for (int i = 0; i < allKeys.Length; i++)
            {
                if (!pressedKeys.ContainsKey((int)allKeys[i]))
                {
                    pressedKeys.Add((int)allKeys[i], false);
                }
            }

            KeyDown += new KeyEventHandler(EasyDrawForm_KeyDown);
            KeyUp += new KeyEventHandler(EasyDrawForm_KeyUp);
        }

        void EasyDrawForm_KeyUp(object sender, KeyEventArgs e)
        {
            pressedKeys[(int)e.KeyCode] = false;
        }

        void EasyDrawForm_KeyDown(object sender, KeyEventArgs e)
        {
            pressedKeys[(int)e.KeyCode] = true;
        }

        public bool IsKeyDown(Keys key)
        {
            return pressedKeys[(int)key];
        }

        void EasyDrawForm_MouseMove(object sender, MouseEventArgs e)
        {
            GridPosition = new Point(e.X / _gridSize, e.Y / _gridSize);
        }
        /// <summary>
        /// Draws the grid onto the grid canvas.
        /// </summary>
        private void prepareGrid()
        {
            _gridGfx.Clear(Color.Transparent);

            for (int x = 0; x < ClientRectangle.Width; x += _gridSize)
            {
                for (int y = 0; y < ClientRectangle.Height; y += _gridSize)
                {
                    _gridGfx.DrawLine(Pens.Black, new Point(0, y), new Point(ClientRectangle.Width, y));
                }

                _gridGfx.DrawLine(Pens.Black, new Point(x, 0), new Point(x, ClientRectangle.Height));
            }
        }
        /// <summary>
        /// Saves everything that's on the main canvas as an image.
        /// </summary>
        public void SaveImage()
        {
            if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _canvas.Save(saveDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        /// <summary>
        /// Gets a sprite with the specified name. Be sure to draw it first.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GraphicsItem GetSprite(string name)
        {
            return Sprites[name];
        }
        /// <summary>
        /// Removes a sprite with the specified name. Be sure to draw it first.
        /// </summary>
        /// <param name="name"></param>
        public void RemoveSprite(string name)
        {
            Sprites.Remove(name);
        }
        /// <summary>
        /// Removes the specified sprite.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveSprite(GraphicsItem item)
        {
            Sprites.Remove(item);
        }

        /// <summary>
        /// Returns the closest grid coordinate relative to the coord, rounding down.
        /// </summary>
        /// <param name="coord">The coordinate to convert</param>
        /// <returns></returns>
        public int ConvertToGridCoordinates(int coord)
        {
            return coord / GridSize;
        }
        /// <summary>
        /// Returns the closest grid coordinate relative to the coord, rounding down.
        /// </summary>
        /// <param name="coord">The coordinate to convert</param>
        /// <returns></returns>
        public Point ConvertToGridCoordinates(Point coord)
        {
            coord.X /= GridSize;
            coord.Y /= GridSize;

            return coord;
        }
        /// <summary>
        /// The form drawing itself
        /// </summary>
        /// <param name="e">The paint event arguments</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_isRefreshNeeded)
            {
                if (_isCanvasUpdateNeeded)
                {
                    //clear everything  
                    _mainGfx.GFX.Clear(BackColor);

                    //draw everything
                    foreach (GraphicsItem graphicsItem in _graphicsItems)
                    {
                        if (graphicsItem.Visible)
                        {
                            graphicsItem.Draw(GridSize);
                        }
                    }
                }
                //set the image
                _formImage = new Bitmap(_canvas);

                if (_showGrid)
                {
                    //draw the grid onto the main BG image
                    Graphics.FromImage(_formImage).DrawImage(_grid, new Point(0, 0));
                }
                //set the image
                DrawBox.BackgroundImage = _formImage;
                ResizeRedraw = true;
                _isRefreshNeeded = false;
            }
        }
        /// <summary>
        /// Updates the main and grid canvases
        /// </summary>
        /// <param name="redrawEntireCanvas"></param>
        protected virtual void updateCanvas(bool redrawEntireCanvas = false)
        {
            _isRefreshNeeded = true;
            _isCanvasUpdateNeeded = redrawEntireCanvas;

            if (redrawEntireCanvas)
            {
                this.Invalidate(true);
            }
        }
        /// <summary>
        /// Updates the form, forces it to redraw
        /// </summary>
        public virtual new void Update()
        {
            updateCanvas(true);
            if (OnUpdate != null)
            {
                OnUpdate(this, null);
            }
        }

        #region Draw Methods

        #region DrawCircle
        /// <summary>
        /// Draws a circle on the main canvas.
        /// </summary>
        /// <param name="x">The X position in grid coordinates</param>
        /// <param name="y">The Y position in grid coordinates</param>
        /// <param name="radius">The radius to draw with</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawCircle(int x, int y, int radius, bool fill)
        {
            return drawCircle(x, y, radius, DrawColor, null, fill);
        }
        /// <summary>
        /// Draws a circle on the main canvas
        /// </summary>
        /// <param name="x">The X position in grid coordinates</param>
        /// <param name="y">The Y position in grid coordinates</param>
        /// <param name="radius">The radius to draw with</param>
        /// <param name="name">The name to refer to this by</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawCircle(int x, int y, int radius, string name, bool fill)
        {
            return drawCircle(x, y, radius, DrawColor, name, fill);
        }
        /// <summary>
        /// Draws a circle on the main canvas
        /// </summary>
        /// <param name="x">The X position in grid coordinates</param>
        /// <param name="y">The Y position in grid coordinates</param>
        /// <param name="radius">The radius to draw with</param>
        /// <param name="drawColor">The color to draw with</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawCircle(int x, int y, int radius, Color drawColor, bool fill)
        {
            return drawCircle(x, y, radius, drawColor, null, fill);
        }
        /// <summary>
        /// Draws a circle on the main canvas
        /// </summary>
        /// <param name="x">The X position in grid coordinates</param>
        /// <param name="y">The Y position in grid coordinates</param>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="drawColor">The color to draw with</param>
        /// <param name="name">The name to refer to this by</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawCircle(int x, int y, int radius, Color drawColor, string name, bool fill)
        {
            return drawCircle(x, y, radius, drawColor, name, fill);
        }

        private GraphicsItem drawCircle(int x, int y, int radius, Color color, string name, bool fill)
        {
            //Add to list of draw items
            Point gridLocation = new Point(x, y);
            GraphicsItem newCircle;
            
            if (fill)
            {
                Action<Brush, int, int, int, int> fillAction = _mainGfx.FillEllipse;
                newCircle = new BasicFillItem(color, gridLocation, new Size(radius * 2, radius * 2), fillAction);
                //_fillEllipsesToUpdate.Add((BasicFillItem)newCircle);
            }
            else
            {
                Action<Pen, int, int, int, int> drawAction = _mainGfx.DrawEllipse;
                newCircle = new BasicDrawItem(color, PenThickness, gridLocation, new Size(radius * 2, radius * 2), drawAction);
                //_drawEllipsesToUpdate.Add((BasicDrawItem)newCircle);
            }
            newCircle.Name = name;
            _graphicsItems.Add(newCircle);

            //Draw the new circle
            newCircle.Draw(GridSize);

            updateCanvas();

            return newCircle;
        }
        #endregion DrawCircle

        #region DrawSquare
        /// <summary>
        /// Draws a square on the main canvas
        /// </summary>
        /// <param name="x">The X position in grid coordinates</param>
        /// <param name="y">The Y position in grid coordinates</param>
        /// <param name="sideLength">The length of each side</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawSquare(int x, int y, int sideLength, bool fill)
        {
            return drawSquare(x, y, sideLength, DrawColor, null, fill);
        }
        /// <summary>
        /// Draws a square on the main canvas
        /// </summary>
        /// <param name="x">The X position in grid coordinates</param>
        /// <param name="y">The Y position in grid coordinates</param>
        /// <param name="sideLength">The length of each side</param>
        /// <param name="name">The name to refer to this by</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawSquare(int x, int y, int sideLength, string name, bool fill)
        {
            return drawSquare(x, y, sideLength, DrawColor, name, fill);
        }
        /// <summary>
        /// Draws a square on the main canvas
        /// </summary>
        /// <param name="x">The X position in grid coordinates</param>
        /// <param name="y">The Y position in grid coordinates</param>
        /// <param name="sideLength">The length of each side</param>
        /// <param name="drawColor">The color to draw with</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawSquare(int x, int y, int sideLength, Color drawColor, bool fill)
        {
            return drawSquare(x, y, sideLength, drawColor, null, fill);
        }
        /// <summary>
        /// Draws a square on the main canvas
        /// </summary>
        /// <param name="x">The X position in grid coordinates</param>
        /// <param name="y">The Y position in grid coordinates</param>
        /// <param name="sideLength">The length of each side</param>
        /// <param name="drawColor">The color to draw with</param>
        /// <param name="name">The name to refer to this by</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawSquare(int x, int y, int sideLength, Color drawColor, string name, bool fill)
        {
            return drawSquare(x, y, sideLength, drawColor, name, fill);
        }

        private GraphicsItem drawSquare(int x, int y, int sideLength, Color drawColor, string name, bool fill)
        {
            //Add to list of draw items
            Point gridLocation = new Point(x, y);
            GraphicsItem newSquare;

            if (fill)
            {
                Action<Brush, int, int, int, int> fillAction = _mainGfx.FillRectangle;
                newSquare = new BasicFillItem(drawColor, gridLocation, new Size(sideLength, sideLength), fillAction);
                //_fillRectanglesToUpdate.Add((BasicFillItem)newSquare);
            }
            else
            {
                Action<Pen, int, int, int, int> drawAction = _mainGfx.DrawRectangle;
                newSquare = new BasicDrawItem(drawColor, PenThickness, gridLocation, new Size(sideLength, sideLength), drawAction);
                //_drawRectanglesToUpdate.Add((BasicDrawItem)newSquare);
            }
            newSquare.Name = name;
            _graphicsItems.Add(newSquare);

            //Draw the new circle
            newSquare.Draw(GridSize);

            updateCanvas();

            return newSquare;
        }
        #endregion DrawSquare

        #region DrawEllipse
        /// <summary>
        /// Draws an ellipse on the main canvas
        /// </summary>
        /// <param name="x">The X position in grid coordinates</param>
        /// <param name="y">The Y position in grid coordinates</param>
        /// <param name="width">The width to draw with</param>
        /// <param name="height">The height to draw with</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawEllipse(int x, int y, int width, int height, bool fill)
        {
            return drawEllipse(x, y, width, height, DrawColor, null, fill);
        }
        /// <summary>
        /// Draws an ellipse on the main canvas
        /// </summary>
        /// <param name="x">The X position in grid coordinates</param>
        /// <param name="y">The Y position in grid coordinates</param>
        /// <param name="width">The width to draw with</param>
        /// <param name="height">The height to draw with</param>
        /// <param name="name">The name to refer to this by</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawEllipse(int x, int y, int width, int height, string name, bool fill)
        {
            return drawEllipse(x, y, width, height, DrawColor, name, fill);
        }
        /// <summary>
        /// Draws an ellipse on the main canvas
        /// </summary>
        /// <param name="x">The X position in grid coordinates</param>
        /// <param name="y">The Y position in grid coordinates</param>
        /// <param name="width">The width to draw with</param>
        /// <param name="height">The height to draw with</param>
        /// <param name="drawColor">The color to draw  with</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawEllipse(int x, int y, int width, int height, Color drawColor, bool fill)
        {
            return drawEllipse(x, y, width, height, drawColor, null, fill);
        }
        /// <summary>
        /// Draws an ellipse on the main canvas
        /// </summary>
        /// <param name="x">The X position in grid coordinates</param>
        /// <param name="y">The Y position in grid coordinates</param>
        /// <param name="width">The width to draw with</param>
        /// <param name="height">The height to draw with</param>
        /// <param name="drawColor">The color to draw  with</param>
        /// <param name="name">The name to refer to this by</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawEllipse(int x, int y, int width, int height, Color drawColor, string name, bool fill)
        {
            return drawEllipse(x, y, width, height, drawColor, name, fill);
        }

        private GraphicsItem drawEllipse(int x, int y, int width, int height, Color color, string name, bool fill)
        {
            //Add to list of draw items
            Point gridLocation = new Point(x, y);
            GraphicsItem newEllipse;

            if (fill)
            {
                Action<Brush, int, int, int, int> fillAction = _mainGfx.FillEllipse;
                newEllipse = new BasicFillItem(color, gridLocation, new Size(width, height), fillAction);
                //_fillEllipsesToUpdate.Add((BasicFillItem)newEllipse);
            }
            else
            {
                Action<Pen, int, int, int, int> drawAction = _mainGfx.DrawEllipse;
                newEllipse = new BasicDrawItem(color, PenThickness, gridLocation, new Size(width, height), drawAction);
                //_drawEllipsesToUpdate.Add((BasicDrawItem)newEllipse);
            }

            newEllipse.Name = name;
            _graphicsItems.Add(newEllipse);

            //Draw the new circle
            newEllipse.Draw(GridSize);

            updateCanvas();

            return newEllipse;
        }
        #endregion DrawEllipse

        #region DrawLine
        /// <summary>
        /// Draws a line on the main canvas
        /// </summary>
        /// <param name="x1">The X coordinate in grid coordinates of the first point</param>
        /// <param name="y1">The Y coordinate in grid coordinates of the first point</param>
        /// <param name="x2">The X coordinate in grid coordinates of the second point</param>
        /// <param name="y2">The Y coordinate in grid coordinates of the second point</param>
        public GraphicsItem DrawLine(int x1, int y1, int x2, int y2)
        {
            return drawLine(new Point(x1, y1), new Point(x2, y2), DrawColor, null);
        }
        /// <summary>
        /// Draws a line on the main canvas
        /// </summary>
        /// <param name="x1">The X coordinate in grid coordinates of the first point</param>
        /// <param name="y1">The Y coordinate in grid coordinates of the first point</param>
        /// <param name="x2">The X coordinate in grid coordinates of the second point</param>
        /// <param name="y2">The Y coordinate in grid coordinates of the second point</param>
        /// <param name="name">The name to refer to this by</param>
        public GraphicsItem DrawLine(int x1, int y1, int x2, int y2, string name)
        {
            return drawLine(new Point(x1, y1), new Point(x2, y2), DrawColor, name);
        }
        /// <summary>
        /// Draws a line on the main canvas
        /// </summary>
        /// <param name="x1">The X coordinate in grid coordinates of the first point</param>
        /// <param name="y1">The Y coordinate in grid coordinates of the first point</param>
        /// <param name="x2">The X coordinate in grid coordinates of the second point</param>
        /// <param name="y2">The Y coordinate in grid coordinates of the second point</param>
        /// <param name="drawColor">The color to draw with</param>
        public GraphicsItem DrawLine(int x1, int y1, int x2, int y2, Color drawColor)
        {
            return drawLine(new Point(x1, y1), new Point(x2, y2), drawColor, null);
        }
        /// <summary>
        /// Draws a line on the main canvas
        /// </summary>
        /// <param name="x1">The X coordinate in grid coordinates of the first point</param>
        /// <param name="y1">The Y coordinate in grid coordinates of the first point</param>
        /// <param name="x2">The X coordinate in grid coordinates of the second point</param>
        /// <param name="y2">The Y coordinate in grid coordinates of the second point</param>
        /// <param name="drawColor">The color to draw with</param>
        /// <param name="name">The name to refer to this by</param>
        public GraphicsItem DrawLine(int x1, int y1, int x2, int y2, Color drawColor, string name)
        {
            return drawLine(new Point(x1, y1), new Point(x2, y2), drawColor, name);
        }
        /// <summary>
        /// Draws a line on the main canvas
        /// </summary>
        /// <param name="p1">The first point to connect</param>
        /// <param name="p2">The second point to connect</param>
        public GraphicsItem DrawLine(Point p1, Point p2)
        {
            return drawLine(p1, p2, DrawColor, null);
        }
        /// <summary>
        /// Draws a line on the main canvas
        /// </summary>
        /// <param name="p1">The first point to connect</param>
        /// <param name="p2">The second point to connect</param>
        /// <param name="name">The name to refer to this by</param>
        public GraphicsItem DrawLine(Point p1, Point p2, string name)
        {
            return drawLine(p1, p2, DrawColor, name);
        }
        /// <summary>
        /// Draws a line on the main canvas
        /// </summary>
        /// <param name="p1">The first point to connect</param>
        /// <param name="p2">The second point to connect</param>
        /// <param name="drawColor">The color to draw with</param>
        public GraphicsItem DrawLine(Point p1, Point p2, Color drawColor)
        {
            return drawLine(p1, p2, drawColor, null);
        }
        /// <summary>
        /// Draws a line on the main canvas
        /// </summary>
        /// <param name="p1">The first point to connect</param>
        /// <param name="p2">The second point to connect</param>
        /// <param name="drawColor">The color to draw with</param>
        /// <param name="name">The name to refer to this by</param>
        public GraphicsItem DrawLine(Point p1, Point p2, Color drawColor, string name)
        {
            return drawLine(p1, p2, drawColor, name);
        }

        private GraphicsItem drawLine(Point p1, Point p2, Color drawColor, string name)
        {
            //Add to list of draw items

            Line newLine;

            Action<Pen, Point, Point> drawAction = _mainGfx.DrawLine;
            newLine = new Line(drawColor, PenThickness, p1, p2, drawAction);
            //_drawLinesToUpdate.Add(newLine);

            newLine.Name = name;
            _graphicsItems.Add(newLine);

            //Draw the new circle
            newLine.Draw(GridSize);

            updateCanvas();
            return newLine;
        }
        #endregion DrawLine

        #region DrawRectangle
        /// <summary>
        /// Draws a rectangle on the main canvas
        /// </summary>
        /// <param name="x">The X coordinate in grid coordinates</param>
        /// <param name="y">The Y coordinate in grid coordinates</param>
        /// <param name="width">The X coordinate in grid coordinates of the second point</param>
        /// <param name="height">The Y coordinate in grid coordinates of the second point</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawRectangle(int x, int y, int width, int height, bool fill)
        {
            return drawRectangle(x, y, width, height, DrawColor, null, fill);
        }
        /// <summary>
        /// Draws a rectangle on the main canvas
        /// </summary>
        /// <param name="x">The X coordinate in grid coordinates</param>
        /// <param name="y">The Y coordinate in grid coordinates</param>
        /// <param name="width">The X coordinate in grid coordinates of the second point</param>
        /// <param name="height">The Y coordinate in grid coordinates of the second point</param>
        /// <param name="name">The name to refer to this by</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawRectangle(int x, int y, int width, int height, string name, bool fill)
        {
            return drawRectangle(x, y, width, height, DrawColor, name, fill);
        }
        /// <summary>
        /// Draws a rectangle on the main canvas
        /// </summary>
        /// <param name="x">The X coordinate in grid coordinates</param>
        /// <param name="y">The Y coordinate in grid coordinates</param>
        /// <param name="width">The X coordinate in grid coordinates of the second point</param>
        /// <param name="height">The Y coordinate in grid coordinates of the second point</param>
        /// <param name="drawColor">The color to draw with</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawRectangle(int x, int y, int width, int height, Color drawColor, bool fill)
        {
            return drawRectangle(x, y, width, height, drawColor, null, fill);
        }
        /// <summary>
        /// Draws a rectangle on the main canvas
        /// </summary>
        /// <param name="x">The X coordinate in grid coordinates</param>
        /// <param name="y">The Y coordinate in grid coordinates</param>
        /// <param name="width">The X coordinate in grid coordinates of the second point</param>
        /// <param name="height">The Y coordinate in grid coordinates of the second point</param>
        /// <param name="drawColor">The color to draw with</param>
        /// <param name="name">The name to refer to this by</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawRectangle(int x, int y, int width, int height, Color drawColor, string name, bool fill)
        {
            return drawRectangle(x, y, width, height, drawColor, name, fill);
        }

        private GraphicsItem drawRectangle(int x, int y, int width, int height, Color color, string name, bool fill)
        {
            //Add to list of draw items
            Point gridLocation = new Point(x, y);
            GraphicsItem newRectangle;

            if (fill)
            {
                Action<Brush, int, int, int, int> fillAction = _mainGfx.FillRectangle;
                newRectangle = new BasicFillItem(color, gridLocation, new Size(width, height), fillAction);
                //_fillRectanglesToUpdate.Add((BasicFillItem)newRectangle);
            }
            else
            {
                Action<Pen, int, int, int, int> drawAction = _mainGfx.DrawRectangle;
                newRectangle = new BasicDrawItem(color, PenThickness, gridLocation, new Size(width, height), drawAction);
                //_drawRectanglesToUpdate.Add((BasicDrawItem)newRectangle);
            }
            newRectangle.Name = name;
            _graphicsItems.Add(newRectangle);

            //Draw the new circle
            newRectangle.Draw(GridSize);

            updateCanvas();
            return newRectangle;
        }

        #endregion DrawRectangle

        #region DrawTriangle
        /// <summary>
        /// Draws a triangle on the main canvas
        /// </summary>
        /// <param name="p1">The first point to connect</param>
        /// <param name="p2">The second point to connect</param>
        /// <param name="p3">The third point to connect</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawTriangle(Point p1, Point p2, Point p3, bool fill)
        {
            return drawTriangle(p1, p2, p3, DrawColor, null, fill);
        }
        /// <summary>
        /// Draws a triangle on the main canvas
        /// </summary>
        /// <param name="p1">The first point to connect</param>
        /// <param name="p2">The second point to connect</param>
        /// <param name="p3">The third point to connect</param>
        /// <param name="name">The name to refer to this by</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawTriangle(Point p1, Point p2, Point p3, string name, bool fill)
        {
            return drawTriangle(p1, p2, p3, DrawColor, name, fill);
        }
        /// <summary>
        /// Draws a triangle on the main canvas
        /// </summary>
        /// <param name="p1">The first point to connect</param>
        /// <param name="p2">The second point to connect</param>
        /// <param name="p3">The third point to connect</param>
        /// <param name="color">The color to draw with</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawTriangle(Point p1, Point p2, Point p3, Color color, bool fill)
        {
            return drawTriangle(p1, p2, p3, color, null, fill);
        }
        /// <summary>
        /// Draws a triangle on the main canvas
        /// </summary>
        /// <param name="p1">The first point to connect</param>
        /// <param name="p2">The second point to connect</param>
        /// <param name="p3">The third point to connect</param>
        /// <param name="color">The color to draw with</param>
        /// <param name="name">The name to refer to this by</param>
        /// <param name="fill">Whether to fill it in or not</param>
        public GraphicsItem DrawTriangle(Point p1, Point p2, Point p3, Color color, string name, bool fill)
        {
            return drawTriangle(p1, p2, p3, color, name, fill);
        }

        private GraphicsItem drawTriangle(Point p1, Point p2, Point p3, Color color, string name, bool fill)
        {
            //Add to list of draw items

            Point[] points = new Point[3];
            points[0] = p1;
            points[1] = p2;
            points[2] = p3;

            Triangle newTriangle;

            newTriangle = new Triangle(color, PenThickness, fill, points);

            if (fill)
            {
                Action<Brush, Point[]> fillAction = _mainGfx.FillPolygon;
                newTriangle.FillAction = fillAction;
                //_fillPolygonsToUpdate.Add(newTriangle);
                
            }
            else
            {
                Action<Pen, Point[]> drawAction = _mainGfx.DrawPolygon;
                newTriangle.DrawAction = drawAction;
                //_drawPolygonsToUpdate.Add(newTriangle);
            }

            newTriangle.Name = name;

            _graphicsItems.Add(newTriangle);

            newTriangle.Draw(GridSize);

            updateCanvas();
            return newTriangle;
        }

        #endregion DrawTriangle

        #region DrawPolygon
        /// <summary>
        /// Draws a polygon with any number of sides on the main canvas
        /// </summary>
        /// <param name="fill">Whether to fill it in or not</param>
        /// <param name="points">The points to connect for the polygon</param>
        public GraphicsItem DrawPolygon(bool fill, params Point[] points)
        {
            return drawPolygon(DrawColor, null, fill, points);
        }
        /// <summary>
        /// Draws a polygon with any number of sides on the main canvas
        /// </summary>
        /// <param name="name">The name to associate this with</param>
        /// <param name="fill">Whether to fill it in or not</param>
        /// <param name="points">The points to connect for the polygon</param>
        public GraphicsItem DrawPolygon(string name, bool fill, params Point[] points)
        {
            return drawPolygon(DrawColor, name, fill, points);
        }
        /// <summary>
        /// Draws a polygon with any number of sides on the main canvas
        /// </summary>
        /// <param name="color">The color to draw with</param>
        /// <param name="fill">Whether to fill it in or not</param>
        /// <param name="points">The points to connect for the polygon</param>
        public GraphicsItem DrawPolygon(Color color, bool fill, params Point[] points)
        {
            return drawPolygon(color, null, fill, points);
        }
        /// <summary>
        /// Draws a polygon with any number of sides on the main canvas
        /// </summary>
        /// <param name="color">The color to draw with</param>
        /// <param name="name">The name to associate this with</param>
        /// <param name="fill">Whether to fill it in or not</param>
        /// <param name="points">The points to connect for the polygon</param>
        public GraphicsItem DrawPolygon(Color color, string name, bool fill, params Point[] points)
        {
            return drawPolygon(color, name, fill, points);
        }

        private GraphicsItem drawPolygon(Color color, string name, bool fill, params Point[] points)
        {
            //Add to list of draw items
            Polygon newShape;
            newShape = new Polygon(color, PenThickness, fill, points);
            if (fill)
            {
                Action<Brush, Point[]> fillAction = _mainGfx.FillPolygon;
                newShape.FillAction = fillAction;
                //_fillPolygonsToUpdate.Add(newShape);
            }
            else
            {
                Action<Pen, Point[]> drawAction = _mainGfx.DrawPolygon;
                newShape.DrawAction = drawAction;
                //_drawPolygonsToUpdate.Add(newShape);
            }

            newShape.Name = name;
            _graphicsItems.Add(newShape);

            //Draw the new circle
            newShape.Draw(GridSize);

            updateCanvas();
            return newShape;
        }

        #endregion DrawPolygon

        #endregion Draw Methods

        /// <summary>
        /// Determines whether any sprite is touching a wall or not
        /// </summary>
        /// <param name="item">The sprite to check</param>
        /// <param name="screenBorders">The edges to check against</param>
        /// <returns></returns>
        public bool IsTouchingEdge(GraphicsItem item, params ScreenBorders[] screenBorders)
        {
            bool hittingEdge = false;
            for (int i = 0; i < screenBorders.Length; i++)
            {
                if (((item.Left <= 0) && screenBorders[i] == ScreenBorders.Left || (item.Right >= ClientSizeGrid.Width) && screenBorders[i] == ScreenBorders.Right || (item.Top <= 0) && screenBorders[i] == ScreenBorders.Top || (item.Bottom >= ClientSizeGrid.Height) && screenBorders[i] == ScreenBorders.Bottom))
                {
                    hittingEdge = true;
                    break;
                }
            }
            return hittingEdge;
        }

        private void EasyDrawForm_Shown(object sender, EventArgs e)
        {
            if (ReadyToDraw != null)
            {
                ReadyToDraw(this, null);
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            Update();
        }

        private void EasyDrawForm_Load(object sender, EventArgs e)
        {

        }

        private void EasyDrawForm_Resize(object sender, EventArgs e)
        {
            DrawBox.Size = ClientSize;
            _grid = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);

            _canvas = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);

            
            _gridGfx = Graphics.FromImage(_grid);

            _mainGfx.GFX = Graphics.FromImage(_canvas);
            //UpdateActions();
  

            updateCanvas(true);
            prepareGrid();
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            HelpForm form = new HelpForm();
            form.ShowDialog();
        }

        private void DrawBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (UpdateCoordinates)
            {
                this.Text = String.Format("X: {0}, Y: {1}", ConvertToGridCoordinates(e.X), ConvertToGridCoordinates(e.Y));
            }
        }
    }
}
