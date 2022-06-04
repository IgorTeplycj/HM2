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
    ////Высокий уровень сложности (с расходом топлива при движении по прямой)
    /// </summary>
    public class HighLevel : ICommand
    {
        ICommand macro;
        public HighLevel(IAction movable)
        {
            List<ICommand> macroColl = new List<ICommand>();
            macroColl.Add(new CheckFuelCommand(movable));
            macroColl.Add(new MoveCommand(movable));
            macroColl.Add(new BurnFuelCommand(movable));
            macro = new MacroCommand(macroColl);
        }

        public void Execute()
        {
            macro.Execute();
        }
    }
}
