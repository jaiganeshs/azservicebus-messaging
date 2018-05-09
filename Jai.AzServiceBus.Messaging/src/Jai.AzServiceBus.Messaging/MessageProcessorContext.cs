using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;

namespace Jai.AzServiceBus.Messaging
{
    public class MessageProcessorContext
    {
        public IDictionary<string, object> UserProperties { get; internal set; }
        public DateTime ScheduledEnqueueTimeUtc { get; internal set; }
        public string Label { get; internal set; }
        public string CorrelationId { get; internal set; }
        public string MessageId { get; internal set; }
        public DateTime ExpiresAtUtc { get; internal set; }
        public int DeliveryCount { get; internal set; }
        public long SequenceNumber { get; internal set; }
        public DateTime EnqueuedTimeUtc { get; internal set; }

        internal static MessageProcessorContext Create(Message message)
        {
            return new MessageProcessorContext
            {
                UserProperties = message.UserProperties,
                ScheduledEnqueueTimeUtc = message.ScheduledEnqueueTimeUtc,
                Label = message.Label,
                CorrelationId = message.CorrelationId,
                MessageId = message.MessageId,
                ExpiresAtUtc = message.ExpiresAtUtc,
                DeliveryCount = message.SystemProperties.DeliveryCount,
                SequenceNumber = message.SystemProperties.SequenceNumber,
                EnqueuedTimeUtc = message.SystemProperties.EnqueuedTimeUtc
            };
        }
    }
}
