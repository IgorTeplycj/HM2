using HM2.GameSolve.Actions;
using HM2.GameSolve.Interfaces;
using HM2.GameSolve.Structures;
using HM2.Tests.Mock;
using NUnit.Framework;
using System;

namespace HM2.Tests
{
    public class MovingRotateTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void MovingObject()
        {
            Vector vectBef = new Vector();
            vectBef.PositionNow = new Coordinats { X = 12.0, Y = 5.0 };
            vectBef.Shift = new Coordinats { X = -7.0, Y = 3.0 };
            IAction movable = new Moving(vectBef);

            Assert.AreEqual(movable.CurrentVector.PositionNow.X, 12.0);
            Assert.AreEqual(movable.CurrentVector.PositionNow.Y, 5.0);

            MoveCommand move = new MoveCommand(movable);
            move.Execute();

            Assert.AreEqual(movable.CurrentVector.PositionNow.X, 5.0);
            Assert.AreEqual(movable.CurrentVector.PositionNow.Y, 8.0);
        }

        [Test]
        public void ShiftByNotPositionNaNX()
        {
            Vector vectBef = new Vector();
            vectBef.PositionNow = new Coordinats { X = double.NaN, Y = 0.0 };
            IAction movable = new Moving(vectBef);

            MoveCommand move = new MoveCommand(movable);
            try
            {
                move.Execute();
                throw new Exception("Invalid command was complited");
            }
            catch (Exception ex)
            {
                if (ex.Message == "Invalid command was complited")
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        public void ShiftByNotPositionInfX()
        {
            Vector vectBef = new Vector();
            vectBef.PositionNow = new Coordinats { X = double.PositiveInfinity, Y = 0.0 };
            IAction movable = new Moving(vectBef);

            MoveCommand move = new MoveCommand(movable);
            try
            {
                move.Execute();
                throw new Exception("Invalid command was complited");
            }
            catch (Exception ex)
            {
                if (ex.Message == "Invalid command was complited")
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        public void ShiftByNotPositionNaNY()
        {
            Vector vectBef = new Vector();
            vectBef.PositionNow = new Coordinats { X = 0.0, Y = double.NaN };
            IAction movable = new Moving(vectBef);

            MoveCommand move = new MoveCommand(movable);
            try
            {
                move.Execute();
                throw new Exception("Invalid command was complited");
            }
            catch (Exception ex)
            {
                if (ex.Message == "Invalid command was complited")
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        public void ShiftByNotPositionInfY()
        {
            Vector vectBef = new Vector();
            vectBef.PositionNow = new Coordinats { X = 0.0, Y = double.PositiveInfinity };
            IAction movable = new Moving(vectBef);

            MoveCommand move = new MoveCommand(movable);
            try
            {
                move.Execute();
                throw new Exception("Invalid command was complited");
            }
            catch (Exception ex)
            {
                if (ex.Message == "Invalid command was complited")
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        public void NotShiftObject()
        {
            IAction movable = null;

            MoveCommand move = new MoveCommand(movable);
            try
            {
                move.Execute();
                throw new Exception("Invalid command was complited");
            }
            catch (Exception ex)
            {
                if (ex.Message == "Invalid command was complited")
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        public void Rotate()
        {
            Vector vectBef = new Vector();
            vectBef.PositionNow = new Coordinats { X = 80.0, Y = 20.0 };
            vectBef.AngularVelosity = 10;
            vectBef.Direction = 15;
            vectBef.DirectionNumber = 24;

            IAction movable = new Moving(vectBef);
            RotateCommand rot = new RotateCommand(movable);

            Assert.AreEqual(movable.CurrentVector.Direction, 15);

            rot.Execute();

            Assert.AreEqual(movable.CurrentVector.Direction, 1);
            Assert.AreEqual(movable.CurrentVector.PositionNow.X, 80.0);
            Assert.AreEqual(movable.CurrentVector.PositionNow.Y, 20.0);
        }

        [Test]
        public void RotateWithoutPositionX()
        {
            Vector vectBef = new Vector();
            vectBef.PositionNow = new Coordinats { X = Double.NaN, Y = 20.0 };
            vectBef.AngularVelosity = 10;
            vectBef.Direction = 15;
            vectBef.DirectionNumber = 24;

            IAction movable = new Moving(vectBef);
            RotateCommand rot = new RotateCommand(movable);

            Assert.AreEqual(movable.CurrentVector.Direction, 15);

            try
            {
                rot.Execute();
                throw new Exception("Invalid command was complited");
            }
            catch (Exception ex)
            {
                if (ex.Message == "Invalid command was complited")
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        public void RotateWithoutPositionY()
        {
            Vector vectBef = new Vector();
            vectBef.PositionNow = new Coordinats { X = 5.0, Y = Double.PositiveInfinity };
            vectBef.AngularVelosity = 10;
            vectBef.Direction = 15;
            vectBef.DirectionNumber = 24;

            IAction movable = new Moving(vectBef);
            RotateCommand rot = new RotateCommand(movable);

            Assert.AreEqual(movable.CurrentVector.Direction, 15);

            try
            {
                rot.Execute();
                throw new Exception("Invalid command was complited");
            }
            catch (Exception ex)
            {
                if (ex.Message == "Invalid command was complited")
                {
                    Assert.Fail();
                }
            }
        }
    }
}