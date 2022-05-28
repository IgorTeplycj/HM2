using HM2.Games;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.Server.Tests
{
    public class MessageTests
    {
        [SetUp]
        public void CreateGameObject()
        {
            Game game = new Game();
            //создаем три игры по 10 игровых объектов в каждой
            game.Create(3, 10);
        }
        [Test]
        public void MessageTest()
        {

        }
    }
}
