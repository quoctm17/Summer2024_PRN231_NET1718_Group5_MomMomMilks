using BusinessObject.Entities;
using Repository.Interface;
using Service.Interfaces;

namespace Service.Services
{
    public class BatchService : IBatchService
    {
        private readonly IBatchRepository _batchRepository;

        public BatchService(IBatchRepository batchRepository)
        {
            _batchRepository = batchRepository;
        }
        public async Task<bool> CreateBatch(Batch batch)
        {
            return await _batchRepository.CreateBatch(batch);
        }

        public async Task<bool> DeleteBatch(int id)
        {
            return await _batchRepository.DeleteBatch(id);
        }

        public async Task<List<Batch>> GetAllBatches()
        {
            return await _batchRepository.GetAllBatches();
        }

        public async Task<Batch> GetSingleBatch(int id)
        {
            return await _batchRepository.GetSingleBatch(id);
        }

        public async Task<int> GetTotalQuantityByMilkId(int milkId)
        {
            return await _batchRepository.GetTotalQuantityByMilkId(milkId);
        }

        public async Task<bool> UpdateBatch(Batch batch)
        {
            return await _batchRepository.UpdateBatch(batch);
        }
        public async Task<bool> UpdateQuantityIfUserBought(int milkId, int quantityBuy)
        {
            return await _batchRepository.UpdateQuantityIfUserBought(milkId, quantityBuy);
        }
        public async Task<bool> AutoDeleteExpiredBatch()
        {
            return await _batchRepository.AutoDeleteExpiredBatch();
        }
    }
}
