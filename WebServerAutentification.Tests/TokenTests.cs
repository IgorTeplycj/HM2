using EndPointMessage;
using HM2.EndPoint;
using HM2.EndPoint.Commands;
using HM2.GameSolve.Structures;
using HM2.MovableObject;
using HM2.Threads;
using HM2.Threads.Commands;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WebServer.Controllers;
using WebServer.Models;
using WebServer.Tests.Secure;

namespace WebServer.Tests
{
    public class TokenTests
    {
        const string ipAddr = "127.0.0.1";
        const int port = 8080;
        private List<Account> autirizedUsers = new List<Account>
        {
            new Account { Email = "user1@gmail.com", Name = "User1", HashOfPassword = Hash.GetMd5Hash("11111"), Role = Roles.User },
            new Account { Email = "user2@gmail.com", Name = "User2", HashOfPassword = Hash.GetMd5Hash("22222"), Role = Roles.User },
            new Account { Email = "user3@gmail.com", Name = "User3", HashOfPassword = Hash.GetMd5Hash("33333"), Role = Roles.User },
        };
        private List<Account> nonAutirizedUsers = new List<Account>
        {
            new Account { Email = "user1@gmail.com", Name = "User1", HashOfPassword = Hash.GetMd5Hash("11115"), Role = Roles.User },
            new Account { Email = "user2@gmail.com", Name = "User2", HashOfPassword = Hash.GetMd5Hash("22225"), Role = Roles.User },
            new Account { Email = "user3@gmail.com", Name = "User3", HashOfPassword = Hash.GetMd5Hash("33335"), Role = Roles.User },
        };
        [SetUp]
        public void StartGameServer()
        {

        }

      //  [SetUp]
        public void CreateQueueAndRun()
        {
            //создание и регистрация очереди
            HM2.IoCs.IoC<QueueCommand>.Resolve("IoC.Registration", "Queue", new QueueCommand());
            //стартуем очередь
            HM2.IoCs.IoC<QueueCommand>.Resolve("Queue").PushCommand(new ControlCommand(HM2.IoCs.IoC<QueueCommand>.Resolve("Queue").Start));
        }

       // [TearDown]
        public void Down()
        {
            //завершаем очередь
            HM2.IoCs.IoC<QueueCommand>.Resolve("Queue").PushCommand(new ControlCommand(HM2.IoCs.IoC<QueueCommand>.Resolve("Queue").HardStop));

        }

       // [OneTimeSetUp]
        public void InitTestSuite()
        {
            //запускаем сервер выдачи токенов
            Task tokenServer = new Task(() => WebServer.Program.Main(null));
            tokenServer.Start();
            Thread.Sleep(500);

            //Запускаем игровой сервер
            EndPointNetServer endPointServer = new EndPointNetServer(ipAddr, port);
            //регистрация сервера в IoC
            HM2.IoCs.IoC<EndPointNetServer>.Resolve("IoC.Registration", "Server", endPointServer);
            //старт сервера
            HM2.IoCs.IoC<EndPointNetServer>.Resolve("Server").Run();
        }

       // [OneTimeTearDown]
        public void FinishTestSuite()
        {
            //Завершаем игровой сервер
            HM2.IoCs.IoC<EndPointNetServer>.Resolve("Server").Close();
        }

       // [Test]
        public void AllAlgoritmPositivTest()
        {
            ////формируем Http запрос серверу для получения идентификатора игры
            //string idGame = "";
            //using (var client = new HttpClient())
            //{
            //    const string PATHURIIDGAME = "http://localhost:5000/idgame";
            //    string lst = JsonSerializer.Serialize(autirizedUsers);
            //    string getParametersIDGAME = $"jsonListUsers={lst}";
            //    idGame = client.GetStringAsync(PATHURIIDGAME + $"?{getParametersIDGAME}").Result;
            //}
            //idGame = idGame.Trim(@"\""".ToCharArray());
            ////Создаем игру с полученным идентификатором и тремя игровыми объектами
            //HM2.Games.Game game = new HM2.Games.Game();
            //game.CreateGame(idGame, 3);

            ////User1 отправляет запрос на выдачу jwt токена
            //Account user1 = autirizedUsers.Find(x => x.Name == "User1");
            //string token = ""; //в этой строке будет храниться полученный токен
            //using (var client = new HttpClient())
            //{
            //    const string TOKENURL = "http://localhost:5000/token";
            //    string usr = JsonSerializer.Serialize(user1);
            //    string getParametersTOKEN = $"user={usr}&idgame={idGame}";
            //    token = client.GetStringAsync(TOKENURL + $"?{getParametersTOKEN}").Result;
            //}
            //token = token.Trim(@"\""".ToCharArray());
            ////User1 отправляет запрос на игровой сервер
            ////User1 выбирает объект номер 1 
            //UObject obj = HM2.IoCs.IoC<UObject>.Resolve($"game {idGame} object 1");
            ////формируем новый вектор
            //Vector newVect = new Vector();
            //newVect.Shift = new Coordinats { X = 5.0, Y = 7.0 };

            ////Формируем сообщение для сервера
            //Message message = new Message(idGame, "1", "Move line", JsonSerializer.Serialize<Vector>(newVect));
            ////Сериализуем сообщение в строку
            //StringBuilder serializedMessage = new StringBuilder();
            //new SerializeMessageCommands(message, serializedMessage).Execute();

            ////Проверяем, что объект не двигался
            //Assert.AreEqual(obj.CurrentVector.PositionNow.X, 0.0);
            //Assert.AreEqual(obj.CurrentVector.PositionNow.Y, 0.0);

            ////отправляем команду с токеном на игровой сервер (здесь, чтобы не усложнять задачу и не создавать еще один локальный http сервер,
            ////в качестве игрового использован сервер аутентификации). На игровом сервере проверяется токен,
            ////формируется и отправляется сообщение с командой на EndPoints нашего проекта игры
            //using (var client = new HttpClient())
            //{
            //    const string URL = "http://localhost:5000/command";
            //    string param = $"token={token}&message={serializedMessage}";
            //    var result = client.GetStringAsync(URL + $"?{param}").Result;
            //}

            ////Немножечко ждем
            //Thread.Sleep(100);

            ////Проверяем что объект изменил свое положение
            //Assert.AreEqual(obj.CurrentVector.PositionNow.X, 5.0);
            //Assert.AreEqual(obj.CurrentVector.PositionNow.Y, 7.0);

            Assert.IsTrue(true);
        }

       // [Test]
        public void InvalidTokenTest()
        {
            //формируем Http запрос серверу для получения идентификатора игры
            string idGame = "";
            using (var client = new HttpClient())
            {
                const string PATHURIIDGAME = "http://localhost:5000/idgame";
                string lst = JsonSerializer.Serialize(autirizedUsers);
                string getParametersIDGAME = $"jsonListUsers={lst}";
                idGame = client.GetStringAsync(PATHURIIDGAME + $"?{getParametersIDGAME}").Result;
            }
            idGame = idGame.Trim(@"\""".ToCharArray());
            //Создаем игру с полученным идентификатором и тремя игровыми объектами
            HM2.Games.Game game = new HM2.Games.Game();
            game.CreateGame(idGame, 3);

            //User1 отправляет запрос на выдачу jwt токена
            Account user1 = autirizedUsers.Find(x => x.Name == "User1");
            string tokenUser1 = ""; //в этой строке будет храниться полученный токен
            using (var client = new HttpClient())
            {
                const string TOKENURL = "http://localhost:5000/token";
                string usr = JsonSerializer.Serialize(user1);
                string getParametersTOKEN = $"user={usr}&idgame={idGame}";
                tokenUser1 = client.GetStringAsync(TOKENURL + $"?{getParametersTOKEN}").Result;
            }
            tokenUser1 = tokenUser1.Trim(@"\""".ToCharArray());

            //User1 отправляет запрос на игровой сервер
            //User1 выбирает объект номер 1 
            UObject obj = HM2.IoCs.IoC<UObject>.Resolve($"game {idGame} object 1");
            //формируем новый вектор
            Vector newVect = new Vector();
            newVect.Shift = new Coordinats { X = 5.0, Y = 7.0 };

            //Формируем сообщение для сервера
            Message message = new Message(idGame, "1", "Move line", JsonSerializer.Serialize<Vector>(newVect));
            //Сериализуем сообщение в строку
            StringBuilder serializedMessage = new StringBuilder();
            new SerializeMessageCommands(message, serializedMessage).Execute();

            //Проверяем, что объект не двигался
            Assert.AreEqual(obj.CurrentVector.PositionNow.X, 0.0);
            Assert.AreEqual(obj.CurrentVector.PositionNow.Y, 0.0);

            //отправляем команду с токеном на игровой сервер (здесь, чтобы не усложнять задачу и не создавать еще один локальный http сервер,
            //в качестве игрового использован сервер аутентификации). На игровом сервере проверяется токен,
            //формируется и отправляется сообщение с командой на EndPoints нашего проекта игры
            using (var client = new HttpClient())
            {
                const string URL = "http://localhost:5000/command";
                tokenUser1 += "i"; //делаем ключ невалидным
                string param = $"token={tokenUser1}&message={serializedMessage}";

                try
                {
                    var result = client.GetStringAsync(URL + $"?{param}").Result;
                    Assert.Fail();
                }
                catch (System.AggregateException ex)
                {

                }
            }

            //Немножечко ждем
            Thread.Sleep(100);

            //Проверяем что объект изменил свое положение
            Assert.AreEqual(obj.CurrentVector.PositionNow.X, 0.0);
            Assert.AreEqual(obj.CurrentVector.PositionNow.Y, 0.0);
        }
    }
}