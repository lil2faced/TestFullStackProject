using WebAPI.Application.DTO.UserAPI;

namespace WebAPI.Core.Interfaces
{
    public interface IUserApiService
    {
        Task Register(DTOUserAPIRegistration user, CancellationToken cancellationToken);
        Task<string> Login(DTOUserAPILogin user, CancellationToken cancellationToken);
    }
}
