using Microsoft.AspNetCore.Identity;
using System;

namespace DiscussionOverflow.Infrastructure.Membership
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string? Title { get; set; }
        public string? Intro { get; set; }
        public string? Location { get; set; }
        public string? PortfolioSite { get; set; }
        public string? ImageFileName { get; set; }
        public int? ImageFileSize { get; set; }
        public string? S3Url { get; set; }
        public int Reputation { get; set; }
        public DateTime TimeStamp { get; set; }
        
    }
}
