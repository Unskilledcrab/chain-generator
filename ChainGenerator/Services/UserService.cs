using ChainGenerator.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ChainGenerator.Services
{
    public class UserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApplicationUser?> GetUserAsync()
        {
            if (httpContextAccessor?.HttpContext?.User == null)
                return null;

            return await userManager.GetUserAsync(httpContextAccessor?.HttpContext?.User);
        }
    }
}
