using Business.Authentication;
using Business.Messages;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BusinessServiceCollectionExtensions
    {
        public static IServiceCollection AddChaTexBusiness(this IServiceCollection services)
        {
            services.AddSingleton<IMessageManager, MessageManager>();
            services.AddSingleton<IUserManager, UserManager>();
            return services;
        }
    }
}
