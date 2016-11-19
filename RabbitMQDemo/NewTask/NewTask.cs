using System;
using RabbitMQ.Client;
using System.Text;

namespace NewTask
{
    class NewTask
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "GBRDSM020001584",
                UserName = "demo",
                Password = "demo"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Declare queue  (durable)
                channel.QueueDeclare(queue: "task_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                // Message properties (persistent)
                var properties = channel.CreateBasicProperties();
                properties.SetPersistent(true);

                // Send message
                var text = GetMessage(args);
                channel.BasicPublish(exchange: "", routingKey: "task_queue", basicProperties: properties, body: Encoding.UTF8.GetBytes(text));

                Console.WriteLine(" [x] Sent {0}", text);
            }
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }
    }
}
