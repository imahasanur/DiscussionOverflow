using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscussionOverflow.Domain.Entities
{
    public class Vote : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid? QuestionId { get; set; }
        public Guid? AnswerId { get; set; }
        public string? QuestionMaker { get; set; }
        public string? Replier { get; set; }
        public int? UpVote { get; set; }
        public int? DownVote { get; set; }
        public string Voter { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
