using DAL;
using Business;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DALServiceCollectionExtensions
    {
        public static IServiceCollection AddChaTexDAL(this IServiceCollection services)
        {
            services.AddSingleton<IMessageRepository, MessageRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IGroupRepository, GroupRepository>();
            services.AddSingleton<IChannelRepository, ChannelRepository>();
            services.AddSingleton<IRoleRepository, RoleRepository>();

            return services;
        }
    }
}