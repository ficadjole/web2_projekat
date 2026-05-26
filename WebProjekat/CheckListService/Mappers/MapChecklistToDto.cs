using CheckListService.Interfaces.DTOs;
using CheckListService.Models;

namespace CheckListService.Mappers
{
    public static class MapChecklistToDto
    {

        public static ChecklistDto MapToDto(Checklist list) => new()
        {
            Id = list.Id,
            TripId = list.TripId,
            UserId = list.UserId,
            Items = list.Items?.Select(i => new ChecklistItemDto
            {
                Id = i.Id,
                Name = i.Name,
                IsChecked = i.IsChecked,
                ChecklistId = i.ChecklistId,
            }).ToList() ?? []
        };

    }
}
