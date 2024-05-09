using Autofac;
using DiscussionOverflow.Application.Features.Services;
using DiscussionOverflow.Application.Utilities;
using DiscussionOverflow.Domain.Entities;
using DiscussionOverflow.Infrastructure.Membership;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using System.Text.Json;

namespace DiscussionOverflow.Web.Models
{
    public class EditProfileModel
    {
        private ILifetimeScope _scope;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IUserManagementService _userManagementService;

        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string? Title { get; set; }
        public string? Intro { get; set; }
        public string? Location { get; set; }
        public string? PortfolioSite { get; set; }
        public string? ImageFileName { get; set; }
        public int? ImageFileSize { get; set; }
        public string? S3Url { get; set; }
        public IFormFile? ImageFile { get; set; }

        public EditProfileModel() { }

        public EditProfileModel(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserManagementService userManagementService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userManagementService = userManagementService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _userManager = _scope.Resolve<UserManager<ApplicationUser>>();
            _signInManager = _scope.Resolve<SignInManager<ApplicationUser>>();
            _userManagementService = _scope.Resolve<IUserManagementService>();
        }

        public async Task<EditProfileModel> GetUserByEmailAsync()
        {
            var email = _signInManager.Context.User.Identity?.Name;
            var result = await _userManagementService.GetUserByEmailAsync(email);
            var deserializedUser = JsonSerializer.Deserialize<ApplicationUser>(result.GetRawText());
            var userInfo = new EditProfileModel
            {
                FullName = deserializedUser.FullName,
                DisplayName = deserializedUser.DisplayName,
                Title = deserializedUser.Title,
                Intro = deserializedUser.Intro,
                Location = deserializedUser.Location,
                PortfolioSite = deserializedUser.PortfolioSite,
                ImageFileName = deserializedUser.ImageFileName,
                ImageFileSize = deserializedUser.ImageFileSize,
                S3Url = deserializedUser.S3Url
            };
            return userInfo;
        }

        public async Task<IdentityResult> UpdateUserAsync()
        {
            var email = _signInManager.Context.User.Identity?.Name;
            var user = await _userManagementService.GetUserByEmailAsync(email);
            var deserializedUser = JsonSerializer.Deserialize<ApplicationUser>(user.GetRawText());

            deserializedUser.FullName = FullName;
            deserializedUser.DisplayName = DisplayName;
            deserializedUser.Title = Title;
            deserializedUser.Intro = Intro;
            deserializedUser.Location = Location;
            deserializedUser.PortfolioSite = PortfolioSite;
            deserializedUser.ImageFileName = ImageFileName;
            deserializedUser.ImageFileSize = ImageFileSize;
            deserializedUser.S3Url = S3Url;

            var userJson = JsonSerializer.Serialize(deserializedUser);

            // Parse the JSON string into a JsonElement
            var jsonElement = JsonDocument.Parse(userJson).RootElement;

            var response = await _userManagementService.UpdateUserAsync(jsonElement);
            return response;
        }

    }
}
