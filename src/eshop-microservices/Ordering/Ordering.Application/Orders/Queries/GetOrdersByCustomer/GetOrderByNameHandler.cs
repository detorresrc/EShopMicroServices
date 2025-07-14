namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer;

public class GetOrderByCustomerNameHandler(
    IApplicationDbContext dbContext
    ) 
    : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
{
    public async Task<GetOrdersByCustomerResult> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .Where(o => o.CustomerId.Value.Equals(request))
            .OrderBy(o => o.OrderName)
            .ToListAsync(cancellationToken);

        return GetOrdersByCustomerResult.Of(orders);
    }
}