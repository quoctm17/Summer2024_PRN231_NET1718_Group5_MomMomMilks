using BusinessObject.Entities;
using DataTransfer;

namespace DataAccess.DAO.Interface
{
    public interface IMilkDAO
    {
        Task<List<MilkDTO>> GetAllMilk();
        Task<Milk> GetByIdAsync(int milkId);
    }
}