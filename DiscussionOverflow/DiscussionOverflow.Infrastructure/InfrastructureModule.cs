using Autofac;
using DiscussionOverflow.Application;
using DiscussionOverflow.Application.Utilities;
using DiscussionOverflow.Domain.Repositories;
using DiscussionOverflow.Infrastructure.Email;
using DiscussionOverflow.Infrastructure.Membership;
using DiscussionOverflow.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscussionOverflow.Infrastructure
{
    public class InfrastructureModule:Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;

        public InfrastructureModule(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>().AsSelf()
                            .WithParameter("connectionString", _connectionString)
                            .WithParameter("migrationAssembly", _migrationAssembly)
                            .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationDbContext>().As<IApplicationDbContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssembly", _migrationAssembly)
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUnitOfWork>().As<IApplicationUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<HtmlEmailService>().As<IEmailService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TokenService>().As<ITokenService>()
               .InstancePerLifetimeScope();

            builder.RegisterType<UserRepository>().As<IUserRepository>()
               .InstancePerLifetimeScope();
            builder.RegisterType<QuestionRepository>().As<IQuestionRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<AnswerRepository>().As<IAnswerRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<CommentRepository>().As<ICommentRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<VoteRepository>().As<IVoteRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<NotificationRepository>().As<INotificationRepository>()
                .InstancePerLifetimeScope();


            //base.Load(builder);
        }
    }
}
