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
        private List<Composite> compList = new List<Composite>();

        private Point location;
        private Size size;

        private List<string> wordParts = new List<string>();
        private List<string> lines;

        private Pen redPen;
        private Form1 form = Form1.Instance;
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

        public static void Accept(VisitorPattern v)
        {
            v.Visit();
        }
    }
}