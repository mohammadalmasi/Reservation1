using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using UserManagement.Domain.Model;
using UserManagement.Persistance.EF.Mapping;

namespace UserManagement.Persistance.EF
{
    public class UserManagmentDbContext : IdentityDbContext<User, Role, Guid,UserClaim,
        UserRole, UserLogin, RoleClaim, UserToken>
    {
        public UserManagmentDbContext(DbContextOptions<UserManagmentDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            new UserEntityConfiguration().Configure(builder.Entity<User>());
            new RoleEntityConfiguration().Configure(builder.Entity<Role>());
            new UserRoleEntityConfiguration().Configure(builder.Entity<UserRole>());
            new UserClaimEntityConfiguration().Configure(builder.Entity<UserClaim>());
            new UserLoginEntityConfiguration().Configure(builder.Entity<UserLogin>());
            new UserTokenEntityConfiguration().Configure(builder.Entity<UserToken>());
            new RoleClaimEntityConfiguration().Configure(builder.Entity<RoleClaim>());
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
    public class UserManagmentDbContextFactory : IDesignTimeDbContextFactory<UserManagmentDbContext>
    {
        public UserManagmentDbContext CreateDbContext(string[] args)
        {
            string conn = "Server=MOHAMMAD-PC;Database=SecurityDb;User ID=sa;Password=890201794;";
            var optionsBuilder = new DbContextOptionsBuilder<UserManagmentDbContext>();
            optionsBuilder.UseSqlServer(conn);

            return new UserManagmentDbContext(optionsBuilder.Options);
        }
    }
}
