using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ReceiveLogsTopic
{
    class ReceiveLogsTopic
    {
        public static void Main(string[] args)
        {
            // Check args
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Usage: {0} [binding_key...]", Environment.GetCommandLineArgs()[0]);
                Environment.ExitCode = 1;
                return;
            }

            var factory = new ConnectionFactory()
            {
                HostName = "GBRDSM020001584",
                UserName = "demo",
                Password = "demo"
            };

            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                // Declare exchange
                channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");

                // Bind queue
                var queueName = channel.QueueDeclare().QueueName;
                foreach (var bindingKey in args)
                {
                    channel.QueueBind(queue: queueName, exchange: "topic_logs", routingKey: bindingKey);
                }

                // Register consumer
                var consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume(queueName, true, consumer);

                Console.WriteLine(" [*] Waiting for messages. To exit press Ctrl+C");

                while (true)
                {
                    BasicDeliverEventArgs message = consumer.Queue.Dequeue();
                    string routingKey = message.RoutingKey;
                    string text = Encoding.UTF8.GetString(message.Body);

                    Console.WriteLine(" [x] Received '{0}':'{1}'", routingKey, text);
                }
            }
        }
    }
}
