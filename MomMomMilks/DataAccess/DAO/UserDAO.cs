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
    public class UserDAO : IUserDAO
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public UserDAO(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ShipperDTO>> GetAllShipper()
        {
            List<ShipperDTO> shippers = null;
            try
            {
                var shipperList = await _context.Shippers.ToListAsync();
                shippers = _mapper.Map<List<ShipperDTO>>(shipperList);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return shippers;
        }
    }
}
