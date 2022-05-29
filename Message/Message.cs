using System;
using System.Text.Json;

namespace EndPointMessage
{
    public class Message
    {
        public Message(string IdGame, string IdGameObject, string IdCommand, string Args)
        {
            this.IdGame = IdGame;
            this.IdGameObject = IdGameObject;
            this.IdCommand = IdCommand;
            this.Args = Args;
        }
        public string IdGame { get; private set; }
        public string IdGameObject { get; private set; }
        public string IdCommand { get; private set; }
        public string Args { get; private set; }

        public string Ser { get; private set; }
        public string Serialized()
        {
            return JsonSerializer.Serialize<EndPointMessage.Message>(this);
        }

        public void Deserialized(string msg)
        {
            Message Deser = JsonSerializer.Deserialize<EndPointMessage.Message>(msg);
            this.IdGame = Deser.IdGame;
            this.IdGameObject = Deser.IdGameObject;
            this.IdCommand = Deser.IdCommand;
            this.Args = Deser.Args;
        }
    }
}
