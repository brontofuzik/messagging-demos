using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Receiver
{
    class Receiver
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

                // Register consumer
                var consumer = new QueueingBasicConsumer(model);
                model.BasicConsume(queue: "hello", noAck: true, consumer: consumer);

                Console.WriteLine(" [*] Waiting for messages. To exit press Ctrl+C");
                    
                while (true)
                {
                    // Receive message
                    BasicDeliverEventArgs message = consumer.Queue.Dequeue();
                    var text = Encoding.UTF8.GetString(message.Body);

                    Console.WriteLine(" [x] Received {0}", text);
                }
            }
        }
    }
}
