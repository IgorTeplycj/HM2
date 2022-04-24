using HM2.GameSolve.Actions;
using HM2.GameSolve.Interfaces;
using HM2.GameSolve.Structures;
using HM2.IoCs;
using HM2.Tests.Mock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.Tests.IoCTests
{
    class AtomicCommandsTests
    {
        /// <summary>
        /// Тест регистрации команды
        /// </summary>
        [Test]
        public void FirstRegistrationCommand()
        {
            Vector vectBef = new Vector();
            vectBef.PositionNow = new Coordinats { X = 12.0, Y = 5.0 };
            vectBef.Shift = new Coordinats { X = -7.0, Y = 3.0 };
            IAction movable = new Moving(vectBef);

            Assert.AreEqual(movable.CurrentVector.PositionNow.X, 12.0);
            Assert.AreEqual(movable.CurrentVector.PositionNow.Y, 5.0);

            MoveCommand move = new MoveCommand(movable);
            //регистрация команды
            ICommand commandMove = IoC<ICommand>.Resolve("IoC.Registration", "command.move", move);

            //проверка отстутсвия измкнений после регистрации
            Assert.AreEqual(movable.CurrentVector.PositionNow.X, 12.0);
            Assert.AreEqual(movable.CurrentVector.PositionNow.Y, 5.0);

            commandMove.Execute();

            Assert.AreEqual(movable.CurrentVector.PositionNow.X, 5.0);
            Assert.AreEqual(movable.CurrentVector.PositionNow.Y, 8.0);

            //повторный вызов команды
            IoC<ICommand>.Resolve("command.move").Execute();

            //проверка изменений после вызова
            Assert.AreEqual(movable.CurrentVector.PositionNow.X, -2.0);
            Assert.AreEqual(movable.CurrentVector.PositionNow.Y, 11.0);
        }
        [Test]
        public void TwoRegistrationCommand()
        {
            Vector vectBef = new Vector();
            vectBef.PositionNow = new Coordinats { X = 12.0, Y = 5.0 };
            vectBef.Shift = new Coordinats { X = -7.0, Y = 3.0 };
            IAction movable = new Moving(vectBef);

            Assert.AreEqual(movable.CurrentVector.PositionNow.X, 12.0);
            Assert.AreEqual(movable.CurrentVector.PositionNow.Y, 5.0);

            MoveCommand move = new MoveCommand(movable);
            //регистрация команды и вызов
            IoC<ICommand>.Resolve("IoC.Registration", "command.move", move).Execute();

            Assert.AreEqual(movable.CurrentVector.PositionNow.X, 5.0);
            Assert.AreEqual(movable.CurrentVector.PositionNow.Y, 8.0);

            //Создание новой команды с другими параметрами перемещения
            vectBef.PositionNow = new Coordinats { X = movable.CurrentVector.PositionNow.X, Y = movable.CurrentVector.PositionNow.Y };
            vectBef.Shift = new Coordinats { X = 5.0, Y = 7.0 };
            movable = new Moving(vectBef);
            move = new MoveCommand(movable);
            //регистрация команды с новыми параметрами
            var command2 = IoC<ICommand>.Resolve("IoC.Registration", "command.move", move);

            //проверка того, что параметры не изменились при регистрации
            Assert.AreEqual(movable.CurrentVector.PositionNow.X, 5.0);
            Assert.AreEqual(movable.CurrentVector.PositionNow.Y, 8.0);

            command2.Execute();

            Assert.AreEqual(movable.CurrentVector.PositionNow.X, 10.0);
            Assert.AreEqual(movable.CurrentVector.PositionNow.Y, 15.0);
        }
        /// <summary>
        ////Тест контейнера при вызове ранее незарегестрированной команды
        /// </summary>
        [Test]
        public void ExecutedNonRegistrationCommand()
        {
            try
            {
                IoC<ICommand>.Resolve("command.move").Execute();
            }
            catch (Exception ex)
            {
                if (ex.GetType().GetHashCode() != new IoCException().GetType().GetHashCode())
                    Assert.Fail();
            }
        }
    }
}
