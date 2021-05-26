using System.Collections.Generic;

namespace Basket.Api.Entities
{
    public class BasketCart
    {
        public string UserName { get; set; }
        public List<BasketItems> Items { get; set; } = new List<BasketItems>();

        public BasketCart()
        {

        }
        public BasketCart(string userName) => UserName = UserName;

        public decimal TotalPrice
        {

            get
            {
                decimal totalPrice = 0;
                foreach (var item in Items)
                {
                    totalPrice += item.Price * item.Quantity;
                }
                return totalPrice;
            }
        }
    }
}
