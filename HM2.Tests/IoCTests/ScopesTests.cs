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
using System.Threading.Tasks;

namespace HM2.Tests.IoCTests
{
    class ScopesTests
    {
        [Test]
        public void ScopeRegisterInIoC()
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


        }
    }
}
