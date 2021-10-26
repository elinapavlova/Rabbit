using Database;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Message
{
    public class MessageRepository: BaseRepository<Models.Messages.Message>, IMessageRepository
    {
        public MessageRepository (IDbContextFactory<AppDbContext> factory) : base (factory)
        {
        }
    }
}