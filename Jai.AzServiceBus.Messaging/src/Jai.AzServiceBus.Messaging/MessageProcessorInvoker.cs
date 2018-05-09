using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Jai.AzServiceBus.Messaging
{
    public class MessageProcessorInvoker
    {
        private Type messageType;
        private Func<object, object, MessageProcessorContext, Task> processMethod;
        public MessageProcessorInvoker(Type messageType)
        {
            this.messageType = messageType;
            MessageProcessorInterfaceType = typeof(IMessageProcessor<>).MakeGenericType(messageType);
            processMethod = GetProcessMethod(messageType, typeof(IMessageProcessor<>));
        }

        public Type MessageProcessorInterfaceType { get; private set; }

        public Task Invoke(object messageProcessor, object message, MessageProcessorContext messageProcessorContext)
        {
            return processMethod(messageProcessor, message, messageProcessorContext);
        }

        static Func<object, object, MessageProcessorContext, Task> GetProcessMethod(Type messageType, Type interfaceGenericType)
        {
            var interfaceType = interfaceGenericType.MakeGenericType(messageType);
            var methodInfo = interfaceType.GetMethods().FirstOrDefault();
            if (methodInfo == null)
            {
                return null;
            }

            var target = Expression.Parameter(typeof(object));
            var messageParam = Expression.Parameter(typeof(object));
            var contextParam = Expression.Parameter(typeof(MessageProcessorContext));

            var castTarget = Expression.Convert(target, interfaceType);
            var methodParameters = methodInfo.GetParameters();
            var messageCastParam = Expression.Convert(messageParam, methodParameters.ElementAt(0).ParameterType);
            Expression body = Expression.Call(castTarget, methodInfo, messageCastParam, contextParam);
            return Expression.Lambda<Func<object, object, MessageProcessorContext, Task>>(body, target, messageParam, contextParam).Compile();
        }
    }
}
