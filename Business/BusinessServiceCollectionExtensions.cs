using Business.Authentication;
using Business.Groups;
using Business.Messages;
using Business.Users;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BusinessServiceCollectionExtensions
    {
        public static IServiceCollection AddChaTexBusiness(this IServiceCollection services)
        {
            services.AddSingleton<IGroupManager, GroupManager>();
            services.AddSingleton<IMessageManager, MessageManager>();
            services.AddSingleton<IUserManager, UserManager>();
            services.AddSingleton<IAuthenticator, Authenticator>();
            services.AddSingleton<Authenticator>();
            return services;
        }
    }
}
