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
        private int addSpaces;

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
                            //countspace voor het woord moet getelt worden
                            //if (countSpaces2.Equals(countSpaces))
                            // {
                            //Shape s = new RectangleShape(form, new Point(int.Parse(wordParts[1]), int.Parse(wordParts[2])), new Size(int.Parse(wordParts[3]), int.Parse(wordParts[4])), form.commandIndex);
                            Shape s = createEllipse(new Point(int.Parse(wordParts[1]), int.Parse(wordParts[2])), new Size(int.Parse(wordParts[3]), int.Parse(wordParts[4])));
                            leaf = new Leaf(s);
                            comp.AddSubordinate(leaf);
                            createRectangle(new Point(s.X, s.Y), new Size(s.Width, s.Height));
                            shapeAmountInGroupCheck++;
                            lines[i].Remove(i);
                            //}
                            break;
                        }
                    case "ellipse":
                        {
                            //countspace voor het woord moet getelt worden
                            if (countSpaces2.Equals(countSpaces))
                            {
                                //Shape s = new EllipseShape(form, new Point(int.Parse(wordParts[1]), int.Parse(wordParts[2])), new Size(int.Parse(wordParts[3]), int.Parse(wordParts[4])), form.commandIndex);
                                leaf = new Leaf(createEllipse(new Point(int.Parse(wordParts[1]), int.Parse(wordParts[2])), new Size(int.Parse(wordParts[3]), int.Parse(wordParts[4]))));
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
            string rectangle = "rectangle ";
            string ellipse = "ellipse ";
            string group = "group ";
            noGroupShapes = Form1.drawnShapes;
            List<string> exportList = new List<string>();
            foreach (Composite c in composites)
            {
                exportList.Add(group + c.subordinates.Count.ToString());
                for (int i = 0; i < c.subordinates.Count; i++)
                {
                    if (c.subordinates[i].GetShape() is RectangleShape)
                    {
                        exportList.Add(rectangle.PadLeft(12) + c.subordinates[i].GetShape().X +
                            " " + c.subordinates[i].GetShape().Y + " " + c.subordinates[i].GetShape().Width +
                            " " + c.subordinates[i].GetShape().Height);
                        removeShapeFromGroup(c.subordinates[i].GetShape());
                    }
                    else
                    {
                        exportList.Add(ellipse.PadLeft(12) + c.subordinates[i].GetShape().X +
                            " " + c.subordinates[i].GetShape().Y + " " + c.subordinates[i].GetShape().Width +
                            " " + c.subordinates[i].GetShape().Height);
                        removeShapeFromGroup(c.subordinates[i].GetShape());
                    }
                }
            }
            for (int i = 0; i < noGroupShapes.Count; i++)
            {
                if (noGroupShapes[i] is RectangleShape)
                {
                    exportList.Add(rectangle + noGroupShapes[i].X + " " + noGroupShapes[i].Y
                        + " " + noGroupShapes[i].Width + " " + noGroupShapes[i].Height);
                }
                else
                {
                    exportList.Add(ellipse + Form1.drawnShapes[i].X.ToString() + " " + Form1.drawnShapes[i].Y.ToString() + " " + Form1.drawnShapes[i].Width.ToString() + " " + Form1.drawnShapes[i].Height.ToString());
                }
            }
            return exportList; ;
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


//if (shapeAmountInGroup == shapeAmountInGroupCheck)
//{
//    for (int j = 0; j < groupShapes.Count; j++)
//    {
//        string[] wordPart = groupShapes[j].Split(' ');
//        if (wordPart.First().Equals("rectangle"))
//        {
//            location = new Point(int.Parse(wordParts[1]), int.Parse(wordParts[2]));
//            size = new Size(int.Parse(wordParts[3]), int.Parse(wordParts[4]));
//            //return hij goed?
//            createRectangle(location, size);
//            groupShapes.RemoveAt(j);
//        }
//        else if (wordPart.First().Equals("ellipse"))
//        {
//            location = new Point(int.Parse(wordParts[1]), int.Parse(wordParts[2]));
//            size = new Size(int.Parse(wordParts[3]), int.Parse(wordParts[4]));
//            //return hij goed?
//            createEllipse(location, size);
//            groupShapes.RemoveAt(j);
//        }
//        else
//        {
//            ornament = groupShapes[j];
//        }
//    }//endforeach                        
//}//endif