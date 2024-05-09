using Autofac;
using DiscussionOverflow.Web.Models;

namespace DiscussionOverflow.Web
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RegistrationModel>().AsSelf();
            builder.RegisterType<LoginModel>().AsSelf();
            builder.RegisterType<EditProfileModel>().AsSelf();
            builder.RegisterType<ProfileModel>().AsSelf();
            builder.RegisterType<QuestionModel>().AsSelf();
            builder.RegisterType<QuestionDetailsModel>().AsSelf();
            builder.RegisterType<HandleVoteModel>().AsSelf();
            builder.RegisterType<QuestionTagModel>().AsSelf();
            builder.RegisterType<NotificationModel>().AsSelf();
        }
    }
}
