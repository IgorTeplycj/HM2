using EndPointMessage;
using HM2.EndPoint.Commands;
using HM2.Games;
using HM2.GameSolve.Interfaces;
using HM2.GameSolve.Structures;
using HM2.IoCs;
using HM2.MovableObject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
    }
}
