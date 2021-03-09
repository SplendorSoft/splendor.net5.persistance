using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using splendor.net5.core.contracts;
using splendor.net5.core.implementers;
using splendor.net5.persistance.contracts;
using splendor.net5.persistance.implementers;

namespace splendor.net5.persistance.tools.ioc
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLinqQueryResolver(this IServiceCollection services)
        {
            services.AddSingleton<LinqQueryResolver>();
        }

        public static void AddEFCoreTransaction<DC>(this IServiceCollection services)
            where DC: DbContext
        {
            services.AddTransient<EFCoreTransaction<DC>>();
        }
    }
}