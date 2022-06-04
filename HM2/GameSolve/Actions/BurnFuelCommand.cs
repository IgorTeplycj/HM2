using HM2.GameSolve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.GameSolve.Actions
{
    public class BurnFuelCommand : ICommand
    {
        IAction fuel;
        public BurnFuelCommand(IAction _fuel)
        {
            fuel = _fuel;
        }

        public void Execute()
        {
            HM2.GameSolve.Structures.Vector vector = fuel.CurrentVector;
            vector.ModifVolumeFuel();
            fuel.Set(vector);
        }
    }
}
