using MailingService.Interface;
using MailingService.Interface.Events;
using MailingService.Interfaces;
using MailingService.Services;
using MailingService.Templates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;

namespace MailingService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class MailingService : StatefulService, IMailingService
    {

        private IReliableQueue<EmailEvent> _emailEventQueue;
        private readonly IEmailService _emailService;
        private readonly ServiceProvider _serviceProvider;
        private ILogger<MailingService> _logger;
        public MailingService(StatefulServiceContext context)
            : base(context)
        {

            var config = context.CodePackageActivationContext.GetConfigurationPackageObject("Config").Settings;

            var smptSection = config.Sections["EmailConfig"];

            var smtpOptions = new Options.SmtpOptions
            {
                Host = smptSection.Parameters["SmtpHost"].Value,
                Port = int.Parse(smptSection.Parameters["SmtpPort"].Value),
                SenderEmail = smptSection.Parameters["SenderEmail"].Value,
                SenderPassword = smptSection.Parameters["SenderPassword"].Value,
                SenderName = smptSection.Parameters["SenderName"].Value
            };

            var services = new ServiceCollection();

            services.AddSingleton(smtpOptions);
            services.AddScoped<IEmailService, EmailService>();

            services.AddLogging();

            _serviceProvider = services.BuildServiceProvider();

            _emailService = _serviceProvider.GetRequiredService<IEmailService>();

            _logger = _serviceProvider.GetRequiredService<ILogger<MailingService>>();

        }

        public async Task PublishEvent(EmailEvent emailEvent)
        {
            _emailEventQueue = await this.StateManager.GetOrAddAsync<IReliableQueue<EmailEvent>>("emailEventQueue");

            using (var tx = this.StateManager.CreateTransaction())
            {
                await _emailEventQueue.EnqueueAsync(tx, emailEvent);
                await tx.CommitAsync();

                _logger.LogInformation($"Email event enqueued for {emailEvent.Email} regarding trip share {emailEvent.TripShareDto.TripId}");
            }

        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return this.CreateServiceRemotingReplicaListeners();
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {

            _emailEventQueue = await this.StateManager.GetOrAddAsync<IReliableQueue<EmailEvent>>("emailEventQueue");

            while (!cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var tx = this.StateManager.CreateTransaction())
                {
                    var result = await _emailEventQueue.TryDequeueAsync(tx);

                    if (result.HasValue)
                    {

                        var body = EmailTemplates.TripShareTemplate(result.Value.TripShareDto);

                        await _emailService.SendEmailAsync(result.Value.Email, $"Trip Share Notification for Trip {result.Value.TripShareDto.TripId}", body);

                        await tx.CommitAsync();

                        _logger.LogInformation($"Email event processed for {result.Value.Email} regarding trip share {result.Value.TripShareDto.TripId}");

                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                    }

                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
