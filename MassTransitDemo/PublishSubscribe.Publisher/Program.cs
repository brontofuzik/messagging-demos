using System;
using MassTransit;
using MassTransit.Context;
using Messages;

namespace PublishSubscribe.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Bus.Initialize(busCfg =>
            {
                // MSMQ
                busCfg.UseMsmq(mqConfig =>
                {
                    mqConfig.UseSubscriptionService("msmq://localhost/mt_subscriptions");
                });
                busCfg.ReceiveFrom("msmq://localhost/Publisher");
                //busCfg.VerifyMsmqConfiguration(); This doesn't work on Windows 8.
            });

            var message = new BasicMessage() { Text = "Hello World!" };
            var sendContext = new SendContext<BasicMessage>(message);
            sendContext.SetMessageId(Guid.NewGuid().ToString());
            sendContext.SetCorrelationId(Guid.NewGuid().ToString());
            sendContext.SetExpirationTime(DateTime.Now.AddDays(1));

            Bus.Instance.Publish<BasicMessage>(message);
        }
    }
}
