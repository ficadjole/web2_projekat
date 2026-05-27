using CheckListService.Interfaces.DTOs;
using CheckListService.Models;

namespace CheckListService.Mappers
{
    public static class MapChecklistItemToDto
    {
        public static ChecklistItemDto MapToDto(ChecklistItem c) => new()
        {
            Id = c.Id,
            Name = c.Name,
            IsChecked = c.IsChecked,
            ChecklistId = c.ChecklistId,
        };
    }
}
