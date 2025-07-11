namespace Catalog.API.Products.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid Id,
    Product Product) : ICommand<Result<UpdateProductResult>>;

public sealed record UpdateProductResult(
    Product Product);

public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(c => c.Product.Name).NotEmpty().WithMessage("Product name is required.");
        RuleFor(c => c.Product.Category).NotEmpty().WithMessage("Product category is required.");
        RuleFor(c => c.Product.Description).NotEmpty().WithMessage("Product description is required.");
        RuleFor(c => c.Product.ImageFile).NotEmpty().WithMessage("Product image file is required.");
        RuleFor(c => c.Product.Price)
            .GreaterThan(0)
            .WithMessage("Product price must be greater than zero.");
    }
}

public class UpdateProductHandler(
    IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, Result<UpdateProductResult>>
{
    public async Task<Result<UpdateProductResult>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
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