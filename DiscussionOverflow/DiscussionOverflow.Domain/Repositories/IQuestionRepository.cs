using DiscussionOverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscussionOverflow.Domain.Repositories
{
    public interface IQuestionRepository : IRepositoryBase<Question, Guid>
    {
        Task<IList<Question>> GetUserQuestionsAsync(string email);
        Task<IList<Question>> GetSearchedTagQuestion(string TagInput);
        Task<IList<Question>> GetSearchedTitleQuestion(string TitleInput);



    }
}
