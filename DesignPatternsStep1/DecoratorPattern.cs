﻿using System;
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
        public Composite ornamentGroup;
        public string ornamentText;

        public DecoratorPattern(Control FormToDrawOn, Point shapeLocation, Size shapeSize, int shapeId,
            bool inGroup, List<Label> labels) : base(FormToDrawOn, shapeLocation, shapeSize, shapeId, inGroup, labels)
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
        public LeftOrnament(Control FormToDrawOn, Point shapeLocation, Size shapeSize, int shapeId,
            bool inGroup, List<Label> labels) : base(FormToDrawOn, shapeLocation, shapeSize, shapeId, inGroup, labels)
        {
        }

        public LeftOrnament(Shape s, Composite c, string text) : base(s, c, text)
        {
            Font stringfont = new Font("Microsoft Sans Serif", 8);
            Graphics g = Form1.ActiveForm.CreateGraphics();
            SizeF stringsSize = g.MeasureString(text, stringfont);
            if (s == null)
            {
                stringsSize = g.MeasureString(text, stringfont);
                ornamentLocation.X = c.Position.X - ((int)stringsSize.Width + 25);
                ornamentLocation.Y = c.Position.Y + (c.Size.Height / 2);
            }
            else
            {
                ornamentLocation.X = s.X - ((int)stringsSize.Width + 25);
                ornamentLocation.Y = s.Y + (s.Height / 2);
            }
        }

    }
    class RightOrnament : DecoratorPattern
    {
        public RightOrnament(Control FormToDrawOn, Point shapeLocation, Size shapeSize, int shapeId,
            bool inGroup, List<Label> labels) : base(FormToDrawOn, shapeLocation, shapeSize, shapeId, inGroup, labels)
        {
        }

        public RightOrnament(Shape s, Composite c, string text) : base(s, c, text)
        {
            if (s == null)
            {
                ornamentLocation.X = c.Position.X + c.Size.Width + 10;
                ornamentLocation.Y = c.Position.Y + (c.Size.Height / 2);
            }
            else
            {
                ornamentLocation.X = s.X + s.Width + 10;
                ornamentLocation.Y = s.Y + (s.Height / 2);
            }
        }
    }
    class AboveOrnament : DecoratorPattern
    {
        public AboveOrnament(Control FormToDrawOn, Point shapeLocation, Size shapeSize, int shapeId,
            bool inGroup, List<Label> labels) : base(FormToDrawOn, shapeLocation, shapeSize, shapeId, inGroup, labels)
        {
        }

        public AboveOrnament(Shape s, Composite c, string text) : base(s, c, text)
        {
            if (s == null)
            {
                ornamentLocation.Y = c.Position.Y - 15;
                ornamentLocation.X = c.Position.X + (c.Size.Width / 2);
            }
            else
            {
                ornamentLocation.Y = s.Y - 15;
                ornamentLocation.X = s.X + (s.Width / 2);
            }
        }
    }
    class BelowOrnament : DecoratorPattern
    {
        public BelowOrnament(Control FormToDrawOn, Point shapeLocation, Size shapeSize, int shapeId,
            bool inGroup, List<Label> labels) : base(FormToDrawOn, shapeLocation, shapeSize, shapeId, inGroup, labels)
        {
        }

        public BelowOrnament(Shape s, Composite c, string text) : base(s, c, text)
        {
            if (s == null)
            {
                ornamentLocation.Y = c.Position.Y + (c.Size.Height + 15);
                ornamentLocation.X = c.Position.X + (c.Size.Width / 2);
            }
            else
            {
                ornamentLocation.Y = s.Y + (s.Height + 15);
                ornamentLocation.X = s.X + (s.Width / 2);
            }
        }
    }
}
