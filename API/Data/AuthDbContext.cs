using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "0bff9575-84ae-4dff-8197-7feca950f319";
            var writerRoleId = "039a81ab-0efc-4dc4-89ae-63af1b35f0c6";

            //Create Reader and Writer Role
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id=readerRoleId,
                    Name="Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };

            //Seed Roles
            builder.Entity<IdentityRole>().HasData(roles);

            //Create Admin User
            var adminUserId = "f2918eb8-649f-4eb9-a81d-7f7870f38b4c";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@blogtech.com",
                Email = "admin@blogtech.com",
                NormalizedEmail = "admin@blogtech.com".ToUpper(),
                NormalizedUserName = "admin@blogtech.com".ToUpper(),
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

            builder.Entity<IdentityUser>().HasData(admin);

            //Give Roles to Admin
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = writerRoleId
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);

        }
    }
}

