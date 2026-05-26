using CheckListService.Interfaces.DTOs;
using Common.Enums;


namespace CheckListService.QueueModels
{
    public class QueueItem
    {
        public Guid itemId { get; set; }

        public Guid userId { get; set; }

        public OperationTypes OperationType { get; set; }

        public CreateChecklistItemDto createDto { get; set; }

    }
}
