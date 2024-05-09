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
    public class NotificationRepository : Repository<Notification, Guid>, INotificationRepository
    {
        
        public NotificationRepository(IApplicationDbContext context) : base((DbContext)context)
        {

        }

        public async Task<IList<Notification>> GetAllNotificationAsync(string email)
        {
            Expression<Func<Notification, bool>> expression = x => x.QuestionMaker == email;

            return await GetAsync(expression, null);
        }


    }
}
