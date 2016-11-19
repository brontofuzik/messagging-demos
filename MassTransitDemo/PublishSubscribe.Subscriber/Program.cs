using System;
using System.Threading;
using MassTransit;
using Messages;

namespace PublishSubscribe.Subscriber
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
                busCfg.ReceiveFrom("msmq://localhost/Subscriber");
                //busCfg.VerifyMsmqConfiguration(); This doesn't work on Windows 8.

                busCfg.Subscribe(subs =>
                {
                    subs.Handler<BasicMessage>(message => Console.WriteLine(message.Text));
                });
            });

            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
