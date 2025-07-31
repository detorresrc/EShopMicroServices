namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderUpdatedEventHandler(
    ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("OrderUpdatedEventHandler:: Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}