using CheckListService.Interfaces;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace WebApi.Services
{
    public class ChecklistServiceProxy
    {
        private readonly Uri _serviceUri = new Uri("fabric:/WebProjekat/CheckListService");

        public IChecklistService GetChecklistProxy()
            => ServiceProxy.Create<IChecklistService>(_serviceUri, new ServicePartitionKey(0));
    }
}
