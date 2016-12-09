using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignPatternsStep1
{
    abstract class DecoratorPattern : Shape
    {
        public Shape ornamentshape;
        public string ornamentText;
        public Composite ornamentGroup;

        public DecoratorPattern(Control FormToDrawOn, Point shapeLocation, Size shapeSize, int shapeId,
            bool inGroup, List<Label> labels, int index) : base(FormToDrawOn, shapeLocation, shapeSize, shapeId, inGroup, labels, index)
        { }

        public DecoratorPattern(Shape s, Composite c, string text) : base(s, c, text)
        {
            this.ornamentshape = s;
            this.ornamentText = text;
            this.ornamentGroup = c;
        }
    }

    class LeftOrnament : DecoratorPattern
    {
        private Form1 form = Application.OpenForms.Cast<Form>().Last() as Form1;

        public LeftOrnament(Control FormToDrawOn, Point shapeLocation, Size shapeSize, int shapeId,
            bool inGroup, List<Label> labels, int index) : base(FormToDrawOn, shapeLocation, shapeSize, shapeId, inGroup, labels, index)
        {
        }

        public LeftOrnament(Shape s, Composite c, string text) : base(s, c, text)
        {
            Font stringfont = new Font("Microsoft Sans Serif", 8);
            Graphics g = Form1.ActiveForm.CreateGraphics();
            SizeF stringsSize = g.MeasureString(text, stringfont);

            if (c == null)
            {
                ornamentLocation.X = s.X - ((int)stringsSize.Width + 25);
                ornamentLocation.Y = s.Y + (s.Height / 2);
            }
            else
            {
                ornamentLocation.X = c.Position.X - ((int)stringsSize.Width + 25);
                ornamentLocation.Y = c.Position.Y + (c.Size.Height / 2);
            }

        }

    }
    class RightOrnament : DecoratorPattern
    {
        public RightOrnament(Control FormToDrawOn, Point shapeLocation, Size shapeSize, int shapeId,
            bool inGroup, List<Label> labels, int index) : base(FormToDrawOn, shapeLocation, shapeSize, shapeId, inGroup, labels, index)
        {
        }

        public RightOrnament(Shape s, Composite c, string text) : base(s, c, text)
        {
            if (c == null)
            {
                ornamentLocation.X = s.X + s.Width + 10;
                ornamentLocation.Y = s.Y + (s.Height / 2);
            }
            else
            {
                ornamentLocation.X = c.Position.X + (c.Size.Width + 10);
                ornamentLocation.Y = c.Position.Y + (c.Size.Height / 2);
            }
        }
    }
    class AboveOrnament : DecoratorPattern
    {
        public AboveOrnament(Control FormToDrawOn, Point shapeLocation, Size shapeSize, int shapeId,
            bool inGroup, List<Label> labels, int index) : base(FormToDrawOn, shapeLocation, shapeSize, shapeId, inGroup, labels, index)
        {
        }

        public AboveOrnament(Shape s, Composite c, string text) : base(s, c, text)
        {
            if (c == null)
            {
                ornamentLocation.Y = s.Y - 15;
                ornamentLocation.X = s.X + (s.Width / 2);
            }
            else
            {
                ornamentLocation.Y = c.Position.Y - 15;
                ornamentLocation.X = c.Position.X + (c.Size.Width / 2);
            }
        }
    }
    class BelowOrnament : DecoratorPattern
    {
        public BelowOrnament(Control FormToDrawOn, Point shapeLocation, Size shapeSize, int shapeId,
            bool inGroup, List<Label> labels, int index) : base(FormToDrawOn, shapeLocation, shapeSize, shapeId, inGroup, labels, index)
        {
        }

        public BelowOrnament(Shape s, Composite c, string text) : base(s, c, text)
        {
            if (c == null)
            {
                ornamentLocation.Y = s.Y + (s.Height + 15);
                ornamentLocation.X = s.X + (s.Width / 2);
            }
            else
            {
                ornamentLocation.Y = c.Position.Y + (c.Size.Height + 15);
                ornamentLocation.X = c.Position.X + (c.Size.Width / 2);
            }
        }
    }
}
