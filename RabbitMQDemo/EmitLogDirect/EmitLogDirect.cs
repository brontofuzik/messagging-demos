using System;
using System.Linq;
using RabbitMQ.Client;
using System.Text;

namespace EmitLogDirect
{
    class EmitLogDirect
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
                channel.ExchangeDeclare(exchange: "direct_logs", type: "direct");

                // Send message
                string severity = (args.Length > 0) ? args[0] : "info";
                string text = (args.Length > 1) ? string.Join(" ", args.Skip(1).ToArray()) : "Hello World!";
                channel.BasicPublish(exchange: "direct_logs", routingKey: severity, basicProperties: null, body: Encoding.UTF8.GetBytes(text));

                Console.WriteLine(" [x] Sent '{0}':'{1}'", severity, text);
            }
        }
    }
}
