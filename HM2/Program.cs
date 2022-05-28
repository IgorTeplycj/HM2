using EndPointMessage;
using HM2.EndPoint;
using HM2.EndPoint.Commands;
using HM2.GameSolve.Interfaces;
using HM2.GameSolve.Structures;
using HM2.IoCs;
using HM2.MovableObject;
using HM2.Threads;
using HM2.Threads.Commands;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HM2
{
    class Program
    {
        static void Main(string[] args)
        {
            Vector vector = new Vector();
            vector.Shift = new Coordinats { X = 5.0, Y = 7.0 };
            vector.DirectionNumber = 1;

            var vr = JsonSerializer.Serialize<Vector>(vector);

            var vextDes = JsonSerializer.Deserialize<Vector>(vr);
        }
    }
}
