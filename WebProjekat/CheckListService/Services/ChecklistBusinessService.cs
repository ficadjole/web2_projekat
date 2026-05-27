using CheckListService.Interfaces;
using CheckListService.Interfaces.DTOs;
using CheckListService.Mappers;
using CheckListService.Models;
using Common.Enums;
using WebProjekat.Common;

namespace CheckListService.Services
{
    public class ChecklistBusinessService : IChecklistBusinessService
    {

        private readonly IChecklistRepository _checklistRepository;
        private readonly IChecklistItemRepository _listItemRepository;

        public ChecklistBusinessService(IChecklistRepository checklistRepository, IChecklistItemRepository listItemRepository)
        {
            _checklistRepository = checklistRepository;
            _listItemRepository = listItemRepository;
        }

        public async Task<Result<ChecklistItemDto>> AddItemAsync(Guid itemId, CreateChecklistItemDto dto, ChecklistDto checklistDto, Guid userId)
        {
            var checklist = await _checklistRepository.GetByTripIdAsync(dto.TripId);

            if (checklist is null)
            {
                var checklistResult = Checklist.Create(checklistDto.Id.ToString(), checklistDto.TripId.ToString(), userId.ToString());

                if (checklistResult.IsFailure)
                    return Result<ChecklistItemDto>.Failure(checklistResult.Error!.Message);

                checklist = checklistResult.Value;

                await _checklistRepository.CreateChecklistAsync(checklist);
            }

            if (checklist.UserId != userId)
                return Result<ChecklistItemDto>.Failure("Unauthorized.", ErrorType.Unauthorized);

            if (string.IsNullOrWhiteSpace(dto.Name))
                return Result<ChecklistItemDto>.Failure("Item name is required.", ErrorType.Validation);

            var itemResult = ChecklistItem.Create(itemId.ToString(), dto.Name, false, checklist.Id.ToString());

            if (itemResult.IsFailure)
                return Result<ChecklistItemDto>.Failure(itemResult.Error!.Message);

            await _listItemRepository.AddItemAsync(itemResult.Value);

            var listItemDto = new ChecklistItemDto
            {
                Id = itemResult.Value.Id,
                Name = itemResult.Value.Name,
                IsChecked = itemResult.Value.IsChecked,
                ChecklistId = itemResult.Value.ChecklistId
            };

            return Result<ChecklistItemDto>.Success(listItemDto);
        }

        public async Task<Result> DeleteChecklistAsync(Guid tripId, Guid userId)
        {
            var checklist = await _checklistRepository.GetByTripIdAsync(tripId);

            if (checklist is null)
                return Result.Failure("Checklist not found.", ErrorType.NotFound);

            if (checklist.UserId != userId)
                return Result.Failure("Unauthorized.", ErrorType.Unauthorized);

            await _checklistRepository.DeleteChecklistAsync(tripId);

            return Result.Success();
        }

        public async Task<Result> DeleteItemAsync(Guid itemId, Guid userId)
        {
            var item = await _listItemRepository.GetItemByIdAsync(itemId);

            if (item is null)
                return Result.Failure("Item not found.", ErrorType.NotFound);

            var checklist = await _checklistRepository.GetByIdAsync(item.ChecklistId);

            if (checklist.UserId != userId)
                return Result.Failure("Unauthorized.", ErrorType.Unauthorized);

            await _listItemRepository.DeleteItemAsync(itemId);

            return Result.Success();
        }

        public async Task<IEnumerable<ChecklistDto>> GetAllAsync()
        {
            var checklists = await _checklistRepository.GetAllAsync();

            return checklists.Select(MapChecklistToDto.MapToDto);
        }

        public async Task<Result<ChecklistDto>> GetByTripIdAsync(Guid tripId, Guid userId)
        {
            var checklist = await _checklistRepository.GetByTripIdAsync(tripId);

            if (checklist is null)
                return Result<ChecklistDto>.Failure("Checklist not found.", ErrorType.NotFound);

            if (checklist.UserId != userId)
                return Result<ChecklistDto>.Failure("Unauthorized.", ErrorType.Unauthorized);

            return Result<ChecklistDto>.Success(MapChecklistToDto.MapToDto(checklist));
        }

        public async Task<Result<ChecklistItemDto>> ToggleItemAsync(Guid itemId, Guid userId)
        {
            var item = await _listItemRepository.GetItemByIdAsync(itemId);

            if (item is null)
                return Result<ChecklistItemDto>.Failure("Item not found.", ErrorType.NotFound);

            var checklist = await _checklistRepository.GetByIdAsync(item.ChecklistId);

            if (checklist.UserId != userId)
                return Result<ChecklistItemDto>.Failure("Unauthorized.", ErrorType.Unauthorized);

            item.IsChecked = !item.IsChecked;

            await _listItemRepository.UpdateItemAsync(item);

            var checklistitemDto = new ChecklistItemDto
            {
                Id = item.Id,
                Name = item.Name,
                IsChecked = item.IsChecked,
                ChecklistId = item.ChecklistId
            };

            return Result<ChecklistItemDto>.Success(checklistitemDto);
        }
    }
}
