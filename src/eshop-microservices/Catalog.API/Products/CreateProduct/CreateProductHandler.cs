namespace Catalog.API.Products.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<CreateProductResult>;

public sealed record CreateProductResult(
    Guid Id);

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("Product name is required.");
        RuleFor(c => c.Category).NotEmpty().WithMessage("Product category is required.");
        RuleFor(c => c.Description).NotEmpty().WithMessage("Product description is required.");
        RuleFor(c => c.ImageFile).NotEmpty().WithMessage("Product image file is required.");
        RuleFor(c => c.Price)
            .GreaterThan(0)
            .WithMessage("Product price must be greater than zero.");
    }
}

internal sealed class CreateProductHandler(
    IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Category = request.Category,
            Description = request.Description,
            ImageFile = request.ImageFile,
            Price = request.Price
        };

        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }
}