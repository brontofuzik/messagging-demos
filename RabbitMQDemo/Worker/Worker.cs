using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;

namespace Worker
{
    // The assumption behind a work queue is that each task is delivered to exactly one worker.
    class Worker
    {
        public static void Main()
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
                // Declare queue (durable)
                channel.QueueDeclare(queue: "task_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                // QoS
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                // Register consumer
                var consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume(queue: "task_queue", noAck: false, consumer: consumer);

                Console.WriteLine(" [*] Waiting for messages. To exit press Ctrl+C");
                    
                while (true)
                {
                    // Receive message
                    BasicDeliverEventArgs message = consumer.Queue.Dequeue();
                    var text = Encoding.UTF8.GetString(message.Body);

                    Console.WriteLine(" [x] Received {0}", text);

                    // Do work
                    int dots = text.Split('.').Length - 1;
                    Thread.Sleep(dots * 1000);

                    Console.WriteLine(" [x] Done");

                    // Ack
                    channel.BasicAck(deliveryTag: message.DeliveryTag, multiple: false);
                }
            }
        }
    }
}
