using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignPatternsStep1
{
    public abstract class VisitorPattern
    {
        public int deltaWidth;
        public int deltaHeight;
        public abstract void Visit(int shapeId);
    }

    class Resize : VisitorPattern
    {
        Form1 lastOpenedForm = Application.OpenForms.Cast<Form>().Last() as Form1;
        MouseEventArgs e;
        List<Composite> composites;
        public Resize(MouseEventArgs e, List<Composite> composites)
        {
            this.e = e;
            this.composites = composites;
        }

        public override void Visit(int selectedShape)
        {
            int newWidth = 0;
            int newHeight = 0;

            if (Form1.drawnShapes[selectedShape].X < e.X)
                newWidth = e.X - Form1.drawnShapes[selectedShape].X;

            if (Form1.drawnShapes[selectedShape].X > e.X)
                newWidth = Form1.drawnShapes[selectedShape].X - e.X;

            if (Form1.drawnShapes[selectedShape].Y < e.Y)
                newHeight = e.Y - Form1.drawnShapes[selectedShape].Y;

            if (Form1.drawnShapes[selectedShape].Y > e.Y)
                newHeight = Form1.drawnShapes[selectedShape].Y - e.Y;

            lastOpenedForm.Refresh();
            if (!Form1.drawnShapes[selectedShape].InGroup)
            {
                if (Form1.drawnShapes[selectedShape] is RectangleShape)
                {
                    Form1.drawnShapes[selectedShape] = new RectangleShape(lastOpenedForm, new Point(Form1.drawnShapes[selectedShape].X, Form1.drawnShapes[selectedShape].Y), new Size(newWidth, newHeight), Form1.drawnShapes[selectedShape].ShapeId, Form1.drawnShapes[selectedShape].InGroup);
                }
                else
                    Form1.drawnShapes[selectedShape] = new EllipseShape(lastOpenedForm, new Point(Form1.drawnShapes[selectedShape].X, Form1.drawnShapes[selectedShape].Y), new Size(newWidth, newHeight), Form1.drawnShapes[selectedShape].ShapeId, Form1.drawnShapes[selectedShape].InGroup);

                Form1.drawnShapes[selectedShape].DrawShape(Form1.drawnShapes[selectedShape].X, Form1.drawnShapes[selectedShape].Y, Form1.drawnShapes[selectedShape].Width, Form1.drawnShapes[selectedShape].Height, new Pen(Color.Blue));
            }
            else
            {
                foreach (Composite c in composites)
                {
                    for (int j = 0; j < c.subordinates.Count; j++)
                    {
                        if (c.subordinates[j].GetShapeId().Equals(Form1.drawnShapes[selectedShape].ShapeId))
                        {
                            c.resizeGroup(newWidth, newHeight);
                        }
                    }
                }
            }
        }
    }

    class Move : VisitorPattern
    {
        Form1 lastOpenedForm = Application.OpenForms.Cast<Form>().Last() as Form1;
        MouseEventArgs e;
        List<Composite> composites;

        public Move(MouseEventArgs e, List<Composite> composites)
        {
            this.e = e;
            this.composites = composites;
        }

        public override void Visit(int selectedShape)
        {
            int deltaX = 0;
            int deltaY = 0;

            if (Form1.drawnShapes[selectedShape].X < e.X)
                deltaX = e.X - Form1.drawnShapes[selectedShape].X;

            if (Form1.drawnShapes[selectedShape].Y < e.Y)
                deltaY = e.Y - Form1.drawnShapes[selectedShape].Y;

            lastOpenedForm.Refresh();

            if (composites.Count == 0)
            {
                if (Form1.drawnShapes[selectedShape] is RectangleShape)
                {
                    Form1.drawnShapes[selectedShape] = new RectangleShape(lastOpenedForm, new Point(Form1.MousePosition.X - Form1.drawnShapes[selectedShape].Width / 2, Form1.MousePosition.Y - Form1.drawnShapes[selectedShape].Height), new Size(Form1.drawnShapes[selectedShape].Width, Form1.drawnShapes[selectedShape].Height), Form1.drawnShapes[selectedShape].ShapeId, Form1.drawnShapes[selectedShape].InGroup);
                }
                else
                    Form1.drawnShapes[selectedShape] = new EllipseShape(lastOpenedForm, new Point(Form1.MousePosition.X - Form1.drawnShapes[selectedShape].Width / 2, Form1.MousePosition.Y - Form1.drawnShapes[selectedShape].Height), new Size(Form1.drawnShapes[selectedShape].Width, Form1.drawnShapes[selectedShape].Height), Form1.drawnShapes[selectedShape].ShapeId, Form1.drawnShapes[selectedShape].InGroup);

                Form1.drawnShapes[selectedShape].DrawShape(Form1.drawnShapes[selectedShape].X, Form1.drawnShapes[selectedShape].Y, Form1.drawnShapes[selectedShape].Width, Form1.drawnShapes[selectedShape].Height, new Pen(Color.Blue));
            }
            else if (composites.Count > 0)
            {
                if (!Form1.drawnShapes[selectedShape].InGroup)
                {
                    if (Form1.drawnShapes[selectedShape] is RectangleShape)
                    {
                        Form1.drawnShapes[selectedShape] = new RectangleShape(lastOpenedForm, new Point(Form1.MousePosition.X - Form1.drawnShapes[selectedShape].Width / 2, Form1.MousePosition.Y - Form1.drawnShapes[selectedShape].Height), new Size(Form1.drawnShapes[selectedShape].Width, Form1.drawnShapes[selectedShape].Height), Form1.drawnShapes[selectedShape].ShapeId, Form1.drawnShapes[selectedShape].InGroup);

                    }
                    else
                        Form1.drawnShapes[selectedShape] = new EllipseShape(lastOpenedForm, new Point(Form1.MousePosition.X - Form1.drawnShapes[selectedShape].Width / 2, Form1.MousePosition.Y - Form1.drawnShapes[selectedShape].Height), new Size(Form1.drawnShapes[selectedShape].Width, Form1.drawnShapes[selectedShape].Height), Form1.drawnShapes[selectedShape].ShapeId, Form1.drawnShapes[selectedShape].InGroup);

                    Form1.drawnShapes[selectedShape].DrawShape(Form1.drawnShapes[selectedShape].X, Form1.drawnShapes[selectedShape].Y, Form1.drawnShapes[selectedShape].Width, Form1.drawnShapes[selectedShape].Height, new Pen(Color.Blue));
                }
                else
                {
                    foreach (Composite c in composites)
                    {
                        for (int j = 0; j < c.subordinates.Count; j++)
                        {
                            if (c.subordinates[j].GetShapeId().Equals(Form1.drawnShapes[selectedShape].ShapeId))
                            {
                                c.MoveObject(deltaX, deltaY);
                            }
                        }
                    }
                }
            }
        }
    }

    class FileIo : VisitorPattern
    {
        public override void Visit(int shapeId)
        {
            throw new NotImplementedException();
        }
    }
}
