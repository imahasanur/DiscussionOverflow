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
    public class VoteRepository : Repository<Vote, Guid>, IVoteRepository
    {
        
        public VoteRepository(IApplicationDbContext context) : base((DbContext)context)
        {

        }

        public async Task<IList<Vote>> GetQuestionVoteAsync(Guid id)
        {
            Expression<Func<Vote, bool>> expression = x => x.QuestionId == id;

            return await GetAsync(expression,null);
        }
        public async Task<IList<Vote>> GetAnswerVoteAsync(Guid id)
        {
            Expression<Func<Vote, bool>> expression = x => x.AnswerId == id;

            return await GetAsync(expression, null);
        }

    }
}
