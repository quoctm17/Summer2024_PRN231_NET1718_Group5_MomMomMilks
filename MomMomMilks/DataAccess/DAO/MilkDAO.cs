using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class MilkDAO
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        private static MilkDAO instance;

        public MilkDAO()
        {
            _context = new AppDbContext();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile.AutoMapperProfile())).CreateMapper();
        }

        public static MilkDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MilkDAO();
                }
                return instance;
            }
        }

        public async Task<List<MilkDTO>> GetAllMilk()
        {
            List<MilkDTO> milk = null;
            try
            {
                var l = await _context.Milks
                    .Include(m => m.Brand)
                    .Include(m => m.Supplier)
                    .Include(m => m.Category)
                    .Include(m => m.MilkAge)
                    .ToListAsync();
                milk = _mapper.Map<List<MilkDTO>>(l);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return milk;
        }

        public async Task<Milk> GetByIdAsync(int milkId)
        {
            return await _context.Milks.FindAsync(milkId);
        }

        public async Task<MilkDTO> GetMilkByIdAsync(int milkId)
        {
            return _mapper.Map<MilkDTO>(await _context.Milks
                .Include(m => m.Category)
                .Include(m => m.Brand)
                .Include(m => m.Supplier)
                .Include(m => m.MilkAge)
                .Where(m => m.Id == milkId).FirstOrDefaultAsync());
        }

        public async Task AddMilkAsync(Milk milk)
        {
            await _context.Milks.AddAsync(milk);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMilkAsync(Milk milk)
        {
            _context.Milks.Update(milk);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMilkAsync(int milkId)
        {
            var milk = await _context.Milks.FindAsync(milkId);
            if (milk != null)
            {
                _context.Milks.Remove(milk);
                await _context.SaveChangesAsync();
            }
        }
    }
}
