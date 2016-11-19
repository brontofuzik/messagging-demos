using System;
using EasyNetQ;
using EasyNetQ.Loggers;
using Messages;

namespace Requestor
{
    class Requestor
    {
        static void Main(string[] args)
        {
            using (var bus = RabbitHutch.CreateBus("host=GBRDSM020001584;username=demo;password=demo", serviceRegister =>
                serviceRegister.Register<IEasyNetQLogger>(serviceProvider => new ConsoleLogger())))
            {
                Send_Sync(bus);
                //Send_Async(bus);
            }
        }

        private static void Send_Sync(IBus bus)
        {
            // Sync
            var request = new Request { Text = "Hello Server" };
            var response = bus.Request<Request, Response>(request);
            Console.WriteLine(response.Text);
        }

        private static void Send_Async(IBus bus)
        {
            // Async
            var request = new Request { Text = "Hello Server" };
            bus.RequestAsync<Request, Response>(request)
                .ContinueWith(resp => Console.WriteLine("Got response: '{0}'", resp.Result.Text));
        }
    }
}
