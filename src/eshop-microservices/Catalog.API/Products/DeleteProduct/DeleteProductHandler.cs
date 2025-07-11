namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(
    Guid Id) : ICommand<Result<Unit>>;

public sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty().WithMessage("Id is required.");
    }
}

public class DeleteProductHandler(
    IDocumentSession session,
    ILogger<DeleteProductHandler> logger)
    : ICommandHandler<DeleteProductCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteProductHandler.Handle called with {@request}", request);

        var product = await session.Query<Product>()
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken); 
        
        if(product is null)
            return Result.Failure<Unit>(GlobalErrors.ProductNotFound);
        
        session.Delete(product);
        await session.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}