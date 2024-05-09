using Autofac;
using DiscussionOverflow.Application.Features.Services;
using DiscussionOverflow.Application.Utilities;
using DiscussionOverflow.Domain.Entities;
using DiscussionOverflow.Infrastructure.Membership;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace DiscussionOverflow.Web.Models
{
    public class QuestionModel
    {
        private ILifetimeScope _scope;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IQuestionManagementService _questionManagementService;

    
        public string Title { get; set; }

        [Required]
        [StringLength(9000, ErrorMessage = "The Details input field should  contain minimum 20 characters ", MinimumLength = 20)]
        public string Details { get; set; }

        [Required]
        [StringLength(9000, ErrorMessage = "The Expected result field should  contain minimum 20 characters ", MinimumLength = 20)]
        public string CurrentStatus { get; set; }
       
        public string Tags { get; set; }
        public string? QuestionMaker { get; set; }
        public DateTime? TimeStamp { get; set; }

        public QuestionModel() { }

        public QuestionModel(UserManager<ApplicationUser> userManager,
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

        public bool VerifyTags(string tags)
        {
            // Split the string by commas and remove any leading or trailing spaces
            string[] tag = tags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // Convert array to list of strings
            List<string> tagList = new List<string>(tag);
            for(var i = 0; i < tagList.Count; i++)
            {
                var singleTag = tagList[i];
                if(singleTag == "" || singleTag == " " || singleTag == "  " || singleTag[0] == ' ')
                {
                    return false;
                }
            }

            int length = tagList.Count;
            if (length <= 5 && length > 0)
                return true;
            return false;
        }

        public async Task CreateQuestionAsync()
        {
            var email = _signInManager.Context.User.Identity?.Name;
            Guid id = Guid.NewGuid();
            var question = new Question
            {
                Id = id,
                Title = Title,
                Details = Details,
                CurrentStatus = CurrentStatus,
                Tags = Tags,
                QuestionMaker = email,
                TimeStamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };
            await _questionManagementService.CreateQuestionAsync(question);
            var voteObject = new Vote
            {
                QuestionId = id,
                QuestionMaker = question.QuestionMaker,
                UpVote = 0,
                DownVote = 0,
                Voter = "testUser",
                TimeStamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };
            await _questionManagementService.CreateVoteAsync(voteObject);

        }


    }
}
