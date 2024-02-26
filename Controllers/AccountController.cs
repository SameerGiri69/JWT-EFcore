using JWT_EF_Core.Interface;
using JWT_EF_Core.Models;
using JWT_EF_Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JWT_EF_Core.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService
            , SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }
        [HttpGet]
        [ValidateAntiForgeryToken]
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
                    if (roleResult.Succeeded)
                    {
                        
                        return RedirectToAction("Index","Account");
                    }
                    return BadRequest();
                }
                else return BadRequest();
                }
            catch(Exception e)
            {
                return View( e);
            }
            
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = await _userManager.FindByEmailAsync(loginVM.Email);
            if(user == null)
            {
                return NotFound("User not registered");
            }
            else
            {
                var appuser = new AppUser()
                {
                    Email = loginVM.Email,
                    
                };
                var signInResult = _signInManager.PasswordSignInAsync(user.UserName, loginVM.Password, false,false);

                if (signInResult.Result.Succeeded)
                {
                    var jwttoken = _tokenService.CreateToken(user);
                    return RedirectToAction("Index", "Account", new { token = jwttoken });
                }
                else
                    return Unauthorized("Invalid email or password");

                
                
            }
        }


    }
}
