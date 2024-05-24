using DataAccess.DAO.Interface;
using DataTransfer;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserDAO _userDAO;
        public UserRepository(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        public async Task<List<ShipperDTO>> GetAllShippers()
        {
            return await _userDAO.GetAllShipper();
        }
    }
}
