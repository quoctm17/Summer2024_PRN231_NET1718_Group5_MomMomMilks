using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DataAccess.DAO
{
    public class UserDAO
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        private static UserDAO instance;

        public UserDAO()
        {
            _context = new AppDbContext();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile.AutoMapperProfile())).CreateMapper();

            var store = new UserStore<AppUser, IdentityRole<int>, AppDbContext, int>(_context);
            var identityOptions = Options.Create(new IdentityOptions());
            var passwordHasher = new PasswordHasher<AppUser>();
            var userValidators = new List<IUserValidator<AppUser>> { new UserValidator<AppUser>() };
            var passwordValidators = new List<IPasswordValidator<AppUser>> { new PasswordValidator<AppUser>() };
            var keyNormalizer = new UpperInvariantLookupNormalizer();
            var errors = new IdentityErrorDescriber();
            var services = new ServiceCollection().AddLogging().BuildServiceProvider();
            var logger = services.GetRequiredService<ILogger<UserManager<AppUser>>>();

            _userManager = new UserManager<AppUser>(store, identityOptions, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger);
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
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.AppRole)
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
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.AppRole)
                .FirstOrDefaultAsync();
        }

        public async Task AddUser(AppUser user)
        {
            var u = await _userManager.FindByEmailAsync(user.Email);
            try
            {
                if(u == null)
                {
                    string password = "Pa$$w0rd";
                    await _userManager.CreateAsync(user, password);
                }
                else
                {
                    throw new Exception("User is already exist");
                }
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
                    user.Status = 0;
                    _context.Users.Update(user);
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
                        var od = await _context.OrderDetails.Where(x => x.OrderId == order.Id).ToListAsync();
                        if (od.Count > 0)
                        {
                            foreach (var d in od)
                            {
                                var batch = await _context.Batches.FirstOrDefaultAsync(x => x.Id == d.BatchId);
                                batch.Quantity += d.Quantity;
                                await _context.SaveChangesAsync();
                            }
                        }
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
                        //TODO


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
