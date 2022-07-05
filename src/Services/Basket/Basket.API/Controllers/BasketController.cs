using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using MassTransit.RabbitMqTransport;
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
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publisher;

        public BasketController(IBasketRepository repository, DicountGrpcService dicountGrpcService, IMapper mapper, IPublishEndpoint publisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _dicountGrpcService = dicountGrpcService ?? throw new ArgumentNullException(nameof(dicountGrpcService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
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

        [HttpPost("[action]")]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // Get exsiting basket with total price
            // Create BasketCheckoutEvent -- Set TotalPrice on basketCheckout eventMessage
            // Send checkout event to rqbbitmq
            //remove  the basekt

            // Get exsiting basket with total price
            var basket = await _repository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            // Send checkout event to rqbbitmq
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publisher.Publish(eventMessage);

            await _repository.DeleteBasket(basketCheckout.UserName);

            return Accepted();
        }
    }
}
