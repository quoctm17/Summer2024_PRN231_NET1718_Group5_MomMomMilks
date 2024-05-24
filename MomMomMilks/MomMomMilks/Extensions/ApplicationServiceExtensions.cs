using DataAccess.DAO.Interface;
using DataAccess.DAO;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Repository;
using Service.Interfaces;
using Service.Services;
using DataAccess;

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
            services.AddScoped<IMilkDAO, MilkDAO>();
            services.AddScoped<IMilkRepository, MilkRepository>();
            services.AddScoped<IOrderDAO, OrderDAO>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }
    }
}
