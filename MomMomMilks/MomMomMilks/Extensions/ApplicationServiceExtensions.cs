using Repository.Interface;
using Repository;
using Service.Interfaces;
using Service.Services;
using Service;
using Service.Interface;
using Service.Helpers;
using MomMomMilks.EmailService.Settings;
using MomMomMilks.EmailService;

namespace MomMomMilks.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

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
            services.AddScoped<IShipperRepository, ShipperRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IMilkAgeRepository, MilkAgeRepository>();


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
            services.AddScoped<IShipperService, ShipperService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IMilkAgeService, MilkAgeService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IEmailService, MomMomMilks.EmailService.EmailService>();

            services.Configure<SendInBlue>(config.GetSection("SendInBlue"));

            return services;
        }
    }
}
