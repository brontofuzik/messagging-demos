using System;
using MassTransit;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Bus.Initialize(busCfg =>
            {
                // MSMQ
                busCfg.UseMsmq(mqCfg =>
                {
                    mqCfg.VerifyMsmqConfiguration();
                    mqCfg.UseMulticastSubscriptionClient();
                });
                busCfg.ReceiveFrom("msmq://localhost/test_queue");
                
                // Subscribe/Receive
                busCfg.Subscribe(subs =>
                {
                    subs.Handler<Message>(message => Console.WriteLine(message.Text));
                });
            });

            // Publish
            Bus.Instance.Publish(new Message { Text = "Hi" });
        }
    }

    public class Message
    {
        public string Text { get; set; }
    }
}
