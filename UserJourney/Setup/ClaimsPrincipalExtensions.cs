using System.Security.Claims;

namespace UserJourney.Setup
{
    public static class ClaimsPrincipalExtensions
    {
        //it's get claims and return userId
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            string stringId = principal.FindFirst(ClaimTypes.Sid)?.Value; 

            return stringId;
        }

        public static Guid GetCompanyId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            string stringId = principal.FindFirst("CId")?.Value;

            return new Guid(stringId); 
        } 

        //It's not used
        public static string GetRole(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var stringId = principal.FindFirst(ClaimTypes.Role)?.Value;

            return stringId;
        }

    }
}
