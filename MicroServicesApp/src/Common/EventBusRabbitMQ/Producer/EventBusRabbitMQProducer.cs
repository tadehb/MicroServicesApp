using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace EventBusRabbitMQ.Producer
{
    public class EventBusRabbitMQProducer
    {
        private readonly IRabbitMQConnection _connection;

        public EventBusRabbitMQProducer(IRabbitMQConnection connection)
        {
            _connection = connection;
        }

        public void PublisBasketCheckout(string queueName, BasketCheckoutEvent publishModel)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var message = JsonConvert.SerializeObject(publishModel);
                var body = Encoding.UTF8.GetBytes(message);


                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;

                channel.ConfirmSelect();
                // Create Exchange

                channel.ExchangeDeclare("demoExchange", ExchangeType.Direct);

                Console.WriteLine("Creating Exchange");



                // Create Queue

                channel.QueueDeclare("demoqueue", true, false, false, null);

                Console.WriteLine("Creating Queue");



                // Bind Queue to Exchange

                channel.QueueBind("demoqueue", "demoExchange", "directexchange_key");
                channel.BasicPublish("", queueName,true,properties,body);
                var publicationAddress =new  PublicationAddress("Direct", "requestTest", "testKey");

                channel.BasicPublish("demoExchange", "directexchange_key", true, properties, body);
                channel.BasicAcks += (sender, eventArgs) =>
                {
                    Console.WriteLine("sent to RabbitMQ");
                };

                channel.ConfirmSelect();

            }
        }
    }
}
