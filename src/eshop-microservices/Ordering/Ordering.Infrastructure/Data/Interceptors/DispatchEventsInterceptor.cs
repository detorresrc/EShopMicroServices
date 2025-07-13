using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Ordering.Infrastructure.Data.Interceptors;

public class DispatchEventsInterceptor(
    IMediator mediator,
    ILogger<DispatchEventsInterceptor> logger) 
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchDomainEventsAsync(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    private async Task DispatchDomainEventsAsync(DbContext? context)
    {
        if (context is null) return;

        var aggregates = context.ChangeTracker
            .Entries<IAggregate>()
            .Where(a => a.Entity.DomainEvents.Any())
            .Select(a => a.Entity);
        var domainEvents = aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList();
        
        aggregates.ToList().ForEach(a => a.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            logger.LogInformation("Dispatching domain event: {DomainEvent} Payload: {Payload}", domainEvent.GetType().Name, domainEvent);
            await mediator.Publish(domainEvent);
        }
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        DispatchDomainEventsAsync(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}