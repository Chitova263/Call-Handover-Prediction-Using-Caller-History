using MediatR;

namespace VerticalHandoverPrediction
{
    public class Ping : IRequest<string>
    {
        public string Message { get; set; }
    }

    public class PingHandler : RequestHandler<Ping,string>
    {
        public PingHandler()
        {
           
        }

        protected override string Handle(Ping request)
        {
            return "jrtuuip";
        }
    }
}