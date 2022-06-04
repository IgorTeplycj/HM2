using HM2.GameSolve.Interfaces;
using HM2.IoCs;
using HM2.State;
using HM2.State.States;
using HM2.Threads.Commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HM2.Threads
{
    public class QueueThread
    {
        public IState state;
        Queue<ICommand> queue;
        public QueueThread()
        {
            // Регистрация признаков команд, управляющих очередью
            Func<ICommand, bool> isControlCommand = (c) =>
            {
                List<int> controlComands = new List<int>
                {
                    typeof(ControlCommand).GetHashCode(),
                };

                return controlComands.Contains(c.GetType().GetHashCode());
            };
            IoCs.IoC<Func<ICommand, bool>>.Resolve("IoC.Registration", "IsControlCommand", isControlCommand);

            taskIsRun = false;
            queue = new Queue<ICommand>();
            Start = StartDataQueue;
            HardStop = HardStopQueue;
            SoftStop = SoftStopQueue;
        }
        public void PushCommand(ICommand command)
        {
            if (IoC<Func<ICommand, bool>>.Resolve("IsControlCommand").Invoke(command))
            {
                command.Execute();
            }
            else
            {
                queue.Enqueue(command);
            }
        }

        Task dataCommandQueue;

        bool taskIsRun;
        public bool TaskIsRun
        {
            get
            {
                return taskIsRun;
            }
            private set
            {
                taskIsRun = value;
            }
        }

        /// <summary>
        /// Делегат для создания управляющей команды старта
        /// </summary>
        public Action Start;
        /// <summary>
        /// Делегат для создания управляющей команды остановки HardStop
        /// </summary>
        public Action HardStop;
        /// <summary>
        /// Делегат для создания управляющей команды остановки SoftStop
        /// </summary>
        public Action SoftStop;

        public delegate void QueueHandler();
        /// <summary>
        /// Обработчик события завершения цикла
        /// </summary>
        public event QueueHandler ComplitedThread;
        /// <summary>
        /// Оработчик события старта цикла 
        /// </summary>
        public event QueueHandler StartThread;

        void HardStopQueue()
        {
            state = new HardStopState(queue, () => { queue.Clear(); taskIsRun = false; });
        }
        void SoftStopQueue()
        {
            state = new SoftStopState(queue, () => { TaskIsRun = false; });
        }

        void StartDataQueue()
        {
            //Устанавливаем состояние Normal
            state = new Normal(queue, null);

            taskIsRun = true;
            dataCommandQueue = new Task(() =>
            {
                StartThread?.Invoke();

                while (taskIsRun)
                {
                    if (queue.Count > 0)
                    {
                        state.Execute();
                    }    
                }
                ComplitedThread?.Invoke();
            });
            dataCommandQueue.Start();
        }



    }
}
