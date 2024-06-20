using BusinessObject.Entities;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class CartDAO
{
    private readonly AppDbContext _context;

    private static CartDAO instance;

    public CartDAO()
    {
        _context = new AppDbContext();
    }

    public static CartDAO Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CartDAO();
            }
            return instance;
        }
    }

    public async Task<Cart> GetCartByUserIdAsync(int userId)
    {
        return await _context.Carts
                             .Include(c => c.CartItems)
                             .ThenInclude(ci => ci.Milk)
                             .AsNoTracking() // Ensure entities are not tracked
                             .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task AddCartAsync(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();
    }

    public async Task AddCartItemAsync(CartItem cartItem)
    {
        var existingCartItem = await _context.CartItems
            .Include(ci => ci.Milk)
            .FirstOrDefaultAsync(ci => ci.CartId == cartItem.CartId && ci.MilkId == cartItem.MilkId);

        if (existingCartItem == null)
        {
            await _context.CartItems.AddAsync(cartItem);
        }
        else
        {
            existingCartItem.Quantity += cartItem.Quantity;
            _context.CartItems.Update(existingCartItem);
        }

        await _context.SaveChangesAsync();
    }

    public async Task UpdateCartItemAsync(CartItem cartItem)
    {
        var existingCartItem = await _context.CartItems
            .Include(ci => ci.Milk)
            .FirstOrDefaultAsync(ci => ci.CartId == cartItem.CartId && ci.MilkId == cartItem.MilkId);

        if (existingCartItem != null)
        {
            existingCartItem.Quantity = cartItem.Quantity;
            _context.CartItems.Update(existingCartItem);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveCartItemAsync(int cartItemId)
    {
        var cartItem = await _context.CartItems.FindAsync(cartItemId);
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearCartAsync(int cartId)
    {
        var cartItems = _context.CartItems.Where(ci => ci.CartId == cartId);
        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();
    }

    public async Task SaveCartAsync(int userId, List<CartItem> cartItems)
    {
        var cart = await GetCartByUserIdAsync(userId);
        if (cart != null)
        {
            // Detach all entities to avoid tracking issues
            _context.ChangeTracker.Clear();

            // Clear existing cart items to avoid duplicate tracking
            _context.CartItems.RemoveRange(cart.CartItems);
            foreach (var item in cartItems)
            {
                item.CartId = cart.Id;
                await _context.CartItems.AddAsync(item);
            }
            _context.Entry(cart).State = EntityState.Modified; // Set state to Modified to track changes
            _context.Carts.Update(cart);
        }
        else
        {
            cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.Now,
                CartItems = cartItems
            };
            await _context.Carts.AddAsync(cart);
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saving cart for user {userId}: {ex.Message}", ex);
        }
    }

    //public async Task<bool> AddPaymentOrderCode(int cartId, long orderCode)
    //{
    //    try
    //    {
    //        var cart = await _context.Carts.FindAsync(cartId);
    //        if (cart != null)
    //        {
    //            cart.PaymentOrderCode = orderCode;
    //        }
    //        return await _context.SaveChangesAsync() > 0;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }
    //}

}
