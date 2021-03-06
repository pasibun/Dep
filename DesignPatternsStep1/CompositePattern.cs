﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignPatternsStep1
{
    public abstract class CompositePattern
    {
        public abstract void resizeGroup(int newWidth, int newHeight);
        public abstract void MoveObject(int deltaX, int deltaY);
        public abstract Shape GetShape();
        public abstract void SetShape(Shape value);
        public abstract int GetShapeId();
    }

    public class Leaf : CompositePattern
    {
        public Shape shape;

        Form1 lastOpenedForm = Form1.Instance;

        public Leaf(Shape shape)
        {
            this.shape = shape;
        }

        public override Shape GetShape()
        {
            return shape;
        }
        public override void SetShape(Shape value)
        {
            shape = value;
        }

        public override int GetShapeId()
        {
            return shape.ShapeId;
        }

        public override void resizeGroup(int newWidth, int newHeight)
        {
            if (shape is RectangleShape)
            {
                shape = new RectangleShape(lastOpenedForm, new Point(shape.X, shape.Y), new Size(newWidth, newHeight), shape.ShapeId, shape.InGroup, shape.OrnamentList, shape.ShapeIndex);
            }
            else shape = new EllipseShape(lastOpenedForm, new Point(shape.X, shape.Y), new Size(newWidth, newHeight), shape.ShapeId, shape.InGroup, shape.OrnamentList, shape.ShapeIndex);

            lastOpenedForm.Refresh();

            for (int i = 0; i < Form1.Instance.drawnShapes.Count; ++i)
            {
                if (Form1.Instance.drawnShapes[i].ShapeId == shape.ShapeId)
                    Form1.Instance.drawnShapes[i] = shape;

                Form1.Instance.drawnShapes[i].DrawShape(Form1.Instance.drawnShapes[i].X, Form1.Instance.drawnShapes[i].Y, Form1.Instance.drawnShapes[i].Width, Form1.Instance.drawnShapes[i].Height, new Pen(Color.Red));
            }
        }

        public override void MoveObject(int deltaX, int deltaY)
        {
            shape.X = shape.X + deltaX - 25;
            shape.Y = shape.Y + deltaY - 25;
            if (shape is RectangleShape)
                shape = new RectangleShape(lastOpenedForm, new Point(shape.X, shape.Y), new Size(shape.Width, shape.Height), shape.ShapeId, shape.InGroup, shape.OrnamentList, shape.ShapeIndex);
            else
                shape = new EllipseShape(lastOpenedForm, new Point(shape.X, shape.Y), new Size(shape.Width, shape.Height), shape.ShapeId, shape.InGroup, shape.OrnamentList, shape.ShapeIndex);

            lastOpenedForm.Refresh();

            // shape.MoveOrnaments(deltaX - 25, deltaY - 25, 0, 0, true);

            for (int i = 0; i < Form1.Instance.drawnShapes.Count; ++i)
            {
                if (Form1.Instance.drawnShapes[i].ShapeId == shape.ShapeId)
                {
                    Form1.Instance.drawnShapes[i] = shape;
                }
                Form1.Instance.drawnShapes[i].DrawShape(Form1.Instance.drawnShapes[i].X, Form1.Instance.drawnShapes[i].Y, Form1.Instance.drawnShapes[i].Width, Form1.Instance.drawnShapes[i].Height, new Pen(Color.Red));
            }
        }
    }

    public class Composite : CompositePattern
    {
        public String name;
        public int compositeSize = 0;
        private Point position;
        private Size size;
        public bool groepInGroup = false;
        public int compositeIndex = 0;
        public List<Label> groupOrnaments = new List<Label>();
        public List<CompositePattern> subordinates = new List<CompositePattern>();

        public Composite(String name, Point position, Size size)
        {
            this.name = name;
            this.position = position;
            this.size = size;
        }

        public Point Position
        {
            get { return position; }
            set { position = value; }
        }

        public List<Label> GroupOrnaments
        {
            get { return groupOrnaments; }
            set { groupOrnaments = value; }
        }

        public Size Size
        {
            get { return size; }
            set { size = value; }
        }

        public int X
        {
            get { return position.X; }
        }
        public int Y
        {
            get { return position.Y; }
        }
        public int Width
        {
            get { return size.Width; }
        }
        public int Height
        {
            get { return size.Height; }
        }

        public bool GroupInGroup
        {
            get { return groepInGroup; }
            set { groepInGroup = value; }
        }

        public override void resizeGroup(int newWidth, int newHeight)
        {
            for (int k = 0; k < subordinates.Count; k++)
            {
                subordinates[k].resizeGroup(newWidth, newWidth);
            }
        }

        public override void MoveObject(int deltaX, int deltaY)
        {
            for (int i = 0; i < subordinates.Count; ++i)
            {
                subordinates[i].MoveObject(deltaX, deltaY);
            }
        }

        public void AddSubordinate(CompositePattern leaf)
        {
            subordinates.Add(leaf);
            compositeSize++;
        }

        public override Shape GetShape()
        {
            return null;
        }

        public override void SetShape(Shape value)
        {
        }

        public override int GetShapeId()
        {
            return 0;
        }
    }
}
