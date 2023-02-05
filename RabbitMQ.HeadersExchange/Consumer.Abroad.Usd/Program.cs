using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Consumer.Abroad.Usd
{
    class Program
    {
        public static void Main()
        {
            Console.Title = "Consumer: Counts abroad USD transfers only";

            int counter = 0;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "headers_ex", type: ExchangeType.Headers);
                Dictionary<string, object> headers = new Dictionary<string, object>();
                headers.Add("x-match", "all"); 
                headers.Add("currency", "usd");
                headers.Add("transfer", "abroad");

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName,
                                         exchange: "headers_ex",
                                         routingKey: "-",
                                         arguments: headers);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, e) =>
                {
                    var body = e.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());

                    counter++;

                    Console.WriteLine($"Received message: {message} {Environment.NewLine}" +
                        $"Total abroad USD transfers: {counter}");
                };

                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine($"Subscribed to the queue '{queueName}'");
                Console.WriteLine($"Counts abroad USD transfers only");

                Console.ReadLine();
            }
        }
    }
}