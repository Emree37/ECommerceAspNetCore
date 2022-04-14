using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace ECommerceAspNetCore.Extensions
{
    public static class AppExtensions
    {
        public static string GetUserId(this HttpContext context)
        {
            var claims = context.User.Claims.ToList();
            return context.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
