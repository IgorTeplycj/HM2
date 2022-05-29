using HM2.GameSolve.Actions;
using HM2.GameSolve.Interfaces;
using HM2.GameSolve.Structures;
using HM2.IoCs;
using HM2.MovableObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.Games
{
    public class Game
    {
        public delegate UObject createObj();
        public void Create(int numberGames, int numberObject)
        {
            //создание игрового объекта
            createObj createObj = () =>
            {
                Vector vect = new Vector();
                vect.PositionNow = new Coordinats { X = 0.0, Y = 0.0 };
                vect.Shift = new Coordinats { X = 0.0, Y = 0.0 };
                UObject uObject = new UObject(vect);
                return uObject;
            };
            IoC<createObj>.Resolve("IoC.Registration", "Create game object", createObj);

            //создание команды движения для игрового объекта
            Func<UObject, ICommand> getCommandMove = (o) =>
            {
                return new MoveCommand(o);
            };
            IoC<Func<UObject, ICommand>>.Resolve("IoC.Registration", "Move line", getCommandMove);

            //создание команды поворота для игрового объекта
            Func<UObject, ICommand> getCommandRotate = (o) =>
            {
                return new RotateCommand(o);
            };
            IoC<Func<UObject, ICommand>>.Resolve("IoC.Registration", "Rotate", getCommandRotate);

            //создание команды расхода топлива для игрового объекта
            Func<UObject, ICommand> getCommandFuel = (o) =>
            {
                return new CheckFuelCommand(o);
            };
            IoC<Func<UObject, ICommand>>.Resolve("IoC.Registration", "Fuel", getCommandFuel);

            //СОЗДАНИЕ ИГРЫ С ИГРОВЫМИ ОБЪЕКТАМИ
            for (int j = 1; j <= numberGames; j++)
            {
                for (int i = 1; i <= numberObject; i++)
                {
                    UObject obj = IoC<createObj>.Resolve("Create game object").Invoke();
                    IoC<UObject>.Resolve("IoC.Registration", $"game {j} object {i}", obj);
                }
            }
        }
    }
}
