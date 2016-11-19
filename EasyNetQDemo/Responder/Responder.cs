using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Loggers;
using Messages;

namespace Responder
{
    class Responder
    {
        static void Main(string[] args)
        {
            using (var bus = RabbitHutch.CreateBus("host=GBRDSM020001584;username=demo;password=demo", serviceRegister =>
                serviceRegister.Register<IEasyNetQLogger>(serviceProvider => new ConsoleLogger())))
            {
                Respond_Sync(bus);
                //Respond_Async(bus);
            }
        }

        private static void Respond_Sync(IBus bus)
        {
            bus.Respond<Request, Response>(req => new Response { Text = "Responding to " + req.Text });
        }

        private static void Respond_Async(IBus bus)
        {
            // Async
            var workers = new BlockingCollection<Worker>();
            for (int i = 0; i < 10; i++)
            {
                workers.Add(new Worker());
            }

            bus.RespondAsync<Request, Response>(request => Task<Response>.Factory.StartNew(() =>
            {
                var worker = workers.Take();
                try
                {
                    return worker.Execute(request);
                }
                finally
                {
                    workers.Add(worker);
                }
            }));
        }
    }
}
