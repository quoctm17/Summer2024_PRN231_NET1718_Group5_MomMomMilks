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
            List<AppUser> users = null;
            try
            {
                var userList = await _context.Users.ToListAsync();
                users = _mapper.Map<List<AppUser>>(userList);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return users;
        }

        public async Task<AppUser> GetUserById(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                return _mapper.Map<AppUser>(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
    }
}
