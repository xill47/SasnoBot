using System.Threading.Tasks;

namespace SasnoBot.Services.Interfaces
{
    public interface IUserManager
    {
        Task<bool> IsUserAllowedToDoConfiguration(ulong userId);
        Task SetUserAllowedToDoConfiguration(ulong userId, bool allowed);
    }
}