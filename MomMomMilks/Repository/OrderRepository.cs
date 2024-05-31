﻿using BusinessObject.Entities;
using DataAccess.DAO;
using DataAccess.DAO.Interface;
using DataTransfer;
using DataTransfer.Shipper;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IOrderDAO _orderDAO;
        private readonly IOrderDetailsDAO _orderDetailsDAO;
        public OrderRepository(IOrderDAO orderDAO)
        {
            _orderDAO = orderDAO;
        }

        public async Task<List<OrderDTO>> GetAllOrders()
        {
            return await _orderDAO.GetAllOrders();
        }

        public async Task CreateOrderAsync(Order order, List<OrderDetail> orderDetails)
        {
            await _orderDAO.AddOrderAsync(order);

            foreach (var detail in orderDetails)
            {
                detail.OrderId = order.Id; // Set OrderId for each detail
                await _orderDetailsDAO.AddOrderDetailAsync(detail);
            }
        }

        public async Task<Order> GetOrderAsync(int orderId)
        {
            return await _orderDAO.GetOrderByIdAsync(orderId);
        }

        public async Task<List<OrderHistoryDTO>> GetAllOrderHistory(int userId)
        {
            return await _orderDAO.GetAllOrderHistory(userId);
        }
        public async Task<List<OrderDetailHistoryDTO>> GetDetailHistory(int orderId)
        {
            return await _orderDAO.GetDetailHistory(orderId);
        }

        public async Task<List<ShipperOrderDTO>> GetShipperOrders(int shipperId)
        {
            return await _orderDAO.GetShipperAssignedOrder(shipperId);
        }

        public async Task<ShipperOrderDetailDTO> GetShipperOrderDetail(int shipperId, int orderId)
        {
            return await _orderDAO.GetShipperOrderDetail(shipperId, orderId);
        }

        public async Task<bool> ConfirmShipped(int shipperId, int orderId)
        {
            return await _orderDAO.ConfirmShipped(shipperId, orderId);
        }

        public async Task<bool> ConfirmCancelled(int shipperId, int orderId)
        {
            return await _orderDAO.ConfirmCancelled(shipperId, orderId);
        }
    }
}
