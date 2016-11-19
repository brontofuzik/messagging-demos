using System;
using MassTransit;
using Messages;

namespace RequestResponse.Requestor
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
                busCfg.ReceiveFrom("msmq://localhost/message_requestor");
            });

            // Send request
            Bus.Instance.PublishRequest(new BasicRequest(), requestCfg =>
            {
                requestCfg.Handle<BasicResponse>(message => Console.WriteLine(message.Text));
                requestCfg.SetTimeout(TimeSpan.FromSeconds(30));
            });
        }
    }
}
