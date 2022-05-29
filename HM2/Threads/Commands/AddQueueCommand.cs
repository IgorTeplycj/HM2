using HM2.GameSolve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.Threads.Commands
{
    public class AddQueueCommand : ICommand
    {
        ICommand addCommand;
        QueueCommand targetQueue;
        public AddQueueCommand(ICommand addCommand, QueueCommand targetQueue)
        {
            this.addCommand = addCommand;
            this.targetQueue = targetQueue;
        }
        public void Execute()
        {
            targetQueue.PushCommand(addCommand);
        }
    }
}
