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
    public class QuestionRepository : Repository<Question, Guid>, IQuestionRepository
    {

        public QuestionRepository(IApplicationDbContext context) : base((DbContext)context)
        {

        }

        public async Task<IList<Question>> GetUserQuestionsAsync(string email)
        {
            Expression<Func<Question, bool>> expression = x => x.QuestionMaker == email;

            return await GetAsync(expression, null);
        }

        public async Task<IList<Question>> GetSearchedTagQuestion(string TagInput)
        {
            Expression<Func<Question, bool>> expression = x => x.Tags.Contains(TagInput);

            return await GetAsync(expression, null);
        }

        public async Task<IList<Question>> GetSearchedTitleQuestion(string TitleInput)
        {
            Expression<Func<Question, bool>> expression = x => x.Title.Contains(TitleInput);

            return await GetAsync(expression, null);
        }

    }
}
