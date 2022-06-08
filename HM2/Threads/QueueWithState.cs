using HM2.GameSolve.Interfaces;
using HM2.IoCs;
using HM2.Threads.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.Threads
{
    public class QueueWithState
    {
        Queue<ICommand> dataQueue = new Queue<ICommand>();
        public QueueWithState()
        {

        }


        Task dataCommandQueue;

        public void PushCommand(ICommand command)
        {
            command.Execute();
        }






        void StartDataQueue()
        {
            dataCommandQueue = new Task(() =>
            {
                if (dataQueue.Count > 0)
                {
                    dataQueue.Dequeue().Execute();
                }
            });
            dataCommandQueue.Start();
        }
    }
}
