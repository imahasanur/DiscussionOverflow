using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscussionOverflow.Domain.Entities
{
    public class Question : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } 
        public string Details { get; set; }
        public string CurrentStatus { get; set; }
        public string Tags { get; set; }
        public string QuestionMaker { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
