using ECommerceAspNetCore.Data;
using ECommerceAspNetCore.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ECommerceAspNetCore.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly MyContext _dbContext;

        public ProductController(MyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult GetProducts()
        {
            var products = _dbContext.Products.ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(Product model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var product = new Product()
            {
                ProductName = model.ProductName,
                ProductDescription = model.ProductDescription,
                Price = model.Price
            };
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
            return RedirectToAction("GetProducts");
        }

        [HttpGet]
        public IActionResult ProductDetail(Guid Id)
        {
            var product = _dbContext.Products.Find(Id);
            return View(product);
        }
    }
}
