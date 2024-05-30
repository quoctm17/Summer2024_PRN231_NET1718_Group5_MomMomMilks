using BusinessObject.Entities;
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
    public class MilkRepository : IMilkRepository
    {
        private readonly IMilkDAO _milkDAO;
        public MilkRepository(IMilkDAO milkDAO)
        {
            _milkDAO = milkDAO;
        }

        public async Task<List<MilkDTO>> GetAllMilk()
        {
            return await _milkDAO.GetAllMilk();
        }

        public async Task<Milk> GetByIdAsync(int milkId)
        {
            return await _milkDAO.GetByIdAsync(milkId);
        }
    }
}
