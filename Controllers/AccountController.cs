using JWT_EF_Core.Models;
using JWT_EF_Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JWT_EF_Core.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
               
                var userResult = _userManager.FindByEmailAsync(registerVM.Email);
                if (userResult.Result != null) return BadRequest(); 

                var user = new AppUser()
                 {
                 Email = registerVM.Email,
                 UserName = registerVM.UserName
                 };

                var registerResult = await _userManager.CreateAsync(user,registerVM.Password);
                if (registerResult.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (roleResult.Succeeded) return View("Index");
                    return BadRequest();
                }
                else return BadRequest();
                }
            catch(Exception e)
            {
                return View( e);
            }
            
        }
    }
}
