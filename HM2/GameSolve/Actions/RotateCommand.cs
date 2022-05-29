using HM2.GameSolve.Interfaces;
using HM2.GameSolve.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.GameSolve.Actions
{
    public class RotateCommand : ICommand
    {
        IAction rot;
        public RotateCommand(IAction _rot)
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
