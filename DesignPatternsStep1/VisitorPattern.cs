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
        int newWidth = 0;
        int newHeight = 0;

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
            if (Form1.drawnShapes[selectedShape].X < e.X)
            {
                newWidth = e.X - Form1.drawnShapes[selectedShape].X;
            }

            if (Form1.drawnShapes[selectedShape].X > e.X)
            {
                newWidth = Form1.drawnShapes[selectedShape].X - e.X;
            }

            if (Form1.drawnShapes[selectedShape].Y < e.Y)
            {
                newHeight = e.Y - Form1.drawnShapes[selectedShape].Y;
            }

            if (Form1.drawnShapes[selectedShape].Y > e.Y)
            {
                newHeight = Form1.drawnShapes[selectedShape].Y - e.Y;
            }

            lastOpenedForm.Refresh();
            if (!Form1.drawnShapes[selectedShape].InGroup)
            {
                if (Form1.drawnShapes[selectedShape] is RectangleShape)
                {
                    //moveOrnament(Form1.drawnShapes[selectedShape]);
                    Form1.drawnShapes[selectedShape] = new RectangleShape(lastOpenedForm, new Point(Form1.drawnShapes[selectedShape].X, Form1.drawnShapes[selectedShape].Y), new Size(newWidth, newHeight), Form1.drawnShapes[selectedShape].ShapeId, Form1.drawnShapes[selectedShape].InGroup, Form1.drawnShapes[selectedShape].OrnamentList);

                }
                else
                {
                    //moveOrnament(Form1.drawnShapes[selectedShape]);
                    Form1.drawnShapes[selectedShape] = new EllipseShape(lastOpenedForm, new Point(Form1.drawnShapes[selectedShape].X, Form1.drawnShapes[selectedShape].Y), new Size(newWidth, newHeight), Form1.drawnShapes[selectedShape].ShapeId, Form1.drawnShapes[selectedShape].InGroup, Form1.drawnShapes[selectedShape].OrnamentList);
                }

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

        public override void Visit()
        {
            throw new NotImplementedException();
        }
    }

    class Move : VisitorPattern
    {
        int deltaX = 0;
        int deltaY = 0;

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
            if (Form1.drawnShapes[selectedShape].X < e.X)
                deltaX = e.X - Form1.drawnShapes[selectedShape].X;

            if (Form1.drawnShapes[selectedShape].Y < e.Y)
                deltaY = e.Y - Form1.drawnShapes[selectedShape].Y;

            lastOpenedForm.Refresh();

            if (composites.Count == 0)
            {
                if (Form1.drawnShapes[selectedShape] is RectangleShape)
                    Form1.drawnShapes[selectedShape] = new RectangleShape(lastOpenedForm, new Point(Form1.MousePosition.X - Form1.drawnShapes[selectedShape].Width / 2, Form1.MousePosition.Y - Form1.drawnShapes[selectedShape].Height), new Size(Form1.drawnShapes[selectedShape].Width, Form1.drawnShapes[selectedShape].Height), Form1.drawnShapes[selectedShape].ShapeId, Form1.drawnShapes[selectedShape].InGroup, Form1.drawnShapes[selectedShape].OrnamentList);
                else
                    Form1.drawnShapes[selectedShape] = new EllipseShape(lastOpenedForm, new Point(Form1.MousePosition.X - Form1.drawnShapes[selectedShape].Width / 2, Form1.MousePosition.Y - Form1.drawnShapes[selectedShape].Height), new Size(Form1.drawnShapes[selectedShape].Width, Form1.drawnShapes[selectedShape].Height), Form1.drawnShapes[selectedShape].ShapeId, Form1.drawnShapes[selectedShape].InGroup, Form1.drawnShapes[selectedShape].OrnamentList);
                
                Form1.drawnShapes[selectedShape].DrawShape(Form1.drawnShapes[selectedShape].X, Form1.drawnShapes[selectedShape].Y, Form1.drawnShapes[selectedShape].Width, Form1.drawnShapes[selectedShape].Height, new Pen(Color.Blue));
            }
            else if (composites.Count > 0)
            {
                if (!Form1.drawnShapes[selectedShape].InGroup)
                {
                    if (Form1.drawnShapes[selectedShape] is RectangleShape)
                        Form1.drawnShapes[selectedShape] = new RectangleShape(lastOpenedForm, new Point(Form1.MousePosition.X - Form1.drawnShapes[selectedShape].Width / 2, Form1.MousePosition.Y - Form1.drawnShapes[selectedShape].Height), new Size(Form1.drawnShapes[selectedShape].Width, Form1.drawnShapes[selectedShape].Height), Form1.drawnShapes[selectedShape].ShapeId, Form1.drawnShapes[selectedShape].InGroup, Form1.drawnShapes[selectedShape].OrnamentList);
                    else
                        Form1.drawnShapes[selectedShape] = new EllipseShape(lastOpenedForm, new Point(Form1.MousePosition.X - Form1.drawnShapes[selectedShape].Width / 2, Form1.MousePosition.Y - Form1.drawnShapes[selectedShape].Height), new Size(Form1.drawnShapes[selectedShape].Width, Form1.drawnShapes[selectedShape].Height), Form1.drawnShapes[selectedShape].ShapeId, Form1.drawnShapes[selectedShape].InGroup, Form1.drawnShapes[selectedShape].OrnamentList);
                    
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

        public Export(List<Composite> composites)
        {
            this.composites = composites;
        }

        #region Export
        //This function checks if the shape is in a group, otherwise call the recursive function for formatting the groups.
        public override void Visit()
        {
            noGroupShapes = Form1.drawnShapes;
            spaces = 12;
            for (int i = 0; i < noGroupShapes.Count; i++)
            {
                if (!noGroupShapes[i].InGroup)
                {
                    if (noGroupShapes[i] is RectangleShape)
                        exportList.Add("rectangle " + noGroupShapes[i].X + " " + noGroupShapes[i].Y + " " + noGroupShapes[i].Width + " " + noGroupShapes[i].Height);
                    else if (noGroupShapes[i] is EllipseShape)
                        exportList.Add("ellipse " + noGroupShapes[i].X + " " + noGroupShapes[i].Y + " " + noGroupShapes[i].Width + " " + noGroupShapes[i].Height);
                }
            }

            foreach (Composite c in composites)
            {
                if (!c.groepInGroup)
                {
                    exportList.Add(group + " " + c.subordinates.Count.ToString());
                    export(c, spaces, 0, 0);
                }
                else
                {
                }
            }
            Form1.exportList = exportList;
        }

        /*
         * recurcive function, this function check th follow things in the groups: Whichs shapes there are, how manny tabs 
         * the need(based on depth of the group) and call format function for the proper format. 
         */
        private bool export(Composite c, int spaces, int totalInGroup, int groupIngroup)
        {
            string rectangle = "rectangle ";
            string ellipse = "ellipse ";

            if (c.subordinates.Count.Equals(totalInGroup))
            {
                if (groupIngroup.Equals(0))
                {
                    //depthIndex = 0;

                    return true;
                }
                else
                {
                    for (int i = 0; i < compList[groupIngroup - 1].compositeIndex; i++)
                    {
                        x = x + "\t ";
                    }

                    exportList.Add(x + group + " " + compList[groupIngroup - 1].subordinates.Count.ToString());
                    x = null;
                    return export(compList[groupIngroup - 1], spaces + 2, 0, groupIngroup - 1);
                }
            }
            else
            {
                if (c.subordinates[totalInGroup].GetShape() is RectangleShape)
                {
                    formatter(c, rectangle, totalInGroup, spaces);
                    return export(c, spaces, totalInGroup + 1, groupIngroup);
                }
                if (c.subordinates[totalInGroup].GetShape() is EllipseShape)
                {
                    formatter(c, ellipse, totalInGroup, spaces);
                    return export(c, spaces, totalInGroup + 1, groupIngroup);
                }
                else
                {
                    if (groupIngroup == 0)
                    {
                        compList.Clear();
                    }

                    //depthIndex++;
                    compList.Add(c.subordinates[totalInGroup] as Composite);
                    x = null;
                    return export(c, spaces, totalInGroup + 1, groupIngroup + 1);
                }
            }
        }

        //format the shapes to the proper way with tabs if this is necessary
        private void formatter(Composite c, string shape, int totalInGroup, int spaces)
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

        public override void Visit(int shapeId)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
