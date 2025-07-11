namespace Basket.API.Basket.DeleteBasket;

public sealed record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;

public sealed record DeleteBasketResult(bool IsSuccess);

public sealed class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("User name must not be empty.");
    }
}

public sealed class DeleteBasketHandler(IBasketRepository basketRepository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        await basketRepository.DeleteBasketAsync(command.UserName, cancellationToken);
        return new DeleteBasketResult(true);
    }
}