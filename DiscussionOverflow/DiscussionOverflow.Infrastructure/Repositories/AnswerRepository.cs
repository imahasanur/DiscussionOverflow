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
    public class AnswerRepository : Repository<Answer, Guid>, IAnswerRepository
    {
        
        public AnswerRepository(IApplicationDbContext context) : base((DbContext)context)
        {

        }

        public async Task<IList<Answer>> GetAllAnswerAsync(Guid id)
        {
            Expression<Func<Answer, bool>> expression = x => x.QuestionId == id;

            return await GetAsync(expression,null);
        }

        //public async Task<Guid> GetAnswerByQuestionIdAsync(Guid id)
        //{
        //    Expression<Func<Answer, bool>> expression = x => x.QuestionId == id;
        //}

    }
}
