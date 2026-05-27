using CheckListService.Interfaces.DTOs;

namespace CheckListService.QueueModels
{
    public class CreateItemQueueDTO
    {
        public Guid itemId { get; set; }

        public CreateChecklistItemDto createDto { get; set; }

        public ChecklistDto? checklistDto { get; set; }
    }
}
