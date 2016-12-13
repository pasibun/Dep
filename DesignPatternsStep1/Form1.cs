using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DesignPatternsStep1
{
    public partial class Form1 : Form
    {
        public List<Shape> drawnShapes = new List<Shape>();
        public List<Shape> shapeCache = new List<Shape>();
        public List<string> exportList = new List<string>();
        public List<Composite> compList = new List<Composite>();


        public int selectedShape = -1;
        private int commandIndex = 0;

        private bool mouseDown = false;
        private bool canAddToList = false;
        private bool canAddCircle = false;

        private bool inRect;

        public static bool drawEllipse = false;
        public static bool drawRectangle = false;

        public bool resizedObject = false;
        public bool movedObject = false;
        public bool newGroup = false;

        Pen redPen;
        Pen bluePen;

        public Stack<Shape> undo = new Stack<Shape>();
        public Stack<Shape> redo = new Stack<Shape>();
        Receiver rec;

        int oldWidth;
        int oldHeight;

        private int deltaWidth;
        private int deltaHeight;

        private int deltaLocationX;
        private int deltaLocationY;
        private int oldLocationX;
        private int oldLocationY;

        public List<Composite> composites = new List<Composite>();

        Composite comp;
        Leaf leaf;
        Move moveVisitor;
        Resize resizeVisitor;
        Export exportVisitor;

        public Form1()
        {
            InitializeComponent();
            redPen = new Pen(Color.Red);
            bluePen = new Pen(Color.Blue);
        }
        private static Form1 instance = null;
        public static Form1 Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Application.OpenForms.Cast<Form>().Last() as Form1;
                }
                return instance;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            rec = new Receiver();
        }
        public Shape DrawShape(int x, int y, Size size)
        {
            if (drawRectangle)
            {
                RectangleShape rectangleShape = new RectangleShape(this, new Point(x, y), size, commandIndex, false, new List<Label>(), 0);
                rectangleShape.DrawShape(x, y, size.Width, size.Height, redPen);
                drawnShapes.Add(rectangleShape);
                drawRectangle = true;
                rec.InsertInUndoRedoForDraw(rectangleShape, x, y, size.Width, size.Height);
                commandIndex++;
                return rectangleShape;
            }
            if (drawEllipse)
            {
                EllipseShape ellipseShape = new EllipseShape(this, new Point(x, y), size, commandIndex, false, new List<Label>(), 0);
                ellipseShape.DrawShape(x, y, size.Width, size.Height, redPen);
                drawnShapes.Add(ellipseShape);
                drawEllipse = true;
                rec.InsertInUndoRedoForDraw(ellipseShape, x, y, size.Width, size.Height);
                commandIndex++;
                return ellipseShape;
            }
            movedObject = false;
            resizedObject = false;

            return null;
        }
        private void drawEllipseBtn_Click(object sender, EventArgs e)
        {
            drawEllipse = true;
            drawRectangle = false;
            selectedLabel.Text = "Ellipse";
        }
        private void drawRectangleBtn_Click(object sender, EventArgs e)
        {
            drawRectangle = true;
            drawEllipse = false;
            selectedLabel.Text = "Rectangle";
        }
        private void noneButton_Click(object sender, EventArgs e)
        {
            drawEllipse = drawRectangle = false;
            selectedLabel.Text = "None";
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            DrawShape(e.X, e.Y, new Size(50, 50));
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            mouseDown = false;

            if (selectedShape != -1 && resize.Checked || selectedShape != -1 && moveCheckBox.Checked)
            {
                Shape s = drawnShapes[selectedShape];

                if (!s.InGroup)
                {
                    moveOrnement(s, null);
                }
                else
                    MoveOrnamentsInGroup(s);
            }
        }

        private void moveOrnementGroup(Shape s)
        {

            foreach (Composite c in composites)
            {
                compList.Clear();
                for (int j = 0; j < c.subordinates.Count; j++)
                {
                    if (c.subordinates[j].GetShapeId().Equals(s.ShapeId))
                    {
                        recursiveOrnament(c, 0, 0);
                    }
                }
            }
        }

        public List<int> groupSizeX = new List<int>();
        public List<int> groupSizeY = new List<int>();
        public int maxWidth;
        public int maxHeight;
        public bool compListCheck = false;
        public bool determineGroupSize(Composite c, int stop, int groupInGroup)
        {
            if (stop.Equals(c.subordinates.Count))
            {
                if (groupInGroup.Equals(0))
                    return true;
                else
                    return determineGroupSize(compList[groupInGroup - 1], 0, groupInGroup - 1);
            }
            else if (c.subordinates[stop].GetShape() is RectangleShape || c.subordinates[stop].GetShape() is EllipseShape)
            {
                if (groupSizeX.Count > 0 && c.subordinates[stop].GetShape().X >= groupSizeX.Max())
                    maxWidth = c.subordinates[stop].GetShape().Width;
                if (groupSizeY.Count > 0 && c.subordinates[stop].GetShape().Y >= groupSizeY.Max())
                    maxHeight = c.subordinates[stop].GetShape().Height;

                groupSizeX.Add(c.subordinates[stop].GetShape().X);
                groupSizeY.Add(c.subordinates[stop].GetShape().Y);
                return determineGroupSize(c, stop + 1, groupInGroup);
            }
            else
            {
                if (groupInGroup == 0)
                    compList.Clear();
                compList.Add(c.subordinates[stop] as Composite);
                return determineGroupSize(c, stop + 1, groupInGroup + 1);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            mouseDown = true;
            SelectShape(e);
        }

        private void SelectShape(MouseEventArgs e)
        {
            Point[] pt = { new Point(e.X, e.Y) };

            for (int i = 0; i < drawnShapes.Count; ++i)
            {
                if (drawnShapes[i].Contains(pt[0].X, pt[0].Y))
                {
                    selectedShape = i;

                    oldWidth = drawnShapes[selectedShape].Width;
                    oldHeight = drawnShapes[selectedShape].Height;

                    oldLocationX = drawnShapes[i].X;
                    oldLocationY = drawnShapes[i].Y;

                    Refresh();

                    if (drawnShapes[i] is RectangleShape)
                    {
                        drawnShapes[i] = new RectangleShape(this, new Point(drawnShapes[i].X, drawnShapes[i].Y), new Size(drawnShapes[i].Width, drawnShapes[i].Height), drawnShapes[i].ShapeId, drawnShapes[i].InGroup, drawnShapes[i].OrnamentList, drawnShapes[i].ShapeIndex);
                    }
                    else
                    {
                        drawnShapes[i] = new EllipseShape(this, new Point(drawnShapes[i].X, drawnShapes[i].Y), new Size(drawnShapes[i].Width, drawnShapes[i].Height), drawnShapes[i].ShapeId, drawnShapes[i].InGroup, drawnShapes[i].OrnamentList, drawnShapes[i].ShapeIndex);
                    }

                    drawnShapes[i].DrawShape(drawnShapes[i].X, drawnShapes[i].Y, drawnShapes[i].Width, drawnShapes[i].Height, bluePen);

                    if (comp != null && !resize.Checked && !moveCheckBox.Checked && newGroup)
                    {
                        leaf = new Leaf(drawnShapes[i]);
                        comp.AddSubordinate(leaf);
                        drawnShapes[i].InGroup = true;
                        if (drawnShapes[i].OrnamentList.Count != 0)
                        {
                            int countOrn = drawnShapes[i].OrnamentList.Count;
                            comp.compositeSize = comp.compositeSize + countOrn;
                        }
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Point[] pt = { new Point(e.X, e.Y) };

            for (int i = 0; i < drawnShapes.Count; ++i)
            {
                inRect = drawnShapes[i].Contains(pt[0].X, pt[0].Y);

                if (selectedShape > -1)
                {
                    if (mouseDown && inRect && !resize.Checked && moveCheckBox.Checked)
                    {
                        MoveShape(drawnShapes, selectedShape, e, true);
                        canAddCircle = true;
                    }
                    else if (!mouseDown && canAddCircle)
                    {
                        int newx;
                        int newy;
                        newx = drawnShapes[selectedShape].X;
                        newy = drawnShapes[selectedShape].Y;
                        deltaLocationX = newx - oldLocationX;
                        deltaLocationY = newy - oldLocationY;
                        canAddCircle = false;

                        // drawnShapes[selectedShape].MoveOrnaments(deltaLocationX, deltaLocationY, 0, 0, true);

                        rec.InsertInUndoRedoForMove(drawnShapes[selectedShape], deltaLocationX, deltaLocationY);
                    }
                    //Make sure the drawings that arent selected dont get lost.
                    else if (!inRect)
                    {
                        if (drawnShapes[i] is RectangleShape)
                            drawnShapes[i] = new RectangleShape(this, new Point(drawnShapes[i].X, drawnShapes[i].Y),
                                new Size(drawnShapes[i].Width, drawnShapes[i].Height), drawnShapes[i].ShapeId,
                                drawnShapes[i].InGroup, drawnShapes[i].OrnamentList, drawnShapes[i].ShapeIndex);
                        else
                            drawnShapes[i] = new EllipseShape(this, new Point(drawnShapes[i].X, drawnShapes[i].Y),
                                new Size(drawnShapes[i].Width, drawnShapes[i].Height), drawnShapes[i].ShapeId,
                                drawnShapes[i].InGroup, drawnShapes[i].OrnamentList, drawnShapes[i].ShapeIndex);

                        drawnShapes[i].DrawShape(drawnShapes[i].X, drawnShapes[i].Y, drawnShapes[i].Width, drawnShapes[i].Height, redPen);

                    }
                    //resize when mouse is down.
                    if (resize.Checked && !moveCheckBox.Checked)
                    {
                        if (mouseDown)
                        {
                            ResizeShape(drawnShapes, selectedShape, e, true);
                            canAddToList = true;
                        }
                        else if (!mouseDown && canAddToList)
                        {
                            int newWidth = drawnShapes[selectedShape].Width;
                            int newHeight = drawnShapes[selectedShape].Height;
                            deltaWidth = newWidth - oldWidth;
                            deltaHeight = newHeight - oldHeight;

                            canAddToList = false;

                            rec.InsertInUndoRedoForResize(drawnShapes[selectedShape], deltaWidth, deltaHeight);
                        }
                    }
                }
            }
        }

        private void MoveShape(List<Shape> shapes, int selectedShape, MouseEventArgs e, bool drawRectangle)
        {
            moveVisitor = new Move(e, composites);
            shapes[selectedShape].Accept(moveVisitor);
        }

        private void ResizeShape(List<Shape> shapes, int selectedShape, MouseEventArgs e, bool drawRectangle)
        {
            resizeVisitor = new Resize(e, composites);
            shapes[selectedShape].Accept(resizeVisitor);
        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            rec.UndoAction();
        }

        private void RedoButton_Click(object sender, EventArgs e)
        {
            rec.RedoAction();
        }

        private void newGroupBtn_Click(object sender, EventArgs e)
        {
            newGroup = true;
            comp = new Composite("Group", new Point(0, 0), new Size(50, 50));
        }

        private void showGroupsBtn_Click(object sender, EventArgs e)
        {
            foreach (Composite c in composites)
            {
                // c.MoveObject();
            }
        }

        //savegroup        
        private void button1_Click(object sender, EventArgs e)
        {
            compList.Clear();
            maxWidth = 0;
            maxHeight = 0;
            groupSizeX.Clear();
            groupSizeY.Clear();
            newGroup = false;
            composites.Add(comp);
            determineGroupSize(comp, 0, 0);

            comp.Position = new Point(groupSizeX.Min(), groupSizeY.Min());
            comp.Size = new Size((groupSizeX.Max() + maxWidth) - groupSizeX.Min(),
                                 (groupSizeY.Max() + maxHeight) - groupSizeY.Min());

            if (compositeBox.SelectedIndex > -1)
            {
                composites[compositeBox.SelectedIndex].AddSubordinate(comp);
                comp.compositeIndex = composites[compositeBox.SelectedIndex].compositeIndex + 1;
                comp.groepInGroup = true;

                determineGroupSize(composites[compositeBox.SelectedIndex], 0, 0);

                composites[compositeBox.SelectedIndex].Position = new Point(groupSizeX.Min(), groupSizeY.Min());
                composites[compositeBox.SelectedIndex].Size = new Size((groupSizeX.Max() + maxWidth) - groupSizeX.Min(),
                                                                        (groupSizeY.Max() + maxHeight) - groupSizeY.Min());
            }

            compositeBox.Items.Clear();
            for (int i = 0; i < composites.Count; i++)
            {
                compositeBox.Items.Add(composites[i].name + " " + composites[i].compositeSize);
            }
            compositeBox.ClearSelected();
        }

        private void exportBtn_Click(object sender, EventArgs e)
        {
            exportVisitor = new Export(composites);
            FileIO.Accept(exportVisitor);

            FileDialog export = new SaveFileDialog();
            export.Filter = "text *.txt|*.txt";
            export.FileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\exportfile.txt";

            if (export.ShowDialog() == DialogResult.OK)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@export.FileName))
                {
                    foreach (string line in exportList)
                    {
                        file.WriteLine(line);
                    }
                }
            }
        }

        private void importBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = importFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string file = importFileDialog.FileName;
                FileIO f = new FileIO();
                List<Composite> temp = f.importFile(file).ToList();
                foreach (Composite c in temp)
                {
                    composites.Add(c);
                    compositeBox.Items.Add(c.name + " " + c.compositeSize);
                }
            }
        }

        public class Receiver
        {
            Pen redPen = new Pen(Color.Red);

            private Stack<CommandPattern> _Undocommands = new Stack<CommandPattern>();
            private Stack<CommandPattern> _Redocommands = new Stack<CommandPattern>();

            public void UndoAction()
            {
                if (_Undocommands.Count > 0)
                {
                    CommandPattern command = _Undocommands.Pop();
                    command.UnExecute();
                    _Redocommands.Push(command);
                }
            }

            public void RedoAction()
            {
                if (_Redocommands.Count > 0)
                {
                    CommandPattern command = _Redocommands.Pop();
                    command.Execute();
                    _Undocommands.Push(command);
                }
            }

            public void InsertInUndoRedoForDraw(Shape shape, int x, int y, int width, int height)
            {
                CommandPattern command = new DrawCommand(shape, x, y, width, height);
                _Undocommands.Push(command);
                _Redocommands.Clear();
            }

            public void InsertInUndoRedoForResize(Shape shape, int width, int height)
            {
                CommandPattern command = new ResizeCommand(shape, width, height);
                _Undocommands.Push(command);
                _Redocommands.Clear();
            }

            public void InsertInUndoRedoForMove(Shape shape, int newX, int newY)
            {
                CommandPattern command = new MoveCommand(shape, newX, newY);
                _Undocommands.Push(command);
                _Redocommands.Clear();
            }

        }

        private void Move(List<Label> ornaments, Shape s, Composite c)
        {
            foreach (Label a in ornaments)
            {
                Font stringfont = new Font("Microsoft Sans Serif", 8);
                Graphics g = CreateGraphics();

                switch (a.Name.ToString())
                {
                    case "Above":
                        {
                            if (s != null)
                                a.Location = new Point(s.X + (s.Width / 2), s.Y - 15);
                            else if (c != null)
                                a.Location = new Point(c.X + (c.Width / 2), c.Y - 15);
                            Refresh();
                            break;
                        }
                    case "Below":
                        {
                            if (s != null)
                                a.Location = new Point(s.X + (s.Width / 2), s.Y + s.Height + 15);
                            else if (c != null)
                                a.Location = new Point(c.X + (c.Width / 2), c.Y + c.Height + 15);
                            Refresh();
                            break;
                        }
                    case "Left":
                        {
                            SizeF stringsSize = g.MeasureString(a.Text, stringfont);
                            if (s != null)
                                a.Location = new Point(s.X - ((int)stringsSize.Width + 25), s.Y + (s.Height / 2));
                            else if (c != null)
                                a.Location = new Point(c.X - ((int)stringsSize.Width + 25), c.Y + (c.Height / 2));
                            Refresh();
                            break;
                        }
                    case "Right":
                        {
                            if (s != null)
                                a.Location = new Point(s.X + s.Width + 10, s.Y + (s.Height / 2));
                            else if (c != null)
                                a.Location = new Point(c.X + c.Width + 10, c.Y + (c.Height / 2));
                            Refresh();
                            break;
                        }
                }
            }
        }

        private void moveOrnement(Shape s, Composite c)
        {
            if (c != null)
            {
                groupSizeX.Clear();
                groupSizeY.Clear();
                maxWidth = 0;
                maxHeight = 0;
                determineGroupSize(c, 0, 0);
                c.Position = new Point(groupSizeX.Min(), groupSizeY.Min());
                c.Size = new Size((groupSizeX.Max() + maxWidth) - groupSizeX.Min(),
                                     (groupSizeY.Max() + maxHeight) - groupSizeY.Min());

                Move(c.groupOrnaments, null, c);
            }
            else if (s != null)
            {
                Move(s.OrnamentList, s, null);
            }
        }

        public void MoveOrnamentsInGroup(Shape s)
        {
            foreach (Composite c in composites)
            {
                for (int j = 0; j < c.subordinates.Count; j++)
                {
                    if (c.subordinates[j].GetShapeId().Equals(s.ShapeId))
                    {
                        recursiveOrnament(c, 0, 0);
                    }
                }
            }

        }

        public bool recursiveOrnament(Composite c, int stop, int groupInGroup)
        {
            if (stop.Equals(c.subordinates.Count))
            {
                if (groupInGroup.Equals(0))
                {
                    return true;
                }
                else
                {
                    return recursiveOrnament(compList[groupInGroup - 1], 0, groupInGroup - 1);
                }
            }
            else
            {
                if (c.subordinates[stop].GetShape() is RectangleShape || c.subordinates[stop].GetShape() is EllipseShape)
                {
                    moveOrnement(c.subordinates[stop].GetShape(), c);
                    return recursiveOrnament(c, stop + 1, 0);
                }
                else
                {
                    if (groupInGroup == 0)
                        compList.Clear();
                    compList.Add(c.subordinates[stop] as Composite);
                    return recursiveOrnament(c, stop + 1, groupInGroup + 1);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (compositeBox.SelectedIndex > -1)
            {
                for (int i = 0; i < composites.Count; i++)
                {
                    compList.Clear();
                    groupSizeX.Clear();
                    groupSizeY.Clear();
                    maxWidth = 0;
                    maxHeight = 0;
                    determineGroupSize(composites[i], 0, 0);
                    composites[i].Position = new Point(groupSizeX.Min(), groupSizeY.Min());
                    composites[i].Size = new Size((groupSizeX.Max() + maxWidth) - groupSizeX.Min(),
                                                (groupSizeY.Max() + maxHeight) - groupSizeY.Min());
                }
                OrnamentPopup ornForm = new OrnamentPopup(null, composites[compositeBox.SelectedIndex]);
                ornForm.Show();
            }
            else if (!selectedShape.Equals(-1))
            {
                OrnamentPopup ornForm = new OrnamentPopup(drawnShapes[selectedShape], null);
                ornForm.Show();
            }
            else MessageBox.Show("Please select a shape first!");
        }
    }
}

