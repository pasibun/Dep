﻿using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DesignPatternsStep1
{
    public abstract class Shape
    {
        protected Control m_frmRef;
        protected Point location;
        protected Size size;

        protected Rectangle shape;
        protected Graphics formGraphics;
        protected Pen formPen;

        protected bool resized = false;
        protected bool moved = false;
        protected int shapeId = -1;
        protected bool drawn = false;
        protected bool inGroup = false;

        protected string myDecoration;
        protected Point decorationPos;

        protected Point ornamentLocation;
        protected string ornamentText;

        protected List<Label> ornamentList = new List<Label>();

        protected int shapeIndex = 0;

        private Shape s;
        private Composite c;
        private string text;

        public Shape(Shape s, Composite c, string text)
        {
            this.s = s;
            this.c = c;
            this.ornamentText = text;
        }

        public Shape(Control FormToDrawOn, Point shapeLocation, Size shapeSize, int shapeId, bool inGroup, List<Label> labels, int index) 
        {
            this.m_frmRef = FormToDrawOn;
            this.location = shapeLocation;
            this.size = shapeSize;
            this.ShapeId = shapeId;
            this.InGroup = inGroup;
            this.OrnamentList = labels;
            this.ShapeIndex = index;
        }

        //constructor for ornaments.
        public Shape(string text, int offsetX, int offsetY, Shape shape)
        {
            this.myDecoration = text;
            this.decorationPos.X = offsetX;
            this.decorationPos.Y = offsetY;
        }

        public int ShapeId
        {
            get { return shapeId; }
            set { shapeId = value; }
        }

        public bool InGroup
        {
            get { return inGroup; }
            set { inGroup = value; }
        }

        public List<Label> OrnamentList
        {
            get { return ornamentList; }
            set { ornamentList = value; }
        }

        public int ShapeIndex
        {
            get { return shapeIndex; }
            set { shapeIndex = value; }
        }

        public string OrnamentText
        {
            get { return ornamentText; }
            set { ornamentText = value; }
        }
        public Point OrnamentLocation
        {
            get { return ornamentLocation; }
            set { ornamentLocation = value; }
        }

        public void Accept(VisitorPattern v)
        {
            v.Visit(this.shapeId);
        }

        public virtual void DrawShape(int x, int y, int Width, int Height, Pen pen)
        {
            shape = new Rectangle(x, y, Width, Height);
            formPen = new Pen(pen.Color);
        }

        public bool Contains(int x, int y)
        {
            return shape.Contains(x,y);
        }

        public int X
        {
            get { return location.X; }
            set { location.X = value; }
        }

        public int Y
        {
            get { return location.Y; }
            set { location.Y = value; }
        }

        public int Width
        {
            get { return size.Width; }
            set { size.Width = value; }
        }

        public int Height
        {
            get { return size.Height; }
            set { size.Height = value; }
        }
    }
}
