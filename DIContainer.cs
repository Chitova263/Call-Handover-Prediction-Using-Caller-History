using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace VerticalHandoverPrediction
{
    public class DIContainer
    {
        private static DIContainer instance = null;
        
        private static readonly object padlock = new object();
        private DIContainer()
        {
            var services = new ServiceCollection();

            services.AddMediatR(typeof(Ping));

            Container = services.BuildServiceProvider();
        }

         public static DIContainer _Container
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DIContainer();
                    }
                    return instance;
                }
            }
        }

        public ServiceProvider Container { get; private set; }
    }
}