using AuthorizationService.Requests;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService.Controllers
{
    public class AuthController : ControllerBase
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
                return BadRequest(new
                {
                    Message = "Failed to create user.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors)
                });
            }

            var user = new IdentityUser(request.Username);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Message = "Failed to create user.",
                    Errors = result.Errors.Select(err => err.Description)
                });
            }

            await _signInManager.SignInAsync(user, false);
            return Redirect(request.RedirectUrl);
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
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();

            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);

            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }
    }
}
