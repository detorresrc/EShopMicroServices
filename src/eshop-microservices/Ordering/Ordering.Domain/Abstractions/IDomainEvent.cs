using MediatR;

namespace Ordering.Domain.Abstractions;

public interface IDomainEvent : INotification
{
    Guid EventId => Guid.Empty;
    public DateTime OccurredOn => DateTime.UtcNow;
    public string? EventType => GetType().AssemblyQualifiedName;
}