using Microsoft.Azure.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Jai.AzServiceBus.Messaging
{
    public class MessageDespatchingHandler : IMessageHandler
    {
        private IServiceProvider serviceProvider;
        private MessageProcessorInvokerProvider messageProcessorInvokerProvider;
        private IMessageSerializer messageSerializer;
        public MessageDespatchingHandler(IServiceProvider serviceProvider, IMessageSerializer messageSerializer)
        {
            this.serviceProvider = serviceProvider;
            this.messageSerializer = messageSerializer;
            this.messageProcessorInvokerProvider = new MessageProcessorInvokerProvider();
        }
        public Task HandleMessage(Message message, CancellationToken cancellationToken)
        {
            return DespatchMessageAsync(message);
        }
        private Task DespatchMessageAsync(Message message)
        {
            var messageType = Type.GetType(message.UserProperties["EnclosedType"] as string);
            var messageProcessorInvoker = messageProcessorInvokerProvider.GetMessageProcessorInvoker(messageType);
            
            var messageProcessorImpl = serviceProvider.GetService(messageProcessorInvoker.MessageProcessorInterfaceType);
            var messageToBeProcessed = messageSerializer.Deserialize(message.Body, messageType);
            var messageProcessorContext = MessageProcessorContext.Create(message);
            return messageProcessorInvoker.Invoke(messageProcessorImpl, messageToBeProcessed, messageProcessorContext);
        }
    }
}
