using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class UserDAO
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        private static UserDAO instance;

        public UserDAO()
        {
            _context = new AppDbContext();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile.AutoMapperProfile())).CreateMapper();
        }

        public static UserDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserDAO();
                }
                return instance;
            }
        }

        public async Task<List<ShipperDTO>> GetAllShippers()
        {
            List<ShipperDTO> shippers = null;
            try
            {
                var shipperList = await _context.Shippers.ToListAsync();
                shippers = _mapper.Map<List<ShipperDTO>>(shipperList);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return shippers;
        }

        public async Task<List<AppUser>> GetAllUsers()
        {
            return await _context.Users
                .Include(u => u.Addresses)
                    .ThenInclude(a => a.Ward)
                .Include(u => u.Addresses)
                    .ThenInclude(a => a.District)
                .Include(u => u.Cart)
                .ThenInclude(c => c.CartItems)
                .Include(u => u.Orders)
                .ToListAsync();
        }

        public async Task<AppUser> GetUserById(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Addresses)
                    .ThenInclude(a => a.Ward)
                .Include(u => u.Addresses)
                    .ThenInclude(a => a.District)
                 .Include(u => u.Cart)
                 .ThenInclude(c => c.CartItems)
                 .Include(u => u.Orders)
                 .FirstOrDefaultAsync();
        }

        public async Task AddUser(AppUser user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateUser(AppUser user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteUser(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CancelOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            try
            {
                if(order != null)
                {
                    if(order.OrderStatusId == 2)
                    {
                        order.OrderStatusId = 5;
                        _context.Orders.Update(order);
                        await _context.SaveChangesAsync();
                    }
                    else if(order.OrderStatusId == 1)
                    {
                        order.OrderStatusId = 6;
                        _context.Orders.Update(order);
                        await _context.SaveChangesAsync();
                    }
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RefundOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            try
            {
                if(order != null)
                {
                    if(order.OrderStatusId == 6)
                    {
                        order.OrderStatusId = 5;
                        _context.Orders.Update(order);
                        await _context.SaveChangesAsync();
                    }
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
