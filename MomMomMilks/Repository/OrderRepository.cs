using BusinessObject.Entities;
using DataAccess.DAO;
using DataTransfer;
using DataTransfer.Manager;
using DataTransfer.Shipper;
using Microsoft.EntityFrameworkCore;
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
                await OrderDAO.Instance.AddOrderAsync(order);
                Console.WriteLine("Order added in OrderRepository");

                foreach (var detail in orderDetails)
                {
                    detail.OrderId = order.Id;
                    await HandleBatchQuantity(detail);
                    await OrderDetailsDAO.Instance.AddOrderDetailAsync(detail);
                    Console.WriteLine($"OrderDetail added: {detail.Id}");
                }

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
                throw new Exception("Not enough stock available to fulfill the order.");
            }
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

        public async Task AutoAssignOrdersToShippers()
        {
            await OrderDAO.Instance.AutoAssignOrdersToShippers();
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
    }
}
