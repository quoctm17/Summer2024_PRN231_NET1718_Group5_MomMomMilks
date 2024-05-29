using BusinessObject.Entities;
using DataTransfer;
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

    }
}
