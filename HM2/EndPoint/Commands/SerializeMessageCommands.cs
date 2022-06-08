using HM2.GameSolve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HM2.EndPoint.Commands
{
    public class SerializeMessageCommands : ICommand
    {
        StringBuilder serialzedMessage;
        EndPointMessage.Message mes;
        public SerializeMessageCommands(EndPointMessage.Message mes, StringBuilder serialzedMessage)
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
