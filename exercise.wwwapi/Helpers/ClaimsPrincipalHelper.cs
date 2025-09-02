﻿using System.Security.Claims;

namespace exercise.wwwapi.Helpers
{
    
    public static class ClaimsPrincipalHelper
    {
   
        public static int? UserRealId(this ClaimsPrincipal user)
        {
            Claim? claim = user.FindFirst(ClaimTypes.Sid);
            return int.Parse(claim?.Value);
            string name = user.Identity?.Name;
            
        }
        public static string UserId(this ClaimsPrincipal user)
        {
            IEnumerable<Claim> claims = user.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier);
            return claims.Count() >= 2 ? claims.ElementAt(1).Value : null;

        }

        public static string? Email(this ClaimsPrincipal user)
        {
            Claim? claim = user.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
        public static string? Role(this ClaimsPrincipal user)
        {
            Claim? claim = user.FindFirst(ClaimTypes.Role);
            return claim?.Value;
        }

    }
}
