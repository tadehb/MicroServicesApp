using Basket.Api.data.Interfaces;
using Basket.Api.Entities;
using Basket.Api.Repositories.Interfaces;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        public readonly IBasketContext _context;

        public BasketRepository(IBasketContext context)
        {
            _context = context;
        }

        public async Task<BasketCart> GetBasket(string userName)
        {
            var basket = await _context
                .Redis
                .StringGetAsync(userName);

            if (basket.IsNullOrEmpty)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<BasketCart>(basket);
        }


        public async Task<bool> DeleteBasket(string userName)
        {
            return await _context.Redis.KeyDeleteAsync(userName);

        }

        public async Task<BasketCart> UpdateBasket(BasketCart basketCart)
        {
            var update = await _context
                .Redis
                .StringSetAsync(basketCart.UserName, JsonConvert.SerializeObject(basketCart));
            if (!update)
            {
                return null;
            }

            return await GetBasket(basketCart.UserName);

        }

    }
}
