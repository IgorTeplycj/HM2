using HM2.GameSolve.Actions;
using HM2.GameSolve.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.Tests
{
    public class FuelTests
    {
        [SetUp]
        public void Setup()
        {
            //тест
        }

        [Test] 
        public void CheckMinFuel()
        {
            HM2.GameSolve.Structures.Vector vect = new HM2.GameSolve.Structures.Vector();
            vect.VolumeFuel = 2;
            vect.VelosityVolumeFuel = -2;
            IAction movable = new HM2.Tests.Mock.Moving(vect);
            CheckFuelCommand checkFuel = new CheckFuelCommand(movable);

            checkFuel.Execute();
        }

        [Test]
        public void CheckNonFuel()
        {
            HM2.GameSolve.Structures.Vector vect = new HM2.GameSolve.Structures.Vector();
            vect.VolumeFuel = 0;
            vect.VelosityVolumeFuel = -1;
            IAction movable = new HM2.Tests.Mock.Moving(vect);
            CheckFuelCommand checkFuel = new CheckFuelCommand(movable);

            try
            {
                checkFuel.Execute();
                throw new Exception("Invalid command was complited");
            }
            catch(Exception ex)
            {
                if(ex.Message == "Invalid command was complited")
                {
                    Assert.Fail();
                }

                if(ex.GetType() != new  Exceptions.CommandException().GetType())
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        public void CheckШnadequateFuel()
        {
            HM2.GameSolve.Structures.Vector vect = new HM2.GameSolve.Structures.Vector();
            vect.VolumeFuel = 2;
            vect.VelosityVolumeFuel = -3;
            IAction movable = new HM2.Tests.Mock.Moving(vect);
            CheckFuelCommand checkFuel = new CheckFuelCommand(movable);

            try
            {
                checkFuel.Execute();
                throw new Exception("Invalid command was complited");
            }
            catch (Exception ex)
            {
                if (ex.Message == "Invalid command was complited")
                {
                    Assert.Fail();
                }

                if (ex.GetType() != new Exceptions.CommandException().GetType())
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        public void CheckNegativeFuel()
        {
            HM2.GameSolve.Structures.Vector vect = new HM2.GameSolve.Structures.Vector();
            vect.VolumeFuel = -1;
            IAction movable = new HM2.Tests.Mock.Moving(vect);
            CheckFuelCommand checkFuel = new CheckFuelCommand(movable);

            try
            {
                checkFuel.Execute();
                throw new Exception("Invalid command was complited");
            }
            catch (Exception ex)
            {
                if (ex.Message == "Invalid command was complited")
                {
                    Assert.Fail();
                }

                if (ex.GetType() != new Exceptions.CommandException().GetType())
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        public void DecreaseZeroFuel()
        {
            HM2.GameSolve.Structures.Vector vect = new HM2.GameSolve.Structures.Vector();
            vect.VolumeFuel = 1;
            vect.VelosityVolumeFuel = -1;
            IAction movable = new HM2.Tests.Mock.Moving(vect);
            BurnFuelCommand burnFuel = new BurnFuelCommand(movable);

            burnFuel.Execute();

            Assert.AreEqual(movable.CurrentVector.VolumeFuel, 0);
        }

        [Test]
        public void DecreaseFuel()
        {
            HM2.GameSolve.Structures.Vector vect = new HM2.GameSolve.Structures.Vector();
            vect.VolumeFuel = 9;
            vect.VelosityVolumeFuel = -7;
            IAction movable = new HM2.Tests.Mock.Moving(vect);
            BurnFuelCommand burnFuel = new BurnFuelCommand(movable);

            burnFuel.Execute();

            Assert.AreEqual(movable.CurrentVector.VolumeFuel, 2);
        }

        [Test]
        public void DecreaseNegativeFuel()
        {
            HM2.GameSolve.Structures.Vector vect = new HM2.GameSolve.Structures.Vector();
            vect.VolumeFuel = 9;
            vect.VelosityVolumeFuel = -14;
            IAction movable = new HM2.Tests.Mock.Moving(vect);
            BurnFuelCommand burnFuel = new BurnFuelCommand(movable);

            burnFuel.Execute();

            Assert.AreEqual(movable.CurrentVector.VolumeFuel, -5);
        }
    }
}
