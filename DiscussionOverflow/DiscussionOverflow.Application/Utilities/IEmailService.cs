using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscussionOverflow.Application.Utilities
{
    public interface IEmailService
    {
         Task SendSingleEmailAsync(string receiverName, string receiverEmail, 
             string subject, string body);
    }
}
