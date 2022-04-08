using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace crmweb.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool HasClaim(this ClaimsPrincipal principal, string type)
        {
            return principal != null && principal.HasClaim(claim => claim.Type == type);
        }

        public static bool HasClaim(this ClaimsIdentity principal, string type)
        {
            return principal != null && principal.HasClaim(claim => claim.Type == type);
        }

        public static string GetValue(this IEnumerable<Claim> claims, string type)
        {
            var vClaim = claims?.SingleOrDefault(claim => claim.Type == type);
            return vClaim?.Value;
        }

        public static IEnumerable<string> GetValues(this IEnumerable<Claim> claims, string claimType)
        {
            if (claims == null)
                return Enumerable.Empty<string>();

            IEnumerable<string> vQuery =
                from vClaim in claims
                where vClaim.Type == claimType
                select vClaim.Value;

            return vQuery;
        }

        public static T GetSubject<T>(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            Claim vClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

            if (vClaim == null)
                return default(T);

            return (T)Convert.ChangeType(vClaim.Value, typeof(T));
        }

        public static T GetValue<T>(this ClaimsPrincipal principal, string type)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            Claim vClaim = principal.FindFirst(type);

            if (vClaim == null)
                return default(T);

            return (T)Convert.ChangeType(vClaim.Value, typeof(T));
        }

        public static T GetEnumValue<T>(this ClaimsPrincipal principal, string type) where T : Enum
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            Claim vClaim = principal.FindFirst(type);

            if (vClaim == null)
                return default(T);

            return (T)Enum.Parse(typeof(T), vClaim.Value, true);
        }
    }
}
