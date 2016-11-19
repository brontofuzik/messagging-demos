using System;
using RabbitMQ.Client;
using System.Text;

namespace EmitLog
{
    class EmitLog
    {
        public static void Main(string[] args)
        {
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
                channel.ExchangeDeclare(exchange: "logs", type: "fanout");

                // Send message
                string text = GetMessage(args);
                channel.BasicPublish(exchange: "logs", routingKey: "IGNORED", basicProperties: null, body: Encoding.UTF8.GetBytes(text));

                Console.WriteLine(" [x] Sent {0}", text);
            }
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "info: Hello World!");
        }
    }
}
