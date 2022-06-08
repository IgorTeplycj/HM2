using HM2.GameSolve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.GameSolve.Actions
{
    /// <summary>
    /// Изменяет вектор скорости
    /// </summary>
    public class ChangeVelocityCommand : ICommand
    {
        IAction _vel;
        public ChangeVelocityCommand(IAction  vel)
        {
            _vel = vel;
        }


        public void Execute()
        {
            HM2.GameSolve.Structures.Vector vector = _vel.CurrentVector;
            vector.ModifVelosityVect();
            _vel.Set(vector);
        }
    }
}
