using DataTransfer;

namespace Service.Interfaces
{
    public interface IMilkService
    {
        Task<List<MilkDTO>> GetAllMilk();
    }
}