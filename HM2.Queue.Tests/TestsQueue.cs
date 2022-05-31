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
            Thread.Sleep(65);

            Assert.IsTrue(command1.CommandIsComplited());
            Assert.IsTrue(command2.CommandIsComplited());
            Assert.IsTrue(command3.CommandIsComplited());
            Assert.IsTrue(command4.CommandIsComplited());
            Assert.IsTrue(command5.CommandIsComplited());
            Assert.IsTrue(command6.CommandIsComplited());

            Assert.IsFalse(queueCommand.TaskIsRun);
        }
        /// <summary>
        /// Тест жесткой остановки выполнения очереди команд
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

            queueCommand.PushCommand(new ControlCommand(queueCommand.Start)); //Запуск очереди
            Thread.Sleep(25); //приблизительное время выполнения двух команд заглушек
            queueCommand.PushCommand(new ControlCommand(queueCommand.HardStop)); //Жесткий стоп очереди
            Thread.Sleep(60);

            Assert.IsTrue(command1.CommandIsComplited());
            Assert.IsTrue(command2.CommandIsComplited());
            Assert.IsTrue(command3.CommandIsComplited());

            Assert.IsFalse(command4.CommandIsComplited());
            Assert.IsFalse(command5.CommandIsComplited());
            Assert.IsFalse(command6.CommandIsComplited());

            Assert.IsFalse(queueCommand.TaskIsRun);
        }

        /// <summary>
        /// Тест события старта
        /// </summary>
        [Test]
        public void TestEventStartAndComplited()
        {
            MockCommandDelay command1 = new MockCommandDelay(10);
            MockCommandDelay command2 = new MockCommandDelay(10);
            MockCommandDelay command3 = new MockCommandDelay(10);
            MockCommandDelay command4 = new MockCommandDelay(10);
            MockCommandDelay command5 = new MockCommandDelay(10);
            MockCommandDelay command6 = new MockCommandDelay(10);

            bool eventStartIsWorked = false;
            bool eventComplitedIsWorked = false;

            QueueCommand queueCommand = new QueueCommand();
            queueCommand.StartThread += () =>
            {
                eventStartIsWorked = true;
            };
            queueCommand.ComplitedThread += () =>
            {
                eventComplitedIsWorked = true;
            };

            queueCommand.PushCommand(command1);
            queueCommand.PushCommand(command2);
            queueCommand.PushCommand(command3);
            queueCommand.PushCommand(command4);
            queueCommand.PushCommand(command5);
            queueCommand.PushCommand(command6);

            queueCommand.PushCommand(new ControlCommand(queueCommand.Start)); //Запуск очереди

            Thread.Sleep(65);

            if(!eventStartIsWorked)
            {
                Assert.Fail();
            }
            if (eventComplitedIsWorked)
            {
                Assert.Fail();
            }

            queueCommand.PushCommand(new ControlCommand(queueCommand.SoftStop)); //Остановка выполнения очереди команд
            Thread.Sleep(5);

            if (!eventComplitedIsWorked)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestRepeatedAddCommand()
        {
            MockCommandDelay command1 = new MockCommandDelay(0);
            MockCommandDelay command2 = new MockCommandDelay(0);
            MockCommandDelay command3 = new MockCommandDelay(0);
            MockCommandDelay command4 = new MockCommandDelay(0);
            MockCommandDelay command5 = new MockCommandDelay(0);
            MockCommandDelay command6 = new MockCommandDelay(0);

            QueueCommand queueCommand = new QueueCommand();
            queueCommand.PushCommand(command1);
            queueCommand.PushCommand(command2);
            queueCommand.PushCommand(command3);
            queueCommand.PushCommand(command4);
            queueCommand.PushCommand(command5);
            queueCommand.PushCommand(command6);

            queueCommand.PushCommand(new ControlCommand(queueCommand.Start)); //Запуск очереди

            Thread.Sleep(5);

            Assert.IsTrue(command1.CommandIsComplited());
            Assert.IsTrue(command2.CommandIsComplited());
            Assert.IsTrue(command3.CommandIsComplited());
            Assert.IsTrue(command4.CommandIsComplited());
            Assert.IsTrue(command5.CommandIsComplited());
            Assert.IsTrue(command6.CommandIsComplited());

            MockCommandDelay command7 = new MockCommandDelay(0);
            MockCommandDelay command8 = new MockCommandDelay(0);
            MockCommandDelay command9 = new MockCommandDelay(0);

            queueCommand.PushCommand(command7);
            queueCommand.PushCommand(command8);
            queueCommand.PushCommand(command9);
            Thread.Sleep(5);

            Assert.IsTrue(command7.CommandIsComplited());
            Assert.IsTrue(command8.CommandIsComplited());
            Assert.IsTrue(command9.CommandIsComplited());

            queueCommand.PushCommand(new ControlCommand(queueCommand.SoftStop)); //Остановка выполнения очереди команд

            //Проверка, что вновь добавленные команды не будут выполнятся 

            MockCommandDelay command10 = new MockCommandDelay(0);
            MockCommandDelay command11 = new MockCommandDelay(0);
            MockCommandDelay command12 = new MockCommandDelay(0);

            queueCommand.PushCommand(command10);
            queueCommand.PushCommand(command11);
            queueCommand.PushCommand(command12);

            Thread.Sleep(5);

            Assert.IsFalse(command10.CommandIsComplited());
            Assert.IsFalse(command11.CommandIsComplited());
            Assert.IsFalse(command12.CommandIsComplited());
        }
    }
}