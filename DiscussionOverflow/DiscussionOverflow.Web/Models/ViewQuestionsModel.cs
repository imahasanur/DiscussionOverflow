using Autofac;
using DiscussionOverflow.Application.Features.Services;
using DiscussionOverflow.Application.Utilities;
using DiscussionOverflow.Domain.Entities;
using DiscussionOverflow.Infrastructure;
using DiscussionOverflow.Infrastructure.Membership;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace DiscussionOverflow.Web.Models
{
    public class ViewQuestionsModel
    {
        private ILifetimeScope _scope;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IQuestionManagementService _questionManagementService;

        public Guid Id { get; set; }

        public ViewQuestionsModel() { }

        public ViewQuestionsModel(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IQuestionManagementService questionManagementService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _questionManagementService = questionManagementService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _userManager = _scope.Resolve<UserManager<ApplicationUser>>();
            _signInManager = _scope.Resolve<SignInManager<ApplicationUser>>();
            _questionManagementService = _scope.Resolve<IQuestionManagementService>();
        }

        public async Task<IList<Question>> GetAllQuestionAsync()
        {
            return await _questionManagementService.GetAllQuestionAsync();
        }

    }
}
