using Microsoft.AspNetCore.Mvc;
using WebSitem.Context;
using WebSitem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using WebSitem.Identity;

namespace WebSitem.Controllers
{

    public class AccountController : Controller
    {

        private readonly BlogDbContext _context;
        private readonly UserManager<BlogIdentityUser> _userManager;
        private readonly SignInManager<BlogIdentityUser> _signInManager;

        public AccountController(BlogDbContext context, UserManager<BlogIdentityUser> userManager,
            SignInManager<BlogIdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;


        }
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");


            }
            else
            {
                return View();
            }
        }
       
       




    }
}