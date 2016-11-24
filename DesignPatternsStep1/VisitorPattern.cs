using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsStep1
{
    abstract class VisitorPattern
    {

    }

    class Resize : VisitorPattern {
    }

    class Move : VisitorPattern {
    }

    class FileIo : VisitorPattern{
    }
}
