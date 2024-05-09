using Autofac;
using DiscussionOverflow.Infrastructure.Membership;
using DiscussionOverflow.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionOverflow.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AccountController(ILifetimeScope scope,
           ILogger<AccountController> logger,
           SignInManager<ApplicationUser> signInManager,
           UserManager<ApplicationUser> userManager,
           IConfiguration configuration,
           ITokenService tokenService)
        {
            _scope = scope;
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _tokenService = tokenService;
        }
        public IActionResult Register()
        {
            var model = _scope.Resolve<RegistrationModel>();
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            string captchaInput = model.CaptchaInput;
            string expectedCaptcha = HttpContext.Session.GetString("CaptchaCode");

            if (ModelState.IsValid && string.Equals(expectedCaptcha, captchaInput, StringComparison.OrdinalIgnoreCase))
            {
                
                model.Resolve(_scope);
                var response = await model.RegisterAsync(Url.Content("~/"));

                if (response.errors is not null)
                {
                    foreach (var error in response.errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        _logger.LogError(error.Description);
                    }
                }
                else
                    return Redirect(response.redirectLocation);
            }

            return View(model);
        }



        [HttpGet]
        public IActionResult RegisterConfirmation(string email, string returnUrl)
        {
            ViewBag.Email = email;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                _logger.LogWarning("Invalid user ID or token, handle accordingly");
                return RedirectToAction("Error", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found.");
                return RedirectToAction("Error", "Home");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                // Email confirmed successfully
                return View("EmailConfirmed");
            }
            else
            {
                _logger.LogWarning("Confirmation failed, handle accordingly");
                return RedirectToAction("Error", "Home", new {errorMessage = "There is an Error in Account Confirmation"});
            }
        }



        public async Task<IActionResult> Login(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            var model = _scope.Resolve<LoginModel>();

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            model.ReturnUrl = returnUrl;

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");
            string captchaInput = model.CaptchaInput;
            string expectedCaptcha = HttpContext.Session.GetString("CaptchaCode");

            if (ModelState.IsValid && string.Equals(expectedCaptcha, captchaInput, StringComparison.OrdinalIgnoreCase))
            {
                
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var claims = (await _userManager.GetClaimsAsync(user)).ToArray();
                    var token = await _tokenService.GetJwtToken(claims,
                            _configuration["Jwt:Key"],
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"]
                        );
                    HttpContext.Session.SetString("token", token);

                    return RedirectToAction("Questions", "User");
                }
                else
                {
                    _logger.LogWarning("Invalid login attempt.");
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            // If we got this far, something failed, redisplay form
            _logger.LogInformation("Model State is not valid or Captcha is not matched");
            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


    }
}
