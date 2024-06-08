using Repository.Interface;
using Repository;
using Service.Interfaces;
using Service.Services;
using Service;
using Service.Interface;

namespace MomMomMilks.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {


            //Add services to container
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Add HostedService
            services.AddSingleton<IHostedService, BackgroundMomMom>();

            // Repositories
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<IMilkRepository, MilkRepository>();
            services.AddScoped<IOrderDetailsRepository, OrderDetailsRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentTypeRepository, PaymentTypeRepository>();
            services.AddScoped<ITimeSlotRepository, TimeSlotRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWardRepository, WardRepository>();
            services.AddScoped<IBatchRepository, BatchRepository>();


            // Services
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICouponService, CouponService>();
            services.AddScoped<IDistrictService, DistrictService>();
            services.AddScoped<IMilkService, MilkService>();
            services.AddScoped<IOrderDetailsService, OrderDetailsService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentTypeService, PaymentTypeService>();
            services.AddScoped<ITimeSlotService, TimeSlotService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWardService, WardService>();
            services.AddScoped<IBatchService, BatchService>();


            return services;
        }
    }
}
