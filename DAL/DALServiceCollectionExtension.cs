using Business;
using DAL;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DALServiceCollectionExtensions
    {
        public static IServiceCollection AddChaTexDAL(this IServiceCollection services)
        {
            services.AddSingleton<IDataAccess, MessageRepository>();
            return services;
        }
    }
}