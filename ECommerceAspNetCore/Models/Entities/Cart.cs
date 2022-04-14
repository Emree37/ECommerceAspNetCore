using ECommerceAspNetCore.Models.Identity;
using System;
using System.Collections.Generic;

namespace ECommerceAspNetCore.Models.Entities
{
    public class Cart : BaseEntity
    {
        public virtual List<CartProducts> CartProducts { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
