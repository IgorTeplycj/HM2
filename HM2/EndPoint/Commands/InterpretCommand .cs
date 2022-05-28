using EndPointMessage;
using HM2.GameSolve.Interfaces;
using HM2.GameSolve.Structures;
using HM2.IoCs;
using HM2.MovableObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HM2.EndPoint.Commands
{
    public class InterpretCommand : ICommand
    {
       Message message;
        DeserializeMessageCommand deserializeMessageCommand;
        public InterpretCommand(string msg)
        {
            message = new Message(null, null, null, null);
            deserializeMessageCommand = new DeserializeMessageCommand(message, new StringBuilder().Append(msg));
        }
        public void Execute()
        {
            deserializeMessageCommand.Execute();
            UObject obj = IoC<UObject>.Resolve($"game {message.IdGame} object {message.IdGameObject}");
            obj.CurrentVector = JsonSerializer.Deserialize<Vector>(message.Args);
            IoC<Func<UObject, ICommand>>.Resolve(message.IdCommand).Invoke(obj).Execute();
        }
    }
}
