using HM2.GameSolve.Actions;
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
    public class MacroCommandTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void PositiveTest_Moving()
        {
            HM2.GameSolve.Structures.Vector vect = new HM2.GameSolve.Structures.Vector();
            vect.VolumeFuel = 9;
            vect.VelosityVolumeFuel = -1;
            vect.PositionNow = new Coordinats { X = 12.0, Y = 5.0 };
            vect.Shift = new Coordinats { X = -7.0, Y = 3.0 };
            IAction movable = new HM2.Tests.Mock.Moving(vect);

            List<ICommand> macroColl = new List<ICommand>();
            macroColl.Add(new CheckFuelCommand(movable));
            macroColl.Add(new MoveCommand(movable));
            macroColl.Add(new BurnFuelCommand(movable));
            MacroCommand macro = new MacroCommand(macroColl);
            macro.Execute();

            Vector vectorBef = movable.CurrentVector;

            Assert.AreEqual(vectorBef.PositionNow.X, 5.0);
            Assert.AreEqual(vectorBef.PositionNow.Y, 8.0);
            Assert.AreEqual(vectorBef.VolumeFuel, 8);
            Assert.AreEqual(vectorBef.Shift.X, -7.0);
            Assert.AreEqual(vectorBef.Shift.Y, 3.0);
        }
        /// <summary>
        /// Тестироание макрокоманды, при недостаточном количестве топлива
        /// </summary>
        [Test]
        public void NegativeTest_Moving_NotFuel()
        {
            HM2.GameSolve.Structures.Vector vect = new HM2.GameSolve.Structures.Vector();
            vect.VolumeFuel = 0;
            vect.VelosityVolumeFuel = -1;
            vect.PositionNow = new Coordinats { X = 12.0, Y = 5.0 };
            vect.Shift = new Coordinats { X = -7.0, Y = 3.0 };
            IAction movable = new HM2.Tests.Mock.Moving(vect);

            List<ICommand> macroColl = new List<ICommand>();
            macroColl.Add(new CheckFuelCommand(movable));
            macroColl.Add(new MoveCommand(movable));
            macroColl.Add(new BurnFuelCommand(movable));
            MacroCommand macro = new MacroCommand(macroColl);

            try
            {
                macro.Execute();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.GetType().GetHashCode(), new Exceptions.CommandException().GetType().GetHashCode());
            }

            Vector vectorBef = movable.CurrentVector;
            //Эта проверка определяет, что позиция не изменилась, это означает,
            //что команда движения не вызывалась после выброса исключения, при проверке уровня топлива
            Assert.AreEqual(vectorBef.PositionNow.X, 12.0);
            Assert.AreEqual(vectorBef.PositionNow.Y, 5.0);

            //проверка того, что остальные поля не изменились
            Assert.AreEqual(vectorBef.VolumeFuel, 0);
            Assert.AreEqual(vectorBef.Shift.X, -7.0);
            Assert.AreEqual(vectorBef.Shift.Y, 3.0);
        }

        /// <summary>
        /// Тестирование макрокоманды, при невозможности сдвинуть объект
        /// </summary>
        [Test]
        public void NegativeTest_Moving_NotShift()
        {
            HM2.GameSolve.Structures.Vector vect = new HM2.GameSolve.Structures.Vector();
            vect.VolumeFuel = 100;
            vect.VelosityVolumeFuel = -1;
            vect.PositionNow = new Coordinats { X = double.NaN, Y = 5.0 };
            vect.Shift = new Coordinats { X = -7.0, Y = 3.0 };
            IAction movable = new HM2.Tests.Mock.Moving(vect);

            List<ICommand> macroColl = new List<ICommand>();
            macroColl.Add(new CheckFuelCommand(movable));
            macroColl.Add(new MoveCommand(movable));
            macroColl.Add(new BurnFuelCommand(movable));
            MacroCommand macro = new MacroCommand(macroColl);

            try
            {
                macro.Execute();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.GetType().GetHashCode(), new Exceptions.CommandException().GetType().GetHashCode());
            }

            Vector vectorBef = movable.CurrentVector;
            //Эта проверка определяет, что позиция не изменилась, это означает,
            //что команда движения не вызывалась после выброса исключения, при проверке уровня топлива
            //Assert.AreEqual(vectorBef.PositionNow.X, 12.0); координату X прочитать не можем, так как ее значение NaN и выбрасывается исключение
            Assert.AreEqual(vectorBef.PositionNow.Y, 5.0);

            //проверка того, что остальные поля не изменились
            Assert.AreEqual(vectorBef.VolumeFuel, 100);
            Assert.AreEqual(vectorBef.Shift.X, -7.0);
            Assert.AreEqual(vectorBef.Shift.Y, 3.0);
        }
    }
}
