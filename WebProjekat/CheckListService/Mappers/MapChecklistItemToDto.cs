using CheckListService.Interfaces.DTOs;
using CheckListService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
