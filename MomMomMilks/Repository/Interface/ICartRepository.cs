using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task AddCartItemAsync(int userId, int milkId, int quantity);
        Task UpdateCartItemAsync(int userId, int milkId, int quantity);
        Task RemoveCartItemAsync(int userId, int cartItemId);
        Task ClearCartAsync(int userId);
        Task SaveCartAsync(int userId, List<CartItem> cartItems);
    }
}
