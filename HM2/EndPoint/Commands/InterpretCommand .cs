using HM2.GameSolve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.EndPoint.Commands
{
    public class InterpretCommand : ICommand
    {
        EndPointMessage.Message message;
        public InterpretCommand(string msg)
        {
            
        }
        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
