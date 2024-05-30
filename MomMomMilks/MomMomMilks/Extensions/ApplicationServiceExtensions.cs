using DataAccess.DAO.Interface;
using DataAccess.DAO;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Repository;
using Service.Interfaces;
using Service.Services;
using DataAccess;
using Service;

namespace MomMomMilks.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            //Add DBContext
            services.AddDbContext<AppDbContext>();
            var connectionString =
            config.GetConnectionString("DefaultConnectionString");
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

            //Add services to container
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // DAOs
            services.AddScoped<IOrderDAO, OrderDAO>();
            services.AddScoped<IOrderDetailsDAO, OrderDetailsDAO>();
            services.AddScoped<IUserDAO, UserDAO>();
            services.AddScoped<IMilkDAO, MilkDAO>();
            services.AddScoped<ICategoryDAO, CategoryDAO>();
            services.AddScoped<ICartDAO, CartDAO>();

            // Repositories
            services.AddScoped<IMilkRepository, MilkRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICartRepository, CartRepository>();

            // Services
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICartService, CartService>();
            return services;
        }
    }
}
