
using AspnetRunBasics.Models;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services
{
    public interface IBasketService
    {
        public Task<BasketModel> GetBasket(string userName);
        public Task<BasketModel> UpdateBasket(BasketModel model);
        public Task CheckoutBasket(BasketCheckoutModel model);
    }
}
