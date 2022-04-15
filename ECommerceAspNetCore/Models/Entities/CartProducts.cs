using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAspNetCore.Models.Entities
{
    public class CartProducts
    {
        public Guid CartId { get; set; }

        public Guid ProductId { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public bool IsSold { get; set; } = false;

        [ForeignKey(nameof(CartId))]
        public Cart Cart { get; set; }


        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

    }
}
