using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrdersByName;

public record GetOrdersByNameQuery(string Name) : IQuery<GetOrdersByNameResult>
{
    public static GetOrdersByNameQuery Of(string name)
    {
        return new GetOrdersByNameQuery(name);
    }
}

public record GetOrdersByNameResult(IEnumerable<OrderDto> Orders)
{
    public static GetOrdersByNameResult Of(IEnumerable<Order> orders)
    {
        return new GetOrdersByNameResult(orders.ToOrderDtoList());
    }
}