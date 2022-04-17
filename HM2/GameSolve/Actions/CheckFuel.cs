using HM2.GameSolve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.GameSolve.Actions
{
    public class CheckFuel
    {
        Movable fuel;
        public CheckFuel(Movable _fuel)
        {
            fuel = _fuel;
        }


        public void Execute()
        {
            HM2.GameSolve.Structures.Vector vector = fuel.CurrentVector;

            if (vector.VolumeFuel <= 0)
                throw new Exceptions.CommandException();

            vector.VolumeFuel--;
            fuel.Set(vector);
        }
    }
}
