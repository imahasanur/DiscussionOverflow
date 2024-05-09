using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscussionOverflow.Domain.Entities
{
    public class Answer : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string AnswerBody { get; set; }
        public string QuestionMaker { get; set; }
        public string Replier { get; set; }
        public DateTime TimeStamp { get; set; }
        
    }
}
