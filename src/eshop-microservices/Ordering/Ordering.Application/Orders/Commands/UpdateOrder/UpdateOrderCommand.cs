namespace Ordering.Application.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand(OrderDto Order)
    : ICommand<UpdateOrderResult>;

public record UpdateOrderResult(bool IsSuccess);

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Order.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("CustomerId is required");
        
        // validate each order item
        RuleForEach(x => x.Order.OrderItems).ChildRules(orderItem =>
        {
            orderItem.RuleFor(x => x.ProductId).NotNull().WithMessage("ProductId is required");
            orderItem.RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity should be greater than 0");
            orderItem.RuleFor(x => x.Price).GreaterThan(0).WithMessage("UnitPrice should be greater than 0");
        });
    }
}