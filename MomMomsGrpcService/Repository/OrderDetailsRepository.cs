using BusinessObject.Entities;
using DataAccess.DAO;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class OrderDetailsRepository : IOrderDetailsRepository
    {
        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            await OrderDetailsDAO.Instance.AddOrderDetailAsync(orderDetail);
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await OrderDetailsDAO.Instance.GetOrderDetailsByOrderIdAsync(orderId);
        }

        public async Task<OrderDetail> GetOrderDetailByIdAsync(int orderDetailId)
        {
            return await OrderDetailsDAO.Instance.GetOrderDetailByIdAsync(orderDetailId);
        }

        public async Task UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            await OrderDetailsDAO.Instance.UpdateOrderDetailAsync(orderDetail);
        }

        public async Task DeleteOrderDetailAsync(int orderDetailId)
        {
            await OrderDetailsDAO.Instance.DeleteOrderDetailAsync(orderDetailId);
        }
    }
}
