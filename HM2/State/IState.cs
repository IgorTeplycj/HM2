using HM2.GameSolve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.State
{
    public interface IState : ICommand
    {
        StateEnum Handle();

    }
}
