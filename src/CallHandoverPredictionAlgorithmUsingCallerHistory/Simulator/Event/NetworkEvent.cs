using Simulator.MultiModeMobileTerminal;

namespace Simulator.Event;

public record NetworkEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required MobileTerminalEvent MobileTerminalEvent { get; init; }
    public required DateTime TimeStamp { get; init; }
    public required MobileTerminal MobileTerminal { get; init; }
    public required Guid EventCorrelationId { get; init; }
}