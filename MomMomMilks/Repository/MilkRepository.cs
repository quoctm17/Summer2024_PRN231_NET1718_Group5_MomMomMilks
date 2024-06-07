using BusinessObject.Entities;
using DataAccess.DAO;
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
        public async Task<List<MilkDTO>> GetAllMilkAsync()
        {
            return await MilkDAO.Instance.GetAllMilk();
        }

        public async Task<Milk> GetByIdAsync(int milkId)
        {
            return await MilkDAO.Instance.GetByIdAsync(milkId);
        }

        public async Task<MilkDTO> GetMilkByIdAsync(int milkId)
        {
            return await MilkDAO.Instance.GetMilkByIdAsync(milkId);
        }

        public async Task AddMilkAsync(Milk milk)
        {
            await MilkDAO.Instance.AddMilkAsync(milk);
        }

        public async Task UpdateMilkAsync(Milk milk)
        {
            await MilkDAO.Instance.UpdateMilkAsync(milk);
        }

        public async Task DeleteMilkAsync(int milkId)
        {
            await MilkDAO.Instance.DeleteMilkAsync(milkId);
        }
    }
}
