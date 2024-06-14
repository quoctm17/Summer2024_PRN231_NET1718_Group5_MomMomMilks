using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
using DataTransfer.Manager;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class OrderDAO
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        private static OrderDAO instance;

        public OrderDAO()
        {
            _context = new AppDbContext();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile.AutoMapperProfile())).CreateMapper();
        }

        public static OrderDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrderDAO();
                }
                return instance;
            }
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

        public async Task<List<ManagerOrderDTO>> GetUnassignedOrders()
        {
            List<ManagerOrderDTO> result = null;
            try
            {
                var orders = await _context.Orders
                    .Include(x => x.PaymentType)
                    .Include(x => x.Buyer)
                    .Include(x => x.OrderStatus)
                    .Include(x => x.TimeSlot)
                    .Where(x => x.ShipperId == null)
                    .ToListAsync();
                result = _mapper.Map<List<ManagerOrderDTO>>(orders);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public async Task AddOrderAsync(Order order)
        {
            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log detailed error message and inner exception details
                Console.WriteLine($"Error in AddOrderAsync: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw new Exception("An error occurred while adding the order.", ex);
            }
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<List<OrderStatus>> GetAllStatus()
        {
            List<OrderStatus> list = null;
            try
            {
                list = await _context.OrderStatuses
                    .Include(o => o.Orders)
                    .ThenInclude(o => o.OrderDetails)
                    .ToListAsync();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }

        public async Task<List<Order>> GetAllOrderHistory(int userId)
        {
            List<Order> list = null;
            try
            {
                list = await _context.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(o => o.Milk)
                    .Include(o => o.PaymentType)
                    .Include(o => o.Shipper)
                    .Include(o => o.OrderStatus)
                    .Where(o => o.BuyerId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
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
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }

        public async Task<List<Order>> GetShipperAssignedOrder(int shipperId)
        {
            try
            {
                var shipper = await _context.Shippers.Where(x => x.AppUserId == shipperId).FirstOrDefaultAsync();
                if (shipper == null)
                {
                    throw new Exception("Do not find Shipper");
                }
                var orders = await _context.Orders.Where(x => x.ShipperId == shipper.Id)
                    .Include(x => x.Address)
                    .Include(x => x.Buyer)
                    .Include(x => x.PaymentType)
                    .Include(x => x.Schedule)
                    .Include(x => x.OrderStatus)
                    .Include(x => x.TimeSlot)
                    .ToListAsync();
                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception("Error");
            }
        }
        public async Task<Order> GetShipperOrderDetail(int shipperId, int orderId)
        {
            try
            {
                var shipper = await _context.Shippers.Where(x => x.AppUserId == shipperId).FirstOrDefaultAsync();
                if (shipper == null)
                {
                    throw new Exception("Do not find Shipper");
                }
                var order = await _context.Orders.Where(x => x.ShipperId == shipper.Id && x.Id == orderId)
                    .Include(x => x.Address)
                    .Include(x => x.Buyer)
                    .Include(x => x.PaymentType)
                    .Include(x => x.Schedule)
                    .Include(x => x.OrderStatus)
                    .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Milk)
                    .Include(x => x.TimeSlot)
                    .FirstOrDefaultAsync();
                return order;
            }
            catch (Exception ex)
            {
                throw new Exception("Error");
            }
        }

        public async Task<bool> ConfirmShipped(int shipperId, int orderId)
        {
            try
            {
                var shipper = await _context.Shippers.Where(x => x.AppUserId == shipperId).FirstOrDefaultAsync();
                if (shipper == null)
                {
                    throw new Exception("Do not find Shipper");
                }
                var order = await _context.Orders.Where(x => x.ShipperId == shipper.Id && x.Id == orderId)
                    .FirstOrDefaultAsync();
                order.OrderStatusId = 3;
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
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
                    throw new Exception("Do not find Shipper");
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

        public async Task AutoAssignOrdersToShippers()
        {
            var currentTime = DateTime.Now;
            var timeSlots = await _context.TimeSlots.ToListAsync();

            foreach (var timeSlot in timeSlots)
            {
                var slotStartTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, timeSlot.StartTime.Hours, timeSlot.StartTime.Minutes, 0);
                if (currentTime >= slotStartTime.AddHours(-1) && currentTime <= slotStartTime.AddHours(timeSlot.EndTime.Hours - timeSlot.StartTime.Hours))
                {
                    var orders = await _context.Orders
                        .Where(o => o.TimeSlotId == timeSlot.Id && o.OrderStatusId == 2 && o.ShipperId == null)
                        .Include(o => o.Address)
                        .ToListAsync();

                    foreach (var order in orders)
                    {
                        var districtId = order.Address.DistrictId;
                        var availableShippers = await _context.Shippers
                            .Where(s => s.DistrictId == districtId && s.Status == "Available")
                            .ToListAsync();

                        if (availableShippers.Any())
                        {
                            var shipper = availableShippers.First();
                            order.ShipperId = shipper.Id;
                            shipper.Status = "Shipping";

                            _context.Update(order);
                            _context.Update(shipper);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<bool> ManagerAssignOrder(int orderId, int shipperId)
        {
            try
            {
                var existedOrder = await _context.Orders.FindAsync(orderId);
                existedOrder.ShipperId = shipperId;
                return await _context.SaveChangesAsync() > 0;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}
