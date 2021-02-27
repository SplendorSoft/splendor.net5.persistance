using System.Linq;
using splendor.net5.core.commons;
using splendor.net5.persistance.commons;
using System.Linq.Dynamic.Core;
using System;

namespace splendor.net5.persistance.contracts
{
    public interface IQueryResolver
    {
        ResolveTO Resolve(DPagination pagination) => throw new NotImplementedException();
        string Operator(DFilter filter) => throw new NotImplementedException();
        void Sort(ResolveTO resolve, DPagination pagination){}
        IQueryable<E> AsQueryable<E>(IQueryable<E> query, ResolveTO resolve)
        {
            if (resolve.Values != null && resolve.Values.Length > 0)
            {
                query = query.Where(resolve.Query, resolve.Values);
            }
            if (!string.IsNullOrWhiteSpace(resolve.Order))
            {
                query = query.OrderBy(resolve.Order);
            }
            return query;
        }
    }
}