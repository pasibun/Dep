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

        public DecoratorPattern(Control FormToDrawOn, Point shapeLocation, Size shapeSize, int shapeId, 
            bool inGroup, List<Label> labels) : base(FormToDrawOn, shapeLocation, shapeSize, shapeId, inGroup,labels)
        {  }

        public DecoratorPattern(Shape s, string text) : base (s, text)
        {
            this.ornamentshape = s;
            this.ornamentText = text;
        }       
    }

    class LeftOrnament : DecoratorPattern
    {
        public LeftOrnament(Control FormToDrawOn, Point shapeLocation, Size shapeSize, int shapeId, 
            bool inGroup, List<Label> labels) : base(FormToDrawOn, shapeLocation, shapeSize, shapeId, inGroup, labels)
        {        
        }

        public LeftOrnament(Shape s, string text) : base(s, text)
        {
            Font stringfont = new Font("Microsoft Sans Serif", 8);
            Graphics g = Form1.ActiveForm.CreateGraphics();
            SizeF stringsSize = g.MeasureString(s.OrnamentText, stringfont);

            ornamentLocation.X = s.X - ((int)stringsSize.Width + 25);
            ornamentLocation.Y = s.Y + (s.Height / 2);
        }

    }
    class RightOrnament : DecoratorPattern
    {
        public RightOrnament(Control FormToDrawOn, Point shapeLocation, Size shapeSize, int shapeId, 
            bool inGroup, List<Label> labels) : base(FormToDrawOn, shapeLocation, shapeSize, shapeId, inGroup, labels)
        {
        }

        public RightOrnament(Shape s, string text) : base(s, text)
        {
            ornamentLocation.X = s.X + s.Width + 10;
            ornamentLocation.Y = s.Y + (s.Height / 2);
        }
    }
    class AboveOrnament : DecoratorPattern
    {
        public AboveOrnament(Control FormToDrawOn, Point shapeLocation, Size shapeSize, int shapeId, 
            bool inGroup, List<Label> labels) : base(FormToDrawOn, shapeLocation, shapeSize, shapeId, inGroup, labels)
        {            
        }

        public AboveOrnament(Shape s, string text) : base(s, text)
        {
            ornamentLocation.Y = s.Y - 15;
            ornamentLocation.X = s.X + (s.Width / 2);
        }
    }
    class BelowOrnament : DecoratorPattern
    {
        public BelowOrnament(Control FormToDrawOn, Point shapeLocation, Size shapeSize, int shapeId, 
            bool inGroup, List<Label> labels) : base(FormToDrawOn, shapeLocation, shapeSize, shapeId, inGroup, labels)
        {
        }
        
        public BelowOrnament(Shape s, string text) : base(s, text)
        {
            ornamentLocation.Y = s.Y + (s.Height + 15);
            ornamentLocation.X = s.X + (s.Width /2);
        }
    }
}
