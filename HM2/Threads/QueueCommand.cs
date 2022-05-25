using HM2.GameSolve.Interfaces;
using HM2.IoCs;
using HM2.Threads.Commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HM2.Threads
{
    
    public class QueueCommand
    {
        Queue<ICommand> dataQueue;
        public QueueCommand()
        {
            cycleIsRun = false;
            dataQueue = new Queue<ICommand>();
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
                dataQueue.Enqueue(command);
            }
        }

        Task dataCommandQueue;
        bool cycleIsRun;

        public bool TaskIsRun
        {
            get
            {
                return cycleIsRun;
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
        public event QueueHandler ComplitedThread;
        public event QueueHandler StartThread;

        void HardStopQueue()
        {
            dataQueue.Clear();
        }
        void SoftStopQueue()
        {
            ICommand softStopedCommand = new ControlCommand(() => {
                cycleIsRun = false;
            });
            dataQueue.Enqueue(softStopedCommand);
        }

        void StartDataQueue()
        {
            cycleIsRun = true;
            dataCommandQueue = new Task(() =>
            {
                StartThread?.Invoke();

                while (cycleIsRun)
                {
                    if (dataQueue.Count > 0)
                        dataQueue.Dequeue().Execute();
                }

                ComplitedThread?.Invoke();
            });
            dataCommandQueue.Start();
        }
    }
}
