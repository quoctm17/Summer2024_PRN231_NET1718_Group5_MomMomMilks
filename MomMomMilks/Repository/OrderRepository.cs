using BusinessObject.Entities;
using DataAccess.DAO;
using DataTransfer;
using DataTransfer.Manager;
using MomMomMilks.Exceptions;
using Repository.Interface;

namespace Repository
{
    public class OrderRepository : IOrderRepository
    {
        public async Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            return await OrderDAO.Instance.GetAllOrders();
        }

        public async Task AddOrderAsync(Order order, List<OrderDetail> orderDetails)
        {
            try
            {
                Console.WriteLine("Start AddOrderAsync in OrderRepository");

                // Tạo danh sách mới để chứa các OrderDetail đã xử lý
                var processedOrderDetails = new List<OrderDetail>();

                // Kiểm tra số lượng hàng tồn kho trước
                foreach (var detail in orderDetails)
                {
                    var newDetails = await HandleBatchQuantity(detail);
                    processedOrderDetails.AddRange(newDetails);
                }

                // Loại bỏ tất cả các OrderDetail từ đơn hàng ban đầu
                order.OrderDetails.Clear();

                // Sau khi đã kiểm tra và đủ số lượng, tiến hành tạo đơn hàng
                await OrderDAO.Instance.AddOrderAsync(order);
                Console.WriteLine("Order added in OrderRepository");

                // Thêm các OrderDetail đã xử lý vào đơn hàng và lưu chúng
                foreach (var detail in processedOrderDetails)
                {
                    detail.OrderId = order.Id;
                    await OrderDetailsDAO.Instance.AddOrderDetailAsync(detail);
                    Console.WriteLine($"OrderDetail added: {detail.Id}");
                }

                // Tạo giao dịch với trạng thái mặc định là Pending
                var transaction = new Transaction
                {
                    Status = "Pending",
                    CreatedAt = DateTime.Now,
                    OrderId = order.Id,
                    GrossAmount = order.TotalAmount
                };

                await TransactionDAO.Instance.AddTransaction(transaction);
                Console.WriteLine("Transaction created successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddOrderAsync: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw new Exception("An error occurred while adding the order details.", ex);
            }
        }

        private async Task<List<OrderDetail>> HandleBatchQuantity(OrderDetail detail)
        {
            var remainingQuantity = detail.Quantity;
            var batches = await BatchDAO.Instance.GetBatchesByMilkId(detail.MilkId);

            // Tính tổng số lượng hàng tồn kho
            var totalAvailableQuantity = batches.Sum(b => b.Quantity);

            // Nếu tổng số lượng hàng tồn kho nhỏ hơn số lượng yêu cầu, ném ra ngoại lệ
            if (totalAvailableQuantity < remainingQuantity)
            {
                throw new OutOfStockException("Not enough stock available to fulfill the order.");
            }

            var newOrderDetails = new List<OrderDetail>();

            // Trừ đi số lượng hàng tồn kho từng lô
            foreach (var batch in batches)
            {
                if (remainingQuantity <= 0)
                {
                    break;
                }

                var batchQuantityToDeduct = Math.Min(batch.Quantity, remainingQuantity);
                batch.Quantity -= batchQuantityToDeduct;
                remainingQuantity -= batchQuantityToDeduct;

                // Tạo OrderDetail mới cho mỗi batch
                var orderDetail = new OrderDetail
                {
                    MilkId = detail.MilkId,
                    Discount = detail.Discount,
                    Price = detail.Price,
                    Quantity = batchQuantityToDeduct,
                    Total = (int)(batchQuantityToDeduct * detail.Price),
                    BatchId = batch.Id,
                    Status = "Normal",
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                };

                newOrderDetails.Add(orderDetail);

                await BatchDAO.Instance.UpdateBatch(batch);
                Console.WriteLine($"Batch updated: {batch.Id}, Remaining Quantity: {batch.Quantity}");
            }

            return newOrderDetails;
        }


        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await OrderDAO.Instance.GetOrderByIdAsync(orderId);
        }

        public async Task<List<Order>> GetAllOrderHistoryAsync(int userId)
        {
            return await OrderDAO.Instance.GetAllOrderHistory(userId);
        }
        public async Task<bool> UpdateOrder(Order order)
        {
            return await OrderDAO.Instance.UpdateOrder(order);
        }
        public async Task<List<OrderDetailHistoryDTO>> GetDetailHistoryAsync(int orderId)
        {
            return await OrderDAO.Instance.GetDetailHistory(orderId);
        }

        public async Task<List<Order>> GetShipperAssignedOrderAsync(int shipperId)
        {
            return await OrderDAO.Instance.GetShipperAssignedOrder(shipperId);
        }

        public async Task<Order> GetShipperOrderDetailAsync(int shipperId, int orderId)
        {
            return await OrderDAO.Instance.GetShipperOrderDetail(shipperId, orderId);
        }

        public async Task<bool> ConfirmShippedAsync(int shipperId, int orderId)
        {
            return await OrderDAO.Instance.ConfirmShipped(shipperId, orderId);
        }

        public async Task<bool> ConfirmCancelledAsync(int shipperId, int orderId)
        {
            return await OrderDAO.Instance.ConfirmCancelled(shipperId, orderId);
        }

        public async Task AutoAssignOrdersToShippers(DateTime orderDate, string timeSlot)
        {
            await OrderDAO.Instance.AutoAssignOrdersToShippers(orderDate, timeSlot);
        }

        public async Task<bool> ManagerAssignOrder(int shipperId, int orderId)
        {
            return await OrderDAO.Instance.ManagerAssignOrder(orderId, shipperId);
        }

        public async Task<List<ManagerOrderDTO>> GetUnassignedOrders()
        {
            return await OrderDAO.Instance.GetUnassignedOrders();
        }
        public async Task<List<OrderStatus>> GetAllStatus()
        {
            return await OrderDAO.Instance.GetAllStatus();
        }

        public async Task<bool> AddPaymentOrderCode(int orderId, long orderCode)
        {
            return await OrderDAO.Instance.AddPaymentOrderCode(orderId, orderCode);
        }

        public async Task<Order> GetOrderByPaymentOrderCode(long paymentOrderCode)
        {
            return await OrderDAO.Instance.GetOrderByPaymentOrderCode(paymentOrderCode);
        }

        public async Task<Order> GetOrderByBuyerIdAndCreateAt(int buyerId, DateTime createAt)
        {
            return await OrderDAO.Instance.GetOrderByBuyerIdAndCreateAt(buyerId, createAt);
        }

        public async Task<List<OrderRevenueDTO>> GetOrdersToCalculateRevenue()
        {
            return await OrderDAO.Instance.GetOrdersToCalculateRevenue();
        }
        public async Task UpdateOrderStatusWhenOverDateAsync()
        {
            await OrderDAO.Instance.UpdateOrderStatusWhenOverDateAsync();
        }
        public async Task<List<TopProduct>> GetTopProducts(int topN)
        {
            return await OrderDAO.Instance.GetTopProducts(topN);
        }
        public async Task<bool> RefundOrder(List<RefundDTO> refundDTOs)
        {
            return await OrderDAO.Instance.RefundOrder(refundDTOs);
        }
        public async Task<bool> IsConpletedOrder(int orderId)
        {
            return await OrderDAO.Instance.IsConpletedOrder(orderId);
        }
        public async Task CancelOrder(int orderId)
        {
            await OrderDAO.Instance.CancelOrder(orderId);
        }
        public async Task<bool> ConfirmRefund(int orderId)
        {
            return await OrderDAO.Instance.ConfirmRefund(orderId);
        }
        public async Task IsLateForShippingToNotifyShipper(string timeslot)
        {
            await OrderDAO.Instance.IsLateForShippingToNotifyShipper(timeslot);
        }
    }
}
