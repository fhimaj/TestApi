using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TestApi.Domain.Entities.Users;
using TestApi.Infrastructure.Extensions;

namespace TestApi.Infrastructure.Helpers
{
    public static class UserHelper
    {
        public static IList<Claim> CreateUserClaims(User user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };
        }

        public static int GetUserId(IHttpContextAccessor httpContextAccessor)
        {
            var userIdString = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdString.IsEmpty())
                return 0;

            bool userIdResult = Int32.TryParse(userIdString, out int userId);

            if (!userIdResult || userId <= 0)
                return 0;

            return userId;
        }
    }
}
