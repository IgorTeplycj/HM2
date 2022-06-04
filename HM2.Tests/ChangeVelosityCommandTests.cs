using HM2.GameSolve.Actions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.Tests
{
    public class ChangeVelosityCommandTests
    {
        [SetUp]
        public void SetUp()
        { }
        [Test]
        public void CheckModifVectorVelosity()
        {
            HM2.GameSolve.Structures.Vector vect = new HM2.GameSolve.Structures.Vector();
            vect.VelosityVectNow = new GameSolve.Structures.VelosityVect { Angular = 30, Velosity = 100 };
            vect.VelosityVectModifer = new GameSolve.Structures.VelosityVect { Angular = 60, Velosity = 200 };

            GameSolve.Interfaces.IAction movable = new HM2.Tests.Mock.Moving(vect);
            ChangeVelocityCommand changeVelocityCommand = new ChangeVelocityCommand(movable);
            changeVelocityCommand.Execute();

            HM2.GameSolve.Structures.Vector vectBef = movable.CurrentVector;

            Assert.AreNotEqual(vectBef.VelosityVectNow.Velosity, 100.0);
            Assert.AreNotEqual(vectBef.VelosityVectNow.Angular, 30.0);
        }
        [Test]
        public void CheckModifVectorVelosityZero()
        {
            HM2.GameSolve.Structures.Vector vect = new HM2.GameSolve.Structures.Vector();
            vect.VelosityVectNow = new GameSolve.Structures.VelosityVect { Angular = 30, Velosity = 0 };
            vect.VelosityVectModifer = new GameSolve.Structures.VelosityVect { Angular = 60, Velosity = 200 };

            GameSolve.Interfaces.IAction movable = new HM2.Tests.Mock.Moving(vect);
            ChangeVelocityCommand changeVelocityCommand = new ChangeVelocityCommand(movable);

            try
            {
                changeVelocityCommand.Execute();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.GetType().GetHashCode(), new DivideByZeroException().GetType().GetHashCode());
            }

            HM2.GameSolve.Structures.Vector vectBef = movable.CurrentVector;

            Assert.AreEqual(vectBef.VelosityVectNow.Velosity, 0.0);
            Assert.AreEqual(vectBef.VelosityVectNow.Angular, 30.0);
            Assert.AreEqual(vectBef.VelosityVectModifer.Velosity, 200);
            Assert.AreEqual(vectBef.VelosityVectModifer.Angular, 60);
        }
        [Test]
        public void CheckModifVectorVelosityNaN()
        {
            HM2.GameSolve.Structures.Vector vect = new HM2.GameSolve.Structures.Vector();
            vect.VelosityVectNow = new GameSolve.Structures.VelosityVect { Angular = 30, Velosity = double.NaN };
            vect.VelosityVectModifer = new GameSolve.Structures.VelosityVect { Angular = 60, Velosity = 200 };

            GameSolve.Interfaces.IAction movable = new HM2.Tests.Mock.Moving(vect);
            ChangeVelocityCommand changeVelocityCommand = new ChangeVelocityCommand(movable);

            try
            {
                changeVelocityCommand.Execute();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.GetType().GetHashCode(), new ArgumentException().GetType().GetHashCode());
            }

            HM2.GameSolve.Structures.Vector vectBef = movable.CurrentVector;

            Assert.AreEqual(vectBef.VelosityVectNow.Angular, 30.0);
            Assert.AreEqual(vectBef.VelosityVectModifer.Velosity, 200);
            Assert.AreEqual(vectBef.VelosityVectModifer.Angular, 60);
        }
        [Test]
        public void CheckModifVectorVelosityInfinity()
        {
            HM2.GameSolve.Structures.Vector vect = new HM2.GameSolve.Structures.Vector();
            vect.VelosityVectNow = new GameSolve.Structures.VelosityVect { Angular = 30, Velosity = double.PositiveInfinity };
            vect.VelosityVectModifer = new GameSolve.Structures.VelosityVect { Angular = 60, Velosity = 200 };

            GameSolve.Interfaces.IAction movable = new HM2.Tests.Mock.Moving(vect);
            ChangeVelocityCommand changeVelocityCommand = new ChangeVelocityCommand(movable);

            try
            {
                changeVelocityCommand.Execute();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.GetType().GetHashCode(), new ArgumentException().GetType().GetHashCode());
            }

            HM2.GameSolve.Structures.Vector vectBef = movable.CurrentVector;

            Assert.AreEqual(vectBef.VelosityVectNow.Angular, 30.0);
            Assert.AreEqual(vectBef.VelosityVectModifer.Velosity, 200);
            Assert.AreEqual(vectBef.VelosityVectModifer.Angular, 60);
        }
        [Test]
        public void CheckModifVectorAngularNaN()
        {
            HM2.GameSolve.Structures.Vector vect = new HM2.GameSolve.Structures.Vector();
            vect.VelosityVectNow = new GameSolve.Structures.VelosityVect { Angular = double.NaN, Velosity = 1.0 };
            vect.VelosityVectModifer = new GameSolve.Structures.VelosityVect { Angular = 60, Velosity = 200 };

            GameSolve.Interfaces.IAction movable = new HM2.Tests.Mock.Moving(vect);
            ChangeVelocityCommand changeVelocityCommand = new ChangeVelocityCommand(movable);

            try
            {
                changeVelocityCommand.Execute();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.GetType().GetHashCode(), new ArgumentException().GetType().GetHashCode());
            }

            HM2.GameSolve.Structures.Vector vectBef = movable.CurrentVector;

            Assert.AreEqual(vectBef.VelosityVectNow.Velosity, 1.0);
            Assert.AreEqual(vectBef.VelosityVectModifer.Velosity, 200);
            Assert.AreEqual(vectBef.VelosityVectModifer.Angular, 60);
        }
        [Test]
        public void CheckModifVectorAngularInfinity()
        {
            HM2.GameSolve.Structures.Vector vect = new HM2.GameSolve.Structures.Vector();
            vect.VelosityVectNow = new GameSolve.Structures.VelosityVect { Angular = double.PositiveInfinity, Velosity = 1.0 };
            vect.VelosityVectModifer = new GameSolve.Structures.VelosityVect { Angular = 60, Velosity = 200 };

            GameSolve.Interfaces.IAction movable = new HM2.Tests.Mock.Moving(vect);
            ChangeVelocityCommand changeVelocityCommand = new ChangeVelocityCommand(movable);

            try
            {
                changeVelocityCommand.Execute();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.GetType().GetHashCode(), new ArgumentException().GetType().GetHashCode());
            }

            HM2.GameSolve.Structures.Vector vectBef = movable.CurrentVector;

            Assert.AreEqual(vectBef.VelosityVectNow.Velosity, 1.0);
            Assert.AreEqual(vectBef.VelosityVectModifer.Velosity, 200);
            Assert.AreEqual(vectBef.VelosityVectModifer.Angular, 60);
        }
    }
}
