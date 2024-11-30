using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Bookstore.Infrastructure
{
    public class IdentitySeed
    {
        public UserManager<IdentityUser> UserManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }
        public ApplicationDbContext ApplicationDbContext { get; }
        public IConfiguration Configuration { get; }

        public IdentitySeed(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext applicationDbContext,
            IConfiguration configuration)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            ApplicationDbContext = applicationDbContext;
            Configuration = configuration;
        }

        public async Task Seed()
        {
            var email = Configuration["ADMIN_EMAIL"];
            var password = Configuration["ADMIN_PW"];

            var roles = new string[] { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!ApplicationDbContext.Roles.Any(r => r.Name == role))
                    await RoleManager.CreateAsync(new IdentityRole(role));
            }

            if (!ApplicationDbContext.Users.Any(u => u.UserName == email))
            {
                var identity = new IdentityUser { UserName = email, Email = email };
                var result = await UserManager.CreateAsync(identity, password);
                var user = await UserManager.FindByNameAsync(email);
                await UserManager.AddToRoleAsync(user, "Admin");
            }

        }

    }
}
