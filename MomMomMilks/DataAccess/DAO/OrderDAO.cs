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
        private Dictionary<int, int> _shippedOrdersCounts; // Used to store ShippedOrdersCount for each Shipper

        private static OrderDAO instance;


        public OrderDAO()
        {
            _context = new AppDbContext();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile.AutoMapperProfile())).CreateMapper();
            _shippedOrdersCounts = new Dictionary<int, int>(); // Initialize the dictionary for shipped orders count

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
                var order = await _context.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(o => o.Milk)
                    .Include(o => o.Buyer)
                    .ToListAsync();
                orderDTO = _mapper.Map<List<OrderDTO>>(order);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orderDTO;
        }
        public async Task<List<TopProduct>> GetTopProducts(int topN)
        {
            var topProducts = await _context.OrderDetails
                .GroupBy(od => od.Milk)
                .Select(g => new TopProduct
                {
                    Milk = g.Key,
                    TotalQuantitySold = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(tp => tp.TotalQuantitySold)
                .Take(topN)
                .ToListAsync();

            return topProducts;
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
                    .Where(x => x.OrderStatusId == 2 || x.OrderStatusId == 6)
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
                var orders = await _context.Orders.Where(x => x.ShipperId == shipper.AppUserId)
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
                var order = await _context.Orders.Where(x => x.ShipperId == shipper.AppUserId && x.Id == orderId)
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
                var order = await _context.Orders.Where(x => x.ShipperId == shipper.AppUserId && x.Id == orderId)
                    .FirstOrDefaultAsync();
                if(order.OrderStatusId == 3)
                {
                    order.OrderStatusId = 4;
                } else if (order.OrderStatusId == 6)
                {
                    order.OrderStatusId= 7;
                } else
                {
                    throw new Exception("Invalid order");
                }
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
                var order = await _context.Orders.Where(x => x.ShipperId == shipper.AppUserId && x.Id == orderId)
                    .FirstOrDefaultAsync();
                var od = await _context.OrderDetails.Where(x => x.OrderId == order.Id).ToListAsync();
                if (od.Count > 0)
                {
                    foreach(var d in od)
                    {
                        //var batch = await _context.Batches.FirstOrDefaultAsync(x => x.Id == d.BatchId);
                        //batch.Quantity += d.Quantity;
                        await BatchDAO.Instance.AddBatchQuantity(d.BatchId.Value, d.Quantity);
                        await _context.SaveChangesAsync();
                    }
                }
                order.OrderStatusId = 5;
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task AutoAssignOrdersToShippers(DateTime orderDate, string timeSlot)
        {
            var ordersToAssign = await GetOrdersToAssign(orderDate, timeSlot);

            foreach (var order in ordersToAssign)
            {
                int districtId = GetDistrictIdFromOrderAddress(order);

                var shippers = GetAvailableShippersByDistrictId(districtId);

                foreach (var shipper in shippers)
                {
                    AssignShipperToOrder(shipper, order);

                    UpdateShipperStatus(shipper, orderDate, timeSlot);

                    UpdateTimeSlotAndOrderDate(ref orderDate, ref timeSlot);

                    await _context.SaveChangesAsync();

                    break;
                }
            }
        }

        private async Task<List<Order>> GetOrdersToAssign(DateTime orderDate, string timeSlot)
        {
            return await _context.Orders
                .Include(o => o.TimeSlot)
                .Where(o => o.OrderDate.Date == orderDate.Date && o.TimeSlot.Name == timeSlot && o.ShipperId == null)
                .ToListAsync();
        }

        private int GetDistrictIdFromOrderAddress(Order order)
        {
            var address = _context.Addresses.Include(a => a.District).FirstOrDefault(a => a.Id == order.AddressId);
            return address?.District?.Id ?? 0;
        }

        private List<Shipper> GetAvailableShippersByDistrictId(int districtId)
        {
            return _context.Shippers
                .Where(s => s.DistrictId == districtId && s.Status == "Available")
                .ToList();
        }

        private void AssignShipperToOrder(Shipper shipper, Order order)
        {

            order.ShipperId = shipper.AppUserId;
            order.OrderStatusId = 3;

            if (!_shippedOrdersCounts.ContainsKey(shipper.Id))
            {
                _shippedOrdersCounts[shipper.Id] = 0;
            }

            _shippedOrdersCounts[shipper.Id]++;
        }

        private void UpdateShipperStatus(Shipper shipper, DateTime orderDate, string timeSlot)
        {
            var assignedOrderCount = _shippedOrdersCounts.ContainsKey(shipper.Id) ? _shippedOrdersCounts[shipper.Id] : 0;

            if (assignedOrderCount >= 15)
            {
                shipper.Status = "Shipping";
            }
            else
            {
                var totalOrdersInTimeSlot = _context.Orders
                    .Where(o => o.OrderDate.Date == orderDate.Date && o.TimeSlot.Name == timeSlot && o.ShipperId == null)
                    .Count();

                if (totalOrdersInTimeSlot == 0)
                {
                    shipper.Status = "Shipping";
                }
                else
                {
                    shipper.Status = "Available";
                }
            }
        }

        private void UpdateTimeSlotAndOrderDate(ref DateTime orderDate, ref string timeSlot)
        {
            if (timeSlot == "Evening")
            {
                orderDate = orderDate.AddDays(1);
                timeSlot = "Morning";
            }
            else if (timeSlot == "Morning")
            {
                timeSlot = "Afternoon";
            }
            else if (timeSlot == "Afternoon")
            {
                timeSlot = "Evening";
            }
        }


        public async Task<bool> ManagerAssignOrder(int orderId, int shipperId)
        {
            try
            {
                var existedOrder = await _context.Orders.FindAsync(orderId);
                existedOrder.ShipperId = shipperId;
                existedOrder.OrderStatusId = 3;
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
        public async Task<bool> RefundOrder(List<RefundDTO> refundDTOs)
        {
            try
            {
                var currentTime = DateTime.Now;
                int buyerId = 0;
                int addressId =0 ;
                int paymentTypeId = 0;
                int? transactionId = 0;
                float totalPrice = 0;
                DateTime createAtOrder = DateTime.Now;
                int ok = 0;
                var orderOrigin = new Order();
                List<RefundDTO> trueRefund = new List<RefundDTO>();
                foreach (var refund in refundDTOs)
                {
                    var order = await _context.OrderDetails.Include(o => o.Order).Where(od => od.Id == refund.OrderDetailId).FirstOrDefaultAsync();
                    buyerId = order.Order.BuyerId;
                    addressId = order.Order.AddressId;
                    paymentTypeId = order.Order.PaymentTypeId;
                    transactionId = order.Order.TransactionId;
                    totalPrice = order.Order.TotalAmount;
                    createAtOrder = order.Order.CreateAt;
                    

                    if (order.Status == "Normal" || (currentTime - createAtOrder).TotalDays < 7)
                    {
                        orderOrigin = order.Order;
                        order.UpdateAt = currentTime;
                        order.Status = "Defect";
                        order.Note = refund.note;
                        await _context.SaveChangesAsync();
                        trueRefund.Add(refund);
                        ok = 1;
                    }
                }
                if (orderOrigin != null)
                {
                    orderOrigin.OrderStatusId = 8;
                    _context.Orders.Update(orderOrigin);
                    await _context.SaveChangesAsync();
                }

                if ((currentTime - createAtOrder).TotalDays < 7 && ok == 1)
                {

                    var orderToAdd = new OrderRefundDTO()
                    {
                        CreateAt = currentTime,
                        UpdateAt = currentTime,
                        TotalAmount = totalPrice,
                        BuyerId = buyerId,
                        ShipperId = null,
                        AddressId = addressId,
                        PaymentTypeId = paymentTypeId,
                        TransactionId = transactionId,
                        OrderStatusId = 6
                    };

                    int currentTimeHour = DateTime.Now.Hour;

                    if (currentTimeHour < 6)
                    {
                        orderToAdd.TimeSlotId = 1;
                        Console.WriteLine("Chưa bắt đầu bất kỳ khung giờ làm việc nào.");
                    }
                    else if (currentTimeHour < 12)
                    {
                        orderToAdd.TimeSlotId = 2;
                        Console.WriteLine("Morning: Đang trong khung giờ '7-10h'.");
                    }
                    else if (currentTimeHour < 17)
                    {
                        orderToAdd.TimeSlotId = 3;
                        Console.WriteLine("Afternoon: Đang trong khung giờ '12h - 15h'.");
                    }
                    else
                    {
                        orderToAdd.TimeSlotId = 1;
                        Console.WriteLine("Evening: Đang trong khung giờ '17h - 20h'.");
                    }
                    var orderAdd = _mapper.Map<Order>(orderToAdd);
                    await _context.Orders.AddAsync(orderAdd);
                    await _context.SaveChangesAsync();

                    var orderId = orderAdd.Id;
                    foreach (var refund in trueRefund)
                    {
                        var order = await _context.OrderDetails.Include(o => o.Order).Where(od => od.Id == refund.OrderDetailId).FirstOrDefaultAsync();
                        var orderDetail = new OrderDetailRefundDTO()
                        {
                            OrderId = orderId,
                            CreateAt = currentTime,
                            UpdateAt = currentTime,
                            MilkId = order.MilkId,
                            Discount = order.Discount,
                            Price = order.Price,
                            Quantity = order.Quantity,
                            Total = order.Total,
                            BatchId = order.BatchId ?? 0,
                            Note = refund.note,
                            Status = "Refunding"
                        };
                        var orderDetails = _mapper.Map<OrderDetail>(orderDetail);
                        await _context.OrderDetails.AddAsync(orderDetails);
                        await _context.SaveChangesAsync();
                        await HandleBatchQuantity(orderDetails);
                        
                    }
                    return true;
            }
                
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task HandleBatchQuantity(OrderDetail detail)
        {
            var remainingQuantity = detail.Quantity;
            var batches = await BatchDAO.Instance.GetBatchesByMilkId(detail.MilkId);

            foreach (var batch in batches)
            {
                if (remainingQuantity <= 0)
                {
                    break;
                }

                var batchQuantityToDeduct = Math.Min(batch.Quantity, remainingQuantity);
                batch.Quantity -= batchQuantityToDeduct;
                remainingQuantity -= batchQuantityToDeduct;

                await BatchDAO.Instance.UpdateBatch(batch);
            }

            if (remainingQuantity > 0)
            {
                return;
            }
        }

        public async Task<bool> IsConpletedOrder(int orderId)
        {
            var order = await _context.Orders.Where(o => o.Id == orderId).FirstOrDefaultAsync();
            if (order != null)
            {
                if (order.OrderStatusId == 4)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task CancelOrder(int orderId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var order = await _context.Orders
                        .Include(o => o.OrderDetails)
                        .FirstOrDefaultAsync(o => o.Id == orderId);

                    if (order != null && (order.OrderStatusId == 2 || order.OrderStatusId == 1))
                    {
                        foreach (var detail in order.OrderDetails)
                        {
                            var batch = await BatchDAO.Instance.GetBatchById((int)detail.BatchId);
                            if (batch != null)
                            {
                                batch.Quantity += detail.Quantity;
                                await BatchDAO.Instance.UpdateBatch(batch);
                                /*await _context.SaveChangesAsync();*/
                            }
                        }

                        order.OrderStatusId = 5;
                        _context.Orders.Update(order);

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("An error occurred while canceling the order: " + ex.Message, ex);
                }
            }
        }

        public async Task<bool> ConfirmRefund(int orderId)
        {
            var order = await _context.Orders.Where(o => o.Id == orderId).FirstOrDefaultAsync();
            if (order != null)
            {
                if (order.OrderStatusId == 6)
                {
                    order.OrderStatusId = 7;
                    await UpdateOrder(order);
                    return true;
                }
            }
            return false;
        }
        public async Task<List<ShipperOrderReminderDTO>> IsLateForShippingToNotifyShipper(string timeslot)
        {
            var currentTime = DateTime.Now;
            var status = "Đang vận chuyển";
            var orders = await _context.Orders
                .Where(o => o.OrderStatus.Name == status)
                .Where(o => o.TimeSlot.Name == timeslot)
                .Include(o => o.Shipper)
                .ToListAsync();
            List<ShipperOrderReminderDTO> result = new List<ShipperOrderReminderDTO>();
            if (orders != null)
            {
                foreach (var order in orders)
                {
                    if (order.ShipperId != null)
                    {
                        //Phần gửi mail sẽ nằm ở đây
                        
                        if (order.OrderDate.Date >= currentTime.Date)
                        {
                            //Shipper sẽ nhận thông báo cảnh báo
                            ShipperOrderReminderDTO reminder = new ShipperOrderReminderDTO()
                            {
                                ShipperEmail = order.Shipper.Email,
                                Title = "Trạng Thái Đơn Hàng Chưa Được Cập Nhật",
                                Message = $"Trạng thái của đơn hàng có số {order.Id} chưa được cập nhật. Vui lòng cập nhật đơn trạng thái đơn hàng."
                            };
                            result.Add(reminder);
                            
                        } else
                        //
                        if (order.OrderDate <= currentTime)
                        {
                            //ở phần này shipper sẽ nhận thông báo là mình đã bị huỷ đơn vì giao trễ
                            var date = order.OrderDate.AddDays(1);                            
                            ShipperOrderReminderDTO reminder = new ShipperOrderReminderDTO()
                            {
                                ShipperEmail = order.Shipper.Email,
                                Title = "Bạn Bị Loại Ra Khỏi Đơn Hàng",
                                Message = $"Trạng thái của đơn hàng có số {order.Id} không được cập nhật trong khoảng thời gian dài. Vì vậy chúng tôi sẽ loại bạn ra khỏi đơn hàng này."
                            };
                            result.Add(reminder);
                            order.OrderDate = date;
                            order.ShipperId = null;
                            order.OrderStatusId = 2;
                            await _context.SaveChangesAsync();
                        }

                    }
                    else { break; }
                }
            }
            return result;
        }
        public async Task<List<OrderDetailWithNoteDTO>> GetAllOrderDetailWithNote(int orderId)
        {
            var orderdetails = await _context.OrderDetails.Where(o => o.OrderId == orderId).ToListAsync();
            if (orderdetails != null)
            {
                return _mapper.Map<List<OrderDetailWithNoteDTO>>(orderdetails);
            }
            return null;
        }
    }
}
