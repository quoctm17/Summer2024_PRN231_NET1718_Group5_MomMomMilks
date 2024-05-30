using BusinessObject.Entities;
using DataAccess.DAO.Interface;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ICartDAO _cartDAO;
        private readonly IUserDAO _userDAO;

        public CartRepository(ICartDAO cartDAO, IUserDAO userDAO)
        {
            _cartDAO = cartDAO;
            _userDAO = userDAO;
        }

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await _cartDAO.GetCartByUserIdAsync(userId);
        }

        public async Task AddCartItemAsync(int userId, int milkId, int quantity)
        {
            var cart = await _cartDAO.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId, CreatedAt = DateTime.Now };
                await _cartDAO.AddCartAsync(cart);
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.MilkId == milkId);
            if (cartItem == null)
            {
                cartItem = new CartItem { CartId = cart.Id, MilkId = milkId, Quantity = quantity };
                await _cartDAO.AddCartItemAsync(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                await _cartDAO.UpdateCartItemAsync(cartItem);
            }
        }

        public async Task UpdateCartItemAsync(int userId, int milkId, int quantity)
        {
            var cart = await _cartDAO.GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.MilkId == milkId);
                if (cartItem != null)
                {
                    cartItem.Quantity = quantity;
                    await _cartDAO.UpdateCartItemAsync(cartItem);
                }
            }
        }

        public async Task RemoveCartItemAsync(int userId, int cartItemId)
        {
            await _cartDAO.RemoveCartItemAsync(cartItemId);
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _cartDAO.GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                await _cartDAO.ClearCartAsync(cart.Id);
            }
        }
    }
}
