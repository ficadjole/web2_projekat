using System.Fabric;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Data;
using FluentValidation;
using WebApi.FluentValidations;
using WebApi.Services;
using WebApi.Extensions;

namespace WebApi
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance.
    /// </summary>
    internal sealed class WebApi : StatelessService
    {
        public WebApi(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        var builder = WebApplication.CreateBuilder();

                        builder.Services.AddSingleton<StatelessServiceContext>(serviceContext);
                        builder.WebHost
                                    .UseKestrel()
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseUrls(url);
                        builder.Services.AddControllers();
                        builder.Services.AddEndpointsApiExplorer();
                        builder.Services.AddSwaggerGen();
                        builder.Services.AddJwtAuthentification(builder.Configuration);

                        builder.Services.AddValidatorsFromAssemblyContaining<AuthRequestValidator>();
                        builder.Services.AddValidatorsFromAssemblyContaining<RegistrationRequestValidator>();

                        builder.Services.AddValidatorsFromAssemblyContaining<CreateTripRequestValidator>();
                        builder.Services.AddValidatorsFromAssemblyContaining<UpdateTripRequestValidator>();

                        builder.Services.AddValidatorsFromAssemblyContaining<CreateDestinationRequestValidator>();
                        builder.Services.AddValidatorsFromAssemblyContaining<UpdateDestinationRequestValidator>();

                        builder.Services.AddValidatorsFromAssemblyContaining<CreateActivityRequestValidator>();
                        builder.Services.AddValidatorsFromAssemblyContaining<UpdateActivityRequestValidator>();

                        builder.Services.AddValidatorsFromAssemblyContaining<CreateExpenseRequestValidator>();
                        builder.Services.AddValidatorsFromAssemblyContaining<UpdateExpenseRequestValidator>();


                        builder.Services.AddSingleton<TripServiceProxy>();

                        var app = builder.Build();
                        if (app.Environment.IsDevelopment())
                        {
                        app.UseSwagger();
                        app.UseSwaggerUI();
                        }
                        app.UseAuthorization();
                        app.MapControllers();
                        
                        return app;

                    }))
            };
        }
    }
}
