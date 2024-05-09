using DiscussionOverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscussionOverflow.Domain.Repositories
{
    public interface IAnswerRepository : IRepositoryBase<Answer,Guid>
    {
        Task<IList<Answer>> GetAllAnswerAsync(Guid id);
    }
}
