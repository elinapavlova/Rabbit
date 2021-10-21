using System.Threading.Tasks;

namespace Infrastructure.Repositories.Base
{
    public interface IBaseRepository<T> where T : class   
    {   
        Task<T> Add(T entity);
    }
}