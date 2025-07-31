using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPI.Application.DTO.UserAPI;
using WebAPI.Core.Entities;
using WebAPI.Core.Exceptions;
using WebAPI.Core.Interfaces;
using WebAPI.Infrastructure.EfCore;

namespace WebAPI.Infrastructure.Services
{
    public class UserApiService : IUserApiService
    {
        private readonly ApiDbContext _databaseContext;
        private readonly IMapper _mapper;
        private readonly IJWTProvider _jwtProvider;
        private readonly ILogger<UserApiService> _logger;
        private readonly IPasswordHasher _passwordHasher;

        public UserApiService(ApiDbContext databaseContext, IMapper mapper, IJWTProvider jwtProvider, ILogger<UserApiService> logger, IPasswordHasher passwordHasher)
        {
            _databaseContext = databaseContext;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Login(DTOUserAPILogin user, CancellationToken cancellationToken)
        {
            var userApi = await _databaseContext.ApiUsers
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Name == user.Name, cancellationToken);

            if (user is null)
                throw new ArgumentNullException("В качестве аргумента получен NULL");

            cancellationToken.ThrowIfCancellationRequested();

            if (!_passwordHasher.VerifyHashedPassword(userApi.Password, user.Password))
                throw new NotAutentificationException("Пользователь не аутентифицирован");

            var userDto = new DTOUserAPIJwt
            {
                Id = userApi.Id,
                Name = userApi.Name,
                Role = new DTOAPIRole { Name = userApi.Role.Role } // Явное преобразование
            };

            var jwtToken = _jwtProvider.GenerateToken(userDto);

            _logger.LogInformation($"Пользователь API {user.Name} вошел в систему, токен был сгенерирован");
            return jwtToken;
        }

        public async Task Register(DTOUserAPIRegistration user, CancellationToken cancellationToken)
        {
            if (user is null)
                throw new ArgumentNullException("В качестве аргумента получен NULL");

            bool userExists = await _databaseContext.ApiUsers
                .AnyAsync(p => p.Name == user.Name, cancellationToken);

            if (userExists)
                throw new BadRequestException("Такой пользователь уже существует");

            var role = await _databaseContext.ApiRoles
                .FirstOrDefaultAsync(r => r.Role == user.RoleName, cancellationToken)
                ?? throw new NotFoundException($"Роль '{user.RoleName}' не найдена");

            if (role == null)
            {
                role = new APIRole { Role = user.RoleName };
                await _databaseContext.ApiRoles.AddAsync(role, cancellationToken);
            }

            var newUser = new UserAPI
            {
                Name = user.Name,
                Password = _passwordHasher.HashPassword(user.Password),
                Role = role // Связываем с существующей или новой ролью
            };


            await _databaseContext.ApiUsers.AddAsync(newUser, cancellationToken);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Создан новый пользователь API: {user.Name} с ролью {user.RoleName}");
        }
    }
}
