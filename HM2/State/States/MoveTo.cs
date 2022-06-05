using HM2.GameSolve.Interfaces;
using HM2.IoCs;
using HM2.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.State.States
{
    public class MoveTo : IState
    {
        Queue<ICommand> queue;
        public MoveTo(Queue<ICommand> queue, Action MoveTo)
        {
            this.queue = queue;
            MoveTo?.Invoke();
        }

        public void Execute()
        {
            QueueThread reservedQueue = IoC<QueueThread>.Resolve("ReservedQueue");
            reservedQueue.PushCommand(queue);
            queue.Clear();
        }

        public StateEnum Handle()
        {
            return StateEnum.MoveTo;
        }
    }
}
