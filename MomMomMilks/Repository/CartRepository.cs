using BusinessObject.Entities;
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

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await CartDAO.Instance.GetCartByUserIdAsync(userId);
        }

        public async Task AddCartItemAsync(int userId, int milkId, int quantity)
        {
            var cart = await CartDAO.Instance.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId, CreatedAt = DateTime.Now };
                await CartDAO.Instance.AddCartAsync(cart);
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.MilkId == milkId);
            if (cartItem == null)
            {
                cartItem = new CartItem { CartId = cart.Id, MilkId = milkId, Quantity = quantity };
                await CartDAO.Instance.AddCartItemAsync(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                await CartDAO.Instance.UpdateCartItemAsync(cartItem);
            }
        }

        public async Task UpdateCartItemAsync(int userId, int milkId, int quantity)
        {
            var cart = await CartDAO.Instance.GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.MilkId == milkId);
                if (cartItem != null)
                {
                    cartItem.Quantity = quantity;
                    await CartDAO.Instance.UpdateCartItemAsync(cartItem);
                }
            }
        }

        public async Task RemoveCartItemAsync(int userId, int cartItemId)
        {
            await CartDAO.Instance.RemoveCartItemAsync(cartItemId);
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await CartDAO.Instance.GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                await CartDAO.Instance.ClearCartAsync(cart.Id);
            }
        }

        public async Task SaveCartAsync(int userId, List<CartItem> cartItems)
        {
            await CartDAO.Instance.SaveCartAsync(userId, cartItems);
        }

        //public async Task<bool> AddPaymentOrderCode(int cartId, long orderCode)
        //{
        //    return await CartDAO.Instance.AddPaymentOrderCode(cartId, orderCode);
        //}
    }
}
