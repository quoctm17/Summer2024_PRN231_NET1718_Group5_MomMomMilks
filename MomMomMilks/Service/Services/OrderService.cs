using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
using DataTransfer.Manager;
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
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task CreateOrderAsync(Order order, List<OrderDetail> orderDetails)
        {
            try
            {
                await _orderRepository.AddOrderAsync(order, orderDetails);
            }
            catch (Exception ex)
            {
                // Log detailed error message and inner exception details
                Console.WriteLine($"Error in CreateOrderAsync: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw new Exception("An error occurred while creating the order.", ex);
            }
        }

        public async Task<Order> GetOrderAsync(int orderId)
        {
            
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public Task<List<OrderDTO>> GetAllOrders()
        {
            return _orderRepository.GetAllOrdersAsync();
        }

        public async Task<List<Order>> GetAllOrderHistory(int userId)
        {
            return await _orderRepository.GetAllOrderHistoryAsync(userId);
        }
        public async Task<List<OrderDetailHistoryDTO>> GetDetailHistory(int orderId)
        {
            return await _orderRepository.GetDetailHistoryAsync(orderId);
        }

        public async Task<List<ShipperOrderDTO>> GetShipperOrders(int shipperId)
        {
            var result = await _orderRepository.GetShipperAssignedOrderAsync(shipperId);
            return _mapper.Map<List<ShipperOrderDTO>>(result);
        }

        public async Task<ShipperOrderDetailDTO> GetShipperOrderDetail(int shipperId, int orderId)
        {
            var result = _mapper.Map<ShipperOrderDetailDTO>(await _orderRepository.GetShipperOrderDetailAsync(shipperId, orderId));
            return result;
        }

        public async Task<bool> ConfirmShipped(int shipperId, int orderId)
        {
            return await _orderRepository.ConfirmShippedAsync(shipperId, orderId);
        }

        public async Task<bool> ConfirmCancelled(int shipperId, int orderId)
        {
            return await _orderRepository.ConfirmCancelledAsync(shipperId, orderId);
        }

        public async Task AutoAssignOrdersToShippers()
        {
            await _orderRepository.AutoAssignOrdersToShippers();
        }

        public async Task<bool> ManagerAssignOrder(int shipperId, int orderId)
        {
            return await _orderRepository.ManagerAssignOrder(orderId, shipperId);
        }

        public async Task<List<ManagerOrderDTO>> GetUnassignedOrders()
        {
            return await _orderRepository.GetUnassignedOrders();
        }
        public async Task<List<OrderStatus>> GetAllStatus()
        {
            return await _orderRepository.GetAllStatus();
        }
    }
}
