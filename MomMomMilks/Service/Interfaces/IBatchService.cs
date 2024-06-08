using BusinessObject.Entities;

namespace Service.Interfaces
{
    public interface IBatchService
    {
        Task<List<Batch>> GetAllBatches();
        Task<Batch> GetSingleBatch(int id);
        Task<bool> CreateBatch(Batch batch);
        Task<bool> UpdateBatch(Batch batch);
        Task<bool> DeleteBatch(int id);
    }
}
