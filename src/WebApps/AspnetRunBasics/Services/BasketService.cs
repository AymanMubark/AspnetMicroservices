
using AspnetRunBasics.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services
{
    public class BasketService: IBasketService
    {
        private readonly HttpClient _client;

        public BasketService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<BasketModel> GetBasket(string userName)
        {
            return await _client.GetFromJsonAsync<BasketModel>($"/Basket/{userName}");
        }

        public async Task<BasketModel> UpdateBasket(BasketModel model) {
            var response = await _client.PostAsJsonAsync<BasketModel>("/Basket", model);
            return await response.Content.ReadFromJsonAsync<BasketModel>();
        }
        public async Task CheckoutBasket(BasketCheckoutModel model)
        {
            await _client.PostAsJsonAsync<BasketCheckoutModel>("/Basket/Checkout", model);
        }
    }
}
