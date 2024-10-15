// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using IdentityModel;
using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer;
using Duende.IdentityServer.Test;

namespace IDP;

public static class TestUsers
{
    public static List<TestUser> Users
    {
        get
        {
           
                
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "adshelly",
                    Password = "Password123$",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Arun Jayabalan"),
                        new Claim(JwtClaimTypes.GivenName, "Arun"),
                        new Claim(JwtClaimTypes.FamilyName, "Jayabalan"),
                        new Claim(JwtClaimTypes.Email, "ashelly@sparinc.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean), 
                        new Claim("SURID","adshelly"),
                        new Claim("UTYPE","S"),
                        new Claim("PID","XXU"),
                        new Claim("WDID","ashelly@sparinc.com")                        
                    }
                },
                
            };
        }
    }
}