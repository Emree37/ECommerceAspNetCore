using ECommerceAspNetCore.Data;
using ECommerceAspNetCore.Models.Entities;
using ECommerceAspNetCore.Models.Identity;
using ECommerceAspNetCore.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceAspNetCore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly MyContext _dbContext;

        public AccountController(
             UserManager<ApplicationUser> userManager,
             SignInManager<ApplicationUser> signInManager,
             RoleManager<ApplicationRole> roleManager,
             MyContext dbContext
             )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Password = string.Empty;
                model.ConfirmPassword = string.Empty;
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                ModelState.AddModelError(nameof(model.UserName), "Bu kullanıcı adı daha önce sisteme kayıt edilmiştir");
                model.UserName = string.Empty;
                model.Password = string.Empty;
                model.ConfirmPassword = string.Empty;
                return View(model);
            }
            user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                ModelState.AddModelError(nameof(model.Email), "Bu email daha önce sisteme kayıt edilmiştir");
                model.Email = string.Empty;
                model.Password = string.Empty;
                model.ConfirmPassword = string.Empty;
                return View(model);
            }
            user = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                Cart cart = new Cart()
                {
                    ApplicationUserId = user.Id
                };
                _dbContext.Carts.Add(cart);
                _dbContext.SaveChanges();

                return RedirectToAction("Login", "Account");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kayıt işleminde bir hata oluştu");
                model.Password = string.Empty;
                model.ConfirmPassword = string.Empty;
                return View(model);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Password = string.Empty;
                return View(model);
            }

            var result =
                await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);

            if (result.Succeeded)
            {
                return RedirectToAction("GetProducts", "Product");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı");
                return View(model);
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
