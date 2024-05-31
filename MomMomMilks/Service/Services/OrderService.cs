using BusinessObject.Entities;
using DataTransfer;
using DataTransfer.Shipper;
using Repository.Interface;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task CreateOrderAsync(Order order, List<OrderDetail> orderDetails)
        {
           
            await _orderRepository.CreateOrderAsync(order, orderDetails);
        }

        public async Task<Order> GetOrderAsync(int orderId)
        {
            
            return await _orderRepository.GetOrderAsync(orderId);
        }

        public Task<List<OrderDTO>> GetAllOrders()
        {
            return _orderRepository.GetAllOrders();
        }

        public async Task<List<OrderHistoryDTO>> GetAllOrderHistory(int userId)
        {
            return await _orderRepository.GetAllOrderHistory(userId);
        }
        public async Task<List<OrderDetailHistoryDTO>> GetDetailHistory(int orderId)
        {
            return await _orderRepository.GetDetailHistory(orderId);
        }

        public async Task<List<ShipperOrderDTO>> GetShipperOrders(int shipperId)
        {
            return await _orderRepository.GetShipperOrders(shipperId);
        }

        public async Task<ShipperOrderDetailDTO> GetShipperOrderDetail(int shipperId, int orderId)
        {
            return await _orderRepository.GetShipperOrderDetail(shipperId, orderId);
        }

        public async Task<bool> ConfirmShipped(int shipperId, int orderId)
        {
            return await _orderRepository.ConfirmShipped(shipperId, orderId);
        }

        public async Task<bool> ConfirmCancelled(int shipperId, int orderId)
        {
            return await _orderRepository.ConfirmCancelled(shipperId, orderId);
        }
    }
}
