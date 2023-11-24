﻿using Microsoft.AspNetCore.Identity;

namespace UserJourney.Data
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
