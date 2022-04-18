using HM2.GameSolve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.GameSolve.Actions
{
    public class MacroCommand : ICommand
    {
        List<ICommand> macroColl;
        public MacroCommand(List<ICommand> _macroColl)
        {
            macroColl = _macroColl;
        }

        public void Execute()
        {
            foreach (ICommand item in macroColl)
            {
                try
                {
                    item.Execute();
                }
                catch
                {
                    throw new Exceptions.CommandException();
                }
            }
        }
    }
}
