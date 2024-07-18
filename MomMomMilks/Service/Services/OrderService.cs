using AutoMapper;
using BusinessObject.Entities;
using DataAccess.DAO;
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
            await _orderRepository.AddOrderAsync(order, orderDetails);
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
        public async Task<bool> UpdateOrder(Order order)
        {
            return await _orderRepository.UpdateOrder(order);
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

        public async Task AutoAssignOrdersToShippers(DateTime orderDate, string timeSlot)
        {
            await _orderRepository.AutoAssignOrdersToShippers(orderDate, timeSlot);
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

        public async Task<bool> AddPaymentOrderCode(int orderId, long orderCode)
        {
            return await _orderRepository.AddPaymentOrderCode(orderId, orderCode);
        }

        public async Task<Order> GetOrderByPaymentOrderCode(long paymentOrderCode)
        {
            return await _orderRepository.GetOrderByPaymentOrderCode(paymentOrderCode);
        }

        public async Task<Order> GetOrderByBuyerIdAndCreateAt(int buyerId, DateTime createAt)
        {
            return await _orderRepository.GetOrderByBuyerIdAndCreateAt(buyerId, createAt);
        }
        public async Task<List<OrderRevenueDTO>> GetOrdersToCalculateRevenue()
        {
            return await _orderRepository.GetOrdersToCalculateRevenue();
        }
        public async Task UpdateOrderStatusWhenOverDateAsync()
        {
            await _orderRepository.UpdateOrderStatusWhenOverDateAsync();
        }
        public async Task<List<TopProduct>> GetTopProducts(int topN)
        {
            return await _orderRepository.GetTopProducts(topN);
        }
        public async Task<bool> RefundOrder(List<RefundDTO> refundDTOs)
        {
            return await _orderRepository.RefundOrder(refundDTOs);
        }
        public async Task<bool> IsCompletedOrder(int orderId)
        {
            return await _orderRepository.IsConpletedOrder(orderId);
        }
        public async Task CancelOrder(int orderId)
        {
            await _orderRepository.CancelOrder(orderId);
        }
        public async Task<bool> ConfirmRefund(int orderId)
        {
            return await _orderRepository.ConfirmRefund(orderId);
        }
        public async Task IsLateForShippingToNotifyShipper(string timeslot)
        {
            await _orderRepository.IsLateForShippingToNotifyShipper(timeslot);
        }
    }
}
