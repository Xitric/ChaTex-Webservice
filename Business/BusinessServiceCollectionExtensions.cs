using Business.Authentication;
using Business.Channels;
using Business.Chats;
using Business.Groups;
using Business.Messages;
using Business.Roles;
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
            services.AddSingleton<IChannelManager, ChannelManager>();
            services.AddSingleton<IAuthenticator, Authenticator>();
            services.AddSingleton<IRoleManager, RoleManager>();
            services.AddSingleton<IChatManager, ChatManager>();
            services.AddSingleton<Authenticator>();
            return services;
        }
    }
}
