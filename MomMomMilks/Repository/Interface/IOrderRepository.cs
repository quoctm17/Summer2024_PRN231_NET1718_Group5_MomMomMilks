using BusinessObject.Entities;
using DataTransfer;
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
        Task<List<OrderDTO>> GetAllOrders();
        Task CreateOrderAsync(Order order, List<OrderDetail> orderDetails);
        Task<Order> GetOrderAsync(int orderId);
        Task<List<OrderHistoryDTO>> GetAllOrderHistory(int userId);
        Task<List<OrderDetailHistoryDTO>> GetDetailHistory(int orderId);
        Task<List<ShipperOrderDTO>> GetShipperOrders(int shipperId);
        Task<ShipperOrderDetailDTO> GetShipperOrderDetail(int shipperId, int orderId);
        Task<bool> ConfirmShipped(int shipperId, int orderId);
        Task<bool> ConfirmCancelled(int shipperId, int orderId);
    }
}
