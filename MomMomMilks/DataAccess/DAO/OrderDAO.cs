using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObject.Entities;
using DataAccess.DAO.Interface;
using DataTransfer;
using DataTransfer.Shipper;
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

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<List<OrderHistoryDTO>> GetAllOrderHistory(int userId)
        {
            List<OrderHistoryDTO> list = null;
            try
            {
                var l = await _context.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(o => o.Milk)
                    .Include(o => o.PaymentType)
                    .Include(o => o.Shipper)
                    .Include(o => o.OrderStatus)
                    .Where(o => o.BuyerId == userId)
                    .ToListAsync();
                list = _mapper.Map<List<OrderHistoryDTO>>(l);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }

        public async Task<List<OrderDetailHistoryDTO>> GetDetailHistory(int orderId)
        {
            List<OrderDetailHistoryDTO> list = null;
            try
            {
                var l = await _context.OrderDetails
                    .Include(o => o.Milk)
                    .Where(o => o.OrderId == orderId)
                    .ToListAsync();
                list = _mapper.Map<List<OrderDetailHistoryDTO>>(l);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }

        public async Task<List<ShipperOrderDTO>> GetShipperAssignedOrder(int shipperId)
        {
            try
            {
                var shipper = await _context.Shippers.Where(x => x.AppUserId == shipperId).FirstOrDefaultAsync();
                if (shipper == null)
                {
                    throw new Exception("Không tìm thấy shipper");
                }
                var orders = await _context.Orders.Where(x => x.ShipperId == shipper.Id)
                    .ProjectTo<ShipperOrderDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                return orders;
            }
            catch(Exception ex)
            {
                throw new Exception("Lỗi");
            }
        }
        public async Task<ShipperOrderDetailDTO> GetShipperOrderDetail(int shipperId, int orderId)
        {
            try
            {
                var shipper = await _context.Shippers.Where(x => x.AppUserId == shipperId).FirstOrDefaultAsync();
                if (shipper == null)
                {
                    throw new Exception("Không tìm thấy shipper");
                }
                var order = await _context.Orders.Where(x => x.ShipperId == shipper.Id && x.Id == orderId)
                    .ProjectTo<ShipperOrderDetailDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return order;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi");
            }
        }

        public async Task<bool> ConfirmShipped(int shipperId, int orderId)
        {
            try
            {
                var shipper = await _context.Shippers.Where(x => x.AppUserId == shipperId).FirstOrDefaultAsync();
                if (shipper == null)
                {
                    throw new Exception("Không tìm thấy shipper");
                }
                var order = await _context.Orders.Where(x => x.ShipperId == shipper.Id && x.Id == orderId)
                    .FirstOrDefaultAsync();
                order.OrderStatusId = 3;
                return await _context.SaveChangesAsync() > 0;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ConfirmCancelled(int shipperId, int orderId)
        {
            try
            {
                var shipper = await _context.Shippers.Where(x => x.AppUserId == shipperId).FirstOrDefaultAsync();
                if (shipper == null)
                {
                    throw new Exception("Không tìm thấy shipper");
                }
                var order = await _context.Orders.Where(x => x.ShipperId == shipper.Id && x.Id == orderId)
                    .FirstOrDefaultAsync();
                order.OrderStatusId = 4;
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }


}
