using StackExchange.Redis;

namespace Basket.Api.data.Interfaces
{
    public interface IBasketContext
    {
        IDatabase Redis { get; }
    }
}
