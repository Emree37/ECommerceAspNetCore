using ECommerceAspNetCore.Data;
using ECommerceAspNetCore.Extensions;
using ECommerceAspNetCore.Models.Entities;
using ECommerceAspNetCore.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceAspNetCore.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MyContext _dbContext;

        public ReportController(MyContext dbContext,UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public async Task<IActionResult> PurchasedProducts()
        {
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());
            var cart = _dbContext.Carts.SingleOrDefault(x => x.ApplicationUserId == user.Id);

            var purchasedProducts = _dbContext.CartProducts
                .Include(x => x.Product)
                .Where(x => x.CartId == cart.Id).ToList();


            return View(purchasedProducts);
        }
    }
}
