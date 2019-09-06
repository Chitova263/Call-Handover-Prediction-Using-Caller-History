using System.Threading;
using System.Threading.Tasks;
using MediatR;


namespace VerticalHandoverPrediction.Events
{
    public class CallStartedEventHandler : INotificationHandler<CallStartedEvent>
    {
        public Task Handle(CallStartedEvent notification, CancellationToken cancellationToken)
        {
            //Update the Priority Queue
            Simulator.NetworkSimulator._NetworkSimulator
                .EventQueue
                .Enqueue(notification);
                
            return Task.FromResult<int>(1);
        }
    }
}