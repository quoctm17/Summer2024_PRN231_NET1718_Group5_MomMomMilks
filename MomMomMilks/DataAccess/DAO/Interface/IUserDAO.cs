using DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO.Interface
{
    public interface IUserDAO
    {
        Task<List<ShipperDTO>> GetAllShipper();
    }
}
