using System.Threading.Tasks;

namespace Jai.AzServiceBus.Messaging
{
    public interface IMessageProcessor<TMessage>
    {
        Task ProcessMessageAsync(TMessage message, MessageProcessorContext messageProcessorContext);
    }
}
