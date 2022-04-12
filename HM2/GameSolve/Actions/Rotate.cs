using HM2.GameSolve.Interfaces;
using HM2.GameSolve.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.GameSolve.Actions
{
    public class Rotate
    {
        Movable rot;
        public Rotate(Movable _rot)
        {
            rot = _rot;
        }


        public void Execute()
        {
            Vector vector = new Vector();
            vector = rot.CurrentVector;
            vector.Direction = rot.CurrentVector.NextDirection;
            rot.Set(vector);
        }
    }
}
