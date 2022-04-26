using HM2.GameSolve.Interfaces;
using HM2.GameSolve.Scopes;
using HM2.GameSolve.Structures;
using HM2.IoCs;
using HM2.Tests.Mock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HM2.Tests.IoCTests
{
    class ScopesTests
    {
        [Test]
        public void ScopeRegisterOneThread()
        {
            HM2.GameSolve.Structures.Vector vect = new HM2.GameSolve.Structures.Vector();
            vect.VolumeFuel = 9;
            vect.VelosityVolumeFuel = -1;
            vect.PositionNow = new Coordinats { X = 0.0, Y = 0.0 };
            vect.Shift = new Coordinats { X = 1.0, Y = 1.0 };
            IAction movable = new Moving(vect);

            ICommand ScopeLowLevel = new LowLevel(movable);
            //Регистрация скоупа низкого уровня сложности игры без учета расхода топлива при движении по прямой
            IoC<ICommand>.Resolve("Registration", "Level.Low", ScopeLowLevel);

            ICommand ScopeHighLevel = new HighLevel(movable);
            //Регистрация скоупа высокого уровня сложности игры с учетом расхода топлива при движении по прямой
            IoC<ICommand>.Resolve("Registration", "Level.High", ScopeHighLevel);

            IoC<ICommand>.Resolve("Level.Low").Execute();

            Assert.AreEqual(movable.CurrentVector.PositionNow.X, 1.0);
            Assert.AreEqual(movable.CurrentVector.PositionNow.Y, 1.0);
            Assert.AreEqual(movable.CurrentVector.VolumeFuel, 9);
            Assert.AreEqual(movable.CurrentVector.VelosityVolumeFuel, -1);

            IoC<ICommand>.Resolve("Level.High").Execute();

            Assert.AreEqual(movable.CurrentVector.PositionNow.X, 2.0);
            Assert.AreEqual(movable.CurrentVector.PositionNow.Y, 2.0);
            Assert.AreEqual(movable.CurrentVector.VolumeFuel, 8);
            Assert.AreEqual(movable.CurrentVector.VelosityVolumeFuel, -1);
        }
        [Test]
        public void ReplaceScopeOneThread()
        {
            HM2.GameSolve.Structures.Vector vect = new HM2.GameSolve.Structures.Vector();
            vect.VolumeFuel = 9;
            vect.VelosityVolumeFuel = -1;
            vect.PositionNow = new Coordinats { X = 0.0, Y = 0.0 };
            vect.Shift = new Coordinats { X = 1.0, Y = 1.0 };
            IAction movable = new Moving(vect);

            ICommand ScopeLowLevel = new LowLevel(movable);
            ICommand ScopeHighLevel = new HighLevel(movable);

            IoC<ICommand>.Resolve("Registration", "Scopes.Current", ScopeLowLevel);
            IoC<ICommand>.Resolve("Scopes.Current").Execute();

            Assert.AreEqual(movable.CurrentVector.PositionNow.X, 1.0);
            Assert.AreEqual(movable.CurrentVector.PositionNow.Y, 1.0);
            Assert.AreEqual(movable.CurrentVector.VolumeFuel, 9);
            Assert.AreEqual(movable.CurrentVector.VelosityVolumeFuel, -1);

            IoC<ICommand>.Resolve("Registration", "Scopes.Current", ScopeHighLevel);
            IoC<ICommand>.Resolve("Scopes.Current").Execute();

            Assert.AreEqual(movable.CurrentVector.PositionNow.X, 2.0);
            Assert.AreEqual(movable.CurrentVector.PositionNow.Y, 2.0);
            Assert.AreEqual(movable.CurrentVector.VolumeFuel, 8);
            Assert.AreEqual(movable.CurrentVector.VelosityVolumeFuel, -1);
        }
        [Test]
        public void ScopesTwoThreadsOnePlayer()
        {
            ThreadLocal<ICommand> threadLocal = new System.Threading.ThreadLocal<ICommand>();

            HM2.GameSolve.Structures.Vector vect1 = new HM2.GameSolve.Structures.Vector();
            vect1.VolumeFuel = 9;
            vect1.VelosityVolumeFuel = -1;
            vect1.PositionNow = new Coordinats { X = 0.0, Y = 0.0 };
            vect1.Shift = new Coordinats { X = 1.0, Y = 1.0 };
            IAction player1 = new Moving(vect1);

            ICommand ScopeLowLevel = new LowLevel(player1);
            ICommand ScopeHighLevel = new HighLevel(player1);

            Task low = new Task(() =>
            {
                threadLocal.Value = ScopeLowLevel;

                IoC<ICommand>.Resolve("Registration", "Scopes.Current", threadLocal.Value);
                IoC<ICommand>.Resolve("Scopes.Current").Execute();

                Assert.AreEqual(player1.CurrentVector.PositionNow.X, 1.0);
                Assert.AreEqual(player1.CurrentVector.PositionNow.Y, 1.0);
                Assert.AreEqual(player1.CurrentVector.VolumeFuel, 9);
                Assert.AreEqual(player1.CurrentVector.VelosityVolumeFuel, -1);
            });


            Task high = new Task(() =>
            {
                threadLocal.Value = ScopeHighLevel;

                IoC<ICommand>.Resolve("Registration", "Scopes.Current", threadLocal.Value);
                IoC<ICommand>.Resolve("Scopes.Current").Execute();

                Assert.AreEqual(player1.CurrentVector.PositionNow.X, 2.0);
                Assert.AreEqual(player1.CurrentVector.PositionNow.Y, 2.0);
                Assert.AreEqual(player1.CurrentVector.VolumeFuel, 8);
                Assert.AreEqual(player1.CurrentVector.VelosityVolumeFuel, -1);
            });

            low.Start();
            low.Wait();
            high.Start();
            high.Wait();
        }

        [Test]
        public void ScopesTwoThreadsTwoPlayer()
        {
            ThreadLocal<ICommand> threadLocal = new System.Threading.ThreadLocal<ICommand>();

            HM2.GameSolve.Structures.Vector vect1 = new HM2.GameSolve.Structures.Vector();
            vect1.VolumeFuel = 20;
            vect1.VelosityVolumeFuel = -1;
            vect1.PositionNow = new Coordinats { X = 0.0, Y = 0.0 };
            vect1.Shift = new Coordinats { X = 1.0, Y = 1.0 };
            IAction player1 = new Moving(vect1);

            HM2.GameSolve.Structures.Vector vect2 = new HM2.GameSolve.Structures.Vector();
            vect2.VolumeFuel = 10;
            vect2.VelosityVolumeFuel = -1;
            vect2.PositionNow = new Coordinats { X = 0.0, Y = 0.0 };
            vect2.Shift = new Coordinats { X = 5.0, Y = 5.0 };
            IAction player2 = new Moving(vect2);

            ICommand ScopeLowLevel = new LowLevel(player1);
            ICommand ScopeHighLevel = new HighLevel(player2);

            Task low = new Task(() =>
            {
                threadLocal.Value = ScopeLowLevel;

                IoC<ICommand>.Resolve("Registration", "Scopes.Player1", threadLocal.Value);
                IoC<ICommand>.Resolve("Scopes.Player1").Execute();

                Assert.AreEqual(player1.CurrentVector.PositionNow.X, 1.0);
                Assert.AreEqual(player1.CurrentVector.PositionNow.Y, 1.0);
                Assert.AreEqual(player1.CurrentVector.VolumeFuel, 20);
                Assert.AreEqual(player1.CurrentVector.VelosityVolumeFuel, -1);
            });


            Task high = new Task(() =>
            {
                threadLocal.Value = ScopeHighLevel;

                IoC<ICommand>.Resolve("Registration", "Scopes.Player2", threadLocal.Value);
                IoC<ICommand>.Resolve("Scopes.Player2").Execute();

                Assert.AreEqual(player2.CurrentVector.PositionNow.X, 5.0);
                Assert.AreEqual(player2.CurrentVector.PositionNow.Y, 5.0);
                Assert.AreEqual(player2.CurrentVector.VolumeFuel, 9);
                Assert.AreEqual(player2.CurrentVector.VelosityVolumeFuel, -1);
            });

            low.Start();
            low.Wait();
            high.Start();
            high.Wait();
        }
    }
}
