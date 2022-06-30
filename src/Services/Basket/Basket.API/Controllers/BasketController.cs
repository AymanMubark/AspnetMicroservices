using Basket.API.Entities;
using Basket.API.GrpcServices;
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
        private readonly DicountGrpcService _dicountGrpcService;

        public BasketController(IBasketRepository repository, DicountGrpcService dicountGrpcService)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
            _dicountGrpcService = dicountGrpcService ?? throw new ArgumentException(nameof(dicountGrpcService));
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart shoppingCart)
        {
            // TODO : Communication with Discount.Grpc
            // and get latest price product into shopping cart
            // cunsume discount grpc
            foreach (var item in shoppingCart.Items)
            {
                var coupon = await _dicountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }
            var basket = await _repository.UpdateBasket(shoppingCart);
            return Ok(basket ?? new ShoppingCart(shoppingCart.UserName));
        }

        [HttpDelete("{userName}")]
        public async Task<ActionResult<ShoppingCart>> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }
    }
}
