using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignPatternsStep1
{
    abstract class CompositePattern
    {
        public abstract void resizeGroup(int newWidth, int newHeight);
        public abstract void MoveObject(int deltaX, int deltaY);
        public abstract Shape GetShape();
        public abstract void SetShape(Shape value);
        public abstract int GetShapeId();
    }

    class Leaf : CompositePattern
    {
        public Shape shape;

        Form1 lastOpenedForm = Application.OpenForms.Cast<Form>().Last() as Form1;

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
                shape = new RectangleShape(lastOpenedForm, new Point(shape.X, shape.Y), new Size(newWidth, newHeight), shape.ShapeId, shape.InGroup);
            }
            else shape = new EllipseShape(lastOpenedForm, new Point(shape.X, shape.Y), new Size(newWidth, newHeight), shape.ShapeId, shape.InGroup);

            lastOpenedForm.Refresh();

            for (int i = 0; i < Form1.drawnShapes.Count; ++i)
            {
                if (Form1.drawnShapes[i].ShapeId == shape.ShapeId)
                    Form1.drawnShapes[i] = shape;

                Form1.drawnShapes[i].DrawShape(Form1.drawnShapes[i].X, Form1.drawnShapes[i].Y, Form1.drawnShapes[i].Width, Form1.drawnShapes[i].Height, new Pen(Color.Red));
            }
        }

        public override void MoveObject(int deltaX, int deltaY)
        {
            shape.X = shape.X + deltaX - 25;
            shape.Y = shape.Y + deltaY - 25;

            if(shape is RectangleShape)
                shape = new RectangleShape(lastOpenedForm, new Point(shape.X, shape.Y), new Size(shape.Width, shape.Height), shape.ShapeId, shape.InGroup);
            else
                shape = new EllipseShape(lastOpenedForm, new Point(shape.X, shape.Y), new Size(shape.Width, shape.Height), shape.ShapeId, shape.InGroup);

            lastOpenedForm.Refresh();
            for (int i = 0; i < Form1.drawnShapes.Count; ++i)
            {
                if (Form1.drawnShapes[i].ShapeId == shape.ShapeId)
                    Form1.drawnShapes[i] = shape;

                Form1.drawnShapes[i].DrawShape(Form1.drawnShapes[i].X, Form1.drawnShapes[i].Y, Form1.drawnShapes[i].Width, Form1.drawnShapes[i].Height, new Pen(Color.Red));
            }
        }
    }

    class Composite : CompositePattern
    {
        public String name;
        private Point position;
        private Size size;

        public List<CompositePattern> subordinates = new List<CompositePattern>();

        public Composite(String name, Point position, Size size)
        {
            this.name = name;
            this.position = position;
            this.size = size;
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
