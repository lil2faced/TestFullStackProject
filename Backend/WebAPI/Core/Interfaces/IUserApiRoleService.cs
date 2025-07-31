using WebAPI.Application.DTO.UserAPI;

namespace WebAPI.Core.Interfaces
{
    public interface IUserApiRoleService
    {
        Task<IEnumerable<DTOAPIRole>> GetAllRoles(CancellationToken cancellationToken);
        Task SetRole(DTOAPIRole userSet, CancellationToken cancellationToken);
        Task EditRole(string role, DTOAPIRole userEdit, CancellationToken cancellationToken);
        Task DeleteRole(string role, CancellationToken cancellationToken);
    }
}
