using BusinessObject.Entities;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task AddCartItemAsync(int userId, int milkId, int quantity);
        Task UpdateCartItemAsync(int userId, int milkId, int quantity);
        Task RemoveCartItemAsync(int userId, int cartItemId);
        Task ClearCartAsync(int userId);
        Task SaveCartAsync(int userId, List<CartItem> cartItems);
    }
}
