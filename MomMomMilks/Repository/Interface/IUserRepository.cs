using BusinessObject.Entities;
using DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IUserRepository
    {
        Task<List<ShipperDTO>> GetAllShippers();
        Task<List<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(int userId);
        Task AddUser(AppUser user);
        Task UpdateUser(AppUser user);
        Task DeleteUser(int userId);
        Task CancelOrder(int orderId);
        Task RefundOrder(int orderId);
    }
}
