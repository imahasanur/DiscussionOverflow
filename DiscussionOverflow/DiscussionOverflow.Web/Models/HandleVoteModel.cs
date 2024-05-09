using Amazon.S3.Model;
using Autofac;
using DiscussionOverflow.Application.Features.Services;
using DiscussionOverflow.Domain.Entities;
using DiscussionOverflow.Infrastructure.Membership;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Text.Json;

namespace DiscussionOverflow.Web.Models
{
    public class HandleVoteModel
    {
        private ILifetimeScope _scope;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IQuestionManagementService _questionManagementService;
        private IUserManagementService _userManagementService;


        public HandleVoteModel() { }

        public HandleVoteModel(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IQuestionManagementService questionManagementService,
            IUserManagementService userManagementService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _questionManagementService = questionManagementService;
            _userManagementService = userManagementService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _userManager = _scope.Resolve<UserManager<ApplicationUser>>();
            _signInManager = _scope.Resolve<SignInManager<ApplicationUser>>();
            _questionManagementService = _scope.Resolve<IQuestionManagementService>();
            _userManagementService = _scope.Resolve<IUserManagementService>();
        }

        public bool IsRepeated(string voters, string newVoter)
        {
            string[] allVoter = voters.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // Convert array to list of strings
            List<string> voterList = new List<string>(allVoter);
            foreach(var voter in voterList)
            {
                if (voter == newVoter)
                    return true;
            }

            return false;
        }

        public async Task<bool> UpdateVoteCountAsync(Guid id, string type, string itemType)
        {
            string newVoter;
            bool res = true;
            string userEmail ="";
            if(itemType == "Question")
            {
                var qsVote = await _questionManagementService.GetQuestionVoteAsync(id);
                var voter = qsVote[qsVote.Count - 1].Voter;
                newVoter = _signInManager.Context.User.Identity?.Name;
                res = IsRepeated(voter, newVoter);
                var qsMaker = qsVote[qsVote.Count - 1].QuestionMaker;
                userEmail = qsMaker;
                if (qsMaker == newVoter)
                {
                    res = true;
                }
            }
            else
            {
                var ansVote = await _questionManagementService.GetAnswerVoteAsync(id);
                var voter = ansVote[ansVote.Count - 1].Voter;
                newVoter = _signInManager.Context.User.Identity?.Name;
                res = IsRepeated(voter, newVoter);
                var ansReplier = ansVote[ansVote.Count - 1].Replier;
                userEmail = ansReplier;
                if(ansReplier == newVoter)
                {
                    res = true;
                }
            }

            if (!res)
            {
                await _questionManagementService.UpdateVoteCountAsync(id, type, itemType, newVoter);
                if(userEmail != "")
                {
                    var user = await _userManagementService.GetUserByEmailAsync(userEmail);
                    var deserializedUser = JsonSerializer.Deserialize<ApplicationUser>(user.GetRawText());

                    if(type == "upvote")
                    {
                        deserializedUser.Reputation += 10;
                    }
                    else
                    {
                        deserializedUser.Reputation -= 2;
                        if(deserializedUser.Reputation < 1)
                        {
                            deserializedUser.Reputation = 1;
                        }
                    }

                    var userJson = JsonSerializer.Serialize(deserializedUser);

                    // Parse the JSON string into a JsonElement
                    var jsonElement = JsonDocument.Parse(userJson).RootElement;

                    var response = await _userManagementService.UpdateUserAsync(jsonElement);
                }
            }
            return !res;

        }
  
        
    }
}
