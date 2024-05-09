using Autofac;
using DiscussionOverflow.Application.Features.Services;
using DiscussionOverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscussionOverflow.Application
{
    public class ApplicationModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserManagementService>().As<IUserManagementService>().InstancePerLifetimeScope();
            builder.RegisterType<QuestionManagementService>().As<IQuestionManagementService>().InstancePerLifetimeScope();
            //base.Load(builder);
        }
    }
}
