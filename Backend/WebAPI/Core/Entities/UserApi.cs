namespace WebAPI.Core.Entities
{
    public class UserAPI
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Guid RoleId { get; set; }
        public APIRole Role { get; set; } = null!;
    }
}
