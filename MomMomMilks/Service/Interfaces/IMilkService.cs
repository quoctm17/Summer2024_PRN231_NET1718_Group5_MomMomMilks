using BusinessObject.Entities;
using DataTransfer;

namespace Service.Interfaces
{
    public interface IMilkService
    {
        Task<List<MilkDTO>> GetAllMilk();
        Task<MilkDTO> GetMilkById(int id);
        Task AddMilkAsync(Milk milk);
        Task UpdateMilkAsync(Milk milk);
        Task DeleteMilkAsync(int milkId);
    }
}