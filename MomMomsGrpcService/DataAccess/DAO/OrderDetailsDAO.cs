using AutoMapper;
using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.DAO
{
    public class OrderDetailsDAO
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        private static OrderDetailsDAO instance;

        public OrderDetailsDAO()
        {
            _context = new AppDbContext();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile.AutoMapperProfile())).CreateMapper();
        }

        public static OrderDetailsDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrderDetailsDAO();
                }
                return instance;
            }
        }

        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            try
            {
                Console.WriteLine("Start AddOrderDetailAsync");
                await _context.OrderDetails.AddAsync(orderDetail);
                await _context.SaveChangesAsync();
                Console.WriteLine("OrderDetail added successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddOrderDetailAsync: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw new Exception("An error occurred while adding the order detail.", ex);
            }
        }


        public async Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _context.OrderDetails.Where(od => od.OrderId == orderId).ToListAsync();
        }

        public async Task<OrderDetail> GetOrderDetailByIdAsync(int orderDetailId)
        {
            return await _context.OrderDetails.FindAsync(orderDetailId);
        }

        public async Task UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderDetailAsync(int orderDetailId)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(orderDetailId);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
                await _context.SaveChangesAsync();
            }
        }
    }
}
