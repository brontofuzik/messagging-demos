using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ReceiveLogsDirect
{
    class ReceiveLogsDirect
    {
        public static void Main(string[] args)
        {
            // Check args
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Usage: {0} [info] [warning] [error]", Environment.GetCommandLineArgs()[0]);
                Environment.ExitCode = 1;
                return;
            }

            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "GBRDSM020001584",
                UserName = "demo",
                Password = "demo"
            };

            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                // Declare exchange
                channel.ExchangeDeclare(exchange: "direct_logs", type: "direct");

                // Bind queue
                var queueName = channel.QueueDeclare().QueueName;
                foreach (var severity in args)
                {
                    channel.QueueBind(queue: queueName, exchange: "direct_logs", routingKey: severity);
                }

                // Register consumer
                QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume(queue: queueName, noAck: true, consumer: consumer);

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
