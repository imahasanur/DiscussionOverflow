using DiscussionOverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscussionOverflow.Domain.Repositories
{
    public interface ICommentRepository:IRepositoryBase<Comment,Guid>
    {
        Task<IList<Comment>> GetAllAnswerCommentAsync(Guid id);
        Task<IList<Comment>> GetQuestionCommentAsync(Guid id);
        
    }
}
