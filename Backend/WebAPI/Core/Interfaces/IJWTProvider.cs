using WebAPI.Application.DTO.UserAPI;

namespace WebAPI.Core.Interfaces
{
    public interface IJWTProvider
    {
        string GenerateToken(DTOUserAPIJwt user);
    }
}
