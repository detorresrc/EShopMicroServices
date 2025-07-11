using Marten.Pagination;

namespace Catalog.API.Products.GetProducts;

public sealed record GetProductsQuery(
    int? Page = 1,
    int? PageSize = 10) : IQuery<GetProductsResult>;

public sealed record GetProductsResult(
    IEnumerable<Product> Products,
    long Count,
    long PageNumber,
    long PageSize,
    long PageCount,
    long TotalItemCount,
    bool HasPreviousPage,
    bool HasNextPage,
    bool IsFirstPage,
    long FirstItemOnPage,
    long LastItemOnPage);

internal sealed class GetProductsHandler(
    IDocumentSession session)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var product = await session
            .Query<Product>()
            .ToPagedListAsync(
                pageNumber: request.Page ?? 1,
                pageSize: request.PageSize ?? 10,
                cancellationToken);

        return new GetProductsResult(
            Products: product,
            Count: product.Count,
            PageNumber: product.PageNumber,
            PageSize: product.PageSize,
            PageCount: product.PageCount,
            TotalItemCount: product.TotalItemCount,
            HasPreviousPage: product.HasPreviousPage,
            HasNextPage: product.HasNextPage,
            IsFirstPage: product.IsFirstPage,
            FirstItemOnPage: product.FirstItemOnPage,
            LastItemOnPage: product.LastItemOnPage
            );
    }
}