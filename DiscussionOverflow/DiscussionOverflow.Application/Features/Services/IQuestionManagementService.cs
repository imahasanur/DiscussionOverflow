using DiscussionOverflow.Domain.Entities;

namespace DiscussionOverflow.Application.Features.Services
{
    public interface IQuestionManagementService
    {
        Task CreateQuestionAsync(Question question);
        Task<IList<Question>> GetAllQuestionAsync();
        Task<Question> GetQuestionAsync(Guid id);
        Task<IList<Answer>> GetAllAnswerAsync(Guid id);
        Task<IList<Comment>> GetAllAnswerCommentAsync(Guid id);
        Task<IList<Comment>> GetQuestionCommentAsync(Guid id);
        Task<IList<Vote>> GetQuestionVoteAsync(Guid id);
        Task<IList<Vote>> GetAnswerVoteAsync(Guid id);
        Task PostQuestionCommentAsync(Comment commentObj);
        Task PostAnswerCommentAsync(Comment commentObj);
        Task PostAnswerAsync(Answer answerObj);
        Task CreateVoteAsync(Vote voteObj);
        Task UpdateVoteCountAsync(Guid id, string type, string itemType, string newVoter);
        Task<IList<Question>> GetUserQuestionsAsync(string email);
        Task<IList<Question>> GetSearchedTagQuestion(string TagInput);
        Task<IList<Question>> GetSearchedTitleQuestion(string TitleInput);
        Task PostNotificationAsync(Notification questionCommentNotification);
        Task<IList<Notification>> GetAllNotificationAsync(string email);
    }
}