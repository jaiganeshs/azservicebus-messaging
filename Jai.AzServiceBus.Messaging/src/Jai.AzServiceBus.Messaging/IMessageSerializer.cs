using System;

namespace Jai.AzServiceBus.Messaging
{
    public interface IMessageSerializer
    {
        object Deserialize(byte[] body, Type messageType);
        TMessage Deserialize<TMessage>(byte[] body);
        byte[] Serialize(object message);
    }
}