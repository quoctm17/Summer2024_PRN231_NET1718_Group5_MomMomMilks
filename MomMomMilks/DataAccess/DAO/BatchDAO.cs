using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class BatchDAO
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        private static BatchDAO instance;

        public BatchDAO()
        {
            _context = new AppDbContext();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile.AutoMapperProfile())).CreateMapper();
        }

        public static BatchDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BatchDAO();
                }
                return instance;
            }
        }

        public async Task<List<Batch>> GetAllBatches()
        {
            try
            {
                var result = await _context
                    .Batches
                    .Include(x => x.Milk)
                    .ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Batch> GetSingleBatch(int id)
        {
            try
            {
                var result = await _context
                    .Batches
                    .Include(x => x.Milk)
                    .SingleOrDefaultAsync(x => x.Id == id);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Batch>> GetBatchesByMilkId(int milkId)
        {
            return await _context.Batches
                                 .Where(b => b.MilkId == milkId && b.Quantity > 0)
                                 .OrderBy(b => b.ExpiredDate)
                                 .ToListAsync();
        }

        public async Task<int> GetTotalQuantityByMilkId(int milkId)
        {
            return await _context.Batches
                                 .Where(b => b.MilkId == milkId)
                                 .SumAsync(b => b.Quantity);
        }


        public async Task<bool> CreateBatch(Batch batch)
        {
            try
            {
                await _context.Batches.AddAsync(batch);
                return await _context.SaveChangesAsync() > 0;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> UpdateBatch(Batch batch)
        {
            try
            {
                var existed = await _context.Batches.FindAsync(batch.Id);
                if (existed != null)
                {
                    existed.ExpiredDate = batch.ExpiredDate;
                    existed.ImportDate = batch.ImportDate;
                    existed.ImportedPrice = batch.ImportedPrice;
                    existed.Quantity = batch.Quantity;
                    existed.MilkId = batch.MilkId;
                }
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteBatch(int id)
        {
            try
            {
                var existed = await _context.Batches.FindAsync(id);
                if (existed != null)
                {
                    _context.Batches.Remove(existed);
                }
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
      
      public async Task<bool> UpdateQuantityIfUserBought(int milkId, int quantityBuy)
        {
            try
            {
                var existed = _context.Batches.Where(b => b.MilkId == milkId).OrderByDescending(b => b.ImportDate);

                for (int i = 1; i <= existed.Count(); i++) {
                    var item = existed.Skip(i - 1).Take(i).FirstOrDefault();
                    if (item.Quantity >= quantityBuy)
                    {
                        item.Quantity -= quantityBuy;
                        return true;
                    }
                    if (item.Quantity <= quantityBuy)
                    {
                        quantityBuy -= item.Quantity;
                        item.Quantity = 0;
                        i++;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> AutoUpdateExpiredDate()
        {
            try
            {
                var existed = await _context.Batches.ToListAsync();
                
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
