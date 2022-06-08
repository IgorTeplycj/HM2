using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.Model.Fields.Interfaces
{
    public interface ISquare
    {
        public double StartX { get; }
        public double EndX { get; }
        public double StartY { get; }
        public double EndY { get; }
    }
}
