using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DiscountService(ILogger<DiscountService> logger, IDiscountRepository repository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRquest request, ServerCallContext context)
        {
            var coupon =  await _repository.GetDiscount(request.ProductName);
            if(coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not dfound"));
            }
            _logger.LogInformation("Discount is retrived for ProductName : {coupon.ProductName}, Amount : {coupon.Amount}", coupon.ProductName, coupon.Amount);
            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;
        }     
        
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRquest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
             await _repository.CreateDiscount(_mapper.Map<Coupon>(request.Coupon));
            _logger.LogInformation("Discount is successfully for created,ProductName : {coupon.ProductName}", coupon.ProductName);
            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }     
        
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRquest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
             await _repository.UpdateDiscount(_mapper.Map<Coupon>(request.Coupon));
            _logger.LogInformation("Discount is successfully for updated,ProductName : {coupon.ProductName}", coupon.ProductName);
            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRquest request, ServerCallContext context)
        {
            var deleted = await _repository.DeleteDiscount(request.ProductName);
            var response = new DeleteDiscountResponse
            {
                Success = deleted
            };
            return response;
        }
    }
}
