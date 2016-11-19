using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RpcServer
{
    class RpcServer
    {
        public static void Main()
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
                // Declare queue
                channel.QueueDeclare(queue: "rpc_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                // QoS
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                
                // Register consumer
                var consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume(queue: "rpc_queue", noAck: false, consumer: consumer);

                Console.WriteLine(" [x] Awaiting RPC requests");

                while (true)
                {
                    // Receive request
                    BasicDeliverEventArgs request = consumer.Queue.Dequeue();
                    var requestText = Encoding.UTF8.GetString(request.Body);
                    var requestProperties = request.BasicProperties;

                    // Reply message
                    string replyText = null;
                    IBasicProperties replyProperties = channel.CreateBasicProperties();
                    replyProperties.CorrelationId = requestProperties.CorrelationId;
                    
                    try
                    {              
                        int n = Int32.Parse(requestText);
                        Console.WriteLine(" [.] Fibonacci({0})", requestText);
                        replyText = Fibonacci(n).ToString();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" [.] " + e.Message);
                        replyText = "";
                    }
                    finally
                    {
                        // Send reply
                        channel.BasicPublish(exchange: "", routingKey: requestProperties.ReplyTo, basicProperties: replyProperties, body: Encoding.UTF8.GetBytes(replyText));

                        // Ack
                        channel.BasicAck(deliveryTag: request.DeliveryTag, multiple: false);
                    }
                }
            }
        }

        private static int Fibonacci(int n)
        {
            if (n == 0 || n == 1) return n;
            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }
    }
}
