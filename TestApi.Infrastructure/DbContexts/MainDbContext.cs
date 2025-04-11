using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;
using TestApi.Domain.Entities;
using TestApi.Domain.Entities.Posts;
using TestApi.Domain.Entities.Users;
using TestApi.Infrastructure.Extensions;

namespace TestApi.Infrastructure.DbContexts
{
    public class MainDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MainDbContext(
            DbContextOptions<MainDbContext> options,
            IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new OracleBooleanInterceptor());
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var boolProperties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(bool) &&
                               p.GetCustomAttribute<OracleBoolAttribute>() != null);
            }
        }
        public DbSet<User> User { get; set; }
        public DbSet<Post> Post { get; set; }


        public override int SaveChanges()
        {
            UpdateEntity();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            UpdateEntity();
            return base.SaveChangesAsync(cancellationToken);
        }

        public void UpdateEntity()
        {
            bool signUp = false;
            var userIdString = _httpContextAccessor.HttpContext.Items["SignUpUserId"] as string;

            if (userIdString.IsEmpty())
                userIdString = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdString.IsEmpty())
                throw new UnauthorizedAccessException("User not authenticated.");

            int userId = 0;

            if (!userIdString.Contains("SignUpId"))
            {
                userId = Convert.ToInt32(userIdString);

                if (userId == 0)
                    throw new UnauthorizedAccessException("User not authenticated.");
            }
            else
                signUp = true;

            var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity && (e.State is EntityState.Added or EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdateDate = DateTime.UtcNow;
                ((BaseEntity)entityEntry.Entity).UpdatedBy = signUp ? 0 : userId;

                if (entityEntry.State is EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).InsertDate = DateTime.UtcNow;
                    ((BaseEntity)entityEntry.Entity).InsertedBy = signUp ? 0 : userId;
                }
            }
        }
    }
}
