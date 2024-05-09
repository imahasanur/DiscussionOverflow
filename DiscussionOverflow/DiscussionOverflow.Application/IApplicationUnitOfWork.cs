using DiscussionOverflow.Domain;
using DiscussionOverflow.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscussionOverflow.Application
{
    public interface IApplicationUnitOfWork:IUnitOfWork
    {
        public IUserRepository UserRepository { get; set; } 
        public IQuestionRepository QuestionRepository { get; set; }
        public IAnswerRepository AnswerRepository { get; set; }
        public ICommentRepository CommentRepository { get; set; }
        public IVoteRepository VoteRepository { get; set; }
        public INotificationRepository NotificationRepository { get; set; }
        Task UpdateEntityAsync<TEntity>(TEntity entity) where TEntity : class;
    }
}
