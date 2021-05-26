using Basket.Api.data.Interfaces;
using StackExchange.Redis;

namespace Basket.Api.data
{
    public class BasketContext : IBasketContext
    {
        public IDatabase Redis { get; }

        private readonly IConnectionMultiplexer _redisConnection;

        public BasketContext(IConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
            Redis = redisConnection.GetDatabase();
        }
    }
}
