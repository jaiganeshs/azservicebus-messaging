using System;
using System.Collections.Generic;
using System.Text;

namespace Jai.AzServiceBus.Messaging
{
    public class MessageListenerConfig
    {
        public string ConnectionString { get; set; }
        public string EntityPath { get; set; }
        public TimeSpan MaxAutoRenewDuration { get; set; }
        public int MaxConcurrentCalls { get; set; }
    }
}
