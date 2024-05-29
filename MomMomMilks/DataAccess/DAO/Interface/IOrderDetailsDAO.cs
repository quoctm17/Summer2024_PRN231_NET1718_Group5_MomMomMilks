using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO.Interface
{
    public interface IOrderDetailsDAO
    {
        Task AddOrderDetailAsync(OrderDetail orderDetail);
        Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
    }
}
