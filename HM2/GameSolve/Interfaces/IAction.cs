using HM2.GameSolve.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.GameSolve.Interfaces
{
    public interface IAction
    {
        Vector CurrentVector { get; set; }
        void Set(Vector newV);

        void Finish();
    }
}
