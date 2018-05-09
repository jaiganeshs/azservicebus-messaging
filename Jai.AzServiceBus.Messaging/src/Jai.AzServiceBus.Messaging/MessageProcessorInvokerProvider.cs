using System;
using System.Collections.Generic;

namespace Jai.AzServiceBus.Messaging
{
    public class MessageProcessorInvokerProvider
    {
        private Dictionary<Type, MessageProcessorInvoker> messageProcessorInvokers = new Dictionary<Type, MessageProcessorInvoker>();
        public MessageProcessorInvoker GetMessageProcessorInvoker(Type messageType)
        {
            if(messageProcessorInvokers.ContainsKey(messageType))
            {
                return messageProcessorInvokers[messageType];
            }
            var messageProcessorInvoker = new MessageProcessorInvoker(messageType);
            messageProcessorInvokers[messageType] = messageProcessorInvoker;
            return messageProcessorInvoker;
        }
    }
}
