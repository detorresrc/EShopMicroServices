using Catalog.API.Errors;

namespace Catalog.API.Products.GetProductById;

public sealed record GetProductByIdQuery(
    Guid Id) : IQuery<Result<GetProductByIdResult>>;

public sealed record GetProductByIdResult(
    Product? Product);

internal sealed class GetProductByIdQueryHandler(
    IDocumentSession session,
    ILogger<GetProductByIdQueryHandler> logger)
    : IQueryHandler<GetProductByIdQuery, Result<GetProductByIdResult>>
{
    public async Task<Result<GetProductByIdResult>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductByIdQueryHandler.Handle called with {@request}", request);

        var product = await session.Query<Product>()
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null) return Result.Failure<GetProductByIdResult>(GlobalErrors.ProductNotFound);

        return Result.Success(new GetProductByIdResult(product));
    }
}