using ECommerceAspNetCore.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;

namespace ECommerceAspNetCore.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Cart Cart { get; set; }

    }
}
