using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using splendor.net5.core.contracts;

namespace splendor.net5.persistance.implementers
{
    public class EFCoreTransaction<DC> : ITransaction
        where DC: DbContext
    {
        public DC _dbContext;

        public EFCoreTransaction(DC dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Begin(Func<Task> action)
        {
            using(this){
                await action();
                await Commit();
            }
        }
        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}