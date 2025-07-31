namespace WebAPI.Core.Entities
{
    public class APIRole
    {
        public Guid Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public ICollection<UserAPI> ApiUsers { get; set; } = new List<UserAPI>();
    }
}
