namespace WebApi.DTOs.Checklist
{
    public class AddChecklistItemRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid TripId { get; set; }
    }
}
