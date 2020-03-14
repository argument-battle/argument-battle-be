using AuthorizationService.Requests;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthorizationService.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthController(
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager, 
            IIdentityServerInteractionService interactionService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interactionService = interactionService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var user = new IdentityUser(request.Username);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Redirect(request.RedirectUrl);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var loginResult = await _signInManager.PasswordSignInAsync(
                request.Username, 
                request.Password, 
                false, false);

            if (loginResult.Succeeded)
            {
                return Redirect(request.RedirectUrl);
            } 
            else if (loginResult.IsLockedOut)
            {

            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();

            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);

            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginRequest
            {
                RedirectUrl = returnUrl
            });
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterRequest 
            { 
                RedirectUrl = returnUrl 
            });
        }
    }
}
