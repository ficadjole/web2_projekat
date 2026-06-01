using FluentValidation;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;
using System.Text.Json.Serialization;
using WebApi.Extensions;
using WebApi.FluentValidations;
using WebApi.Services;

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
                        builder.Services.AddControllers().AddJsonOptions(options=>{options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
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

                        builder.Services.AddValidatorsFromAssemblyContaining<AddChecklistItemRequestValidator>();

                        builder.Services.AddSingleton<TripServiceProxy>();
                        builder.Services.AddSingleton<UserServiceProxy>();
                        builder.Services.AddSingleton<ChecklistServiceProxy>();
                        builder.Services.AddSingleton<MailingServiceProxy>();

                        builder.Services.AddCors(options =>
                        {
                            options.AddPolicy("AllowSpecificOrigin",
                                builder => builder.WithOrigins("http://localhost:5173")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod());
                        });

                        var app = builder.Build();
                        if (app.Environment.IsDevelopment())
                        {
                        app.UseSwagger();
                        app.UseSwaggerUI();
                        }
                        app.UseAuthorization();
                        app.MapControllers();
                        app.UseCors("AllowSpecificOrigin");

                        return app;

                    }))
            };
        }
    }
}
