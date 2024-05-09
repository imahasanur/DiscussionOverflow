using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscussionOverflow.Domain.Entities
{
    public class Comment : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid? QuestionId { get; set; } 
        public Guid? AnswerId { get; set; }
        public string CommentBody { get; set; }
        public string? QuestionMaker { get; set; }
        public string? Replier { get; set; }
        public string Commentator { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
