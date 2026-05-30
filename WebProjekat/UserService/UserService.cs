using Common.Interfaces;
using Common.Options;
using Common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;
using UserService.DatabaseContext;
using UserService.DTOs;
using UserService.Interfaces;
using UserService.Interfaces.DTOs;
using UserService.Repositories;
using UserService.Services;
using WebProjekat.Common;

namespace UserService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class UserService : StatelessService, IUserService
    {

        private readonly ServiceProvider _serviceProvider;
        private readonly IAuthService _authService;
        private readonly IUserBusinessService userBusinessService;

        public UserService(StatelessServiceContext context)
            : base(context)
        {

            var config = context.CodePackageActivationContext.GetConfigurationPackageObject("Config").Settings;

            var jwtSection = config.Sections["JwtConfig"];
            var secretKey = jwtSection.Parameters["SecretKey"].Value;
            var issuer = jwtSection.Parameters["Issuer"].Value;
            var audience = jwtSection.Parameters["Audience"].Value;
            var expirationMinutes = int.Parse(jwtSection.Parameters["ExpirationMinutes"].Value);

            var sqlSection = config.Sections["SqlConfig"];
            var sqlConn = sqlSection.Parameters["SqlConnectionString"].Value;

            var services = new ServiceCollection();

            services.Configure<JwtOptions>(options =>
            {
                options.SecretKey = secretKey;
                options.Issuer = issuer;
                options.Audience = audience;
                options.ExpirationMinutes = expirationMinutes;
            });

            services.AddScoped<IJwtService, JwtService>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IUserBusinessService, UserBusinessService>();

            services.AddLogging();

            services.AddDbContext<UsersDbContext>(options => options.UseSqlServer(sqlConn));

            _serviceProvider = services.BuildServiceProvider();

            _authService = _serviceProvider.GetRequiredService<IAuthService>();

            userBusinessService = _serviceProvider.GetRequiredService<IUserBusinessService>();
        }

        public Task<Result> DeleteUserAsync(string id)
        {

            return userBusinessService.DeleteUser(Guid.Parse(id));
        }

        public Task<Result<UserDto>> GetByIdAsync(Guid id)
        {
            return userBusinessService.GetByIdAsync(id);
        }

        public Task<Result<IEnumerable<UserDto>>> GetUsersAsync()
        {
            return userBusinessService.GetAllUsers();
        }

        public Task<Result<AuthResponseDto>> LoginAsync(string username, string password)
        {
            return _authService.AuthenticateAsync(username, password);
        }

        public Task<Result<AuthResponseDto>> RegisterAsync(string name, string email, string password, string role)
        {
            return _authService.RegisterAsync(name, email, password, role);
        }

        public Task<Result<UserDto>> UpdateRoleAsync(Guid id, UpdateUserRoleDto dto)
        {
            return userBusinessService.UpdateRoleAsync(id, dto);
        }

        public Task<Result<UserDto>> UpdateUserAsync(Guid id, UpdateUserDto dto, Guid requestingUserId)
        {
            return userBusinessService.UpdateUserAsync(id, dto, requestingUserId);
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return this.CreateServiceRemotingInstanceListeners();

        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
