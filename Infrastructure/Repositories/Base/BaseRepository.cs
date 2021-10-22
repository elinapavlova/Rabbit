using System.Threading.Tasks;
using Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        protected BaseRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        
        public async Task<T> AddAsync(T data)
        {
            await using var context = _contextFactory.CreateDbContext();
            await context.Set<T>().AddAsync(data);
            await context.SaveChangesAsync();
            return data;
        }
    }
}