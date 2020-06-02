﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetDemo
{
    public static class IdentityExtensions
    {
        public static string GetConnectionId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.FindFirstValue(Models.AuthConstants.Claim_ConnectionId);
        }

        public static async Task Login(this HttpContext httpContext, string connectionId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, connectionId),
                new Claim(Models.AuthConstants.Claim_ConnectionId, connectionId)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, Models.AuthConstants.CookieScheme);
            
            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await httpContext.SignInAsync(
                claimsIdentity.AuthenticationType,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public static async Task Logout(this HttpContext httpContext, string authScheme)
        {
            await httpContext.SignOutAsync(authScheme);
        }
    }
}