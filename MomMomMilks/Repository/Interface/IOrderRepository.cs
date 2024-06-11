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
        Task<List<OrderHistoryDTO>> GetAllOrderHistoryAsync(int userId);
        Task<List<ManagerOrderDTO>> GetUnassignedOrders();
        Task<List<OrderDetailHistoryDTO>> GetDetailHistoryAsync(int orderId);
        Task<List<Order>> GetShipperAssignedOrderAsync(int shipperId);
        Task<Order> GetShipperOrderDetailAsync(int shipperId, int orderId);
        Task<bool> ConfirmShippedAsync(int shipperId, int orderId);
        Task<bool> ConfirmCancelledAsync(int shipperId, int orderId);
        Task<bool> ManagerAssignOrder(int shipperId, int orderId);
        Task AutoAssignOrdersToShippers();
    }
}
