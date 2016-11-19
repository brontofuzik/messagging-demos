using MassTransit;
using Messages;

namespace RequestResponse.Responder
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
                busCfg.ReceiveFrom("msmq://localhost/message_responder");

                // Receive response
                busCfg.Subscribe(subs =>
                {
                    subs.Handler<BasicRequest>((context, message) =>
                    {
                        context.Respond(new BasicResponse { Text = "RESP" + message.Text });
                    });
                });
            });
        }
    }
}
