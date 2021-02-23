using System;
using splendor.net5.core.contracts;
using Microsoft.EntityFrameworkCore;
using splendor.net5.persistance.contracts;
using Microsoft.Extensions.DependencyInjection;

namespace splendor.net5.persistance.implementers
{
    public abstract class EFCoreRepository<E, K> : IRepository<E, K>
        where E : class, new()
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DbContext _dbContext;
        private readonly IQueryResolver _queryResolver;

        public EFCoreRepository(
            IServiceProvider serviceProvider,
            Type dbContextType)
        {
            _serviceProvider = serviceProvider;
            _dbContext = _serviceProvider.GetRequiredService(dbContextType) as DbContext;
            _queryResolver = _serviceProvider.GetRequiredService<LinqQueryResolver>();
        }

        public EFCoreRepository(
            IServiceProvider serviceProvider,
            Type dbContextType,
            Type queryResolverType)
        {
            _serviceProvider = serviceProvider;
            _dbContext = _serviceProvider.GetRequiredService(dbContextType) as DbContext;
            _queryResolver = _serviceProvider.GetRequiredService(queryResolverType) as IQueryResolver;
        }

        public virtual void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
