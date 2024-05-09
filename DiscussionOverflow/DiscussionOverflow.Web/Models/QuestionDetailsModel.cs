using Autofac;
using DiscussionOverflow.Application.Features.Services;
using DiscussionOverflow.Domain.Entities;
using DiscussionOverflow.Infrastructure.Membership;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace DiscussionOverflow.Web.Models
{
    public class QuestionDetailsModel
    {
        private ILifetimeScope _scope;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IQuestionManagementService _questionManagementService;

        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? Details { get; set; }
        public string? CurrentStatus { get; set; }
        public string? Tags { get; set; }
        public string? QuestionMaker { get; set; }
        public DateTime? TimeStamp { get; set; }
        public IList<Answer>? Answers { get; set; }
        public IList<IList<Comment>>? Comments { get; set; }
        public IList<IList<Vote>>? Votes { get; set; }
        //Handling comment of Qs and Ans
        public Guid? QuestionId { get; set; }
        public Guid? AnswerId { get; set; }
        public string? CommentBody { get; set; }
        public string? QuestionMakerOfQs { get; set; }
        public string? ReplierOfQs { get; set; }
        public string? Commentator { get; set; }

        //Handling Answer
        public Guid? RepliedQuestionId { get; set; }
        public string? AnswerBody { get; set; }
        public string? RepliedQuestionMaker { get; set; }
        public string? Replier { get; set; }

        public Guid? GetId { get; set; }


        public QuestionDetailsModel() { }

        public QuestionDetailsModel(UserManager<ApplicationUser> userManager,
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
        public async Task<QuestionDetailsModel> GetQuestionDeatails(Guid id)
        {
            var question =await  _questionManagementService.GetQuestionAsync(id);
            var answer = await _questionManagementService.GetAllAnswerAsync(question.Id);

            List<IList<Comment>> comment = new List<IList<Comment>>();
            List<IList<Vote>> vote = new List<IList<Vote>>();

            var qsVote = await _questionManagementService.GetQuestionVoteAsync(question.Id);
            if(qsVote != null)
                vote.Add(qsVote);

            foreach (var ans in answer) {
                IList<Comment> comments = await _questionManagementService.GetAllAnswerCommentAsync(ans.Id);
                var ansVote = await _questionManagementService.GetAnswerVoteAsync(ans.Id);
                if (comments != null)
                {
                    comment.Add(comments);
                }
                if (ansVote != null)
                    vote.Add(ansVote);
            }
            var qsComment = await _questionManagementService.GetQuestionCommentAsync(id);
            if (qsComment != null)
                comment.Add(qsComment);

            var questionDetailsModel = new QuestionDetailsModel
            {
                Id = question.Id,
                Title = question.Title,
                Details = question.Details,
                CurrentStatus = question.CurrentStatus,
                Tags = question.Tags,
                QuestionMaker = question.QuestionMaker,
                TimeStamp = question.TimeStamp,
                Answers = answer,
                Comments = comment,
                Votes = vote
            };
            return questionDetailsModel;
        }

        public async Task PostResponseAsync()
        {
            if (QuestionId != null)
            {
                var commentObj = new Comment
                {
                    QuestionId = QuestionId,
                    CommentBody = CommentBody,
                    QuestionMaker = QuestionMakerOfQs,
                    Commentator = Commentator,
                    TimeStamp = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day)
                };
                await _questionManagementService.PostQuestionCommentAsync(commentObj);

                var question = await _questionManagementService.GetQuestionAsync((Guid)QuestionId);
                
                //Handling notification for Question Comment
                var questionCommentNotification = new Notification
                {
                    QuestionId =(Guid) QuestionId,
                    QuestionMaker = question.QuestionMaker,
                    QuestionTitle = question.Title,
                    TimeStamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                    Commentator = commentObj.Commentator

                };
                await _questionManagementService.PostNotificationAsync(questionCommentNotification);
            }
            if (AnswerId != null)
            {
                var commentObj = new Comment
                {
                    AnswerId = AnswerId,
                    CommentBody = CommentBody,
                    Replier = ReplierOfQs,
                    Commentator = Commentator,
                    TimeStamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
                };
                await _questionManagementService.PostAnswerCommentAsync(commentObj);

            }
            if (RepliedQuestionId != null)
            {
                Guid id = Guid.NewGuid();
                var answerObj = new Answer
                {
                    Id = id,
                    QuestionId = RepliedQuestionId.HasValue? RepliedQuestionId.Value:Guid.Empty,
                    QuestionMaker = RepliedQuestionMaker,
                    Replier = Replier,
                    AnswerBody = AnswerBody,
                    TimeStamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
                };
                await _questionManagementService.PostAnswerAsync(answerObj);

                var voteObj = new Vote
                {
                    AnswerId = id,
                    Replier = answerObj.Replier,
                    UpVote = 0,
                    DownVote = 0,
                    Voter = "testUser",
                    TimeStamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
                };
                await _questionManagementService.CreateVoteAsync(voteObj);


                //Handling notification for Reply
                var question = await _questionManagementService.GetQuestionAsync((Guid)RepliedQuestionId);
                var questionReplyNotification = new Notification
                {
                    QuestionId = (Guid)RepliedQuestionId,
                    QuestionMaker = question.QuestionMaker,
                    QuestionTitle = question.Title,
                    TimeStamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                    Replier = answerObj.Replier

                };
                await _questionManagementService.PostNotificationAsync(questionReplyNotification);

            }
        }
    }
}
