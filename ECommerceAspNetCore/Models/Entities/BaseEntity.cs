﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerceAspNetCore.Models.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

    }
}
