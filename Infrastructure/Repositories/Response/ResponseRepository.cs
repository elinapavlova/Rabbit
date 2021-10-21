using Database;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories.Response
{
    public class ResponseRepository: BaseRepository<Models.Responses.Response>, IResponseRepository
    {
        public ResponseRepository (AppDbContext context) : base (context)
        {
        }
    }
}