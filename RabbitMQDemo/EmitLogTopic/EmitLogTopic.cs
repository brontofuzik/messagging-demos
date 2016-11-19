using System;
using System.Linq;
using RabbitMQ.Client;
using System.Text;

namespace EmitLogTopic
{
    class EmitLogTopic
    {
        public static void Main(string[] args)
        {
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

                // Send message
                string routingKey = (args.Length > 0) ? args[0] : "anonymous.info";
                string text = (args.Length > 1) ? string.Join(" ", args.Skip(1).ToArray()) : "Hello World!";
                channel.BasicPublish(exchange: "topic_logs", routingKey: routingKey, basicProperties: null, body: Encoding.UTF8.GetBytes(text));

                Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, text);
            }
        }
    }
}
