using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories.Message
{
    public interface IMessageRepository : IBaseRepository<Models.Messages.Message>
    {
    }
}