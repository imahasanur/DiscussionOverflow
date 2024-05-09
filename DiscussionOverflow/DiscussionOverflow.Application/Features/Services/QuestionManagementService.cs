using DiscussionOverflow.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscussionOverflow.Application.Features.Services
{
    public class QuestionManagementService:IQuestionManagementService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public QuestionManagementService(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateQuestionAsync(Question question)
        {
            await _unitOfWork.QuestionRepository.AddAsync(question);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IList<Question>> GetAllQuestionAsync()
        {
            return await _unitOfWork.QuestionRepository.GetAllAsync();
        }

        public async Task<Question> GetQuestionAsync(Guid id)
        {
            return await _unitOfWork.QuestionRepository.GetByIdAsync(id);
        }

        public async Task<IList<Answer>> GetAllAnswerAsync(Guid id)
        {
            
            return await _unitOfWork.AnswerRepository.GetAllAnswerAsync(id);
        }

        public async Task<IList<Comment>> GetAllAnswerCommentAsync(Guid id)
        {

            return await _unitOfWork.CommentRepository.GetAllAnswerCommentAsync(id);
        }

        public async Task<IList<Comment>> GetQuestionCommentAsync(Guid id)
        {

            return await _unitOfWork.CommentRepository.GetQuestionCommentAsync(id);
        }

        public async Task<IList<Vote>> GetQuestionVoteAsync(Guid id)
        {
            return await _unitOfWork.VoteRepository.GetQuestionVoteAsync(id);
        }

        public async Task<IList<Vote>> GetAnswerVoteAsync(Guid id)
        {
            return await _unitOfWork.VoteRepository.GetAnswerVoteAsync(id);
        }

        public async Task<IList<Question>> GetUserQuestionsAsync(string email)
        {
            return await _unitOfWork.QuestionRepository.GetUserQuestionsAsync(email);
        }

        public async Task PostQuestionCommentAsync(Comment commentObj)
        {
            await _unitOfWork.CommentRepository.AddAsync(commentObj);
            await _unitOfWork.SaveAsync();
        }

        public async Task PostAnswerCommentAsync(Comment commentObj)
        {
            await _unitOfWork.CommentRepository.AddAsync(commentObj);
            await _unitOfWork.SaveAsync();
        }

        public async Task PostAnswerAsync(Answer answerObj)
        {
            await _unitOfWork.AnswerRepository.AddAsync(answerObj);
            await _unitOfWork.SaveAsync();
        }


        public async Task CreateVoteAsync(Vote voteObj)
        {
            await _unitOfWork.VoteRepository.AddAsync(voteObj);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IList<Question>> GetSearchedTagQuestion(string TagInput)
        {
            return await _unitOfWork.QuestionRepository.GetSearchedTagQuestion(TagInput);
        }

        public async Task<IList<Question>> GetSearchedTitleQuestion(string TitleInput)
        {
            return await _unitOfWork.QuestionRepository.GetSearchedTitleQuestion(TitleInput);
        }

        public async Task UpdateVoteCountAsync(Guid id, string type, string itemType, string newVoter)
        {
            //id -> this Question/Answer id already should be on database
            //newVoter -> this user is not a voter of this own Question/Answer
            if (itemType == "Question")
            {
                
                var qsVotes = await _unitOfWork.VoteRepository.GetQuestionVoteAsync(id);
                var qsVote = await _unitOfWork.VoteRepository.GetByIdAsync(qsVotes[qsVotes.Count - 1].Id);
                if(type == "upvote")
                {
                    qsVote.UpVote += 1; 
                }
                else
                {
                    qsVote.DownVote -= 1;
                }
                qsVote.Voter = $"{qsVote.Voter},{newVoter}";

                await _unitOfWork.UpdateEntityAsync(qsVote);
            }
            else
            {
                var ansVotes = await _unitOfWork.VoteRepository.GetAnswerVoteAsync(id);
                var ansVote = await _unitOfWork.VoteRepository.GetByIdAsync(ansVotes[ansVotes.Count - 1].Id);
                if (type == "upvote")
                {
                    ansVote.UpVote += 1;
                }
                else
                {
                    ansVote.DownVote -= 1;
                }
                ansVote.Voter = $"{ansVote.Voter},{newVoter}";
                await _unitOfWork.UpdateEntityAsync(ansVote);
            }


        }

        public async Task PostNotificationAsync(Notification questionCommentNotification)
        {
            await _unitOfWork.NotificationRepository.AddAsync(questionCommentNotification);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IList<Notification>> GetAllNotificationAsync(string email)
        {
            
            return await _unitOfWork.NotificationRepository.GetAllNotificationAsync(email);

        }

    }
}
