﻿using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IBatchRepository
    {
        Task<List<Batch>> GetAllBatches();
        Task<List<Batch>> GetBatchByMilkId(int milkId);
        Task<Batch> GetSingleBatch(int id);
        Task<int> GetTotalQuantityByMilkId(int milkId);
        Task<bool> CreateBatch(Batch batch);
        Task<bool> UpdateBatch(Batch batch);
        Task<bool> DeleteBatch(int id);
        Task<bool> UpdateQuantityIfUserBought(int milkId, int quantityBuy);
        Task<bool> AutoDeleteExpiredBatch();
    }
}
