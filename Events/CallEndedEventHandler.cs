using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace VerticalHandoverPrediction.Events
{
    public class CallEndedEventHandler : INotificationHandler<CallEndedEvent>
    {
        public Task Handle(CallEndedEvent notification, CancellationToken cancellationToken)
        {
            //Update the Priority Queue
            Simulator.NetworkSimulator._NetworkSimulator
                .EventQueue
                .Enqueue(notification);
                
            return Task.FromResult<int>(1);
        }
    }
}