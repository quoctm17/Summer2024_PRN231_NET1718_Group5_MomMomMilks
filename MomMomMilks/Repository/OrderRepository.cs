using BusinessObject.Entities;
using DataAccess.DAO;
using DataTransfer;
using DataTransfer.Shipper;
using Repository.Interface;

namespace Repository
{
    public class OrderRepository : IOrderRepository
    {
        public async Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            return await OrderDAO.Instance.GetAllOrders();
        }


        public async Task AddOrderAsync(Order order, List<OrderDetail> orderDetails)
        {
            await OrderDAO.Instance.AddOrderAsync(order);

            foreach (var detail in orderDetails)
            {
                detail.OrderId = order.Id; // Set OrderId for each detail
                await OrderDetailsDAO.Instance.AddOrderDetailAsync(detail);
            }
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await OrderDAO.Instance.GetOrderByIdAsync(orderId);
        }

        public async Task<List<OrderHistoryDTO>> GetAllOrderHistoryAsync(int userId)
        {
            return await OrderDAO.Instance.GetAllOrderHistory(userId);
        }
        public async Task<List<OrderDetailHistoryDTO>> GetDetailHistoryAsync(int orderId)
        {
            return await OrderDAO.Instance.GetDetailHistory(orderId);
        }

        public async Task<List<ShipperOrderDTO>> GetShipperAssignedOrderAsync(int shipperId)
        {
            return await OrderDAO.Instance.GetShipperAssignedOrder(shipperId);
        }

        public async Task<ShipperOrderDetailDTO> GetShipperOrderDetailAsync(int shipperId, int orderId)
        {
            return await OrderDAO.Instance.GetShipperOrderDetail(shipperId, orderId);
        }

        public async Task<bool> ConfirmShippedAsync(int shipperId, int orderId)
        {
            return await OrderDAO.Instance.ConfirmShipped(shipperId, orderId);
        }

        public async Task<bool> ConfirmCancelledAsync(int shipperId, int orderId)
        {
            return await OrderDAO.Instance.ConfirmCancelled(shipperId, orderId);
        }
    }
}
