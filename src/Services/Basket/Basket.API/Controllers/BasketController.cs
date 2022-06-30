using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;

        public BasketController(IBasketRepository repository)
        {
            _repository = repository?? throw new ArgumentException(nameof(repository));
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<ShoppingCart>> Get(string userName)
        {
            var basket = await _repository.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> Put([FromBody] ShoppingCart shoppingCart)
        {
            var basket = await _repository.UpdateBasket(shoppingCart);
            return Ok(basket ?? new ShoppingCart(shoppingCart.UserName));
        }

        [HttpDelete("{userName}")]
        public async Task<ActionResult<ShoppingCart>> Delete(string userName)
        {
             await _repository.DeleteBasket(userName);
            return Ok();
        }
    }
}
