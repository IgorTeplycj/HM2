using EndPointMessage;
using HM2.EndPoint;
using HM2.EndPoint.Commands;
using HM2.GameSolve.Interfaces;
using HM2.GameSolve.Structures;
using HM2.IoCs;
using HM2.MovableObject;
using HM2.Threads;
using HM2.Threads.Commands;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HM2
{
    class Program
    {
        static void Main(string[] args)
        {
            Message message = new Message("Game12345", "GameObj12345", "124578", "GameArgs");

            StringBuilder s = new StringBuilder();

            SerializeMessageCommands serializeMessageCommands = new SerializeMessageCommands(message, s);
            serializeMessageCommands.Execute();

            Message message2 = new Message(null, null, null, null);
            DeserializeMessageCommand deserializeMessageCommand = new DeserializeMessageCommand(message2, s);
            deserializeMessageCommand.Execute();

            Console.WriteLine(s);

        }
    }
}
