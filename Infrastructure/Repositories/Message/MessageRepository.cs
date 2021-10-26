using Database;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Response
{
    public class MessageRepository: BaseRepository<Models.Message>, IMessageRepository
    {
        public MessageRepository (IDbContextFactory<AppDbContext> factory) : base (factory)
        {
        }
    }
}