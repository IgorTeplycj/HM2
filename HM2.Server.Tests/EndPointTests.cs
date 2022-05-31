using EndPointMessage;
using HM2.EndPoint;
using HM2.EndPoint.Commands;
using HM2.Games;
using HM2.GameSolve.Actions;
using HM2.GameSolve.Interfaces;
using HM2.GameSolve.Structures;
using HM2.IoCs;
using HM2.MovableObject;
using HM2.Threads;
using HM2.Threads.Commands;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HM2.Server.Tests
{
    public class EndPointTests
    {
        const string ipAddr = "127.0.0.1";
        const int port = 8080;

      //  [SetUp]
        public void StartServer()
        {
            EndPointNetServer endPointServer = new EndPointNetServer(ipAddr, port);
            //регистрация сервера в IoC
            IoC<EndPointNetServer>.Resolve("IoC.Registration", "Server", endPointServer);
            //старт сервера
            IoC<EndPointNetServer>.Resolve("Server").Run();
        }
       // [SetUp]
        public void CreateGameObject()
        {
            Game game = new Game();
            //создаем три игры по 10 игровых объектов в каждой
            game.Create(3, 10);
        }
      //  [SetUp]
        public void CreateQueueAndRun()
        {
            //создание и регистрация очереди
            IoC<QueueCommand>.Resolve("IoC.Registration", "Queue", new QueueCommand());
            //стартуем очередь
            IoC<QueueCommand>.Resolve("Queue").PushCommand(new ControlCommand(IoC<QueueCommand>.Resolve("Queue").Start));
        }
     //   [TearDown]
        public void Down()
        {
            //завершаем очередь
            IoC<QueueCommand>.Resolve("Queue").PushCommand(new ControlCommand(IoC<QueueCommand>.Resolve("Queue").HardStop));
            //Завершаем сервер
            IoC<EndPointNetServer>.Resolve("Server").Close();
        }


       // [Test]
        public void MoveObjectByServer()
        {
            //Выбираем игровой объект под номером 3 из игры номер 1. Им и будем управлять в игре.
            UObject obj = IoC<UObject>.Resolve($"game 1 object 3");
            Vector vector = new Vector();
            vector.Shift = new Coordinats { X = 5.0, Y = 7.0 };

            //Формируем сообщение для сервера
            Message message = new Message("1", "3", "Move line", JsonSerializer.Serialize<Vector>(vector));
            //Сериализуем сообщение в строку
            StringBuilder serializedMessage = new StringBuilder();
            new SerializeMessageCommands(message, serializedMessage).Execute();

            //Проверяем, что объект не двигался
            Assert.AreEqual(obj.CurrentVector.PositionNow.X, 0.0);
            Assert.AreEqual(obj.CurrentVector.PositionNow.Y, 0.0);

            //отправляем сообщение серверу 
            CreateClientAndSendMessage(serializedMessage.ToString());
            //Немножечко ждем
            Thread.Sleep(20);

            //Проверяем что объект изменил свое положение
            Assert.AreEqual(obj.CurrentVector.PositionNow.X, 5.0);
            Assert.AreEqual(obj.CurrentVector.PositionNow.Y, 7.0);
        }


        void CreateClientAndSendMessage(string msg)
        {
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ipAddr), port);
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            var data = Encoding.UTF8.GetBytes(msg);

            tcpSocket.Connect(tcpEndPoint);
            tcpSocket.Send(data);

            var buffer = new byte[2048];
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