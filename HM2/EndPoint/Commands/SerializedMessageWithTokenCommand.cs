using HM2.GameSolve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.EndPoint.Commands
{
    public class SerializedMessageWithTokenCommand : ICommand
    {
        StringBuilder serialzedMessage;
        EndPointMessage.Message mes;
        public SerializedMessageWithTokenCommand(EndPointMessage.Message mes, string token, StringBuilder serialzedMessage)
        {
            this.mes = mes;
            this.serialzedMessage = serialzedMessage;
        }
        public void Execute()
        {
            serialzedMessage.Append(mes.Serialized());
        }
    }
}
