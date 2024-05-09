using Autofac;
using DiscussionOverflow.Application.Features.Services;
using DiscussionOverflow.Domain.Entities;
using DiscussionOverflow.Infrastructure.Membership;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace DiscussionOverflow.Web.Models
{
    public class QuestionTagModel
    {
        private ILifetimeScope _scope;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IQuestionManagementService _questionManagementService;

        public string? TagInput { get; set; }
        public string? TitleInput { get; set; }
        public IList<Question>? SearchedTagQs { get; set; }
        public IList<Question>? SearchedTitleQs { get; set; }

        public QuestionTagModel() { }

        public QuestionTagModel(UserManager<ApplicationUser> userManager,
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
        
        public async Task GetSearchQuestionAsync()
        {
            if(TagInput != null)
            {
                SearchedTagQs =await  _questionManagementService.GetSearchedTagQuestion(TagInput);
            }
            if (TitleInput != null)
            {
                SearchedTitleQs = await _questionManagementService.GetSearchedTitleQuestion(TitleInput);
            }
        }

    }
}
