using HM2.GameSolve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.State.States
{
    public class NoRun : IState
    {
        Queue<ICommand> queue;
        public NoRun(Queue<ICommand> queue, Action normal)
        {
            this.queue = queue;
            normal?.Invoke();
        }

        public void Execute()
        {
            queue.Dequeue().Execute();
        }

        public StateEnum Handle()
        {
            return StateEnum.NoRun;
        }
    }
}
