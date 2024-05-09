using DiscussionOverflow.Domain;
using DiscussionOverflow.Domain.Entities;
using DiscussionOverflow.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DiscussionOverflow.Infrastructure.Repositories
{
    public class CommentRepository : Repository<Comment, Guid>, ICommentRepository
    {
        
        public CommentRepository(IApplicationDbContext context) : base((DbContext)context)
        {

        }

        public async Task<IList<Comment>> GetAllAnswerCommentAsync(Guid id)
        {
            Expression<Func<Comment, bool>> expression = x => x.AnswerId == id;

            return await GetAsync(expression, null);
        }

        public async Task<IList<Comment>> GetQuestionCommentAsync(Guid id)
        {
            Expression<Func<Comment, bool>> expression = x => x.QuestionId == id;

            return await GetAsync(expression, null);
        }



    }
}
