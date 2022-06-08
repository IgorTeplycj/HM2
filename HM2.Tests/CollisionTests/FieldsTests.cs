using HM2.GameSolve.Interfaces;
using HM2.IoCs;
using HM2.Model.Fields;
using HM2.Model.Fields.Commands;
using HM2.Model.Fields.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.Tests.CollisionTests
{
    public class FieldsTests
    {
        [SetUp]
        public void Setup()
        {
            IoC<double>.Resolve("IoC.Registration", "discrete", 0.1); //регистрируем дискрету. Это та наименьшая величина, на которую мы можем сдвинуть объект на поле
        }

        [Test]
        public void CreateAndSplitFieldsTest()
        {
            double discrete = IoC<double>.Resolve("discrete");
            Square<Square<IAction>> mainField = new Square<Square<IAction>>(0.0, 1000.0, 0, 1000.0);    //Создание главного поля

            List<Square<IAction>> quares = new List<Square<IAction>>();
            //Создание команды разбиения главного поля на квадраты
            //Здесь два последних параметра это число квадратов на которое разбиваем поле по X и по Y
            new SplitFieldCommand<Square<IAction>, IAction>(mainField, quares, 5, 5).Execute();   

            mainField.objContainer.AddRange(quares);    //Добавление квадратов в контейнер главного поля

            //Проверка что квадрат с указанными координатами существует
            var item1 = mainField.objContainer.Find(c => c.StartX == 200 + discrete && c.EndX == 400.0 && c.StartY == 0.0 && c.EndY == 200.0);
            Assert.IsNotNull(item1);

            //Проверка что квадрат с указанными координатами существует
            var item2 = mainField.objContainer.Find(c => c.StartX == 400 + discrete && c.EndX == 600.0 && c.StartY == 200 + discrete && c.EndY == 400.0);
            Assert.IsNotNull(item2);

            //Проверка что квадрат с указанными координатами не существует
            var item3= mainField.objContainer.Find(c => c.StartX == 400 && c.EndX == 600.0 && c.StartY == 200 && c.EndY == 400.0);
            Assert.IsNull(item3);
        }
    }
}
