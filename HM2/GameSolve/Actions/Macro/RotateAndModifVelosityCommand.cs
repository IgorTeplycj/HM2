using HM2.GameSolve.Interfaces;
using HM2.GameSolve.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.GameSolve.Actions.Macro
{
    public class RotateAndModifVelosityCommand : ICommand
    {
        IAction _action;
        public RotateAndModifVelosityCommand(IAction action)
        {
            _action = action;
        }

        public void Execute()
        {
            List<ICommand> commands = new List<ICommand>();

            Vector vector = _action.CurrentVector;
            vector.VelosityVectModifer = new Structures.VelosityVect { Angular = 90, Velosity = 1 };
            _action.CurrentVector = vector;

            commands.Add(new RotateCommand(_action));
            if (_action.CurrentVector.VelosityVectNow.Velosity > 0 || _action.CurrentVector.VelosityVectNow.Velosity < 0)
                commands.Add(new ChangeVelocityCommand(_action));

            MacroCommand macroCommand = new MacroCommand(commands);
            macroCommand.Execute();
        }
    }
}
