using AutoMapper;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Interfaces;
using MediatR;
using Newtonsoft.Json;
using Ordering.Application.Commands;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Api.RabbitMQ
{
    public class EventBusRabbitMQConsumer
    {
        private readonly IRabbitMQConnection _connection;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;

        public EventBusRabbitMQConsumer(IRabbitMQConnection connection, IMediator mediator, IMapper mapper, IOrderRepository repository)
        {
            _connection = connection;
            _mediator = mediator;
            _mapper = mapper;
            _repository = repository;
        }

        public void Consume()
        {
            var channel = _connection.CreateModel();
            /* channel.ExchangeDeclare(exchange: "amq.direct", type: "direct",true);
             channel.QueueDeclare(queue: EventBusConstants.BasketCheckoutQueue, exclusive: false,autoDelete:false);
             channel.QueueBind(queue: EventBusConstants.BasketCheckoutQueue,
                               exchange: "amq.direct",
                               routingKey: "directexchange_key");


             var consumer = new EventingBasicConsumer(channel);
             consumer.Received += (model, ea) =>
             {
                 var body = ea.Body.Span;
                 var message = Encoding.UTF8.GetString(body);
                 var result = Process(message);
                 if (result.Id !=0 || result !=null)
                 {
                     channel.BasicAck(ea.DeliveryTag, false);
                 }
             };*/
            channel.BasicQos(0, 1, false);

            DefaultBasicConsumer consumer = new DefaultBasicConsumer(channel);
            channel.BasicConsume(queue: EventBusConstants.BasketCheckoutQueue, consumer: consumer);

        }

  /*      private async void ReceivedEvent(object sender, BasicDeliverEventArgs @e)
        {
            if (e.RoutingKey == "directexchange_key")
            {
                var message = Encoding.UTF8.GetString(@e.Body.Span);
                var basketCheckoutEvent = JsonConvert.DeserializeObject<BasketCheckoutEvent>(message);
                var command = _mapper.Map<CheckoutOrderCommand>(basketCheckoutEvent);

                var result = await _mediator.Send(command);

            }


        }*/

        private async Task<OrderResponse> Process(string message)
        {
            var basketCheckoutEvent = JsonConvert.DeserializeObject<BasketCheckoutEvent>(message);
            var command = _mapper.Map<CheckoutOrderCommand>(basketCheckoutEvent);

            return await _mediator.Send(command);
        }

        public void Disconnect()
        {
            _connection.Dispose();
        }
    }
}
