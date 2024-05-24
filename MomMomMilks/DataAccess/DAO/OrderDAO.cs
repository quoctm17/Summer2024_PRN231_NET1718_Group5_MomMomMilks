using AutoMapper;
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
    public class OrderDAO : IOrderDAO
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public OrderDAO(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<OrderDTO>> GetAllOrders()
        {
            List<OrderDTO> orderDTO = null;
            try
            {
                var order = await _context.Orders.ToListAsync();
                orderDTO = _mapper.Map<List<OrderDTO>>(order);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orderDTO;
        }
    }
}
