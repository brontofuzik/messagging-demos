using EasyNetQ;
using EasyNetQ.Loggers;
using Messages;

namespace Sender
{
    class Sender
    {
        static void Main(string[] args)
        {
            using (var bus = RabbitHutch.CreateBus("host=GBRDSM020001584;username=demo;password=demo", serviceRegister =>
                serviceRegister.Register<IEasyNetQLogger>(serviceProvider => new ConsoleLogger())))
            {
                bus.Send("my.queue", new Message { Text = "Hello Widgets!" });
            }
        }
    }
}
