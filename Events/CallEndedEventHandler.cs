using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace VerticalHandoverPrediction.Events
{
    public class CallEndedEventHandler : INotificationHandler<CallEndedEvent>
    {
        public Task Handle(CallEndedEvent notification, CancellationToken cancellationToken)
        {
        
            Simulator.NetworkSimulator._NetworkSimulator
                .EventQueue
                .Enqueue(notification);
                
            return Task.FromResult<Unit>(Unit.Value);
        }
    }
}