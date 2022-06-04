using HM2.GameSolve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.State.States
{
    public class HardStopState : IState
    {
        Queue<ICommand> queue;
        public HardStopState(Queue<ICommand> queue, Action stopExecute)
        {
            this.queue = queue;
            stopExecute.Invoke();
        }

        public void Execute()
        {
            queue.Dequeue().Execute();
        }

        public StateEnum Handle()
        {
            return StateEnum.HardStoped;
        }
    }
}
