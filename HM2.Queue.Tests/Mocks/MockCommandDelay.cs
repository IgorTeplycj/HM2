using HM2.GameSolve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HM2.Queue.Tests.Mocks
{
    class MockCommandDelay : ICommand
    {
        int _dalay_ms = 0;
        public MockCommandDelay(int delay_ms)
        {
            _dalay_ms = delay_ms;
        }
        bool commandIsComplited = false;
        public bool CommandIsComplited()
        {
            return commandIsComplited;
        }
        public void Execute()
        {
            Thread.Sleep(_dalay_ms);
            commandIsComplited = true;
        }
    }
}
