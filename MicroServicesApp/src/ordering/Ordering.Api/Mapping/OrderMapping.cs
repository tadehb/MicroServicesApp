using AutoMapper;
using EventBusRabbitMQ.Events;
using Ordering.Application.Commands;

namespace Ordering.Api.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<BasketCheckoutEvent, CheckoutOrderCommand>().ReverseMap();
        }
    }
}
