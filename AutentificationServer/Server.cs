using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AutentificationServer
{
    public class Server
    {
        IPAddress address;
        int port;

        IPEndPoint tcpEndPoint;
        Socket tcpSocket;
        Task serv;
        public Server(string ipaddres, int port)
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
                tcpSocket.Listen(10);    //число клиентов

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
    }
}
