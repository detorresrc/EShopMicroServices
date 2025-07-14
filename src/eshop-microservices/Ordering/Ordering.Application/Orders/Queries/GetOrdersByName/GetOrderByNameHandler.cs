using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrdersByName;

public class GetOrderByNameHandler(
    IApplicationDbContext dbContext
    ) 
    : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
{
    public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery request, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .Where(o => o.OrderName.Value.Contains(request.Name))
            .OrderBy(o => o.OrderName)
            .ToListAsync(cancellationToken);

        return GetOrdersByNameResult.Of(orders);
    }
}