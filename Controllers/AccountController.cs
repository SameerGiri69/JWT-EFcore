using JWT_EF_Core.Interface;
using JWT_EF_Core.Models;
using JWT_EF_Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JWT_EF_Core.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpContextAccessor _httpContext;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService
            , SignInManager<AppUser> signInManager, IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _httpContext = httpContext;
        }
        [HttpGet]
        
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
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("User", "error login credentials");
                return View();
            }
            var myUser = await _userManager.FindByEmailAsync(loginVM.Email);
            
            var signInResult = await _signInManager.PasswordSignInAsync(myUser.UserName,loginVM.Password,true, false);
            if (signInResult.Succeeded)
            {

                var jwttoken = _tokenService.CreateToken(myUser);
                return RedirectToAction("Index", "Account", new { token = jwttoken });
            }
            ModelState.AddModelError("SignIn", "Error signing in");
            return View();
        }
        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult DeleteUser()
        {
            return View();
        }
        
        public async Task<IActionResult> Delete(DeleteUserViewModel deleteVM)
        {
            var userh = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

            var user = await _userManager.FindByEmailAsync(userh);
            var deleteUser = await _userManager.DeleteAsync(user);

            if (!deleteUser.Succeeded) return Unauthorized("Could not delete user please contact admin");
            else
                return RedirectToAction("Register", "Account");
            
        }
    }
}
