using System.Threading.Tasks;
using Database;

namespace Infrastructure.Repositories.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        protected BaseRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<T> Add(T data)
        {
            await _context.Set<T>().AddAsync(data);
            return data;
        }

    }
}