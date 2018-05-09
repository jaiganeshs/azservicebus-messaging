using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace Jai.AzServiceBus.Messaging
{
    public interface IMessageHandler
    {
        Task HandleMessage(Message message, CancellationToken cancellationToken);
    }
}