using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace DesignPatternsStep1
{
    class EllipseShape : Shape
    {
        public EllipseShape(Control FormToDrawOn, Point Location, Size Size, int shapeId, bool inGroup) : base(FormToDrawOn, Location, Size, shapeId, inGroup)
        { }

        public override void DrawShape(int x, int y, int Width, int Height, Pen pen)
        {
            formGraphics = m_frmRef.CreateGraphics();

            base.DrawShape(x,y, Width, Height, pen);
            
            formGraphics.DrawEllipse(formPen, shape);
            formPen.Dispose();
            formGraphics.Dispose();
        }
    }
}
