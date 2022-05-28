using EndPointMessage;
using HM2.EndPoint;
using HM2.Games;
using HM2.GameSolve.Actions;
using HM2.GameSolve.Interfaces;
using HM2.GameSolve.Structures;
using HM2.IoCs;
using HM2.MovableObject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HM2.Server.Tests
{
    public class EndPointTests
    {
        const string ipAddr = "127.0.0.1";
        const int port = 8080;
        
        //[SetUp]
        public void StartServer()
        {
            //запуск сервера
            Task Server = new Task(() =>
            {
                EndPointNetServer endPointServer = new EndPointNetServer(ipAddr, port);
                endPointServer.Run();
            });
            Server.Start();
        }

        [SetUp]
        public void CreateGameObject()
        {
            Game game = new Game();
            //создаем три игры по 10 игровых объектов в каждой
            game.Create(3, 10);
        }

        [Test]
        public void MoveObjectByClient()
        {
            //Выбираем игровой объект под номером 3 из игры номер 1. Им и будем управлять в игре.
            UObject obj = IoC<UObject>.Resolve($"game 1 object 3");

            Vector vector = new Vector();
            vector.Shift = new Coordinats { X = 5.0, Y = 7.0 };
            obj.CurrentVector = vector;

            //получение команды движения по прямой
            var moveCommand = IoC<Func<UObject, ICommand>>.Resolve("move line").Invoke(obj);

            Assert.AreEqual(obj.CurrentVector.PositionNow.X, 0.0);
            Assert.AreEqual(obj.CurrentVector.PositionNow.Y, 0.0);

            moveCommand.Execute();

            Assert.AreEqual(obj.CurrentVector.PositionNow.X, 5.0);
            Assert.AreEqual(obj.CurrentVector.PositionNow.Y, 7.0);
        }

        [Test]
        public void MoveObjectByServer()
        {
            //Выбираем игровой объект под номером 3 из игры номер 1. Им и будем управлять в игре.
            UObject obj = IoC<UObject>.Resolve($"game 1 object 3");
            Vector vector = new Vector();
            vector.Shift = new Coordinats { X = 5.0, Y = 7.0 };

            Message message = new Message("1", "3", "Движение по прямой", JsonSerializer.Serialize<Vector>(vector));
        }


        void CreateClientAndSendMessage(string msg)
        {
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ipAddr), port);
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            var data = Encoding.UTF8.GetBytes(msg);

            tcpSocket.Connect(tcpEndPoint);
            tcpSocket.Send(data);

            var buffer = new byte[1024];
            var size = 0;
            var answer = new StringBuilder();

            do
            {
                size = tcpSocket.Receive(buffer);
                answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
            }
            while (tcpSocket.Available > 0);

            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();
        }
    }
}