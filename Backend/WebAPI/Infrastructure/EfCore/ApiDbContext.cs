using Microsoft.EntityFrameworkCore;
using WebAPI.Core.Entities;

namespace WebAPI.Infrastructure.EfCore
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
            var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
            var db = Environment.GetEnvironmentVariable("POSTGRES_DB");
            var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
            var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

            var connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={password};";
            options.UseNpgsql(connectionString);
        }
        public DbSet<APIRole> ApiRoles { get; set; }
        public DbSet<UserAPI> ApiUsers { get; set; }
    }
}
