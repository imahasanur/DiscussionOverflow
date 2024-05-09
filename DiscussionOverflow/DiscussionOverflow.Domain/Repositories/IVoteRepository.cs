using DiscussionOverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscussionOverflow.Domain.Repositories
{
    public interface IVoteRepository : IRepositoryBase<Vote,Guid>
    {
        Task<IList<Vote>> GetQuestionVoteAsync(Guid id);
        Task<IList<Vote>> GetAnswerVoteAsync(Guid id);
    }
}
