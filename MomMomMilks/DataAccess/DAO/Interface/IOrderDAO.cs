using BusinessObject.Entities;
using DataTransfer;
using DataTransfer.Shipper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO.Interface
{
    public interface IOrderDAO
    {
        Task<List<OrderDTO>> GetAllOrders();
        Task AddOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<List<OrderHistoryDTO>> GetAllOrderHistory(int userId);
        Task<List<OrderDetailHistoryDTO>> GetDetailHistory(int orderId);
        Task<List<ShipperOrderDTO>> GetShipperAssignedOrder(int shipperId);
        Task<ShipperOrderDetailDTO> GetShipperOrderDetail(int shipperId, int orderId);
        Task<bool> ConfirmShipped(int shipperId, int orderId);
        Task<bool> ConfirmCancelled(int shipperId, int orderId);
        
    }
}
