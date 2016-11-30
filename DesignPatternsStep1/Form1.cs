using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DesignPatternsStep1
{
    public partial class Form1 : Form
    {
        public static List<Shape> drawnShapes = new List<Shape>();
        public static List<Shape> shapeCache = new List<Shape>();  

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

        Pen redPen;
        Pen bluePen;

        public static Stack<Shape> undo = new Stack<Shape>();
        public static Stack<Shape> redo = new Stack<Shape>();
        Receiver rec;

        int oldWidth;
        int oldHeight;

        private int deltaWidth;
        private int deltaHeight;

        private int deltaLocationX;
        private int deltaLocationY;
        private int oldLocationX;
        private int oldLocationY;

        List<Composite> composites = new List<Composite>();
        Composite comp;
        Leaf leaf;

        public Form1()
        {
            InitializeComponent();
            redPen = new Pen(Color.Red);
            bluePen = new Pen(Color.Blue);
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            rec = new Receiver();
        }
        public Shape DrawShape(int x, int y, Size size)
        {
            if (drawRectangle)
            {
                RectangleShape rectangleShape = new RectangleShape(this, new Point(x, y), size, commandIndex, false);
                rectangleShape.DrawShape(x, y, size.Width, size.Height, redPen);
                drawnShapes.Add(rectangleShape);
                drawRectangle = true;
                rec.InsertInUndoRedoForDraw(rectangleShape, x, y, size.Width, size.Height, this);
                commandIndex++;
                return rectangleShape;
            }
            if (drawEllipse)
            {
                EllipseShape ellipseShape = new EllipseShape(this, new Point(x, y), size, commandIndex, false);
                ellipseShape.DrawShape(x, y, size.Width, size.Height, redPen);
                drawnShapes.Add(ellipseShape);
                drawEllipse = true;
                rec.InsertInUndoRedoForDraw(ellipseShape, x, y, size.Width, size.Height, this);
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
            DrawShape(e.X, e.Y, new Size(50,50));            
        }        
        protected override void OnMouseUp(MouseEventArgs e)
        {
            mouseDown = false;
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
                        drawnShapes[i] = new RectangleShape(this, new Point(drawnShapes[i].X, drawnShapes[i].Y), new Size(drawnShapes[i].Width, drawnShapes[i].Height), drawnShapes[i].ShapeId, drawnShapes[i].InGroup);
                    else
                        drawnShapes[i] = new EllipseShape(this, new Point(drawnShapes[i].X, drawnShapes[i].Y), new Size(drawnShapes[i].Width, drawnShapes[i].Height), drawnShapes[i].ShapeId, drawnShapes[i].InGroup);

                    drawnShapes[i].DrawShape(drawnShapes[i].X, drawnShapes[i].Y, drawnShapes[i].Width, drawnShapes[i].Height, bluePen);

                    if (comp != null && !resize.Checked && !moveCheckBox.Checked)
                    {                        
                            leaf = new Leaf(drawnShapes[i]);
                            comp.AddSubordinate(leaf);
                            drawnShapes[i].InGroup = true;                        
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
                        MoveShape(drawnShapes, selectedShape,e , true);
                        canAddCircle = true;
                    }
                    else if(!mouseDown && canAddCircle)
                    {
                        int newx;
                        int newy;
                        newx = drawnShapes[selectedShape].X;
                        newy = drawnShapes[selectedShape].Y;
                        deltaLocationX = newx - oldLocationX;
                        deltaLocationY = newy - oldLocationY;
                        canAddCircle = false;
                        rec.InsertInUndoRedoForMove(drawnShapes[selectedShape], deltaLocationX, deltaLocationY, this);
                    }
                    //Make sure the drawings that arent selected dont get lost.
                    else if (!inRect)
                    {
                        if (drawnShapes[i] is RectangleShape)
                            drawnShapes[i] = new RectangleShape(this, new Point(drawnShapes[i].X, drawnShapes[i].Y), new Size(drawnShapes[i].Width, drawnShapes[i].Height), drawnShapes[i].ShapeId, drawnShapes[i].InGroup);
                         else
                             drawnShapes[i] = new EllipseShape(this, new Point(drawnShapes[i].X, drawnShapes[i].Y), new Size(drawnShapes[i].Width, drawnShapes[i].Height), drawnShapes[i].ShapeId, drawnShapes[i].InGroup);

                        drawnShapes[i].DrawShape(drawnShapes[i].X, drawnShapes[i].Y, drawnShapes[i].Width, drawnShapes[i].Height, redPen);
                        
                    }
                    //resize when mouse is down.
                    if (resize.Checked && !moveCheckBox.Checked)
                    {
                        if (mouseDown)
                        {                            
                            ResizeShape(drawnShapes, /*c.subordinates[k].GetShapeId()*/selectedShape, e, true);
                            canAddToList = true;
                        }
                        else if (!mouseDown && canAddToList)
                        {
                            int newWidth = drawnShapes[selectedShape].Width;
                            int newHeight = drawnShapes[selectedShape].Height;
                            deltaWidth = newWidth - oldWidth;
                            deltaHeight = newHeight - oldHeight;

                            canAddToList = false;
                            rec.InsertInUndoRedoForResize(drawnShapes[selectedShape], deltaWidth, deltaHeight, this);
                        }
                    }
                }
            }
        }
        private void MoveShape(List<Shape> shapes, int selectedShape, MouseEventArgs e, bool drawRectangle)
        {
            int deltaX = 0;
            int deltaY = 0;

            int offset = 25;

            if (shapes[selectedShape].X < e.X)
                deltaX = e.X - shapes[selectedShape].X;

            if (shapes[selectedShape].Y < e.Y)
                deltaY = e.Y - shapes[selectedShape].Y;

            Refresh();

            if (composites.Count == 0)
            {
                if (shapes[selectedShape] is RectangleShape)
                {
                shapes[selectedShape] = new RectangleShape(this, new Point(MousePosition.X - shapes[selectedShape].Width / 2, MousePosition.Y - shapes[selectedShape].Height), new Size(shapes[selectedShape].Width, shapes[selectedShape].Height), shapes[selectedShape].ShapeId, drawnShapes[selectedShape].InGroup);

                 }
                else
                    shapes[selectedShape] = new EllipseShape(this, new Point(MousePosition.X - shapes[selectedShape].Width / 2, MousePosition.Y - shapes[selectedShape].Height), new Size(shapes[selectedShape].Width, shapes[selectedShape].Height), shapes[selectedShape].ShapeId, drawnShapes[selectedShape].InGroup);

                shapes[selectedShape].DrawShape(shapes[selectedShape].X, shapes[selectedShape].Y, shapes[selectedShape].Width, shapes[selectedShape].Height, bluePen);
            }
            else if (composites.Count > 0)
            {
                if (!drawnShapes[selectedShape].InGroup)
                {
                    if (shapes[selectedShape] is RectangleShape)
                    {
                    shapes[selectedShape] = new RectangleShape(this, new Point(MousePosition.X - shapes[selectedShape].Width / 2, MousePosition.Y - shapes[selectedShape].Height), new Size(shapes[selectedShape].Width, shapes[selectedShape].Height), shapes[selectedShape].ShapeId, drawnShapes[selectedShape].InGroup);

                     }
                    else
                        shapes[selectedShape] = new EllipseShape(this, new Point(MousePosition.X - shapes[selectedShape].Width / 2, MousePosition.Y - shapes[selectedShape].Height), new Size(shapes[selectedShape].Width, shapes[selectedShape].Height), shapes[selectedShape].ShapeId, drawnShapes[selectedShape].InGroup);

                    shapes[selectedShape].DrawShape(shapes[selectedShape].X, shapes[selectedShape].Y, shapes[selectedShape].Width, shapes[selectedShape].Height, bluePen);
                }
                else
                {
                    foreach (Composite c in composites)
                    {
                        for (int j = 0; j < c.subordinates.Count; j++)
                        {
                            if (c.subordinates[j].GetShapeId().Equals(drawnShapes[selectedShape].ShapeId))
                            {
                                c.MoveObject(deltaX, deltaY);
                            }
                        }
                    }
                }
            }
        }

        private void ResizeShape(List<Shape> shapes, int selectedShape, MouseEventArgs e, bool drawRectangle)
        {
            int newWidth = 0;
            int newHeight = 0;

            if (shapes[selectedShape].X < e.X)
                newWidth = e.X - shapes[selectedShape].X;

            if (shapes[selectedShape].X > e.X)
                newWidth = shapes[selectedShape].X - e.X;

            if (shapes[selectedShape].Y < e.Y)
                newHeight = e.Y - shapes[selectedShape].Y;

            if (shapes[selectedShape].Y > e.Y)
                newHeight = shapes[selectedShape].Y - e.Y;

            Refresh();
            if (!drawnShapes[selectedShape].InGroup)
            {
                if (shapes[selectedShape] is RectangleShape)
                {
                    shapes[selectedShape] = new RectangleShape(this, new Point(shapes[selectedShape].X, shapes[selectedShape].Y), new Size(newWidth, newHeight), shapes[selectedShape].ShapeId, drawnShapes[selectedShape].InGroup);

                }
                else
                    shapes[selectedShape] = new EllipseShape(this, new Point(shapes[selectedShape].X, shapes[selectedShape].Y), new Size(newWidth, newHeight), shapes[selectedShape].ShapeId, drawnShapes[selectedShape].InGroup);

                shapes[selectedShape].DrawShape(shapes[selectedShape].X, shapes[selectedShape].Y, shapes[selectedShape].Width, shapes[selectedShape].Height, bluePen);
            }
            else
            {
                foreach (Composite c in composites)
                {
                    for (int j = 0; j < c.subordinates.Count; j++)
                    {
                        if (c.subordinates[j].GetShapeId().Equals(drawnShapes[selectedShape].ShapeId))
                        {
                            c.resizeGroup(newWidth, newHeight);
                        }
                    }
                }
            }
        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            rec.UndoAction();
        }

        private void RedoButton_Click(object sender, EventArgs e)
        {
            rec.RedoAction();           
        }

        public class Receiver
        {
            Form1 form = new Form1();
            Form1 lastOpenedForm = Application.OpenForms.Cast<Form>().Last() as Form1;

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

            public void InsertInUndoRedoForDraw(Shape shape, int x, int y, int width, int height, Form1 form)
            {
                CommandPattern command = new DrawCommand( shape,  x,  y,  width,  height,  form);
                _Undocommands.Push(command);
                _Redocommands.Clear();
            }

            public void InsertInUndoRedoForResize(Shape shape, int width, int height, Form1 form)
            {
                CommandPattern command = new ResizeCommand(shape, width, height, form);
                _Undocommands.Push(command);
                _Redocommands.Clear();
            }

            public void InsertInUndoRedoForMove(Shape shape, int newX, int newY, Form1 form)
            {
                CommandPattern command = new MoveCommand(shape, newX, newY, form);
                _Undocommands.Push(command);
                _Redocommands.Clear();
            }

        }

        private void importBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = importFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string file = importFileDialog.FileName;
                FileIO f = new FileIO();
                f.importFile(file);
            }
        }

        private void newGroupBtn_Click(object sender, EventArgs e)
        {
            comp = new Composite("Group", new Point(0,0), new Size(50,50));
        }

        private void showGroupsBtn_Click(object sender, EventArgs e)
        {
            foreach(Composite c in composites)
            {
               // c.MoveObject();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            composites.Add(comp);

            if (compositeBox.SelectedIndex > -1)
            {
                composites[compositeBox.SelectedIndex].AddSubordinate(comp);
                comp.compositeIndex = composites[compositeBox.SelectedIndex].compositeIndex + 1;
                comp.groepInGroup = true;
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
            FileIO f = new FileIO();
            List<string> exportList = f.exportFile(composites);
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
    }
}     

