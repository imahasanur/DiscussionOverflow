using DiscussionOverflow.Domain.Entities;
using DiscussionOverflow.Infrastructure.Membership;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscussionOverflow.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,
    ApplicationRole, Guid,
    ApplicationUserClaim, ApplicationUserRole,
    ApplicationUserLogin, ApplicationRoleClaim,
    ApplicationUserToken>,
    IApplicationDbContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;
        public ApplicationDbContext(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString,
                    x => x.MigrationsAssembly(_migrationAssembly));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Answer>()
                .HasOne<Question>()
                .WithMany()
                .HasForeignKey(x => x.QuestionId);

            builder.Entity<Comment>()
                .HasOne<Question>()
                .WithMany()
                .HasForeignKey(x => x.QuestionId);

            builder.Entity<Comment>()
                .HasOne<Answer>()
                .WithMany()
                .HasForeignKey(x => x.AnswerId);

            builder.Entity<Vote>()
                .HasOne<Question>()
                .WithMany()
                .HasForeignKey(x => x.QuestionId);

            builder.Entity<Vote>()
                .HasOne<Answer>()
                .WithMany()
                .HasForeignKey(x => x.AnswerId);

            builder.Entity<Notification>()
                .HasOne<Question>()
                .WithMany()
                .HasForeignKey(x => x.QuestionId);

            builder.Entity<Question>().HasData(new Question[]
            {
                new Question{ Id = new Guid("003805c3-938c-43b7-a768-03d6c0242ece"),
                    Title = "This is the test Question 1",
                    Details = "lorem impsum de color sutracun lorem impsum de color lorem ipsum de color",
                    CurrentStatus = "lorem impsum de color sutracun lorem impsum de color lorem ipsum de color",
                    Tags = "C,F,D",
                    QuestionMaker = "skill1@gmail.com",
                    TimeStamp=new DateTime(2024, 4, 12, 17, 36, 39)},
                new Question{ Id = new Guid("E580CF4D-FA7A-421D-8D0F-AB406D0A2E23"),
                    Title = "This is the test Question 2",
                    Details = "lorem impsum de color sutracun lorem impsum de color lorem ipsum de color",
                    CurrentStatus = "lorem impsum de color sutracun lorem impsum de color lorem ipsum de color",
                    Tags = "C,F,D,E",
                    QuestionMaker = "skill1@gmail.com",
                    TimeStamp=new DateTime(2024, 4, 12, 17, 36, 39)}
            });


            builder.Entity<Answer>().HasData(new Answer[]
            {
                new Answer{ Id = new Guid("963C81F0-4BF3-4C91-961D-945C2D8872F8"),
                    QuestionId = new Guid("003805c3-938c-43b7-a768-03d6c0242ece"),
                    AnswerBody = "lorem impsumlorem impsum lorem impsum lorem impsum",
                    QuestionMaker = "skill1@gmail.com",
                    Replier = "skill1@gmail.com",
                    TimeStamp=new DateTime(2024, 4, 12, 17, 36, 39)},

                new Answer{ Id = new Guid("68503176-0314-4937-92E4-400A6F4F4472"),
                    QuestionId = new Guid("003805c3-938c-43b7-a768-03d6c0242ece"),
                    AnswerBody = "lorem impsumlorem impsum lorem impsum lorem impsum",
                    QuestionMaker = "skill1@gmail.com",
                    Replier = "skill2@gmail.com",
                    TimeStamp=new DateTime(2024, 4, 12, 17, 36, 39)}
            });


            builder.Entity<Comment>().HasData(new Comment[]
            {
                new Comment{ Id = new Guid("69A75666-C25D-4736-92F6-66BBBC08D926"),
                    QuestionId = new  Guid("003805c3-938c-43b7-a768-03d6c0242ece"),
                    CommentBody = "lorem impsumlorem impsum lorem impsum lorem impsum",
                    QuestionMaker = "skill1@gmail.com",
                    Commentator = "skill2@gmail.com",
                    TimeStamp=new DateTime(2024, 4, 12, 17, 36, 39)},

                new Comment{ Id = new Guid("CD19778A-FB86-4AC9-96A4-CD4C5B423018"),
                    AnswerId = new Guid("963C81F0-4BF3-4C91-961D-945C2D8872F8"),
                    CommentBody = "lorem impsum  lorem impsum lorem impsum lorem impsum ,,,,,,,,",
                    Replier = "skill1@gmail.com",
                    Commentator = "skill2@gmail.com",
                    TimeStamp=new DateTime(2024, 4, 12, 17, 36, 39)}
            });


            builder.Entity<Vote>().HasData(new Vote[]
{
                new Vote{ Id = new Guid("A58E02EF-A025-492C-9746-1DB9D2067D36"),
                    QuestionId = new  Guid("003805c3-938c-43b7-a768-03d6c0242ece"),
                    QuestionMaker = "skill1@gmail.com",
                    Voter = "skill2@gmail.com",
                    UpVote = 1,
                    TimeStamp=new DateTime(2024, 4, 12, 18, 36, 39)},

                new Vote{ Id = new Guid("6559B465-83AE-4005-A415-2E24D2728CC4"),
                    QuestionId = new  Guid("E580CF4D-FA7A-421D-8D0F-AB406D0A2E23"),
                    QuestionMaker = "skill1@gmail.com",
                    Voter = "skill3@gmail.com",
                    UpVote = 1,
                    DownVote = 1,
                    TimeStamp=new DateTime(2024, 4, 12, 18, 36, 39)},

                new Vote{ Id = new Guid("20ED9BEE-E628-4C44-BA3D-B25A68827DCC"),
                    AnswerId = new Guid("963C81F0-4BF3-4C91-961D-945C2D8872F8"),
                    Replier = "skill1@gmail.com",
                    Voter = "skill2@gmail.com",
                    DownVote = 1,
                    TimeStamp=new DateTime(2024, 4, 12, 18, 36, 39)},

                new Vote{ Id = new Guid("6AAF6885-7B65-44A0-9E65-9B89C28F7673"),
                    AnswerId = new Guid("68503176-0314-4937-92E4-400A6F4F4472"),
                    Replier = "skill2@gmail.com",
                    Voter = "skill3@gmail.com",
                    DownVote = 1,
                    UpVote = 1,
                    TimeStamp=new DateTime(2024, 4, 12, 18, 36, 39)}
});

            base.OnModelCreating(builder);
        }

        public DbSet<Question> Question { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Vote> Vote { get; set; }
        public DbSet<Notification> Notification {get; set;}
    }
}
