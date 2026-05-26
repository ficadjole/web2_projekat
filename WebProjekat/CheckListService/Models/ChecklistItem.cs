using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProjekat.Common;

namespace CheckListService.Models
{
    public class ChecklistItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsChecked { get; set; } = false;

        public Guid ChecklistId { get; set; }

        public Checklist Checklist { get; set; }

        protected ChecklistItem() { }

        private ChecklistItem(Guid id, string name, bool isChecked, Guid checklistId)
        {
            Id = id;
            Name = name;
            IsChecked = isChecked;
            ChecklistId = checklistId;
        }

        public static Result<ChecklistItem> Create(string name, bool isChecked, string checklistId) { 
        
            if(!Guid.TryParse(checklistId, out Guid id))
                return Result<ChecklistItem>.Failure("Invalid checklist id",ErrorType.Validation);

            var checklistItem = new ChecklistItem(new Guid(), name, isChecked, id);

            return Result<ChecklistItem>.Success(checklistItem);
        
        }

        public static Result<ChecklistItem> Load(string id, string name, bool isChecked, Guid checklistId)
        {
            if (!Guid.TryParse(id, out Guid Id))
                return Result<ChecklistItem>.Failure("Invalid checklist item id", ErrorType.Validation);

            var checklistItem = new ChecklistItem(Id,name,isChecked,checklistId); 
            
            return Result<ChecklistItem>.Success(checklistItem);
        }
    } 
    
}
