using MailingService.Interface.Events;
using Microsoft.ServiceFabric.Services.Remoting;

namespace MailingService.Interface
{
    public interface IMailingService : IService
    {
        public Task PublishEvent(EmailEvent emailEvent);
    }
}
