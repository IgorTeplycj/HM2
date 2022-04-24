using HM2.GameSolve.Actions;
using HM2.GameSolve.Interfaces;
using HM2.GameSolve.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.GameSolve.Scopes
{
    /// <summary>
    ///Низкий уровень сложности (движение по прямой без расхода топлива)
    /// </summary>
    public class LowLevel : ICommand
    {
        ICommand macro;
        public LowLevel(IAction movable)
        {
            List<ICommand> macroColl = new List<ICommand>();
            macroColl.Add(new MoveCommand(movable));
            macro = new MacroCommand(macroColl);
        }

        public void Execute()
        {
            macro.Execute();
        }
    }
}
