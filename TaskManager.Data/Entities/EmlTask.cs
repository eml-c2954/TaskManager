using System.ComponentModel.DataAnnotations;
using TaskManager.Common.Enums;

namespace TaskManager.Data.Entities
{
    public class EmlTask
    {
        [Key]
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public TaskStatusEnum Status { get; set; }
        public virtual ApplicationUser? Assignee { get; set; } = new ApplicationUser();
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
