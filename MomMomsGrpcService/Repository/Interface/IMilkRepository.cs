using BusinessObject.Entities;
using DataTransfer;

namespace Repository.Interface
{
    public interface IMilkRepository
    {
        Task<List<MilkDTO>> GetAllMilkAsync();
        Task<Milk> GetByIdAsync(int milkId);
        Task<MilkDTO> GetMilkByIdAsync(int milkId);
        Task AddMilkAsync(Milk milk);
        Task UpdateMilkAsync(Milk milk);
        Task DeleteMilkAsync(int milkId);
    }
}