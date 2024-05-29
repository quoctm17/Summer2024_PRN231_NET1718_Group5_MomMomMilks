using AutoMapper;
using BusinessObject.Entities;
using DataAccess.DAO.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class OrderDetailsDAO : IOrderDetailsDAO
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrderDetailsDAO(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _context.OrderDetails.Where(od => od.OrderId == orderId).ToListAsync();
        }
    }
}
