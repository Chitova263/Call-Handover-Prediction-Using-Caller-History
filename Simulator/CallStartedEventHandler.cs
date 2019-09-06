using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace VerticalHandoverPrediction.Simulator
{
    public class CallStartedEventHandler : INotificationHandler<CallStartedEvent>
    {
        public Task Handle(CallStartedEvent notification, CancellationToken cancellationToken)
        {
            //Update the Priority Queue
            throw new System.NotImplementedException();
        }
    }
}