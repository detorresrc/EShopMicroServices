namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer;

public record GetOrdersByCustomerQuery(Guid CustomerId) : IQuery<GetOrdersByCustomerResult>;

public record GetOrdersByCustomerResult(IEnumerable<OrderDto> Orders)
{
    public static GetOrdersByCustomerResult Of(IEnumerable<Order> orders)
    {
        return new GetOrdersByCustomerResult(orders.ToOrderDtoList());
    }
}