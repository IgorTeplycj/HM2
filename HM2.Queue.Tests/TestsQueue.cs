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
            //–егистраци€ признаков команд, управл€ющих очередью
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

        [Test]
        public void TestStartAndSoftStop()
        {
            MockCommandDelay command1 = new MockCommandDelay(10);
            MockCommandDelay command2 = new MockCommandDelay(10);
            MockCommandDelay command3 = new MockCommandDelay(10);
            MockCommandDelay command4 = new MockCommandDelay(10);
            MockCommandDelay command5 = new MockCommandDelay(10);
            MockCommandDelay command6 = new MockCommandDelay(10);

            QueueCommand queueCommand = new QueueCommand();
            queueCommand.PushCommand(command1);
            queueCommand.PushCommand(command2);
            queueCommand.PushCommand(command3);
            queueCommand.PushCommand(command4);
            queueCommand.PushCommand(command5);
            queueCommand.PushCommand(command6);

            Assert.IsFalse(queueCommand.TaskIsRun);
            //отправка команды в очередь
            queueCommand.PushCommand(new ControlCommand(queueCommand.Start));
            Thread.Sleep(2);
            Assert.IsTrue(queueCommand.TaskIsRun);
            Thread.Sleep(23);
            queueCommand.PushCommand(new ControlCommand(queueCommand.SoftStop));
            Thread.Sleep(40);

            Assert.IsTrue(command1.CommandIsComplited());
            Assert.IsTrue(command2.CommandIsComplited());
            Assert.IsTrue(command3.CommandIsComplited());
            Assert.IsTrue(command4.CommandIsComplited());
            Assert.IsTrue(command5.CommandIsComplited());
            Assert.IsTrue(command6.CommandIsComplited());

            Assert.IsFalse(queueCommand.TaskIsRun);
        }
        /// <summary>
        /// “ест жесткой остановки выполнени€ очереди команд
        /// </summary>
        [Test]
        public void TestHardStop()
        {
            MockCommandDelay command1 = new MockCommandDelay(10);
            MockCommandDelay command2 = new MockCommandDelay(10);
            MockCommandDelay command3 = new MockCommandDelay(10);
            MockCommandDelay command4 = new MockCommandDelay(10);
            MockCommandDelay command5 = new MockCommandDelay(10);
            MockCommandDelay command6 = new MockCommandDelay(10);

            QueueCommand queueCommand = new QueueCommand();
            queueCommand.PushCommand(command1);
            queueCommand.PushCommand(command2);
            queueCommand.PushCommand(command3);
            queueCommand.PushCommand(command4);
            queueCommand.PushCommand(command5);
            queueCommand.PushCommand(command6);

            Assert.IsFalse(queueCommand.TaskIsRun);

            queueCommand.PushCommand(new ControlCommand(queueCommand.Start)); //«апуск очереди
            Thread.Sleep(25); //приблизительное врем€ выполнени€ двух команд заглушек
            queueCommand.PushCommand(new ControlCommand(queueCommand.HardStop)); //∆есткий стоп очереди
            Thread.Sleep(60);

            Assert.IsTrue(command1.CommandIsComplited());
            Assert.IsTrue(command2.CommandIsComplited());
            Assert.IsTrue(command3.CommandIsComplited());

            Assert.IsFalse(command4.CommandIsComplited());
            Assert.IsFalse(command5.CommandIsComplited());
            Assert.IsFalse(command6.CommandIsComplited());
        }

        /// <summary>
        /// “ест событи€ старта
        /// </summary>
        [Test] 
        public void TestEventStart()
        {

        }
    }
}