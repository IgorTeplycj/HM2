using HM2.GameSolve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.GameSolve.Actions
{
    public class CheckFuelCommand : ICommand
    {
        IAction fuel;
        public CheckFuelCommand(IAction _fuel)
        {
            fuel = _fuel;
        }


        public void Execute()
        {
            HM2.GameSolve.Structures.Vector vector = fuel.CurrentVector;

            vector.ModifVolumeFuel();

            if (vector.VolumeFuel < 0)
                throw new Exceptions.CommandException();

        }
    }
}
