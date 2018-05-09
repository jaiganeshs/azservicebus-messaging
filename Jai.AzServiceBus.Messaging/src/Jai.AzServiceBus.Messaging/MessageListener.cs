using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Jai.AzServiceBus.Messaging
{
    public class MessageListener : IMessageListener
    {
        private IMessageReceiver messageReceiver;
        private IMessageHandler messageHandler;
        private MessageListenerConfig messageListenerConfig;
        public MessageListener(MessageListenerConfig messageListenerConfig, IMessageHandler messageHandler)
        {
            this.messageHandler = messageHandler;
            this.messageListenerConfig = messageListenerConfig;
        }
        public void Start()
        {
            messageReceiver = new MessageReceiver(messageListenerConfig.ConnectionString, messageListenerConfig.EntityPath);
            MessageHandlerOptions messageHandlerOptions = new MessageHandlerOptions(HandleException)
            {
                AutoComplete = false,
                MaxConcurrentCalls = messageListenerConfig.MaxConcurrentCalls,
                MaxAutoRenewDuration = messageListenerConfig.MaxAutoRenewDuration
            };
            messageReceiver.RegisterMessageHandler(HandleMessage, messageHandlerOptions);
        }
        private async Task HandleMessage(Message message, CancellationToken cancellationToken)
        {
            //Console.WriteLine($"Handled message id {message.MessageId}, diagnostic id {message.UserProperties["Diagnostic-Id"]}, enclosed type {message.UserProperties["EnclosedType"]}");
            try
            {
                await messageHandler.HandleMessage(message, cancellationToken);
                await messageReceiver.CompleteAsync(message.SystemProperties.LockToken);
            }
            catch (Exception ex)
            {
                Dictionary<string, object> abandonReasons = new Dictionary<string, object>
                {
                    { "Exception.Message", ex.Message },
                    { "Exception.StackTrace", ex.ToString()}
                };
                await messageReceiver.AbandonAsync(message.SystemProperties.LockToken, abandonReasons);
            }
        }
        private Task HandleException(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            return Task.CompletedTask;
        }
    }
}
