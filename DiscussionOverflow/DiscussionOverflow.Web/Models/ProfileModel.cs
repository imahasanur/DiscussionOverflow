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
    public class ProfileModel
    {
        private ILifetimeScope _scope;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IUserManagementService _userManagementService;
        private IQuestionManagementService _questionManagementService;

        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string? Title { get; set; }
        public string? Intro { get; set; }
        public string? Location { get; set; }
        public string? PortfolioSite { get; set; }
        public string? ImageFileName { get; set; }
        public int? ImageFileSize { get; set; }
        public string? S3Url { get; set; }
        public DateTime? Time { get; set; }
        public int? Reputation { get; set; }

        //For handle the user question 
        public  IList<Question>? Questions { get; set; }

        public ProfileModel() { }

        public ProfileModel(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserManagementService userManagementService,
            IQuestionManagementService questionManagementService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userManagementService = userManagementService;
            _questionManagementService = questionManagementService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _userManager = _scope.Resolve<UserManager<ApplicationUser>>();
            _signInManager = _scope.Resolve<SignInManager<ApplicationUser>>();
            _userManagementService = _scope.Resolve<IUserManagementService>();
            _questionManagementService = _scope.Resolve<IQuestionManagementService>();
        }

        public async Task<ProfileModel> GetProfileUserByEmailAsync()
        {
            var email = _signInManager.Context.User.Identity?.Name;

            var result = await _userManagementService.GetProfileUserByEmailAsync(email);
            var deserializedUser = JsonSerializer.Deserialize<ApplicationUser>(result.GetRawText());
            var userInfo = new ProfileModel
            {
                FullName = deserializedUser.FullName,
                DisplayName = deserializedUser.DisplayName,
                Title = deserializedUser.Title,
                Intro = deserializedUser.Intro,
                Location = deserializedUser.Location,
                PortfolioSite = deserializedUser.PortfolioSite,
                ImageFileName = deserializedUser.ImageFileName,
                ImageFileSize = deserializedUser.ImageFileSize,
                S3Url = deserializedUser.S3Url,
                Time = deserializedUser.TimeStamp,
                Reputation = deserializedUser.Reputation,
                Questions = await _questionManagementService.GetUserQuestionsAsync(email)
            };

            return userInfo;
        }


    }
}
