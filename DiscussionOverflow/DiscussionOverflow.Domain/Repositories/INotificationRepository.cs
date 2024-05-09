using DiscussionOverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscussionOverflow.Domain.Repositories
{
    public interface INotificationRepository : IRepositoryBase<Notification,Guid>
    {
        Task<IList<Notification>> GetAllNotificationAsync(string email);
    }
}
