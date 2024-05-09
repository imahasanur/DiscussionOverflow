using DiscussionOverflow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscussionOverflow.Infrastructure
{
    public interface IApplicationDbContext
    {
        DbSet<Question> Question { get; set; }
        DbSet<Answer> Answer { get; set; }
        DbSet<Comment> Comment { get; set; }
        DbSet<Vote> Vote { get; set; }
        DbSet<Notification> Notification { get; set; }
    }
}
