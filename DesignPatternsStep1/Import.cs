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
        private List<string> wordParts = new List<string>();
        private List<string> ornamentitems = new List<string>();
        private List<string> lines;


        private Point location;
        private Size size;

        private Pen redPen;
        private Form1 form = Form1.Instance;
        private Composite comp;
        private Leaf leaf;

        #region import  
        //Read text file and add it to a list     
        public List<Composite> importFile(string importFile)
        {
            lines = File.ReadAllLines(importFile).ToList();
            compList.Clear();
            List<Composite> tempComp = recursiveImport(0);
            foreach (Composite c in Form1.Instance.composites)
            {
                for (int i = 0; i < c.subordinates.Count; i++)
                {
                    if (c.subordinates[i].GetShape() is RectangleShape || c.subordinates[i].GetShape() is EllipseShape)
                        Form1.Instance.MoveOrnamentsInGroup(c.subordinates[i].GetShape());
                }               
            }
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
                            if (ornamentitems.Count != 0)
                            {
                                foreach (string a in ornamentitems)
                                {
                                    List<string> cache = new List<string>();
                                    cache = a.Split(' ').ToList();
                                    RemoveTabs(cache, 0);
                                    string ornamentLocatie = cache[1];
                                    cache[2].Replace(@"\", "");
                                    string ornamentText = cache[2];
                                    createOrnament(null, comp, ornamentLocatie, ornamentText);
                                }
                                ornamentitems.Clear();
                            }
                            comp.compositeIndex = countTabs;
                            comp.compositeSize = int.Parse(wordParts[1]);
                            if (!countTabs.Equals(0))
                            {
                                comp.groepInGroup = true;
                                int amountGroup = compList.Count;
                                int amountInGroup = compList[amountGroup - 1].subordinates.Count;
                                if (compList[amountGroup - 1].compositeSize != amountInGroup)
                                {
                                    compList[amountGroup - 1].AddSubordinate(comp);
                                    compList[amountGroup - 1].compositeSize = compList[amountGroup - 1].subordinates.Count;
                                }
                            }
                            wordParts.Clear();
                            stopCount++;
                            compList.Add(comp);
                            Form1.Instance.composites.Add(comp);
                            return recursiveImport(stopCount);
                        }

                    case "rectangle":
                        {
                            Shape s;
                            location = new Point(int.Parse(wordParts[1]), int.Parse(wordParts[2]));
                            size = new Size(int.Parse(wordParts[3]), int.Parse(wordParts[4]));
                            if (!countTabs.Equals(0))
                            {
                                s = createRectangle(location, size);
                                leaf = new Leaf(s);
                                s.InGroup = true;
                                comp.subordinates.Add(leaf);
                                Form1.Instance.maxWidth = 0;
                                Form1.Instance.maxHeight = 0;
                                Form1.Instance.groupSizeX.Clear();
                                Form1.Instance.groupSizeY.Clear();
                                Form1.Instance.determineGroupSize(comp, 0, 0);
                                comp.Position = new Point(Form1.Instance.groupSizeX.Min(), Form1.Instance.groupSizeY.Min());
                                comp.Size = new Size((Form1.Instance.groupSizeX.Max() + Form1.Instance.maxWidth) - Form1.Instance.groupSizeX.Min(),
                                                     (Form1.Instance.groupSizeY.Max() + Form1.Instance.maxHeight) - Form1.Instance.groupSizeY.Min());
                            }
                            else
                            {
                                s = createRectangle(location, size);
                                Form1.Instance.moveOrnement(s, null);
                            }
                            wordParts.Clear();
                            checkIfShapeGetsOrnament(s);
                            stopCount++;
                            return recursiveImport(stopCount);
                        }

                    case "ellipse":
                        {
                            Shape s;
                            location = new Point(int.Parse(wordParts[1]), int.Parse(wordParts[2]));
                            size = new Size(int.Parse(wordParts[3]), int.Parse(wordParts[4]));
                            if (!countTabs.Equals(0))
                            {
                                
                                s = createEllipse(location, size);
                                leaf = new Leaf(s);
                                s.InGroup = true;
                                comp.subordinates.Add(leaf);
                                Form1.Instance.maxWidth = 0;
                                Form1.Instance.maxHeight = 0;
                                Form1.Instance.groupSizeX.Clear();
                                Form1.Instance.groupSizeY.Clear();
                                Form1.Instance.determineGroupSize(comp, 0, 0);
                                comp.Position = new Point(Form1.Instance.groupSizeX.Min(), Form1.Instance.groupSizeY.Min());
                                comp.Size = new Size((Form1.Instance.groupSizeX.Max() + Form1.Instance.maxWidth) - Form1.Instance.groupSizeX.Min(),
                                                     (Form1.Instance.groupSizeY.Max() + Form1.Instance.maxHeight) - Form1.Instance.groupSizeY.Min());
                            }
                            else
                            {
                                s = createEllipse(location, size);
                                Form1.Instance.moveOrnement(s, null);
                            }
                            wordParts.Clear();
                            checkIfShapeGetsOrnament(s);
                            stopCount++;
                            return recursiveImport(stopCount);
                        }
                    case "ornament":
                        {
                            ornamentitems.Add(lines[stopCount]);
                            stopCount++;
                            return recursiveImport(stopCount);
                        }
                }
            }
            return recursiveImport(stopCount);
        }

        private void checkIfShapeGetsOrnament(Shape s)
        {
            if (ornamentitems.Count != 0)
            {
                foreach (string a in ornamentitems)
                {
                    List<string> cache = new List<string>();
                    cache = a.Split(' ').ToList();
                    RemoveTabs(cache, 0);
                    string ornamentLocatie = cache[1];
                    cache[2].Replace(@"\", "");
                    string ornamentText = cache[2];
                    createOrnament(s, null, ornamentLocatie, ornamentText);
                }
                ornamentitems.Clear();
            }
        }

        private void createOrnament(Shape s, Composite c, string location, string text)
        {
            switch (location)
            {
                case "Above":
                    {
                        AboveOrnament aOrnament = new AboveOrnament(s, c, text);
                        CreateLabel(aOrnament, location, text, s, c);
                        break;
                    }
                case "Below":
                    {
                        BelowOrnament bOrnament = new BelowOrnament(s, c, text);
                        CreateLabel(bOrnament, location, text, s, c);
                        break;
                    }
                case "Left":
                    {
                        LeftOrnament lOrnament = new LeftOrnament(s, c, text);
                        CreateLabel(lOrnament, location, text, s, c);
                        break;
                    }
                case "Right":
                    {
                        RightOrnament rOrnament = new RightOrnament(s, c, text);
                        CreateLabel(rOrnament, location, text, s, c);
                        break;
                    }
            }
        }

        private void CreateLabel(DecoratorPattern typeOrnament, string type, string text, Shape s, Composite c)
        {
            Label OrnamentLabel = new Label();
            OrnamentLabel.Location = typeOrnament.OrnamentLocation;
            OrnamentLabel.Name = type;
            OrnamentLabel.Text = text;
            OrnamentLabel.Size = new Size(30, 15);
            OrnamentLabel.AutoSize = true;

            form.Controls.Add(OrnamentLabel);
            if (c == null)
                s.OrnamentList.Add(OrnamentLabel);
            else
            {
                c.GroupOrnaments.Add(OrnamentLabel);
                Form1.Instance.determineGroupSize(c, 0, 0);
                Form1.Instance.recursiveOrnament(c, 0, 0);
            }
            form.Refresh();
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


        private int DetermineTabs(List<string> checkSpaces)
        {
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

        //Recursive funtion, remove tabs after determine tabs.
        private bool RemoveTabs(List<string> removespaces, int index)
        {
            if (removespaces.Count.Equals(index))
            {
                return true;
            }
            else
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