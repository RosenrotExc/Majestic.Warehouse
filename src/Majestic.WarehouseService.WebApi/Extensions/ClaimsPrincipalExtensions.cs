using System.Security.Claims;
using Majestic.WarehouseService.Models.Misc;

namespace Majestic.WarehouseService.WebApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Initiator GetStubInitiator(this ClaimsPrincipal principal)
        {
            return new Initiator
            {
                SubjectId = Guid.NewGuid().ToString(),
                Email = $"test{Guid.NewGuid():D}@gmail.com",
            };
        }

        public static Initiator GetInitiator(this ClaimsPrincipal principal)
        {
            return new Initiator
            {
                SubjectId = principal.GetSubjectId(),
                Email = principal.GetEmail(),
            };
        }

        public static string GetEmail(this ClaimsPrincipal principal)
        {
            return principal.Claims.SingleOrDefault(claim => claim.Type == "email")?.Value;
        }

        public static string GetSubjectId(this ClaimsPrincipal principal)
        {
            return principal.Claims.SingleOrDefault(claim => claim.Type == "sub")?.Value;
        }

        public static string GetRequestId(this HttpContext httpContext)
        {
            return httpContext.Request.Headers["X-Request-ID"];
        }
    }
}
