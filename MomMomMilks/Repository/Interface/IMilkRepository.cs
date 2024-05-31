using BusinessObject.Entities;
using DataTransfer;

namespace Repository.Interface
{
    public interface IMilkRepository
    {
        Task<List<MilkDTO>> GetAllMilk();
        Task<Milk> GetByIdAsync(int milkId);
        Task<MilkDTO> GetMilkByIdAsync(int milkId);
    }
}