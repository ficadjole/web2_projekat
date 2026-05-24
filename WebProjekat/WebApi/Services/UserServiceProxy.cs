using Microsoft.ServiceFabric.Services.Remoting.Client;
using UserService.Interfaces;

namespace WebApi.Services
{
    public class UserServiceProxy
    {

        private readonly Uri _serviceUri = new Uri("fabric:/WebProjekat/UserService");

        public IUserService GetUserProxy()
            => ServiceProxy.Create<IUserService>(_serviceUri);

    }
}
