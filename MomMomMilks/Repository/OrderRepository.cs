using DataAccess.DAO.Interface;
using DataTransfer;
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
        public OrderRepository(IOrderDAO orderDAO)
        {
            _orderDAO = orderDAO;
        }

        public async Task<List<OrderDTO>> GetAllOrders()
        {
            return await _orderDAO.GetAllOrders();
        }
    }
}
