namespace Catalog.API.Products.GetProductsByCategory;

public sealed record GetProductsByCategoryQuery(
    string Category) : IQuery<GetProductsByCategoryResult>;

public sealed record GetProductsByCategoryResult(
    IEnumerable<Product> Products);

internal sealed class GetProductsByCategoryQueryHandler(
    IDocumentSession session)
    : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>()
            .Where(p => p.Category.Contains(request.Category))
            .ToListAsync(cancellationToken);
        
        return new GetProductsByCategoryResult(products);
    }
}