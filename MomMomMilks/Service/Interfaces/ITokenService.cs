using BusinessObject.Entities;
using DataTransfer;

namespace Service.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(AppUser user);
    }
}
