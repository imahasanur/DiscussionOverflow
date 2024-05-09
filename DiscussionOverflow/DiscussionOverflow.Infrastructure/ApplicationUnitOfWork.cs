using DiscussionOverflow.Application;
using DiscussionOverflow.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscussionOverflow.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public IUserRepository UserRepository { get; set; }
        public IQuestionRepository QuestionRepository { get; set; }
        public IAnswerRepository AnswerRepository { get; set; }
        public ICommentRepository CommentRepository { get; set; }
        public IVoteRepository VoteRepository { get; set; }

        public INotificationRepository NotificationRepository { get; set; }

        public ApplicationUnitOfWork(IUserRepository userRepository,
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            ICommentRepository commentRepository,
            IVoteRepository voteRepository,
            INotificationRepository notificationRepository,
            IApplicationDbContext dbContext) : base((DbContext)dbContext)
        {
            UserRepository = userRepository;
            QuestionRepository = questionRepository;
            AnswerRepository = answerRepository;
            CommentRepository = commentRepository;
            VoteRepository = voteRepository;
            NotificationRepository = notificationRepository;
            _dbContext = (ApplicationDbContext)dbContext;
        }

        public async Task UpdateEntityAsync<TEntity>(TEntity entity) where TEntity : class
        {
            // Attach and mark the entity as modified
            _dbContext.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;

            // Save changes
            await _dbContext.SaveChangesAsync();
        }
    }
}
