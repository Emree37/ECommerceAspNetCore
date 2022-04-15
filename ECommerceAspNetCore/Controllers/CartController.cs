using ECommerceAspNetCore.Data;
using ECommerceAspNetCore.Extensions;
using ECommerceAspNetCore.Models.Entities;
using ECommerceAspNetCore.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceAspNetCore.Controllers
{
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
            if(cart == null)
            {
                cart = new Cart();
                cart.ApplicationUserId = user.Id;
            }

            var productsInCart = _dbContext.CartProducts
                .Include(x => x.Product)
                .Where(x => x.CartId == cart.Id && x.IsSold == false).ToList();

            return View(productsInCart);
        }

        public async Task<IActionResult> AddProduct(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());
            var cart = _dbContext.Carts.SingleOrDefault(x => x.ApplicationUserId == user.Id);
            if (cart == null)
            {
                cart = new Cart();
                cart.ApplicationUserId = user.Id;
                _dbContext.Carts.Add(cart);
            }
            var product = _dbContext.Products.Find(Id);

            var productsInCart = _dbContext.CartProducts
                .Include(x => x.Product)
                .Where(x => x.CartId == cart.Id && x.IsSold == false).ToList();

            var control = productsInCart.SingleOrDefault(x => x.ProductId == product.Id);
            if (control == null)
            {
                //Ürün yok
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
            if(cart == null)
            {
                return BadRequest();
            }

            var productInCarts = _dbContext.CartProducts.Where(x => x.CartId == cart.Id).ToList();
            foreach (var item in productInCarts)
            {
                item.IsSold = true;
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
