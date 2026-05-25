using Microsoft.ServiceFabric.Services.Remoting.Client;
using TripService.Interfaces;

namespace WebApi.Services
{
    public class TripServiceProxy
    {
        private readonly Uri _serviceUri = new Uri("fabric:/WebProjekat/TripService");

        public ITripService GetTripProxy()
            => ServiceProxy.Create<ITripService>(_serviceUri);

        public IDestinationService GetDestinationProxy()
            => ServiceProxy.Create<IDestinationService>(_serviceUri);

        public IActivityService GetActivityProxy()
            => ServiceProxy.Create<IActivityService>(_serviceUri);

        public IExpenseService GetExpenseProxy()
            => ServiceProxy.Create<IExpenseService>(_serviceUri);

        public ITripShareService GetTripShareProxy()
            => ServiceProxy.Create<ITripShareService>(_serviceUri);
    }
}
