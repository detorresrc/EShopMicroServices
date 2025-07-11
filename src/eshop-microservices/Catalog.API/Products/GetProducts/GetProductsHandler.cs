namespace Catalog.API.Products.GetProducts;

public sealed record GetProductsQuery() : IQuery<GetProductsResult>;

public sealed record GetProductsResult(
    IEnumerable<Product> Products);

internal sealed class GetProductsHandler(
    IDocumentSession session,
    ILogger<GetProductsHandler> logger)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductsQueryHandler.Handle called with {@request}", request);

        var product = await session.Query<Product>()
            .ToListAsync(cancellationToken);

        return new GetProductsResult(Products: product);
    }
}