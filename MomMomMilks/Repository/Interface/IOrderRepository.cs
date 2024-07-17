using BusinessObject.Entities;
using DataTransfer;
using DataTransfer.Manager;
using DataTransfer.Shipper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IOrderRepository
    {
        Task<List<OrderDTO>> GetAllOrdersAsync();
        Task AddOrderAsync(Order order, List<OrderDetail> orderDetails);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetAllOrderHistoryAsync(int userId);
        Task<bool> UpdateOrder(Order order);
        Task<List<ManagerOrderDTO>> GetUnassignedOrders();
        Task<List<OrderDetailHistoryDTO>> GetDetailHistoryAsync(int orderId);
        Task<List<Order>> GetShipperAssignedOrderAsync(int shipperId);
        Task<Order> GetShipperOrderDetailAsync(int shipperId, int orderId);
        Task<bool> ConfirmShippedAsync(int shipperId, int orderId);
        Task<bool> ConfirmCancelledAsync(int shipperId, int orderId);
        Task<bool> ManagerAssignOrder(int shipperId, int orderId);
        Task AutoAssignOrdersToShippers(DateTime orderDate, string timeSlot);
        Task<List<OrderStatus>> GetAllStatus();
        Task<bool> AddPaymentOrderCode(int orderId, long orderCode);
        Task<Order> GetOrderByPaymentOrderCode(long paymentOrderCode);
        Task<Order> GetOrderByBuyerIdAndCreateAt(int buyerId, DateTime createAt);
        Task<List<OrderRevenueDTO>> GetOrdersToCalculateRevenue();
        Task UpdateOrderStatusWhenOverDateAsync();
        Task<List<TopProduct>> GetTopProducts(int topN);
        Task<bool> RefundOrder(List<RefundDTO> refundDTOs);
        Task<bool> IsConpletedOrder(int orderId);
        Task<bool> ConfirmRefund(int orderId);
    }
}
