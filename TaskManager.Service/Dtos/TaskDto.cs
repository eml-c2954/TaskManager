using System.ComponentModel.DataAnnotations;
using TaskManager.Common.Enums;

namespace TaskManager.Service.Dtos
{
    public record TaskDto
    {
        public Guid? Id { get; set; }
        public required string Title { get; set; }

        [MinLength(10, ErrorMessage = "Description must be at least 10 characters long.")]
        public string? Description { get; set; }
        public UserDto? Assignee { get; set; }
        public TaskStatusEnum Status { get; set; }
    }
}
