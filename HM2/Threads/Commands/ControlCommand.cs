using HM2.GameSolve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.Threads.Commands
{
    public class ControlCommand : ICommand
    {
        Action _run;
        public ControlCommand(Action run)
        {
            _run = run;
        }

        public void Execute()
        {;
            _run.Invoke();
        }
    }
}
