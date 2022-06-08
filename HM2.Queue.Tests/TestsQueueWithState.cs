using HM2.GameSolve.Interfaces;
using HM2.IoCs;
using HM2.Queue.Tests.Mocks;
using HM2.State;
using HM2.Threads;
using HM2.Threads.Commands;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HM2.Queue.Tests
{
    public class TestsQueueWithState
    {
        [SetUp]
        public void Setup()
        {

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

            QueueThread queueCommand = new QueueThread();
            queueCommand.PushCommand(command1);
            queueCommand.PushCommand(command2);
            queueCommand.PushCommand(command3);
            queueCommand.PushCommand(command4);
            queueCommand.PushCommand(command5);
            queueCommand.PushCommand(command6);

            Assert.IsFalse(queueCommand.TaskIsRun);
            Assert.AreEqual(queueCommand.state.Handle(), StateEnum.NoRun); //проверяем что состояние очереди NoRun

            queueCommand.PushCommand(new ControlCommand(queueCommand.Start)); //Запуск очереди
            Thread.Sleep(5);
            Assert.AreEqual(queueCommand.state.Handle(), StateEnum.Normal); //проверяем что состояние очереди после хапуска Normal
            Thread.Sleep(20); //приблизительное время выполнения двух команд заглушек
            queueCommand.PushCommand(new ControlCommand(queueCommand.HardStop)); //Жесткий стоп очереди
            Thread.Sleep(60);

            Assert.IsTrue(command1.CommandIsComplited());
            Assert.IsTrue(command2.CommandIsComplited());
            Assert.IsTrue(command3.CommandIsComplited());

            Assert.IsFalse(command4.CommandIsComplited());
            Assert.IsFalse(command5.CommandIsComplited());
            Assert.IsFalse(command6.CommandIsComplited());

            Assert.AreEqual(queueCommand.state.Handle(), StateEnum.HardStoped); //проверяем состояние очереди после HardStop 
            Assert.IsFalse(queueCommand.TaskIsRun); //проверяем что после HarStop поток завершился.
        }

        [Test]
        public void TestMoveToCommand()
        {
            //Для перегрузки команд в резервную очередь создадим и зарегестрируем резервную очередь в контейнере
            IoC<QueueThread>.Resolve("IoC.Registration", "ReservedQueue", new QueueThread());

            MockCommandDelay command1 = new MockCommandDelay(10);
            MockCommandDelay command2 = new MockCommandDelay(10);
            MockCommandDelay command3 = new MockCommandDelay(10);
            MockCommandDelay command4 = new MockCommandDelay(10);
            MockCommandDelay command5 = new MockCommandDelay(10);
            MockCommandDelay command6 = new MockCommandDelay(10);

            QueueThread queueCommand = new QueueThread();
            queueCommand.PushCommand(command1);
            queueCommand.PushCommand(command2);
            queueCommand.PushCommand(command3);
            queueCommand.PushCommand(command4);
            queueCommand.PushCommand(command5);
            queueCommand.PushCommand(command6);

            queueCommand.PushCommand(new ControlCommand(queueCommand.Start)); //Запуск очереди
            Thread.Sleep(25); //Ждем выполнения трех команд из 6 добавленных в очередь
            queueCommand.PushCommand(new ControlCommand(queueCommand.MoveTo)); //отправляем команду MoveTo
            Thread.Sleep(5);
            Assert.AreEqual(queueCommand.state.Handle(), StateEnum.MoveTo); //проверяем что состояние очереди MoveTo

            Thread.Sleep(60);
            Assert.IsTrue(command1.CommandIsComplited()); //проверяем выполнение первой команды
            Assert.IsTrue(command2.CommandIsComplited()); //проверяем выполнение второй команды
            Assert.IsTrue(command3.CommandIsComplited()); //проверяем выполнение третьей команды

            Assert.IsFalse(command4.CommandIsComplited()); //а эти команды не выполнились, потому что были перегружены в резервную очерель
            Assert.IsFalse(command5.CommandIsComplited()); //а эти команды не выполнились, потому что были перегружены в резервную очерель
            Assert.IsFalse(command6.CommandIsComplited()); //а эти команды не выполнились, потому что были перегружены в резервную очерель

            QueueThread reservedQueue = IoC<QueueThread>.Resolve("ReservedQueue"); //получаем резервную очередь из контейнера
            reservedQueue.PushCommand(new ControlCommand(reservedQueue.Start)); //Запусскаем резервную очередь

            Thread.Sleep(1000);
            Assert.AreEqual(reservedQueue.state.Handle(), StateEnum.Normal);

            Assert.IsTrue(command4.CommandIsComplited()); //Проверяем выполнение перегруженных команд в резервной очереди
            Assert.IsTrue(command5.CommandIsComplited()); //Проверяем выполнение перегруженных команд в резервной очереди
            Assert.IsTrue(command6.CommandIsComplited()); //Проверяем выполнение перегруженных команд в резервной очереди

            reservedQueue.PushCommand(new ControlCommand(reservedQueue.HardStop)); //останавливаем резервную очередь
            Thread.Sleep(5);
            Assert.AreEqual(reservedQueue.state.Handle(), StateEnum.HardStoped);
        }
    }
}
