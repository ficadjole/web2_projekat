using Common.Enums;


namespace CheckListService.QueueModels
{
    public class QueueItem
    {
        public Guid userId { get; set; }

        public OperationTypes OperationType { get; set; }

        public string Payload { get; set; }

    }
}
