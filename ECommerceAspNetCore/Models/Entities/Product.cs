using System.Collections.Generic;

namespace ECommerceAspNetCore.Models.Entities
{
    public class Product : BaseEntity
    {
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal Price { get; set; }

        public virtual List<CartProducts> CartProducts { get; set; }
    }
}
