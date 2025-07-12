using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(
    DiscountDbContext db, 
    ILogger<DiscountService> logger) 
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await db.Coupons.FirstOrDefaultAsync(x => x.ProductName.Equals(request.ProductName));
        if (coupon is not null) return coupon.Adapt<CouponModel>();
        
        logger.LogError("Discount with product name {ProductName} not found", request.ProductName);
        
        return new CouponModel
        {
            ProductName = "No Discount",
            Description = "No Discount",
            Amount = 0
        };
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if(coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid coupon data"));
        
        db.Coupons.Add(coupon);
        await db.SaveChangesAsync();

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await db.Coupons.FirstOrDefaultAsync(x => x.ProductName.Equals(request.ProductName));
        if (coupon is null) return new DeleteDiscountResponse{Success = false};
        
        db.Coupons.Remove(coupon);
        await db.SaveChangesAsync();
        return new DeleteDiscountResponse{Success = true};
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = await db.Coupons.FirstOrDefaultAsync(x => x.Id == request.Coupon.Id);
        if (coupon is null)
        {
            var newCoupon = await CreateDiscount(request.Coupon.Adapt<CreateDiscountRequest>(), context);
            return newCoupon.Adapt<CouponModel>();
        }

        coupon.ProductName = request.Coupon.ProductName;
        coupon.Amount = request.Coupon.Amount;
        coupon.Description = request.Coupon.Description;
        db.Coupons.Update(coupon);
        await db.SaveChangesAsync();
        return coupon.Adapt<CouponModel>();
    }
}