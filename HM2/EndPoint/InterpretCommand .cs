using HM2.GameSolve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.EndPoint
{
    public class InterpretCommand : ICommand
    {
        EndPointMessage.Message message;
        public InterpretCommand(string msg)
        {
            //message = new EndPointMessage.Message(msg);
        }
        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
