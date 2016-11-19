using System;
using EasyNetQ;
using EasyNetQ.Loggers;
using Messages;

namespace Receiver
{
    class Receiver
    {
        static void Main(string[] args)
        {
            using (var bus = RabbitHutch.CreateBus("host=GBRDSM020001584;username=demo;password=demo", serviceRegister =>
                serviceRegister.Register<IEasyNetQLogger>(serviceProvider => new ConsoleLogger())))
            {
                bus.Receive<Message>("my.queue", message => Console.WriteLine("MyMessage: {0}", message.Text));
            }
        }
    }
}
