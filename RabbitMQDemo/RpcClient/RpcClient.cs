using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RpcClient
{
    class RPC
    {
        public static void Main()
        {
            var rpcClient = new RpcClient();

            Console.WriteLine(" [x] Requesting fib(30)");

            string response = rpcClient.Call("30");

            Console.WriteLine(" [.] Got '{0}'", response);

            rpcClient.Close();
        }
    }

    class RpcClient
    {
        private IConnection connection;
        private IModel channel;
        private string replyQueueName;
        private QueueingBasicConsumer consumer;

        public RpcClient()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "GBRDSM020001584",
                UserName = "demo",
                Password = "demo"
            };

            // Open connection & channel
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            // Declare reply queue
            replyQueueName = channel.QueueDeclare().QueueName;

            // Register consumer with the reply queue
            consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(queue: replyQueueName, noAck: true, consumer: consumer);
        }

        public string Call(string text)
        {
            string correlationId = Guid.NewGuid().ToString();
            
            // Message properties (reply queue & correlation id)
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.ReplyTo = replyQueueName;
            properties.CorrelationId = correlationId;

            // Send request
            channel.BasicPublish(exchange: "", routingKey: "rpc_queue", basicProperties: properties, body: Encoding.UTF8.GetBytes(text));

            while (true)
            {
                // Receive reply
                BasicDeliverEventArgs message = consumer.Queue.Dequeue();

                // Check correlation id
                if (message.BasicProperties.CorrelationId == correlationId)
                {
                    return Encoding.UTF8.GetString(message.Body);
                }
            }
        }

        public void Close()
        {
            channel.Close();
            connection.Close();
        }
    }
}
