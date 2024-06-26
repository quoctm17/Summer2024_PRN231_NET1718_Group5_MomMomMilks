using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
using DataTransfer.Manager;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
        public async Task<List<OrderRevenueDTO>> GetOrdersToCalculateRevenue()
        {
            List<OrderRevenueDTO> orderDTO = null;
            try
            {
                var order = await _context.Orders.Where(o => o.OrderStatusId == 4).Include(o => o.OrderDetails).ThenInclude(od => od.Milk).ToListAsync();
                orderDTO = _mapper.Map<List<OrderRevenueDTO>>(order);
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
                Console.WriteLine("Start AddOrderAsync");
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                Console.WriteLine("Order added successfully");
            }
            catch (Exception ex)
            {
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
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Milk)
                .FirstOrDefaultAsync(o => o.Id == orderId);
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
            }
            catch (Exception ex)
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

        public async Task<bool> UpdateOrder(Order order)
        {
            try
            {
                _context.Orders.Update(order);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating order: " + ex.Message);
            }
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
                    throw new Exception("Cannot find Shipper");
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
                    throw new Exception("Cannot find Shipper");
                }
                var order = await _context.Orders.Where(x => x.ShipperId == shipper.Id && x.Id == orderId)
                    .Include(x => x.Address)
                    .ThenInclude(a => a.Ward)
                    .ThenInclude(a => a.District)
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
                    throw new Exception("Cannot find Shipper");
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

        public async Task<bool> ConfirmCancelled(int shipperId, int orderId)
        {
            try
            {
                var shipper = await _context.Shippers.Where(x => x.AppUserId == shipperId).FirstOrDefaultAsync();
                if (shipper == null)
                {
                    throw new Exception("Cannot find Shipper");
                }
                var order = await _context.Orders.Where(x => x.ShipperId == shipper.Id && x.Id == orderId)
                    .FirstOrDefaultAsync();
                order.OrderStatusId = 5;
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

                // Chỉ xử lý các đơn hàng trong TimeSlot tiếp theo
                if (currentTime >= slotStartTime.AddMinutes(-90) && currentTime <= slotStartTime.AddHours(timeSlot.EndTime.Hours - timeSlot.StartTime.Hours))
                {
                    var orders = await _context.Orders
                        .Where(o => o.TimeSlotId == timeSlot.Id && o.OrderStatusId == 2 && o.ShipperId == null)
                        .Include(o => o.Address)
                        .ToListAsync();

                    var districtId = orders.FirstOrDefault()?.Address?.DistrictId;

                    if (districtId == null)
                        continue;

                    var assignedShipperIds = new HashSet<int>();
                    var ordersToAssignLater = new List<Order>();
                    var ordersInDistrict = await _context.Orders
                        .Where(o => o.TimeSlotId == timeSlot.Id && o.OrderStatusId == 2 && o.Address.DistrictId == districtId)
                        .ToListAsync();

                    foreach (var order in orders)
                    {
                        var availableShippers = await _context.Shippers
                            .Where(s => s.DistrictId == districtId && s.Status == "Available" && !assignedShipperIds.Contains(s.Id))
                            .ToListAsync();

                        if (availableShippers.Any())
                        {
                            var shipper = availableShippers.First();
                            assignedShipperIds.Add(shipper.Id);

                            if (assignedShipperIds.Count < 5 && ordersInDistrict.Any())
                            {
                                order.ShipperId = shipper.Id;
                                shipper.Status = "Shipping";
                                _context.Update(shipper);
                                ordersInDistrict.Remove(order);
                            }
                            else
                            {
                                ordersToAssignLater.Add(order); // Save order to assign later
                            }

                            _context.Update(order);
                        }
                        else
                        {
                            ordersToAssignLater.Add(order); // No available shippers, assign later
                        }
                    }

                    // Assign remaining orders to later slots
                    foreach (var order in ordersToAssignLater)
                    {
                        AssignOrdersToNextSlot(ordersToAssignLater, timeSlot.Id);
                    }

                    await _context.SaveChangesAsync();

                    // Update status for shippers if all orders in district are assigned
                    if (ordersInDistrict.Any())
                    {
                        var remainingShippers = await _context.Shippers
                            .Where(s => s.DistrictId == districtId && s.Status == "Available")
                            .ToListAsync();

                        foreach (var shipper in remainingShippers)
                        {
                            shipper.Status = "Shipping";
                            _context.Update(shipper);
                        }

                        await _context.SaveChangesAsync();
                    }
                }
            }
        }


        // Hàm hỗ trợ để chuyển đơn hàng sang TimeSlot tiếp theo
        private void AssignOrdersToNextSlot(List<Order> ordersToAssignLater, int currentSlotId)
        {
            var nextSlot = GetNextTimeSlot(currentSlotId);

            foreach (var order in ordersToAssignLater)
            {
                order.TimeSlotId = nextSlot.Id;
                _context.Update(order);
            }

            _context.SaveChanges();
        }

        // Hàm hỗ trợ để tìm TimeSlot tiếp theo
        private TimeSlot GetNextTimeSlot(int currentSlotId)
        {
            // Tìm TimeSlot tiếp theo của TimeSlot hiện tại
            var nextSlot = _context.TimeSlots.FirstOrDefault(ts => ts.Id > currentSlotId);

            // Nếu không tìm thấy TimeSlot tiếp theo, tìm TimeSlot đầu tiên của ngày tiếp theo
            if (nextSlot == null)
            {
                nextSlot = _context.TimeSlots.OrderBy(ts => ts.Id).FirstOrDefault();
            }

            return nextSlot;
        }


        public async Task<bool> ManagerAssignOrder(int orderId, int shipperId)
        {
            try
            {
                var existedOrder = await _context.Orders.FindAsync(orderId);
                existedOrder.ShipperId = shipperId;
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AddPaymentOrderCode(int orderId, long orderCode)
        {
            try
            {
                var order = await _context.Set<Order>().FindAsync(orderId);
                if (order != null)
                {
                    order.PaymentOrderCode = orderCode;
                }
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<Order> GetOrderByPaymentOrderCode(long paymentOrderCode)
        {
            return await _context.Set<Order>()
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Milk)
                .FirstOrDefaultAsync(o => o.PaymentOrderCode == paymentOrderCode);
        }

        public async Task<Order> GetOrderByBuyerIdAndCreateAt(int buyerId, DateTime createAt)
        {
            try
            {
                return await _context.Orders
                    .Where(o => o.BuyerId == buyerId && o.CreateAt == createAt)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching order by buyerId and createAt", ex);
            }
        }
        public async Task UpdateOrderStatusWhenOverDateAsync()
        {
            try
            {
                var currentTime = DateTime.Now;
                var orders = await _context.Orders.Where(od => od.OrderStatusId == 1).ToListAsync();
                foreach (var order in orders)
                {
                    if (order != null)
                    {
                        if ((currentTime - order.CreateAt).TotalMinutes > 10)
                            order.OrderStatusId = 5;
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
