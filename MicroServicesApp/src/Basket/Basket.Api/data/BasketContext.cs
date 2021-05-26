using Basket.Api.data.Interfaces;
using StackExchange.Redis;

namespace Basket.Api.data
{
    public class BasketContext : IBasketContext
    {
        public IDatabase Redis { get; }

        private readonly ConnectionMultiplexer _redisConnection;

        public BasketContext(ConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
            Redis = redisConnection.GetDatabase();
        }
    }
}
