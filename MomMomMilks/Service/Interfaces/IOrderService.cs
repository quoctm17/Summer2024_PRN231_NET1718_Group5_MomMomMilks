using BusinessObject.Entities;
using DataTransfer;
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
        Task<List<OrderHistoryDTO>> GetAllOrderHistory(int userId);
    }
}
