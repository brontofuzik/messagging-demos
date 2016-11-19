using System;
using RabbitMQ.Client;
using System.Text;

namespace Sender
{
    class Sender
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "GBRDSM020001584",
                UserName = "demo",
                Password = "demo"
            };

            using (IConnection connection = factory.CreateConnection())
            using (IModel model = connection.CreateModel())
            {
                // Declare queue
                model.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

                // Send message
                string text = "Hello World!";
                model.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: Encoding.UTF8.GetBytes(text));
                    
                Console.WriteLine(" [x] Sent {0}", text);
            }
        }
    }
}
