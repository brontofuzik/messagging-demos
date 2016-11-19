using System;
using EasyNetQ;
using EasyNetQ.Loggers;
using Messages;
using Messages.Animals;

namespace Subscriber
{
    class Subscriber
    {
        static void Main(string[] args)
        {
            using (var bus = RabbitHutch.CreateBus("host=GBRDSM020001584;username=demo;password=demo", serviceRegister =>
                serviceRegister.Register<IEasyNetQLogger>(serviceProvider => new ConsoleLogger())))
            {
                Subscribe_PolymorphicMessage(bus);
            }
        }

        private static void Subscribe_SimpleMessage(IBus bus)
        {
            bus.Subscribe<Message>(subscriptionId: "test", onMessage: message =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Got message: {0}", message.Text);
                Console.ResetColor();
            });

            Console.WriteLine("Listening for messages. Hit <return> to quit.");
            Console.ReadLine();
        }

        private static void Subscribe_PolymorphicMessage(IBus bus)
        {
            bus.Subscribe<IAnimal>("polymorphic_test", @interface =>
            {
                var cat = @interface as Cat;
                var dog = @interface as Dog;

                if (cat != null)
                {
                    Console.Out.WriteLine("Name = {0}", cat.Name);
                    Console.Out.WriteLine("Meow = {0}", cat.Meow);
                }
                else if (dog != null)
                {
                    Console.Out.WriteLine("Name = {0}", dog.Name);
                    Console.Out.WriteLine("Bark = {0}", dog.Bark);
                }
                else
                {
                    Console.Out.WriteLine("message was not a dog or a cat");
                }
            });

            Console.WriteLine("Listening for messages. Hit <return> to quit.");
            Console.ReadLine();
        }
    }
}
