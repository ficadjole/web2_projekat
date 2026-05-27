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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;
using System.Text.Json;
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

        private ILogger<CheckListService> logger;


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

            services.AddLogging();

            serviceProvider = services.BuildServiceProvider();

            checklistService = serviceProvider.GetRequiredService<IChecklistBusinessService>();

            logger = serviceProvider.GetRequiredService<ILogger<CheckListService>>();
        }

        public async Task<Result<ChecklistItemDto>> AddItemAsync(CreateChecklistItemDto dto, Guid userId)
        {
            using (var tx = StateManager.CreateTransaction())
            {


                if (!(await checklistDictionary.ContainsKeyAsync(tx, dto.TripId)))
                {

                    Checklist checklist = Checklist.Create(Guid.NewGuid().ToString(), dto.TripId.ToString(), userId.ToString()).Value;

                    await checklistDictionary.AddAsync(tx, dto.TripId, MapChecklistToDto.MapToDto(checklist));

                }

                var result = await checklistDictionary.TryGetValueAsync(tx, dto.TripId);

                var listDto = result.Value;

                var newItemId = Guid.NewGuid();

                var newItem = ChecklistItem.Create(newItemId.ToString(), dto.Name, false, listDto.Id.ToString());

                var itemDto = MapChecklistItemToDto.MapToDto(newItem.Value);

                listDto.Items.Add(itemDto);

                await checklistDictionary.SetAsync(tx, dto.TripId, listDto);

                var createItemDto = new CreateItemQueueDTO { itemId = newItemId, createDto = dto, checklistDto = listDto };

                await sqlQueue.EnqueueAsync(tx, new QueueItem { userId = userId, OperationType = OperationTypes.Create, Payload = JsonSerializer.Serialize(createItemDto) });


                await tx.CommitAsync();

                return Result<ChecklistItemDto>.Success(itemDto);

            }


        }

        public async Task<Result> DeleteChecklistAsync(Guid tripId, Guid userId)
        {
            using (var tx = StateManager.CreateTransaction())
            {

                var result = checklistDictionary.TryGetValueAsync(tx, tripId);

                if (result.Result.HasValue && result.Result.Value.UserId == userId)
                {

                    await checklistDictionary.TryRemoveAsync(tx, tripId);

                    await sqlQueue.EnqueueAsync(tx, new QueueItem { userId = userId, OperationType = OperationTypes.Delete, Payload = JsonSerializer.Serialize(tripId) });

                    await tx.CommitAsync();

                    return Result.Success();
                }
                else
                {
                    return Result.Failure("Checklist not found", ErrorType.NotFound);
                }
            }
        }

        public async Task<Result> DeleteItemAsync(Guid tripId, Guid itemId, Guid userId)
        {
            using (var tx = StateManager.CreateTransaction())
            {

                var result = checklistDictionary.TryGetValueAsync(tx, tripId);

                if (result.Result.HasValue && result.Result.Value.UserId == userId)
                {
                    var checklist = result.Result.Value;

                    var item = checklist.Items.FirstOrDefault(i => i.Id == itemId);

                    if (item != null)
                    {
                        checklist.Items.Remove(item);

                        await checklistDictionary.SetAsync(tx, tripId, checklist);

                        await sqlQueue.EnqueueAsync(tx, new QueueItem { userId = userId, OperationType = OperationTypes.DeleteItem, Payload = JsonSerializer.Serialize(itemId) });

                        await tx.CommitAsync();

                        return Result.Success();
                    }
                    else
                    {
                        return Result.Failure("Item not found", ErrorType.NotFound);
                    }
                }
                else
                {
                    return Result.Failure("Checklist not found", ErrorType.NotFound);

                }
            }
        }

        public async Task<Result<ChecklistDto>> GetByTripIdAsync(Guid tripId, Guid userId)
        {
            using (var tx = StateManager.CreateTransaction())
            {

                var result = await checklistDictionary.TryGetValueAsync(tx, tripId);

                if (result.HasValue && result.Value.UserId == userId)
                {


                    return Result<ChecklistDto>.Success(result.Value);

                }

            }

            return Result<ChecklistDto>.Failure("Checklist not found", ErrorType.NotFound);
        }

        public async Task<Result<ChecklistItemDto>> ToggleItemAsync(Guid tripId, Guid itemId, Guid userId)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                var result = checklistDictionary.TryGetValueAsync(tx, tripId);

                if (result.Result.HasValue && result.Result.Value.UserId == userId)
                {

                    var checklist = result.Result.Value;

                    var item = checklist.Items.FirstOrDefault(i => i.Id == itemId);

                    if (item != null)
                    {
                        item.IsChecked = !item.IsChecked;

                        await checklistDictionary.SetAsync(tx, tripId, checklist);

                        await sqlQueue.EnqueueAsync(tx, new QueueItem { userId = userId, OperationType = OperationTypes.Toggle, Payload = JsonSerializer.Serialize(itemId) });

                        await tx.CommitAsync();

                        return Result<ChecklistItemDto>.Success(item);
                    }
                    else
                    {
                        return Result<ChecklistItemDto>.Failure("Item not found", ErrorType.NotFound);
                    }
                }
                else
                {
                    return Result<ChecklistItemDto>.Failure("Checklist not found", ErrorType.NotFound);
                }
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
            checklistDictionary = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, ChecklistDto>>("checklists");

            sqlQueue = await StateManager.GetOrAddAsync<IReliableQueue<QueueItem>>("sqlQueue");

            var loaded = false;
            while (!loaded && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var allChecklists = await checklistService.GetAllAsync();

                    if (allChecklists.Any())
                    {
                        using (var tx = StateManager.CreateTransaction())
                        {
                            foreach (var checklist in allChecklists)
                            {
                                await checklistDictionary.SetAsync(tx, checklist.TripId, checklist);
                            }
                            await tx.CommitAsync();
                        }
                    }

                    loaded = true;

                    logger.LogInformation("Checklist dictionary successfully loaded from SQL.");
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed to load checklists. Exception type: {ExceptionType}, Message: {Message}, InnerException: {InnerException}",
                    ex.GetType().Name,
                    ex.Message,
                    ex.InnerException?.Message);
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                }
            }

            while (!cancellationToken.IsCancellationRequested)
            {
                using (var tx = StateManager.CreateTransaction())
                {
                    var result = await sqlQueue.TryDequeueAsync(tx);

                    if (result.HasValue)
                    {
                        var command = result.Value;


                        try
                        {
                            switch (command.OperationType)
                            {
                                case OperationTypes.Create:

                                    var createItemQeueuDTO = JsonSerializer.Deserialize<CreateItemQueueDTO>(command.Payload);

                                    await checklistService.AddItemAsync(createItemQeueuDTO!.itemId, createItemQeueuDTO.createDto, createItemQeueuDTO.checklistDto!, command.userId);
                                    logger.LogInformation("Successfully written to SQL for item: {ItemId}", createItemQeueuDTO.itemId);
                                    break;
                                case OperationTypes.Delete:

                                    var tripId = JsonSerializer.Deserialize<Guid>(command.Payload);

                                    await checklistService.DeleteChecklistAsync(tripId, command.userId);

                                    break;
                                case OperationTypes.Toggle:

                                    var itemId = JsonSerializer.Deserialize<Guid>(command.Payload);

                                    await checklistService.ToggleItemAsync(itemId, command.userId);

                                    break;
                                case OperationTypes.DeleteItem:

                                    var deleteItemId = JsonSerializer.Deserialize<Guid>(command.Payload);

                                    await checklistService.DeleteItemAsync(deleteItemId, command.userId);

                                    break;
                            }

                            await tx.CommitAsync();
                        }
                        catch (Exception ex)
                        {

                            logger.LogError(ex, "Error processing queue item. OperationType: {OperationType}, Payload: {Payload}", command.OperationType, command.Payload);

                            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                        }


                    }
                    else
                    {
                        await Task.Delay(100, cancellationToken);
                    }
                }
            }
        }
    }
}
