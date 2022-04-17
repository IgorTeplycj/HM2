using HM2.GameSolve.Interfaces;
using HM2.GameSolve.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.Tests.Mock
{
    public class MovableMock : Movable
    {
        public MovableMock(Vector v)
        {
            CurrentVector = v;
        }
        public Vector CurrentVector { get; set; }
        public void Set(Vector newV)
        {
            CurrentVector = newV;
        }
    }
}
