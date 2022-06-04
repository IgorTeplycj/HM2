using HM2.GameSolve.Interfaces;
using HM2.Threads.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.State.States
{
    public class SoftStopState : IState
    {
        Queue<ICommand> queue;
        public SoftStopState(Queue<ICommand> queue, Action stopExecute)
        {
            this.queue = queue;

            ICommand softStopedCommand = new ControlCommand(() =>
            {
                stopExecute.Invoke();
            });
            queue.Enqueue(softStopedCommand);
        }
        public void Execute()
        {
            queue.Dequeue().Execute();
        }

        public StateEnum Handle()
        {
            return StateEnum.SoftStopped;
        }
    }
}
