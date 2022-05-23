using HM2.GameSolve.Actions;
using HM2.GameSolve.Actions.Macro;
using HM2.GameSolve.Interfaces;
using HM2.GameSolve.Structures;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.Tests
{
    /// <summary>
    /// Тесты макрокоманды поворота и изменения мгновенной скорости
    /// </summary>
    public class MacroCommandModifVelosityByRotate
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void RotateWithModifVelosityVect()
        {
            const double vel = 100.0;
            const double anglr = 5.0;
            const int anglrVel = 10;
            const int direction = 15;
            const int dirNum = 24;

            Vector vectBef = new Vector();
            vectBef.AngularVelosity = anglrVel;
            vectBef.Direction = direction;
            vectBef.DirectionNumber = dirNum;
            vectBef.VelosityVectNow = new VelosityVect { Velosity = vel, Angular = anglr };

            IAction rot = new Mock.Moving(vectBef);
            RotateAndModifVelosityCommand rotateAndModifVelosityCommand= new RotateAndModifVelosityCommand(rot);
            rotateAndModifVelosityCommand.Execute();

            Assert.AreEqual(rot.CurrentVector.AngularVelosity, anglrVel);
            Assert.AreNotEqual(rot.CurrentVector.Direction, direction);
            Assert.AreEqual(rot.CurrentVector.DirectionNumber, dirNum);
            Assert.AreNotEqual(rot.CurrentVector.VelosityVectNow.Velosity, vel);
            Assert.AreNotEqual(rot.CurrentVector.VelosityVectNow.Angular, anglr);
        }
        [Test]
        public void RotateWithModifVelosityVectNonMoveObject()
        {
            const double vel = 0.0;
            const double anglr = 0.0;
            const int anglrVel = 10;
            const int direction = 15;
            const int dirNum = 24;

            Vector vectBef = new Vector();
            vectBef.AngularVelosity = anglrVel;
            vectBef.Direction = direction;
            vectBef.DirectionNumber = dirNum;
            vectBef.VelosityVectNow = new VelosityVect { Velosity = vel, Angular = anglr };

            IAction rot = new Mock.Moving(vectBef);
            RotateAndModifVelosityCommand rotateAndModifVelosityCommand = new RotateAndModifVelosityCommand(rot);
            rotateAndModifVelosityCommand.Execute();

            Assert.AreEqual(rot.CurrentVector.AngularVelosity, anglrVel);
            Assert.AreNotEqual(rot.CurrentVector.Direction, direction);
            Assert.AreEqual(rot.CurrentVector.DirectionNumber, dirNum);
            Assert.AreEqual(rot.CurrentVector.VelosityVectNow.Velosity, vel);
            Assert.AreEqual(rot.CurrentVector.VelosityVectNow.Angular, anglr);
        }
    }
}
