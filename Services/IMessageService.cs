using System.Threading.Tasks;

namespace Services
{
    public interface IMessageService
    {
        Task ListenAsync();
    }
}