namespace WebAPI.Application.DTO.UserAPI
{
    public class DTOAPIRole
    {
        public string Name { get; set; } = "User";
    }
    public class DTOUserAPIJwt 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DTOAPIRole Role { get; set; }
    }
}
