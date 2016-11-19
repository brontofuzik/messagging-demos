using System;
using EasyNetQ;
using EasyNetQ.Loggers;
using Messages;
using Messages.Animals;

namespace Publisher
{
    class Publisher
    {
        static void Main(string[] args)
        {
            using (var bus = RabbitHutch.CreateBus("host=GBRDSM020001584;username=demo;password=demo", serviceRegister =>
                serviceRegister.Register<IEasyNetQLogger>(serviceProvider => new ConsoleLogger())))
            {

                Publish_PolymorphicMessage(bus);
            }
        }

        private static void Publish_SimpleMessage(IBus bus)
        {
            Console.WriteLine("Enter a message. 'Quit' to quit.");

            var input = String.Empty;
            while ((input = Console.ReadLine()) != "Quit")
            {
                bus.Publish(new Message { Text = input });
            }
        }

        private static void Publish_PolymorphicMessage(IBus bus)
        {
            var cat = new Cat
            {
                Name = "Gobbolino",
                Meow = "Purr"
            };

            var dog = new Dog
            {
                Name = "Rover",
                Bark = "Woof"
            };

            bus.Publish<IAnimal>(cat);
            bus.Publish<IAnimal>(dog);
        }
    }
}
