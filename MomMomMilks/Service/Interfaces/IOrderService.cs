using BusinessObject.Entities;
using DataTransfer;
using DataTransfer.Manager;
using DataTransfer.Shipper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderDTO>> GetAllOrders();
        Task CreateOrderAsync(Order order, List<OrderDetail> orderDetails);
        Task<Order> GetOrderAsync(int orderId);
        Task<List<Order>> GetAllOrderHistory(int userId);
        Task<bool> UpdateOrder(Order order);
        Task<List<ManagerOrderDTO>> GetUnassignedOrders();
        Task<List<OrderDetailHistoryDTO>> GetDetailHistory(int orderId);
        Task<List<ShipperOrderDTO>> GetShipperOrders(int shipperId);
        Task<ShipperOrderDetailDTO> GetShipperOrderDetail(int shipperId, int orderId);
        Task<bool> ConfirmShipped(int shipperId, int orderId);
        Task<bool> ConfirmCancelled(int shipperId, int orderId);
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
        Task<bool> IsCompletedOrder(int orderId);
        Task<bool> ConfirmRefund(int orderId);
    }
}
