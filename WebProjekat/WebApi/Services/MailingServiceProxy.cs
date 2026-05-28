using MailingService.Interface;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace WebApi.Services
{
    public class MailingServiceProxy
    {
        private readonly Uri _serviceUri = new Uri("fabric:/WebProjekat/MailingService");
        public IMailingService GetMailingProxy()
            => ServiceProxy.Create<IMailingService>(_serviceUri, new ServicePartitionKey(0));
    }
}
