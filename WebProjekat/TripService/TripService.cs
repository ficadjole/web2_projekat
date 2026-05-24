using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;
using TripService.DatabaseContext;
using TripService.Enums;
using TripService.Interfaces;
using TripService.Interfaces.DTOs.Activity;
using TripService.Interfaces.DTOs.Destination;
using TripService.Interfaces.DTOs.Expense;
using TripService.Interfaces.DTOs.Trip;
using TripService.Repositories;
using TripService.Services;
using WebProjekat.Common;

namespace TripService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class TripService : StatelessService, ITripService, IDestinationService, IActivityService, IExpenseService
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly ITripBusinessService _tripService;
        private readonly IDestinationBusinessService _destinationService;
        private readonly IActivityBusinessService _activityService;
        private readonly IExpenseBusinessService _expenseService;

        public TripService(StatelessServiceContext context)
            : base(context)
        {

            var config = context.CodePackageActivationContext.GetConfigurationPackageObject("Config").Settings;

            var sqlSection = config.Sections["SqlConfig"];
            var sqlConn = sqlSection.Parameters["SqlConnectionString"].Value;
            var services = new ServiceCollection();

            services.AddLogging();

            services.AddDbContext<TripsDbContext>(options => options.UseSqlServer(sqlConn));

            services.AddScoped<ITripRepository, TripRepository>();
            services.AddScoped<IDestinationRepository, DestinationRepository>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();

            services.AddScoped<ITripBusinessService, TripBusinessService>();
            services.AddScoped<IDestinationBusinessService, DestinationBusinessService>();
            services.AddScoped<IActivityBusinessService, ActivityBusinessService>();
            services.AddScoped<IExpenseBusinessService, ExpenseBusinessService>();

            _serviceProvider = services.BuildServiceProvider();

            _tripService = _serviceProvider.GetRequiredService<ITripBusinessService>();
            _destinationService = _serviceProvider.GetRequiredService<IDestinationBusinessService>();
            _activityService = _serviceProvider.GetRequiredService<IActivityBusinessService>();
            _expenseService = _serviceProvider.GetRequiredService<IExpenseBusinessService>();
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

        #region Trip
        public Task<Result<TripDto>> CreateTripAsync(CreateTripDto dto, Guid userId)
            => _tripService.CreateAsync(dto, userId);

        public Task<Result<TripDto>> GetTripByIdAsync(Guid id, Guid userId)
            => _tripService.GetByIdAsync(id, userId);

        public Task<Result<TripDetailsDto>> GetTripWithDetailsAsync(Guid id, Guid userId)
            => _tripService.GetWithDetailsAsync(id, userId);

        public Task<Result<IEnumerable<TripDto>>> GetAllTripsByUserAsync(Guid userId)
            => _tripService.GetAllByUserAsync(userId);

        public Task<Result<TripDto>> UpdateTripAsync(Guid id, UpdateTripDto dto, Guid userId)
            => _tripService.UpdateAsync(id, dto, userId);

        public Task<Result> DeleteTripAsync(Guid id, Guid userId)
            => _tripService.DeleteAsync(id, userId);

        #endregion Trip

        #region Destination
        public Task<Result<DestinationDto>> CreateDestinationAsync(CreateDestinationDto dto, Guid userId)
            => _destinationService.CreateAsync(dto, userId);

        public Task<Result<DestinationDto>> GetDestinationByIdAsync(Guid id, Guid userId)
            => _destinationService.GetByIdAsync(id, userId);

        public Task<Result<IEnumerable<DestinationDto>>> GetDestinationsByTripIdAsync(Guid tripId, Guid userId)
            => _destinationService.GetByTripIdAsync(tripId, userId);

        public Task<Result<DestinationDto>> UpdateDestinationAsync(Guid id, UpdateDestinationDto dto, Guid userId)
            => _destinationService.UpdateAsync(id, dto, userId);

        public Task<Result> DeleteDestinationAsync(Guid id, Guid userId)
            => _destinationService.DeleteAsync(id, userId);

        #endregion Destination

        #region Activity
        public Task<Result<ActivityDto>> CreateActivityAsync(CreateActivityDto dto, Guid userId)
            => _activityService.CreateAsync(dto, userId);

        public Task<Result<ActivityDto>> GetActivityByIdAsync(Guid id, Guid userId)
            => _activityService.GetByIdAsync(id, userId);

        public Task<Result<IEnumerable<ActivityDto>>> GetActivitiesByDestinationIdAsync(Guid destinationId, Guid userId)
            => _activityService.GetByDestinationIdAsync(destinationId, userId);

        public Task<Result<IEnumerable<ActivityDto>>> GetActivitiesByDateAsync(Guid tripId, DateTime date, Guid userId)
            => _activityService.GetByDateAsync(tripId, date, userId);

        public Task<Result<ActivityDto>> UpdateActivityAsync(Guid id, UpdateActivityDto dto, Guid userId)
            => _activityService.UpdateAsync(id, dto, userId);

        public Task<Result> DeleteActivityAsync(Guid id, Guid userId)
            => _activityService.DeleteAsync(id, userId);

        #endregion Activity

        #region Expense
        public Task<Result<ExpenseDto>> CreateExpenseAsync(CreateExpenseDto dto, Guid userId)
            => _expenseService.CreateAsync(dto, userId);

        public Task<Result<ExpenseDto>> GetExpenseByIdAsync(Guid id, Guid userId)
            => _expenseService.GetByIdAsync(id, userId);

        public Task<Result<IEnumerable<ExpenseDto>>> GetExpensesByTripIdAsync(Guid tripId, Guid userId)
            => _expenseService.GetByTripIdAsync(tripId, userId);

        public Task<Result<IEnumerable<ExpenseDto>>> GetExpensesByCategoryAsync(Guid tripId, BudgetCategory category, Guid userId)
            => _expenseService.GetByCategoryAsync(tripId, category, userId);

        public Task<Result<BudgetSummaryDto>> GetBudgetSummaryAsync(Guid tripId, Guid userId)
            => _expenseService.GetBudgetSummaryAsync(tripId, userId);

        public Task<Result<ExpenseDto>> UpdateExpenseAsync(Guid id, UpdateExpenseDto dto, Guid userId)
            => _expenseService.UpdateAsync(id, dto, userId);

        public Task<Result> DeleteExpenseAsync(Guid id, Guid userId)
            => _expenseService.DeleteAsync(id, userId);
        #endregion Expense
    }
}
