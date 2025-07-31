namespace SharedKernel.Messaging.Events;

public record IntegrationEvent
{
    public Guid Id => Guid.NewGuid();
    public DateTime OccuredOn { get; init; } = DateTime.UtcNow;
    public string EventType => GetType().AssemblyQualifiedName ?? string.Empty;
}