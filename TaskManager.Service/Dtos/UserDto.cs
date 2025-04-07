namespace TaskManager.Service.Dtos
{
    public record UserDto
    {
        public string? Id { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
    }
}
