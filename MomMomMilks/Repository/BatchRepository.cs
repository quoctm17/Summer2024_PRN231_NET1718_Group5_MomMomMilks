using BusinessObject.Entities;
using DataAccess.DAO;
using Repository.Interface;

namespace Repository
{
    public class BatchRepository : IBatchRepository
    {
        public async Task<bool> CreateBatch(Batch batch)
        {
            return await BatchDAO.Instance.CreateBatch(batch);
        }

        public async Task<bool> DeleteBatch(int id)
        {
            return await BatchDAO.Instance.DeleteBatch(id);
        }

        public async Task<List<Batch>> GetAllBatches()
        {
            return await BatchDAO.Instance.GetAllBatches();
        }

        public async Task<Batch> GetSingleBatch(int id)
        {
            return await BatchDAO.Instance.GetSingleBatch(id);
        }

        public async Task<int> GetTotalQuantityByMilkId(int milkId)
        {
            return await BatchDAO.Instance.GetTotalQuantityByMilkId(milkId);
        }

        public async Task<bool> UpdateBatch(Batch batch)
        {
            return await BatchDAO.Instance.UpdateBatch(batch);
        }
    }
}
