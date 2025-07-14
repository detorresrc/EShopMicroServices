namespace Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(OrderDto Order)
    : ICommand<CreateOrderResult>
{
    public static CreateOrderCommand Of(OrderDto order)
    {
        return new CreateOrderCommand(order);
    }
}
    
public record CreateOrderResult(Guid Id);

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("CustomerId is required");
        RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("OrderItems should not be empty");
        
        // validate each order item
        RuleForEach(x => x.Order.OrderItems).ChildRules(orderItem =>
        {
            orderItem.RuleFor(x => x.ProductId).NotNull().WithMessage("ProductId is required");
            orderItem.RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity should be greater than 0");
            orderItem.RuleFor(x => x.Price).GreaterThan(0).WithMessage("UnitPrice should be greater than 0");
        });
    }
}