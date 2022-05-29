using HM2.EndPoint.Commands;
using HM2.IoCs;
using HM2.Threads;
using HM2.Threads.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HM2.EndPoint
{
    public class EndPointNetServer
    {
        IPAddress address;
        int port;

        IPEndPoint tcpEndPoint;
        Socket tcpSocket;
        Task serv;
        public EndPointNetServer(string ipaddres, int port)
        {
            address = IPAddress.Parse(ipaddres);
            this.port = port;
        }

        public void Run()
        {
            serv = new Task(() =>
            {
                tcpEndPoint = new IPEndPoint(address, port);
                tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                tcpSocket.Bind(tcpEndPoint);
                tcpSocket.Listen(1);    //число клиентов

                while (true)
                {
                    var listener = tcpSocket.Accept();
                    var buffer = new byte[2048];
                    var size = 0;
                    var data = new StringBuilder();

                    do
                    {
                        size = listener.Receive(buffer);
                        data.Append(Encoding.UTF8.GetString(buffer, 0, size));
                    }
                    while (listener.Available > 0);

                    QueueAdd(data.ToString());

                    listener.Send(Encoding.UTF8.GetBytes("Complited"));

                    listener.Shutdown(SocketShutdown.Both);
                    listener.Close();
                }

            });
            serv.Start();
        }
        public void Close()
        {
            tcpSocket.Close();
            tcpSocket.Dispose();
        }

        void QueueAdd(string message)
        {
            new AddQueueCommand(new InterpretCommand(message), IoC<QueueCommand>.Resolve("Queue")).Execute();
        }
    }
}
