using Newtonsoft.Json;
using System;
using System.Text;

namespace Jai.AzServiceBus.Messaging
{
    public class MessageSerializer : IMessageSerializer
    {
        public object Deserialize(byte[] body, Type messageType)
        {
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(body), messageType);
        }
        public TMessage Deserialize<TMessage>(byte[] body)
        {
            return JsonConvert.DeserializeObject<TMessage>(Encoding.UTF8.GetString(body));
        }
        public byte[] Serialize(object message)
        {
            var messageJson = JsonConvert.SerializeObject(message);
            return Encoding.UTF8.GetBytes(messageJson);
        }
    }
}
