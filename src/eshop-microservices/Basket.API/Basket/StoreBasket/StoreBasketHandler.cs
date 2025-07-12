

using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket;

public sealed record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public sealed record StoreBasketResult(ShoppingCart Cart);

public sealed class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null.");
        RuleFor(x => x.Cart.Username).NotEmpty().WithMessage("Username cannot be empty.");
        // Validate quantity
        RuleFor(x => x.Cart.Items)
            .NotEmpty().WithMessage("Cart must contain at least one item.");
        RuleForEach(x => x.Cart.Items).ChildRules(items =>
        {
            items.RuleFor(item => item.ProductId)
                .NotEmpty().WithMessage("Product ID cannot be empty.");
            items.RuleFor(item => item.ProductName)
                .NotEmpty().WithMessage("Product name cannot be empty.");
            items.RuleFor(item => item.Quantity)
                .NotEmpty().WithMessage("Quantity cannot be empty.")
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.");
            items.RuleFor(item => item.Price)
                .NotEmpty().WithMessage("Price cannot be empty.")
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero.");
        });
    }
}

public sealed class StoreBasketHandler(
    IBasketRepository basketRepository,
    DiscountProtoService.DiscountProtoServiceClient discountProto) 
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        foreach (var item in command.Cart.Items)
        {
            var coupon = await discountProto.GetDiscountAsync(request: new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
            item.Price -= coupon.Amount;
        }

        await basketRepository.StoreBasketAsync(command.Cart, cancellationToken);

        return new StoreBasketResult(command.Cart);
    }
}