using System;
using splendor.net5.core.contracts;
using Microsoft.EntityFrameworkCore;
using splendor.net5.persistance.contracts;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;
using splendor.net5.core.commons;
using splendor.net5.persistance.commons;

namespace splendor.net5.persistance.implementers
{
    public abstract class EFCoreRepository<E, K> : IRepository<E, K>
        where E : class, new()
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly DbContext _dbContext;
        protected readonly IQueryResolver _queryResolver;
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
        protected virtual Expression<Func<E, bool>> BaseExpression => default;
        public virtual async Task<E> Add (E entity) => (await _dbContext.Set<E>().AddAsync(entity)).Entity;
        public virtual async Task<E> Get(K id) => await _dbContext.Set<E>().FindAsync(id);
        public virtual Task<IQueryable<E>> All(){
            if(BaseExpression is not null)
                return Task.FromResult(_dbContext.Set<E>().Where(BaseExpression));
            return Task.FromResult(_dbContext.Set<E>().AsQueryable());
        }
        public virtual Task<IQueryable<E>> Page(DPagination pagination) {
            IQueryable<E> query = ResolveQuery(pagination);
            return Task.FromResult(PageLimit(query, pagination));
        }
        protected IQueryable<E> ResolveQuery(DPagination pagination)
        {
            ResolveTO resolve = _queryResolver.Resolve(pagination);
            if(BaseExpression is not null)
                return _queryResolver.AsQueryable(_dbContext.Set<E>(), resolve).Where(BaseExpression);
            return _queryResolver.AsQueryable(_dbContext.Set<E>(), resolve);
        }
        protected IQueryable<T> PageLimit<T>(IQueryable<T> query, DPagination pagination)
        {
            if (pagination != null && pagination.Limit != null && pagination.Page != null)
            {
                return query.Skip((pagination.Page.Value - 1) * pagination.Limit.Value)
                .Take(pagination.Limit.Value);
            }
            return query;
        }
        public virtual async Task ForceCommit() => await _dbContext.SaveChangesAsync();
        
        public virtual void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
