using EndPointMessage;
using HM2.GameSolve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HM2.EndPoint.Commands
{
    public class DeserializeMessageCommand : ICommand
    {
        Message _mes;
        StringBuilder _serialzedMessage;
        public DeserializeMessageCommand(Message mes, StringBuilder serialzedMessage)
        {
           _mes = mes;
            _serialzedMessage = serialzedMessage;
        }
        public void Execute()
        {
            _mes.Deserialized(_serialzedMessage.ToString());
        }
    }
}
