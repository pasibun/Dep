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
        public abstract void Visit();

    }

    class Resize : VisitorPattern
    {
        Form1 lastOpenedForm = Form1.Instance;
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

            if (Form1.Instance.drawnShapes[selectedShape].X < e.X)
                newWidth = e.X - Form1.Instance.drawnShapes[selectedShape].X;

            if (Form1.Instance.drawnShapes[selectedShape].X > e.X)
                newWidth = Form1.Instance.drawnShapes[selectedShape].X - e.X;

            if (Form1.Instance.drawnShapes[selectedShape].Y < e.Y)
                newHeight = e.Y - Form1.Instance.drawnShapes[selectedShape].Y;

            if (Form1.Instance.drawnShapes[selectedShape].Y > e.Y)
                newHeight = Form1.Instance.drawnShapes[selectedShape].Y - e.Y;

            lastOpenedForm.Refresh();
            if (!Form1.Instance.drawnShapes[selectedShape].InGroup)
            {
                if (Form1.Instance.drawnShapes[selectedShape] is RectangleShape)
                {
                    //moveOrnament(Form1.Instance.drawnShapes[selectedShape]);
                    Form1.Instance.drawnShapes[selectedShape] = new RectangleShape(lastOpenedForm, new Point(Form1.Instance.drawnShapes[selectedShape].X, Form1.Instance.drawnShapes[selectedShape].Y), new Size(newWidth, newHeight), Form1.Instance.drawnShapes[selectedShape].ShapeId, Form1.Instance.drawnShapes[selectedShape].InGroup, Form1.Instance.drawnShapes[selectedShape].OrnamentList, Form1.Instance.drawnShapes[selectedShape].ShapeIndex);

                }
                else
                {
                    //moveOrnament(Form1.Instance.drawnShapes[selectedShape]);
                    Form1.Instance.drawnShapes[selectedShape] = new EllipseShape(lastOpenedForm, new Point(Form1.Instance.drawnShapes[selectedShape].X, Form1.Instance.drawnShapes[selectedShape].Y), new Size(newWidth, newHeight), Form1.Instance.drawnShapes[selectedShape].ShapeId, Form1.Instance.drawnShapes[selectedShape].InGroup, Form1.Instance.drawnShapes[selectedShape].OrnamentList, Form1.Instance.drawnShapes[selectedShape].ShapeIndex);
                }

                Form1.Instance.drawnShapes[selectedShape].DrawShape(Form1.Instance.drawnShapes[selectedShape].X, Form1.Instance.drawnShapes[selectedShape].Y, Form1.Instance.drawnShapes[selectedShape].Width, Form1.Instance.drawnShapes[selectedShape].Height, new Pen(Color.Blue));
            }
            else
            {
                foreach (Composite c in composites)
                {
                    for (int j = 0; j < c.subordinates.Count; j++)
                    {
                        if (c.subordinates[j].GetShapeId().Equals(Form1.Instance.drawnShapes[selectedShape].ShapeId))
                        {
                            c.resizeGroup(newWidth, newHeight);
                        }
                    }
                }
            }
        }

        public override void Visit()
        {
            throw new NotImplementedException();
        }
    }

    class Move : VisitorPattern
    {
        Form1 lastOpenedForm = Form1.Instance;
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

            if (Form1.Instance.drawnShapes[selectedShape].X < e.X)
                deltaX = e.X - Form1.Instance.drawnShapes[selectedShape].X;

            if (Form1.Instance.drawnShapes[selectedShape].Y < e.Y)
                deltaY = e.Y - Form1.Instance.drawnShapes[selectedShape].Y;

            lastOpenedForm.Refresh();

            if (composites.Count == 0)
            {
                if (Form1.Instance.drawnShapes[selectedShape] is RectangleShape)
                    Form1.Instance.drawnShapes[selectedShape] = new RectangleShape(lastOpenedForm, new Point(Form1.MousePosition.X - Form1.Instance.drawnShapes[selectedShape].Width / 2, Form1.MousePosition.Y - Form1.Instance.drawnShapes[selectedShape].Height), new Size(Form1.Instance.drawnShapes[selectedShape].Width, Form1.Instance.drawnShapes[selectedShape].Height), Form1.Instance.drawnShapes[selectedShape].ShapeId, Form1.Instance.drawnShapes[selectedShape].InGroup, Form1.Instance.drawnShapes[selectedShape].OrnamentList, Form1.Instance.drawnShapes[selectedShape].ShapeIndex);
                else
                    Form1.Instance.drawnShapes[selectedShape] = new EllipseShape(lastOpenedForm, new Point(Form1.MousePosition.X - Form1.Instance.drawnShapes[selectedShape].Width / 2, Form1.MousePosition.Y - Form1.Instance.drawnShapes[selectedShape].Height), new Size(Form1.Instance.drawnShapes[selectedShape].Width, Form1.Instance.drawnShapes[selectedShape].Height), Form1.Instance.drawnShapes[selectedShape].ShapeId, Form1.Instance.drawnShapes[selectedShape].InGroup, Form1.Instance.drawnShapes[selectedShape].OrnamentList, Form1.Instance.drawnShapes[selectedShape].ShapeIndex);

                Form1.Instance.drawnShapes[selectedShape].DrawShape(Form1.Instance.drawnShapes[selectedShape].X, Form1.Instance.drawnShapes[selectedShape].Y, Form1.Instance.drawnShapes[selectedShape].Width, Form1.Instance.drawnShapes[selectedShape].Height, new Pen(Color.Blue));
            }
            else if (composites.Count > 0)
            {
                if (!Form1.Instance.drawnShapes[selectedShape].InGroup)
                {
                    if (Form1.Instance.drawnShapes[selectedShape] is RectangleShape)
                        Form1.Instance.drawnShapes[selectedShape] = new RectangleShape(lastOpenedForm, new Point(Form1.MousePosition.X - Form1.Instance.drawnShapes[selectedShape].Width / 2, Form1.MousePosition.Y - Form1.Instance.drawnShapes[selectedShape].Height), new Size(Form1.Instance.drawnShapes[selectedShape].Width, Form1.Instance.drawnShapes[selectedShape].Height), Form1.Instance.drawnShapes[selectedShape].ShapeId, Form1.Instance.drawnShapes[selectedShape].InGroup, Form1.Instance.drawnShapes[selectedShape].OrnamentList, Form1.Instance.drawnShapes[selectedShape].ShapeIndex);
                    else
                        Form1.Instance.drawnShapes[selectedShape] = new EllipseShape(lastOpenedForm, new Point(Form1.MousePosition.X - Form1.Instance.drawnShapes[selectedShape].Width / 2, Form1.MousePosition.Y - Form1.Instance.drawnShapes[selectedShape].Height), new Size(Form1.Instance.drawnShapes[selectedShape].Width, Form1.Instance.drawnShapes[selectedShape].Height), Form1.Instance.drawnShapes[selectedShape].ShapeId, Form1.Instance.drawnShapes[selectedShape].InGroup, Form1.Instance.drawnShapes[selectedShape].OrnamentList, Form1.Instance.drawnShapes[selectedShape].ShapeIndex);

                    Form1.Instance.drawnShapes[selectedShape].DrawShape(Form1.Instance.drawnShapes[selectedShape].X, Form1.Instance.drawnShapes[selectedShape].Y, Form1.Instance.drawnShapes[selectedShape].Width, Form1.Instance.drawnShapes[selectedShape].Height, new Pen(Color.Blue));
                }
                else
                {
                    foreach (Composite c in composites)
                    {
                        for (int j = 0; j < c.subordinates.Count; j++)
                        {
                            if (c.subordinates[j].GetShapeId().Equals(Form1.Instance.drawnShapes[selectedShape].ShapeId))
                            {
                                c.MoveObject(deltaX, deltaY);
                            }
                        }
                    }
                }
            }
        }

        public override void Visit()
        {
            throw new NotImplementedException();
        }
    }

    class Export : VisitorPattern
    {
        private int spaces;
        private string group = "group";
        private List<string> exportList = new List<string>();
        private List<Composite> compList = new List<Composite>();
        private List<Shape> noGroupShapes = new List<Shape>();
        private List<Composite> composites = new List<Composite>();
        string x = null;
        string z = null;

        public Export(List<Composite> composites) {
            this.composites = composites;
        }

        #region Export
        public override void Visit()
        {
            noGroupShapes = Form1.Instance.drawnShapes;
            spaces = 12;
            for (int i = 0; i < noGroupShapes.Count; i++)
            {
                if (!noGroupShapes[i].InGroup)
                {
                    if (noGroupShapes[i] is RectangleShape)
                    {
                        ornamentCheck(noGroupShapes[i], null);
                        exportList.Add("rectangle " + noGroupShapes[i].X + " " + noGroupShapes[i].Y + " " + noGroupShapes[i].Width + " " + noGroupShapes[i].Height);
                    }
                    else if (noGroupShapes[i] is EllipseShape)
                    {
                        ornamentCheck(noGroupShapes[i], null);
                        exportList.Add("ellipse " + noGroupShapes[i].X + " " + noGroupShapes[i].Y + " " + noGroupShapes[i].Width + " " + noGroupShapes[i].Height);
                    }
                }
            }

            foreach (Composite c in composites)
            {
                if (!c.groepInGroup)
                {
                    ornamentCheck(null, c);
                    exportList.Add(group + " " + c.subordinates.Count.ToString());
                    export(c, spaces, 0, 0);
                }
                else
                {
                }
            }
            Form1.Instance.exportList = exportList;
        }

        private bool export(Composite c, int spaces, int totalInGroup, int groupIngroup)
        {
            string rectangle = "rectangle ";
            string ellipse = "ellipse ";

            if (c.subordinates.Count.Equals(totalInGroup))
            {
                if (groupIngroup.Equals(0))
                    return true;
                else
                {
                    for (int i = 0; i < compList[groupIngroup - 1].compositeIndex; i++)
                        x = x + "\t ";

                    exportList.Add(x + group + " " + compList[groupIngroup - 1].subordinates.Count.ToString());
                    x = null;
                    return export(compList[groupIngroup - 1], spaces + 2, 0, groupIngroup - 1);
                }
            }
            else
            {
                if (c.subordinates[totalInGroup].GetShape() is RectangleShape)
                {
                    ornamentCheck(c.subordinates[totalInGroup].GetShape(), null);
                    formatterShapes(c, rectangle, totalInGroup);
                    return export(c, spaces, totalInGroup + 1, groupIngroup);
                }
                if (c.subordinates[totalInGroup].GetShape() is EllipseShape)
                {
                    ornamentCheck(c.subordinates[totalInGroup].GetShape(), null);
                    formatterShapes(c, ellipse, totalInGroup);
                    return export(c, spaces, totalInGroup + 1, groupIngroup);
                }
                else
                {
                    if (groupIngroup == 0)
                        compList.Clear();

                    ornamentCheck(null, c.subordinates[totalInGroup] as Composite);
                    compList.Add(c.subordinates[totalInGroup] as Composite);
                    x = null;
                    return export(c, spaces, totalInGroup + 1, groupIngroup + 1);
                }
            }
        }

        //format the shapes to the proper way with tabs if this is necessary
        private void formatterShapes(Composite c, string shape, int totalInGroup)
        {
            for (int i = 0; i < c.compositeIndex + 1; i++)
            {
                z = z + "\t ";
            }

            exportList.Add(z + shape + c.subordinates[totalInGroup].GetShape().X +
                            " " + c.subordinates[totalInGroup].GetShape().Y + " " + c.subordinates[totalInGroup].GetShape().Width +
                            " " + c.subordinates[totalInGroup].GetShape().Height);
            removeShapeFromGroup(c.subordinates[totalInGroup].GetShape());

            z = null;
        }

        private void formatterOrnaments(Composite c, Shape s, List<Label> ornamentlist)
        {
            string orn = "ornament";
            foreach (Label a in ornamentlist)
            {
                if (c != null)
                {
                    for (int i = 0; i < c.compositeIndex; i++)
                    {
                        z = z + "\t ";
                    }
                }
                else
                {
                    for (int i = 0; i < s.ShapeIndex + 1; i++)
                    {
                        z = z + "\t ";
                    }
                }
                exportList.Add(z + orn + " " + a.Name + " " + "\"" + a.Text + "\"");
                z = null;
            }
        }

        private void removeShapeFromGroup(Shape shape)
        {
            for (int i = 0; i < noGroupShapes.Count; i++)
            {
                if (shape.ShapeId.Equals(noGroupShapes[i].ShapeId))
                {
                    noGroupShapes.Remove(noGroupShapes[i]);
                }
            }
        }

        private void ornamentCheck(Shape s, Composite c)
        {
            if (s == null)
            {
                if (c.groupOrnaments.Count != 0)
                    formatterOrnaments(c, s, c.groupOrnaments);
            }
            else if (s.OrnamentList.Count != 0)
                formatterOrnaments(null, s, s.OrnamentList);
        }

        public override void Visit(int shapeId)
        {
            throw new NotImplementedException();
        }        
        #endregion

    }
}
