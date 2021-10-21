using Infrastructure.Repositories.Response;

namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        int Save();
        IResponseRepository ResponseRepository { get; }
    }
}