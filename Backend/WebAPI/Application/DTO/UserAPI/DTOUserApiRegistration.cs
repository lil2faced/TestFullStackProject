namespace WebAPI.Application.DTO.UserAPI
{
    public class DTOUserAPIRegistration
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; } = "User";
    }
}
