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
                channel.QueueBind(queueName, "amq.direct", "directexchange_key");
                channel.BasicPublish("amq.direct", "directexchange_key", true, properties, body);
                channel.BasicAcks += (sender, eventArgs) =>
                {
                    Console.WriteLine("sent to RabbitMQ");
                };

                channel.ConfirmSelect();

            }
        }
    }
}
