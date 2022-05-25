using HM2.GameSolve.Interfaces;
using HM2.Queue.Tests.Mocks;
using HM2.Threads;
using HM2.Threads.Commands;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HM2.Queue.Tests
{
    public class TestsQueue
    {
        [SetUp]
        public void Setup()
        {
            //Регистрация признаков команд, управляющих очередью
            Func<ICommand, bool> isControlCommand = (c) =>
            {
                List<int> controlComands = new List<int>
                {
                    typeof(ControlCommand).GetHashCode(),
                };

                return controlComands.Contains(c.GetType().GetHashCode());
            };

            IoCs.IoC<Func<ICommand, bool>>.Resolve("IoC.Registration", "IsControlCommand", isControlCommand);
        }

        /// <summary>
        /// Тест запуска потока. Состояние запуска определяется исходя из признака выполнения MockCommand
        /// </summary>
        [Test]
        public void TestRunThread()
        {
            MockCommandDelay command1 = new MockCommandDelay(20);
            MockCommandDelay command2 = new MockCommandDelay(20);

            QueueCommand queueCommand = new QueueCommand();
            queueCommand.PushCommand(command1);
            queueCommand.PushCommand(command2);

            Thread.Sleep(50);
            Assert.IsFalse(command1.CommandIsComplited());
            Assert.IsFalse(command2.CommandIsComplited());

            //создание управляющей команды на запуск очереди
            ICommand commandStart = new ControlCommand(queueCommand.Start);
            queueCommand.PushCommand(commandStart);

            Thread.Sleep(10);
            Assert.IsFalse(command1.CommandIsComplited());
            Thread.Sleep(11);
            Assert.IsTrue(command1.CommandIsComplited());

            Assert.IsFalse(command2.CommandIsComplited());
            Thread.Sleep(20);
            Assert.IsTrue(command2.CommandIsComplited());
        }
        /// <summary>
        /// Тест признака старта параллельной задачи
        /// </summary>
        [Test]
        public void TestEventStart()
        {
            MockCommandDelay command1 = new MockCommandDelay(10); //команда заглушка
            QueueCommand queueCommand = new QueueCommand();
            queueCommand.PushCommand(command1);
            //подписываемся на событие старта
            queueCommand.StartThread += () =>
            {
                Assert.IsTrue(queueCommand.TaskIsRun);
            };
            queueCommand.ComplitedThread += () =>
            {
                Assert.IsFalse(queueCommand.TaskIsRun);
            };
            Assert.IsFalse(queueCommand.TaskIsRun);

            //создание управляющей команды на запуск очереди
            ICommand commandStart = new ControlCommand(queueCommand.Start);
            //отправка команды в очередь
            queueCommand.PushCommand(commandStart);

            Thread.Sleep(30);
            queueCommand.PushCommand(new ControlCommand(queueCommand.SoftStop));
            Thread.Sleep(30);
            Assert.IsFalse(queueCommand.TaskIsRun);
        }
        /// <summary>
        /// Тест остановки параллельной задачи
        /// </summary>
        [Test]
        public void TestEventSoftStop()
        {
            MockCommandDelay command1 = new MockCommandDelay(10); //команда заглушка
            QueueCommand queueCommand = new QueueCommand();
            queueCommand.PushCommand(command1);
            //подписываемся на событие остановки цикла
            queueCommand.ComplitedThread += () =>
            {
                Assert.IsFalse(queueCommand.TaskIsRun);
            };

            Assert.IsFalse(queueCommand.TaskIsRun);

            //создание управляющей команды на запуск очереди
            ICommand commandStart = new ControlCommand(queueCommand.Start);
            //отправка команды в очередь
            queueCommand.PushCommand(commandStart);
            queueCommand.PushCommand(new ControlCommand(queueCommand.SoftStop));

            Thread.Sleep(50);
        }

    }
}