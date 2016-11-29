using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignPatternsStep1
{
    class FileIO
    {
        int spaces;
        string group = "group";
        private int addSpaces;
        List<string> exportList = new List<string>();
        List<Composite> compList = new List<Composite>();
        private string[] lines;
        private int shapeAmountInGroup;
        private int shapeAmountInGroupCheck;
        private Point location;
        private Size size;
        private int countSpaces;
        private int countSpaces2;
        private string ornament = "";
        private List<string> groupShapes;
        private string[] wordParts;
        List<Shape> noGroupShapes;
        Pen redPen;
        Form1 form = Application.OpenForms.Cast<Form>().Last() as Form1;
        Composite comp;
        Leaf leaf;

        //Wat nog moet is dat hij verwijdert wordt ui de wordpart shit.

        /* Deze functie zorgt er voor dat een gebruiker een bestand kan toevoegen om verschillende shapes toe te voegen.
         * Hij kijk welke shapes in welke groep zit en kijken of er nog een bijschift bij de shape zit.*/
        public void importFile(string importFile)
        {
            redPen = new Pen(Color.Red);
            lines = System.IO.File.ReadAllLines(importFile);
            try
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    //de line opdelen in verschillende woorden door het op te splitsen.
                    wordParts = lines[i].Split(' ');
                    string spaces = wordParts.First();

                    //eerste woord van line controleren
                    if (spaces.Equals("group"))
                    {
                        comp = new Composite("Group", new Point(0, 0), new Size(50, 50));
                        /*wit regels tellen, moet ff getest worden, maar afhankelijk 
                        hiervan weet je welke shape bij welke groep hoort.*/
                        countSpaces = spaces.Count(Char.IsWhiteSpace);
                        //na het woord Group wordt het amount van de shapes verteld. Hier haal je hier op.
                        shapeAmountInGroup = Int32.Parse(wordParts.Last());
                        getterGroupItems();
                    }
                    else if (wordParts.First().Equals("rectangle"))
                    {
                        location = new Point(int.Parse(wordParts[1]), int.Parse(wordParts[2]));
                        size = new Size(int.Parse(wordParts[3]), int.Parse(wordParts[4]));
                        //return hij goed?
                        lines[i] = "";
                        createRectangle(location, size);
                    }
                    else if (wordParts.First().Equals("ellipse"))
                    {
                        location = new Point(int.Parse(wordParts[1]), int.Parse(wordParts[2]));
                        size = new Size(int.Parse(wordParts[3]), int.Parse(wordParts[4]));
                        //return hij goed?
                        lines[i] = "";
                        createEllipse(location, size);
                    }
                    //else
                    //{
                    //    ornament = groupShapes[i];
                    //    lines[i] = "";
                    //}
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<string> getterGroupItems()
        {
            /* De hele hele text wordt gecontroleerd op alle shaped die bij deze groep hoort.
             * Deze worden opgeslagen in een lijst en als alles gevonden is dan worden ze pas gedrawed.*/
            //groupShapes = new List<string>();
            for (int i = 0; i < lines.Length; i++)
            {
                //count spaces
                countSpaces2 = lines[i].Count(Char.IsWhiteSpace) - 4;
                //seperat words
                string[] wordParts2 = lines[i].Trim().Split(' ');

                switch (wordParts2.First())
                {
                    case "rectangle":
                        {
                            Shape s = createEllipse(new Point(int.Parse(wordParts2[1]), int.Parse(wordParts2[2])), new Size(int.Parse(wordParts2[3]), int.Parse(wordParts2[4])));
                            leaf = new Leaf(s);
                            comp.AddSubordinate(leaf);
                            createRectangle(new Point(s.X, s.Y), new Size(s.Width, s.Height));
                            shapeAmountInGroupCheck++;
                            lines[i].Remove(i);
                            break;
                        }
                    case "ellipse":
                        {
                            //countspace voor het woord moet getelt worden
                            if (countSpaces2.Equals(countSpaces))
                            {
                                //Shape s = new EllipseShape(form, new Point(int.Parse(wordParts[1]), int.Parse(wordParts[2])), new Size(int.Parse(wordParts[3]), int.Parse(wordParts[4])), form.commandIndex);
                                leaf = new Leaf(createEllipse(new Point(int.Parse(wordParts2[1]), int.Parse(wordParts2[2])), new Size(int.Parse(wordParts2[3]), int.Parse(wordParts2[4]))));
                                comp.AddSubordinate(leaf);

                                groupShapes.Add(lines[i]);
                                shapeAmountInGroupCheck = shapeAmountInGroupCheck + 1;
                                lines[i] = "";
                            }
                            break;
                        }
                    case "ornament":
                        {
                            countSpaces2 = lines[i].Count(Char.IsWhiteSpace) - 2;
                            //countspace voor het woord moet getelt worden
                            if (countSpaces2.Equals(countSpaces))
                            {
                                groupShapes.Add(lines[i]);
                                lines[i] = "";
                            }
                            break;
                        }
                    case "group": { break; }
                }//endswitch
            }//end foreach
            return groupShapes;
        }
        private Shape createRectangle(Point location, Size size)
        {
            Form1.drawRectangle = true;
            return form.DrawShape(location.X, location.Y, size);
        }
        private Shape createEllipse(Point location, Size size)
        {
            Form1.drawEllipse = true;
            return form.DrawShape(location.X, location.Y, size);
        }

        public List<string> exportFile(List<Composite> composites)
        {           
            noGroupShapes = Form1.drawnShapes;
            spaces = 12;          
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
            return exportList; ;
        }

        private bool export(Composite c, int spaces, int totalInGroup, int groupIngroup)
        {
            string rectangle = "rectangle ";
            string ellipse = "ellipse ";
            if (c.subordinates.Count.Equals(totalInGroup))
            {
                if (groupIngroup.Equals(0))
                {
                    return true;
                }
                else
                {
                    exportList.Add(group.PadLeft(7) + " " + compList[groupIngroup -1].subordinates.Count.ToString());                    
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
                    compList.Add(c.subordinates[totalInGroup] as Composite);
                    return export(c, spaces, totalInGroup + 1, groupIngroup + 1);
                }
            }
        }

        private void formatter(Composite c, string shape, int totalInGroup, int spaces)
        {
            exportList.Add(shape.PadLeft(spaces) + c.subordinates[totalInGroup].GetShape().X +
                            " " + c.subordinates[totalInGroup].GetShape().Y + " " + c.subordinates[totalInGroup].GetShape().Width +
                            " " + c.subordinates[totalInGroup].GetShape().Height);
            removeShapeFromGroup(c.subordinates[totalInGroup].GetShape());
        }

        public void removeShapeFromGroup(Shape shape)
        {
            for (int i = 0; i < noGroupShapes.Count; i++)
            {
                if (shape.ShapeId.Equals(noGroupShapes[i].ShapeId))
                {
                    noGroupShapes.Remove(noGroupShapes[i]);
                }
            }
        }

    }
}