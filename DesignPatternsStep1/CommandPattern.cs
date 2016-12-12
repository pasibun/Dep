using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DesignPatternsStep1.Form1;

namespace DesignPatternsStep1
{
    abstract class CommandPattern
    {
        public abstract void Execute();
        public abstract void UnExecute();
    }

    class DrawCommand : CommandPattern
    {
        Form1 lastOpenedForm = Form1.Instance;
        Shape shape;
        protected Point position;
        protected int width;
        protected int height;
        // Constructor
        public DrawCommand(Shape shape, int x, int y, int width, int height)
        {
            this.shape = shape;
            this.position = new Point(x, y);
            this.width = width;
            this.height = height;
        }

        public override void Execute()
        {
            if(!Form1.Instance.drawnShapes.Contains(shape))
                Form1.Instance.drawnShapes.Add(shape);

            lastOpenedForm.Refresh();

            for(int i = 0; i < Form1.Instance.drawnShapes.Count; ++i)
            {
                Form1.Instance.drawnShapes[i].DrawShape(Form1.Instance.drawnShapes[i].X, Form1.Instance.drawnShapes[i].Y, Form1.Instance.drawnShapes[i].Width, Form1.Instance.drawnShapes[i].Height, new Pen(Color.Red));
            }
        }
        public override void UnExecute()
        {
            Form1.Instance.drawnShapes.RemoveAt(Form1.Instance.drawnShapes.Count-1);

            lastOpenedForm.Refresh();
            for (int i = 0; i < Form1.Instance.drawnShapes.Count; ++i)
            {
                Form1.Instance.drawnShapes[i].DrawShape(Form1.Instance.drawnShapes[i].X, Form1.Instance.drawnShapes[i].Y, Form1.Instance.drawnShapes[i].Width, Form1.Instance.drawnShapes[i].Height, new Pen(Color.Red));
            }
        }
    }

    class ResizeCommand : CommandPattern
    {
        private int changeOfWidth;
        private int changeOfHeight;
        Form1 lastOpenedForm = Form1.Instance;
        Shape shape;

        public ResizeCommand(Shape shape, int newWidth, int newHeight)
        {
            this.shape = shape;
            this.changeOfWidth = newWidth;
            this.changeOfHeight = newHeight;
        }

        public override void Execute()
        {
            shape.Width = shape.Width + changeOfWidth;
            shape.Height = shape.Height + changeOfHeight;

            lastOpenedForm.Refresh();
            for (int i = 0; i < Form1.Instance.drawnShapes.Count; ++i)
            {
                if (Form1.Instance.drawnShapes[i].ShapeId == shape.ShapeId)
                    Form1.Instance.drawnShapes[i] = shape;

                Form1.Instance.drawnShapes[i].DrawShape(Form1.Instance.drawnShapes[i].X, Form1.Instance.drawnShapes[i].Y, Form1.Instance.drawnShapes[i].Width, Form1.Instance.drawnShapes[i].Height, new Pen(Color.Red));
            }
        }

        public override void UnExecute()
        {
            shape.Width = shape.Width - changeOfWidth;
            shape.Height = shape.Height - changeOfHeight;           

            lastOpenedForm.Refresh();
            for (int i = 0; i < Form1.Instance.drawnShapes.Count; ++i)
            {
                if (Form1.Instance.drawnShapes[i].ShapeId == shape.ShapeId)
                    Form1.Instance.drawnShapes[i] = shape;

                Form1.Instance.drawnShapes[i].DrawShape(Form1.Instance.drawnShapes[i].X, Form1.Instance.drawnShapes[i].Y, Form1.Instance.drawnShapes[i].Width, Form1.Instance.drawnShapes[i].Height, new Pen(Color.Red));
            }
        }
    }

    class MoveCommand : CommandPattern
    {
        Form1 lastOpenedForm = Form1.Instance;
        private Shape shape;
        private int changeOfLocationX;
        private int changeOfLocationY;

        public MoveCommand(Shape shape, int newX, int newY)
        {
            this.shape = shape;
            this.changeOfLocationX = newX;
            this.changeOfLocationY = newY;
        }

        public override void Execute()
        {
            shape.X = shape.X + changeOfLocationX;
            shape.Y = shape.Y + changeOfLocationY;

            lastOpenedForm.Refresh();
            for (int i = 0; i < Form1.Instance.drawnShapes.Count; ++i)
            {
                if (Form1.Instance.drawnShapes[i].ShapeId == shape.ShapeId)
                    Form1.Instance.drawnShapes[i] = shape;

                Form1.Instance.drawnShapes[i].DrawShape(Form1.Instance.drawnShapes[i].X, Form1.Instance.drawnShapes[i].Y, Form1.Instance.drawnShapes[i].Width, Form1.Instance.drawnShapes[i].Height, new Pen(Color.Red));
            }
        }

        public override void UnExecute()
        {
            shape.X = shape.X - changeOfLocationX;
            shape.Y = shape.Y - changeOfLocationY;

            lastOpenedForm.Refresh();
            for (int i = 0; i < Form1.Instance.drawnShapes.Count; ++i)
            {
                if (Form1.Instance.drawnShapes[i].ShapeId == shape.ShapeId)
                    Form1.Instance.drawnShapes[i] = shape;

                Form1.Instance.drawnShapes[i].DrawShape(Form1.Instance.drawnShapes[i].X, Form1.Instance.drawnShapes[i].Y, Form1.Instance.drawnShapes[i].Width, Form1.Instance.drawnShapes[i].Height, new Pen(Color.Red));
            }
        }
    }
}
