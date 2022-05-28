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
        public EndPointNetServer(string ipaddres, int port)
        {
            address = IPAddress.Parse(ipaddres);
            this.port = port;
        }

        public void Run()
        {
            var tcpEndPoint = new IPEndPoint(address, port);
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(1);    //число клиентов

            while (true)
            {
                var listener = tcpSocket.Accept();
                var buffer = new byte[256];
                var size = 0;
                var data = new StringBuilder();

                do
                {
                    size = listener.Receive(buffer);
                    data.Append(Encoding.UTF8.GetString(buffer, 0, size));
                }
                while (listener.Available > 0);

                listener.Send(Encoding.UTF8.GetBytes("Complited"));

                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
        }
    }
}
