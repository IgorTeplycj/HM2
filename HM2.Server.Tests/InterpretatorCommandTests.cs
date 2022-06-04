using EndPointMessage;
using HM2.EndPoint.Commands;
using HM2.Games;
using HM2.GameSolve.Interfaces;
using HM2.GameSolve.Structures;
using HM2.IoCs;
using HM2.MovableObject;
using HM2.Threads;
using HM2.Threads.Commands;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HM2.Server.Tests
{
    public class InterpretatorCommandTests
    {
        [SetUp]
        public void CreateGameObject()
        {
            Game game = new Game();
            //создаем три игры по 10 игровых объектов в каждой
            game.Create(3, 10);
        }
        [SetUp]
        public void CreateQueue()
        {
            //создание и регистрация очереди
            QueueThread queueCommand = new QueueThread();
            IoC<QueueThread>.Resolve("IoC.Registration", "Queue", queueCommand);
        }

        [Test]
        public void InterpretCommandMoveLineTest()
        {
            // Выбираем игровой объект под номером 3 из игры номер 1. Им и будем управлять в игре.
            UObject obj = IoC<UObject>.Resolve($"game 1 object 3");
            Vector newVector = new Vector();
            newVector.Shift = new Coordinats { X = 5.0, Y = 7.0 };

            Message message = new Message("1", "3", "Move line", JsonSerializer.Serialize<Vector>(newVector));

            StringBuilder sb = new StringBuilder();
            //Сериализация message
            new SerializeMessageCommands(message, sb).Execute();

            //Создание команды интерпретатора
            InterpretCommand interpretCommand = new InterpretCommand(sb.ToString());

            Assert.AreEqual(obj.CurrentVector.PositionNow.X, 0.0);
            Assert.AreEqual(obj.CurrentVector.PositionNow.Y, 0.0);

            interpretCommand.Execute();

            Assert.AreEqual(obj.CurrentVector.PositionNow.X, 5.0);
            Assert.AreEqual(obj.CurrentVector.PositionNow.Y, 7.0);
        }
        [Test]
        public void InterpretCommandRotateTest()
        {
            // Выбираем игровой объект под номером 3 из игры номер 1. Им и будем управлять в игре.
            UObject obj = IoC<UObject>.Resolve($"game 1 object 3");
            Vector newVect = new Vector();
            newVect.PositionNow = new Coordinats { X = 80.0, Y = 20.0 };
            newVect.AngularVelosity = 10;
            newVect.Direction = 15;
            newVect.DirectionNumber = 24;

            Message message = new Message("1", "3", "Rotate", JsonSerializer.Serialize<Vector>(newVect));

            StringBuilder sb = new StringBuilder();
            //Сериализация message
            new SerializeMessageCommands(message, sb).Execute();

            //Создание команды интерпретатора
            InterpretCommand interpretCommand = new InterpretCommand(sb.ToString());

            Assert.AreEqual(obj.CurrentVector.Direction, 0);

            interpretCommand.Execute();

            Assert.AreEqual(obj.CurrentVector.Direction, 1);
            Assert.AreEqual(obj.CurrentVector.PositionNow.X, 80.0);
            Assert.AreEqual(obj.CurrentVector.PositionNow.Y, 20.0);
        }
        [Test]
        public void InterpretCommandMoveLineRunInQueueTest()
        {
            // Выбираем игровой объект под номером 3 из игры номер 1.Им и будем управлять в игре.
           UObject obj = IoC<UObject>.Resolve($"game 1 object 3");
            Vector newVector = new Vector();
            newVector.Shift = new Coordinats { X = 5.0, Y = 7.0 };

            Message message = new Message("1", "3", "Move line", JsonSerializer.Serialize<Vector>(newVector));

            //В этом объекте будет наше сериализованное сообщение после выполнения команды SerializeMessageCommands
            StringBuilder serializedMessage = new StringBuilder();
            //Сериализация message
            new SerializeMessageCommands(message, serializedMessage).Execute();

            //Создание команды интерпретатора
            InterpretCommand interpretCommand = new InterpretCommand(serializedMessage.ToString());

            //проверяем что очередь не запущена
            Assert.IsFalse(IoC<QueueThread>.Resolve("Queue").TaskIsRun);
            //стартуем очередь
            IoC<QueueThread>.Resolve("Queue").PushCommand(new ControlCommand(IoC<QueueThread>.Resolve("Queue").Start));
            //проверка что очередь запустилась
            Assert.IsTrue(IoC<QueueThread>.Resolve("Queue").TaskIsRun);

            //проверка отсутствия изменений вектора объекта
            Assert.AreEqual(obj.CurrentVector.PositionNow.X, 0.0);
            Assert.AreEqual(obj.CurrentVector.PositionNow.Y, 0.0);
            //кладем команду интерпретатора в очерредь
            IoC<QueueThread>.Resolve("Queue").PushCommand(interpretCommand);
            //немножечко ждем
            Thread.Sleep(20);
            //проверяем выполнение команды интерпретатора в очереди
            Assert.AreEqual(obj.CurrentVector.PositionNow.X, 5.0);
            Assert.AreEqual(obj.CurrentVector.PositionNow.Y, 7.0);

            //завершаем очередь
            IoC<QueueThread>.Resolve("Queue").PushCommand(new ControlCommand(IoC<QueueThread>.Resolve("Queue").HardStop));

            //проверяем что очередь остановилась
            Assert.IsFalse(IoC<QueueThread>.Resolve("Queue").TaskIsRun);
        }
    }
}
