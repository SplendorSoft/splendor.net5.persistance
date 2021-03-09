using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace splendor.net5.persistance.tools.config
{
    public static class ContextBuilderExtensions
    {
        public const int RETRY_COUNT = 30;
        public const int RETRY_INTERVAL = 10;
        public static void UseLowerTranslate(this ModelBuilder modelBuilder)
        {
            modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties())
            .ToList()
            .ForEach(p => p.SetColumnName(p.Name.ToLower()));
        }

        public static void UseCommandTimeOut30x10<TB, TE>(this RelationalDbContextOptionsBuilder<TB, TE> options)
            where TB : RelationalDbContextOptionsBuilder<TB, TE>
            where TE : RelationalOptionsExtension, new()
        {
            options.CommandTimeout(RETRY_COUNT * RETRY_INTERVAL);
        }
    }
}