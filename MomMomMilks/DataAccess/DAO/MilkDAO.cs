using AutoMapper;
using BusinessObject.Entities;
using DataAccess.DAO.Interface;
using DataTransfer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class MilkDAO : IMilkDAO
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public MilkDAO(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<MilkDTO>> GetAllMilk()
        {
            List<MilkDTO> milk = null;
            try
            {
                var l = await _context.Milks.Include(m => m.Brand).Include(m => m.Supplier).Include(m => m.Category).ToListAsync();
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
    }
}
