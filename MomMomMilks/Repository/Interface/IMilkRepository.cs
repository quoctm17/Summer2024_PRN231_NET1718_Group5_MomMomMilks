using DataTransfer;

namespace Repository.Interface
{
    public interface IMilkRepository
    {
        Task<List<MilkDTO>> GetAllMilk();
    }
}