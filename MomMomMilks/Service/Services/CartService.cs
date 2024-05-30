using BusinessObject.Entities;
using Repository.Interface;
using Service.Interfaces;
using System.Threading.Tasks;

namespace Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMilkRepository _milkRepository;

        public CartService(ICartRepository cartRepository, IMilkRepository milkRepository)
        {
            _cartRepository = cartRepository;
            _milkRepository = milkRepository;
        }

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await _cartRepository.GetCartByUserIdAsync(userId);
        }

        public async Task AddCartItemAsync(int userId, int milkId, int quantity)
        {
            var milk = await _milkRepository.GetByIdAsync(milkId);
            if (milk == null)
            {
                throw new ArgumentNullException(nameof(milk), "Milk not found");
            }
            await _cartRepository.AddCartItemAsync(userId, milkId, quantity);
        }

        public async Task UpdateCartItemAsync(int userId, int milkId, int quantity)
        {
            var milk = await _milkRepository.GetByIdAsync(milkId);
            if (milk == null)
            {
                throw new ArgumentNullException(nameof(milk), "Milk not found");
            }
            await _cartRepository.UpdateCartItemAsync(userId, milkId, quantity);
        }

        public async Task RemoveCartItemAsync(int userId, int cartItemId)
        {
            await _cartRepository.RemoveCartItemAsync(userId, cartItemId);
        }

        public async Task ClearCartAsync(int userId)
        {
            await _cartRepository.ClearCartAsync(userId);
        }
    }
}
