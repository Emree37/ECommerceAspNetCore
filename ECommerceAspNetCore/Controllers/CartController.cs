using ECommerceAspNetCore.Data;
using ECommerceAspNetCore.Extensions;
using ECommerceAspNetCore.Models.Entities;
using ECommerceAspNetCore.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceAspNetCore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly MyContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(MyContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        
        public async Task<IActionResult> MyCart()
        {
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());
            var cart = _dbContext.Carts.SingleOrDefault(x=>x.ApplicationUserId == user.Id);
            
            var productsInCart = _dbContext.CartProducts
                .Include(x => x.Product)
                .Where(x => x.CartId == cart.Id).ToList();

            ViewBag.CartId = cart.Id;

            return View(productsInCart);
        }

        public async Task<IActionResult> AddProduct(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());
            var cart = _dbContext.Carts.SingleOrDefault(x => x.ApplicationUserId == user.Id);
            
            var product = _dbContext.Products.Find(Id);

            var productsInCart = _dbContext.CartProducts
                .Include(x => x.Product)
                .Where(x => x.CartId == cart.Id).ToList();

            var control = productsInCart.SingleOrDefault(x => x.ProductId == product.Id);
            if (control == null)
            {
                CartProducts newProduct = new CartProducts()
                {
                    CartId = cart.Id,
                    ProductId = product.Id,
                    Quantity = 1,
                    Price = product.Price
                };
                _dbContext.CartProducts.Add(newProduct);
            }
            else
            {
                control.Quantity++;
                control.Price = control.Quantity * product.Price;
            }

            _dbContext.SaveChanges();

            return RedirectToAction("MyCart");
        }

        public IActionResult FinishShopping(Guid Id)
        {
            var cart = _dbContext.Carts.Find(Id);

            var productInCarts = _dbContext.CartProducts
                .Include(x=> x.Product)
                .Where(x => x.CartId == cart.Id).ToList();
            foreach (var item in productInCarts)
            {
                Report report = new Report()
                {
                    ProductName = item.Product.ProductName,
                    ProductPrice = item.Product.Price,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    CartId = item.CartId
                };
                _dbContext.Reports.Add(report);
            }
            foreach (var item in productInCarts)
            {
                _dbContext.CartProducts.Remove(item);
            }
            _dbContext.SaveChanges();

            return RedirectToAction("ShoppingOk");
        }

        public IActionResult ShoppingOk()
        {
            return View();
        }
    }
}
