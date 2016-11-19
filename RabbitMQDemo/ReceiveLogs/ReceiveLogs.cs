using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ReceiveLogs
{
    class ReceiveLogs
    {
        public static void Main()
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

                // Bind queue
                // In the .NET client, when we supply no parameters to queueDeclare() we create a non-durable, exclusive, autodelete queue with a generated name.
                string queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName, exchange: "logs", routingKey: "");

                // Register consumer
                QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume(queue: queueName, noAck: true, consumer: consumer);

                Console.WriteLine(" [*] Waiting for logs. To exit press Ctrl+C");
                
                while (true)
                {
                    BasicDeliverEventArgs message = consumer.Queue.Dequeue();
                    string text = Encoding.UTF8.GetString(message.Body);

                    Console.WriteLine(" [x] {0}", text);
                }
            }
        }
    }
}
