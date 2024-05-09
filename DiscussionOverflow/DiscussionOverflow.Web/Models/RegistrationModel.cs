using Autofac;
using DiscussionOverflow.Infrastructure.Membership;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using DiscussionOverflow.Application.Utilities;
using System.Web;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace DiscussionOverflow.Web.Models
{
    public class RegistrationModel
    {
        private ILifetimeScope _scope;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailService _emailService;
        private LinkGenerator _linkGenerator;
        private IHttpContextAccessor _httpContextAccessor;

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name ="Your Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage ="The {0} must be at least {2} and at max {1} characters long for pass.",MinimumLength =6)]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        [Compare("Password",ErrorMessage ="Type properly! It's a mismatch with password")]
        public string ConfirmPassword { get; set; }

        public string? ReturnUrl { get; set; }

        [Required]
        public string CaptchaInput { get; set; }


        public RegistrationModel() { }

        public RegistrationModel(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _userManager = _scope.Resolve<UserManager<ApplicationUser>>();
            _signInManager = _scope.Resolve<SignInManager<ApplicationUser>>();
            _emailService = _scope.Resolve<IEmailService>();
            _linkGenerator = _scope.Resolve<LinkGenerator>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();

        }

        internal async Task<(IEnumerable<IdentityError>? errors, string? redirectLocation)> RegisterAsync(string urlPrefix)
        {
            ReturnUrl ??= urlPrefix;

            var user = new ApplicationUser { UserName = Email, Email = Email, FirstName = FirstName,
                LastName = LastName, FullName = $"{FirstName} {LastName}",
                DisplayName= $"{FirstName} {LastName}", Reputation=1, TimeStamp =new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };
            var result = await _userManager.CreateAsync(user, Password);
            
            if (result.Succeeded)
            {

				await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("CreateQuestion", "true"));
				await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("ProfileEdit", "true"));
				await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("ProfileView", "true"));
				await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("VoteHandle", "true"));
				await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("CommentHandle", "true"));

				var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = HttpUtility.UrlEncode(code);

                var callbackUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/Account/ConfirmEmail?userId={user.Id}&code={code}";

                await _emailService.SendSingleEmailAsync(FirstName + " " + LastName, Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                _userManager.Options.SignIn.RequireConfirmedAccount = true;
                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    var confirmationPageLink = $"RegisterConfirmation?email={Email}&returnUrl={ReturnUrl}";
                    return (null, confirmationPageLink);
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return (null, ReturnUrl);
                }
            }
            else
            {
                return (result.Errors, null);
            }
        }


    }

}
