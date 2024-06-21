using BusinessObject.Entities;

namespace Service.Interfaces
{
    public interface IBatchService
    {
        Task<List<Batch>> GetAllBatches();
        Task<Batch> GetSingleBatch(int id);
        Task<int> GetTotalQuantityByMilkId(int milkId);
        Task<bool> CreateBatch(Batch batch);
        Task<bool> UpdateBatch(Batch batch);
        Task<bool> DeleteBatch(int id);
        Task<bool> UpdateQuantityIfUserBought(int milkId, int quantityBuy);
        Task<List<Batch>> GetBatchByMilkId(int milkId);
    }
}
