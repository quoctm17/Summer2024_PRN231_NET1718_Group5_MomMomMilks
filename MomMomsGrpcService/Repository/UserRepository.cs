using BusinessObject.Entities;
using DataAccess.DAO;
using DataTransfer;
using Repository.Interface;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        public async Task<List<ShipperDTO>> GetAllShippers()
        {
            return await UserDAO.Instance.GetAllShippers();
        }

        public async Task<List<AppUser>> GetAllUsers()
        {
            return await UserDAO.Instance.GetAllUsers();
        }

        public async Task<AppUser> GetUserById(int userId)
        {
            return await UserDAO.Instance.GetUserById(userId);
        }

        public async Task AddUser(AppUser user)
        {
            await UserDAO.Instance.AddUser(user);
        }

        public async Task UpdateUser(AppUser user)
        {
            await UserDAO.Instance.UpdateUser(user);
        }

        public async Task DeleteUser(int userId)
        {
            await UserDAO.Instance.DeleteUser(userId);
        }
        public async Task CancelOrder(int orderId)
        {
            await UserDAO.Instance.CancelOrder(orderId);
        }
        public async Task RefundOrder(int orderId)
        {
            await UserDAO.Instance.RefundOrder(orderId);
        }
    }
}
