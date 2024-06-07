using BusinessObject.Entities;
using DataTransfer;
using Repository.Interface;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<ShipperDTO>> GetAllShippers()
        {
            return await _userRepository.GetAllShippers();
        }

        public async Task<List<AppUser>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<AppUser> GetUserById(int userId)
        {
            return await _userRepository.GetUserById(userId);
        }

        public async Task AddUser(AppUser user)
        {
            await _userRepository.AddUser(user);
        }

        public async Task UpdateUser(AppUser user)
        {
            await _userRepository.UpdateUser(user);
        }

        public async Task DeleteUser(int userId)
        {
            await _userRepository.DeleteUser(userId);
        }
    }
}
