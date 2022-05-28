using EndPointMessage;
using HM2.EndPoint;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HM2.Server.Tests
{
    public class EndPointTests
    {
        const string ipAddr = "127.0.0.1";
        const int port = 8080;

       // [SetUp]
        public void StartServer()
        {
            Task Server = new Task(() =>
            {
                EndPointNetServer endPointServer = new EndPointNetServer(ipAddr, port);
                endPointServer.Run();
            });
            Server.Start();
        }

        //[Test]
        public void TestSendMessage()
        {
            Message message = new Message("Game12345", "GameObj12345", "124578", "GameArgs");


        }


        void CreateClientAndSendMessage(string msg)
        {
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ipAddr), port);
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            var data = Encoding.UTF8.GetBytes(msg);

            tcpSocket.Connect(tcpEndPoint);
            tcpSocket.Send(data);

            var buffer = new byte[1024];
            var size = 0;
            var answer = new StringBuilder();

            do
            {
                size = tcpSocket.Receive(buffer);
                answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
            }
            while (tcpSocket.Available > 0);

            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();
        }
    }
}