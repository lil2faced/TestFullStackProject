using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebAPI.Application.DTO.UserAPI;
using WebAPI.Core.Entities;
using WebAPI.Core.Exceptions;
using WebAPI.Core.Interfaces;
using WebAPI.Infrastructure.EfCore;

namespace WebAPI.Infrastructure.Services
{
    public class ApiRoleService : IUserApiRoleService
    {
        private readonly IMapper _mapper;
        private readonly ApiDbContext _dbContext;
        private readonly ILogger<ApiRoleService> _logger;
        public ApiRoleService(IMapper mapper, ApiDbContext databaseContext, ILogger<ApiRoleService> logger)
        {
            _dbContext = databaseContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task DeleteRole(string role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            bool IsHave = await _dbContext.ApiRoles.AnyAsync(p => p.Role == role, cancellationToken);

            if (!IsHave)
                throw new NotFoundException("Роль не найдена");

            var temp = await _dbContext.ApiRoles.FirstOrDefaultAsync(p => p.Role == role, cancellationToken)
                ?? throw new NotFoundException("Роль не найдена");

            _dbContext.ApiRoles.Remove(temp);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Роль {role} была удалена");
        }

        public async Task EditRole(string roleToEdit, DTOAPIRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role is null)
                throw new ArgumentNullException("На входе пришел NULL");

            APIRole existingUser = await _dbContext.ApiRoles
                .FirstOrDefaultAsync(p => p.Role == role.Name, cancellationToken) ??
                throw new NotFoundException("Роль не найдена");

            _mapper.Map(role, existingUser);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Роль {role} была отредактирована");
        }

        public async Task<IEnumerable<DTOAPIRole>> GetAllRoles(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var roles = await _dbContext.ApiRoles
                .ProjectTo<DTOAPIRole>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            _logger.LogInformation($"Запрос на получение всех ролей");

            return roles;
        }

        public async Task SetRole(DTOAPIRole roleSet, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (roleSet is null)
                throw new ArgumentNullException("На входе пришел NULL");

            cancellationToken.ThrowIfCancellationRequested();

            bool IsHave = await _dbContext.ApiRoles.AnyAsync(p => p.Role == roleSet.Name, cancellationToken);

            if (IsHave)
                throw new BadRequestException("Такая роль уже существует");

            APIRole role = _mapper.Map<APIRole>(roleSet);

            await _dbContext.AddAsync(role);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Роль {role.Role} была добавлена");
        }
    }
}
