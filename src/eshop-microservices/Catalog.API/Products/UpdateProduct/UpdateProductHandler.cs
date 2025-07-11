namespace Catalog.API.Products.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid Id,
    Product Product) : ICommand<Result<UpdateProductResult>>;

public sealed record UpdateProductResult(
    Product Product);

public class UpdateProductHandler(
    IDocumentSession session,
    ILogger<UpdateProductHandler> logger)
    : ICommandHandler<UpdateProductCommand, Result<UpdateProductResult>>
{
    public async Task<Result<UpdateProductResult>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateProductHandler.Handle called with {@request}", request);

        var product = await session.Query<Product>()
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken); 
        
        if(product is null)
            return Result.Failure<UpdateProductResult>(GlobalErrors.ProductNotFound);

        product.Category = request.Product.Category;
        product.Name = request.Product.Name;
        product.Description = request.Product.Description;
        product.ImageFile = request.Product.ImageFile;
        product.Price = request.Product.Price;
        
        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);
        
        return new UpdateProductResult(product);
    }
}