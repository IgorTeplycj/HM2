using EndPointMessage;
using HM2.EndPoint.Commands;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.Server.Tests
{
    class CommandsTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void CommandSerializedTest()
        {
            StringBuilder sb = new StringBuilder();
            Message message = new Message("Game12345", "GameObj12345", "124578", "GameArgs");

            SerializeMessageCommands serializeMessageCommands = new SerializeMessageCommands(message, sb);

            Assert.AreEqual(sb.ToString(), "");

            serializeMessageCommands.Execute();

            string etalon = @"{""IdGame"":""Game12345"",""IdGameObject"":""GameObj12345"",""IdCommand"":""124578"",""Args"":""GameArgs"",""Ser"":null}";
            Assert.AreEqual(sb.ToString(), etalon);
        }
        [Test]
        public void CommandDeserializedTest()
        {
            string msg = @"{""IdGame"":""Game12345"",""IdGameObject"":""GameObj12345"",""IdCommand"":""124578"",""Args"":""GameArgs"",""Ser"":null}";

            Message message = new Message(null, null, null, null);
            DeserializeMessageCommand deserializeMessageCommand = new DeserializeMessageCommand(message, new StringBuilder().Append(msg));

            Assert.AreEqual(message.IdGame, null);
            Assert.AreEqual(message.IdGameObject, null);
            Assert.AreEqual(message.IdCommand, null);
            Assert.AreEqual(message.Args, null);

            deserializeMessageCommand.Execute();

            Assert.AreEqual(message.IdGame, "Game12345");
            Assert.AreEqual(message.IdGameObject, "GameObj12345");
            Assert.AreEqual(message.IdCommand, "124578");
            Assert.AreEqual(message.Args, "GameArgs");
        }
    }
}
