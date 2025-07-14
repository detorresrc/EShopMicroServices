using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrdersByName;

public record GetOrdersByNameQuery(string Name) : IQuery<GetOrdersByNameResult>;

public record GetOrdersByNameResult(IEnumerable<OrderDto> Orders)
{
    public static GetOrdersByNameResult Of(IEnumerable<Order> orders)
    {
        return new GetOrdersByNameResult(orders.ToOrderDtoList());
    }
}