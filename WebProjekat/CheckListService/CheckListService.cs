using CheckListService.DatabaseContext;
using CheckListService.Interfaces;
using CheckListService.Interfaces.DTOs;
using CheckListService.Mappers;
using CheckListService.Models;
using CheckListService.QueueModels;
using CheckListService.Repositories;
using CheckListService.Services;
using Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;
using System.Runtime.InteropServices;
using WebProjekat.Common;

namespace CheckListService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class CheckListService : StatefulService, IChecklistService
    {
        private readonly IChecklistBusinessService checklistService;
        private readonly ServiceProvider serviceProvider;
        //kljuc mi je tripId, radi lakse pretrage 
        private IReliableDictionary<Guid, ChecklistDto> checklistDictionary;
        private IReliableQueue<QueueItem> sqlQueue;


        public CheckListService(StatefulServiceContext context)
            : base(context)
        {

            var config = context.CodePackageActivationContext.GetConfigurationPackageObject("Config").Settings;

            var sqlSection = config.Sections["SqlConfig"];

            var sqlConn = sqlSection.Parameters["SqlConnectionString"].Value;

            var services = new ServiceCollection();

            services.AddDbContext<CheckListsDbContext>(options => options.UseSqlServer(sqlConn));

            services.AddScoped<IChecklistRepository, ChecklistRepository>();
            services.AddScoped<IChecklistItemRepository, ChecklistItemRepository>();

            services.AddScoped<IChecklistBusinessService, ChecklistBusinessService>();

            serviceProvider = services.BuildServiceProvider();

            checklistService = serviceProvider.GetRequiredService<IChecklistBusinessService>();

        }

        public async Task<Result<ChecklistItemDto>> AddItemAsync(CreateChecklistItemDto dto, Guid userId)
        {
            using (var tx = StateManager.CreateTransaction())
            {


                if (!(await checklistDictionary.ContainsKeyAsync(tx,dto.TripId)))
                {

                    Checklist checklist = Checklist.Create(dto.TripId.ToString(), userId.ToString()).Value;

                    await checklistDictionary.AddAsync(tx, dto.TripId, MapChecklistToDto.MapToDto(checklist));

                }

                var listDto = checklistDictionary.TryGetValueAsync(tx, dto.TripId).GetAwaiter().GetResult().Value;

                var newItemId = new Guid();

                var newItem = ChecklistItem.Create(newItemId.ToString(), dto.Name, false, listDto.Id.ToString());

                var itemDto = MapChecklistItemToDto.MapToDto(newItem.Value);

                listDto.Items.Add(itemDto);

                await checklistDictionary.SetAsync(tx, dto.TripId, listDto);

                await sqlQueue.EnqueueAsync(tx, new QueueItem { itemId = newItemId, userId = userId, OperationType = OperationTypes.Create, createDto = dto });
                

                await tx.CommitAsync();

                return Result<ChecklistItemDto>.Success(itemDto);

            }

           
        }

        public Task<Result> DeleteChecklistAsync(Guid checklistId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<ChecklistDto>> GetByTripIdAsync(Guid tripId, Guid userId)
        {
            using (var tx = StateManager.CreateTransaction()) {

                var result = checklistDictionary.TryGetValueAsync(tx, tripId);

                if (result.Result.HasValue && result.Result.Value.UserId == userId) { 
                
                    
                    return Result<ChecklistDto>.Success(result.Result.Value);

                }

            }

            return Result<ChecklistDto>.Failure("Checklist not found", ErrorType.NotFound);
        }

        public Task<Result<ChecklistItemDto>> ToggleItemAsync(Guid itemId, Guid userId)
        {
            throw new NotImplementedException();
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
            checklistDictionary = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, ChecklistDto>>("checklists");

            sqlQueue = await StateManager.GetOrAddAsync<IReliableQueue<QueueItem>>("sqlQueue");

            var allChecklists = await checklistService.GetAllAsync();

            using (var tx = StateManager.CreateTransaction()) {

                foreach (var checklist in allChecklists) {

                    await checklistDictionary.SetAsync(tx, checklist.TripId, checklist);

                }

                await tx.CommitAsync();

            }

            while (!cancellationToken.IsCancellationRequested)
            {
                using (var tx = StateManager.CreateTransaction())
                {
                    var result = await sqlQueue.TryDequeueAsync(tx);

                    if (result.HasValue)
                    {
                        var command = result.Value;

                        switch (command.OperationType)
                        {
                            case OperationTypes.Create:

                                // promeniti iz new Guid()-a u userId

                                await checklistService.AddItemAsync(command.itemId,command.createDto, command.userId);

                                break;
                            case OperationTypes.Delete:
                                break;
                            case OperationTypes.Update:
                                break;
                        }
                    }
                }
            }
        }
    }
}
