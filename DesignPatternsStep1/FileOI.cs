using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignPatternsStep1
{
    class FileIO
    {
        private int spaces;
        private string group = "group";
        private List<string> exportList = new List<string>();
        private List<Composite> compList = new List<Composite>();
        private List<Shape> noGroupShapes = new List<Shape>();
        string x = null;
        string z = null;

        private Point location;
        private Size size;

        private List<string> wordParts = new List<string>();
        private List<string> lines;

        private Pen redPen;
        private Form1 form = Application.OpenForms.Cast<Form>().Last() as Form1;
        private Composite comp;
        private Leaf leaf;

        #region import       
        public List<Composite> importFile(string importFile) {
            lines = File.ReadAllLines(importFile).ToList();
            compList.Clear();
            List<Composite> tempComp = recursiveImport(0);
            return tempComp;
        }

        private List<Composite> recursiveImport(int stopCount)
        {
            if (lines.Count.Equals(stopCount))
            {
                return compList;
            }
            else
            {
                redPen = new Pen(Color.Red);
                //de line opdelen in verschillende woorden door het op te splitsen.
                wordParts = lines[stopCount].Split(' ').ToList();
                int countTabs = DetermineTabs(wordParts);
                RemoveTabs(wordParts, 0);
                //eerste woord van line controleren
                switch (wordParts.First())
                {
                    case "group":
                        {
                            comp = new Composite("Group", new Point(0, 0), new Size(50, 50));
                            comp.compositeIndex = countTabs;
                            comp.compositeSize = int.Parse(wordParts[1]);
                            if (!countTabs.Equals(0))
                            {
                                comp.groepInGroup = true;
                                int amountGroup = compList.Count;
                                int amountInGroup = compList[amountGroup - 1].subordinates.Count;
                                if (compList[amountGroup -1].compositeSize != amountInGroup)
                                {
                                    compList[amountGroup-1].AddSubordinate(comp);
                                    compList[amountGroup - 1].compositeSize = compList[amountGroup - 1].subordinates.Count;
                                }
                            }
                            wordParts.Clear();
                            stopCount++;
                            compList.Add(comp);
                            return recursiveImport(stopCount);
                        }

                    case "rectangle":
                        {
                            location = new Point(int.Parse(wordParts[1]), int.Parse(wordParts[2]));
                            size = new Size(int.Parse(wordParts[3]), int.Parse(wordParts[4]));
                            if (!countTabs.Equals(0))
                            {
                                Shape s = createRectangle(location, size);
                                leaf = new Leaf(s);
                                s.InGroup = true;
                                comp.subordinates.Add(leaf);
                            }
                            else createRectangle(location, size);
                            wordParts.Clear();
                            stopCount++;
                            return recursiveImport(stopCount);
                        }

                    case "ellipse":
                        {
                            location = new Point(int.Parse(wordParts[1]), int.Parse(wordParts[2]));
                            size = new Size(int.Parse(wordParts[3]), int.Parse(wordParts[4]));
                            if (!countTabs.Equals(0))
                            {
                                Shape s = createEllipse(location, size);
                                leaf = new Leaf(s);
                                s.InGroup = true;
                                comp.subordinates.Add(leaf);
                            }
                            else createEllipse(location, size);
                            wordParts.Clear();
                            stopCount++;                            
                            return recursiveImport(stopCount);
                        }
                }
            }
            return null;
        }

        private Shape createRectangle(Point location, Size size)
        {
            Form1.drawRectangle = true;
            Form1.drawEllipse = true;
            return form.DrawShape(location.X, location.Y, size);
        }

        private Shape createEllipse(Point location, Size size)
        {
            Form1.drawEllipse = true;
            Form1.drawRectangle = false;
            return form.DrawShape(location.X, location.Y, size);
        }

        private int DetermineTabs(List<string> checkSpaces) {
            int countTabs = 0;
            for (int i = 0; i < checkSpaces.Count; i++)
            {
                if (checkSpaces[i].Equals("\t"))
                {
                    countTabs++;
                }
            }
            return countTabs;
        }

        private bool RemoveTabs(List<string> removespaces, int index) {
            if (removespaces.Count.Equals(index))
            {
                return true;
            }else
            {
                if (removespaces[index].Equals("") || removespaces[index].Equals("\t"))
                {
                    removespaces.RemoveAt(index);
                    return RemoveTabs(removespaces, 0);
                }
                else return RemoveTabs(removespaces, index + 1);
            }            
        }
        #endregion
        #region Export
        public List<string> exportFile(List<Composite> composites)
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
            return exportList; 
        }      

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
                    for (int i = 0; i < compList[groupIngroup-1].compositeIndex; i++)
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
        #endregion
    }
}