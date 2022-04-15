using System;

namespace ECommerceAspNetCore.Models.Entities
{
    public class Report : BaseEntity
    {
        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public Guid CartId { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
