using BusinessObject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Emit;


namespace DataAccess
{
    public class AppDbContext : IdentityDbContext<AppUser,
                                                AppRole,
                                                int,
                                                IdentityUserClaim<int>,
                                                AppUserRole,
                                                IdentityUserLogin<int>,
                                                IdentityRoleClaim<int>,
                                                IdentityUserToken<int>>
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AppUser> Accounts { get; set; }
        public DbSet<AppRole> Roles { get; set; }
        public DbSet<AppUserRole> AppUserRole { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MilkAge> MilkAges { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CouponUsageHistory> CouponUsageHistories { get; set; }
        public DbSet<Milk> Milks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Batch> Batches{ get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("server=(local);Database=MomMomMilksDb;uid=sa;pwd=12345;TrustServerCertificate=true");

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", true, true)
                 .Build();
            var strConn = config.GetConnectionString("DefaultConnectionString");

            return strConn;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Address>()
                .HasOne(a => a.Ward)
                .WithMany(w => w.Addresses)
                .HasForeignKey(a => a.WardId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Address>()
                .HasOne(a => a.District)
                .WithMany(d => d.Addresses)
                .HasForeignKey(a => a.DistrictId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<AppUser>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.AppUser)
                .HasForeignKey(u => u.UserId)
                .IsRequired();

            builder.Entity<AppUser>()
            .HasOne(a => a.Cart)
            .WithOne(c => c.User)
            .HasForeignKey<Cart>(c => c.UserId);

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(ur => ur.AppRole)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();


            builder.Entity<Brand>()
            .HasKey(b => b.Id);


            builder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<CartItem>()
                .HasOne(ci => ci.Milk)
                .WithMany(m => m.CartItems)
                .HasForeignKey(ci => ci.MilkId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Category>()
                .HasKey(c => c.Id);


            builder.Entity<Coupon>()
                .HasKey(c => c.Id);


            builder.Entity<CouponUsageHistory>()
                .HasKey(k => new { k.OrderId, k.CouponId });
            builder.Entity<CouponUsageHistory>()
                .HasOne(cuh => cuh.Coupon)
                .WithMany(c => c.CouponUsageHistories)
                .HasForeignKey(cuh => cuh.CouponId);
            builder.Entity<CouponUsageHistory>()
                .HasOne(cuh => cuh.Order)
                .WithMany(o => o.CouponUsageHistories)
                .HasForeignKey(cuh => cuh.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<District>()
                .HasKey(d => d.Id);
            builder.Entity<District>()
                .HasMany(d => d.Wards)
                .WithOne(w => w.District)
                .HasForeignKey(w => w.DistrictId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<Order>()
                .HasKey(or => or.Id);
            builder.Entity<Order>()
                .HasOne(or => or.PaymentType)
                .WithMany(pt => pt.Orders)
                .HasForeignKey(or => or.PaymentTypeId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>()
                .HasOne(or => or.Address)
                .WithMany(a => a.Orders)
                .HasForeignKey(or => or.AddressId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>()
                .HasOne(or => or.Buyer)
                .WithMany(b => b.Orders)
                .HasForeignKey(or => or.BuyerId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>()
                .HasOne(or => or.OrderStatus)
                .WithMany(c => c.Orders)
                .HasForeignKey(or => or.OrderStatusId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>()
                .HasOne(o => o.TimeSlot)
                .WithMany(ts => ts.Orders)
                .HasForeignKey(o => o.TimeSlotId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<OrderDetail>()
                .HasKey(od => od.Id);
            builder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<OrderDetail>()
                .HasOne(od => od.Milk)
                .WithMany(m => m.OrderDetails)
                .HasForeignKey(od => od.MilkId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<OrderStatus>()
                .HasKey(c => c.Id);


            builder.Entity<Report>()
                .HasKey(r => r.Id);
            builder.Entity<Report>()
                .HasOne(r => r.Order)
                .WithMany(or => or.Reports)
                .HasForeignKey(r => r.OrderId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Report>()
                .HasOne(r => r.Staff)
                .WithMany(s => s.Reports)
                .HasForeignKey(r => r.StaffId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<Feedback>()
                .HasKey(f => f.Id);
            builder.Entity<Feedback>()
                .HasOne(f => f.Milk)
                .WithMany(m => m.Feedbacks)
                .HasForeignKey(f => f.MilkId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Feedback>()
                .HasOne(f => f.User)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<Milk>()
                .HasOne(m => m.Brand)
                .WithMany(b => b.Milks)
                .HasForeignKey(m => m.BrandId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Milk>()
                .HasOne(m => m.Category)
                .WithMany(c => c.Milks)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Milk>()
                .HasOne(m => m.MilkAge)
                .WithMany(a => a.Milks)
                .HasForeignKey(m => m.MilkAgeId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Milk>()
                .HasOne(m => m.Supplier)
                .WithMany(s => s.Milks)
                .HasForeignKey(m => m.SupplierId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<MilkAge>()
                .HasKey(ma => ma.Id);


            builder.Entity<PaymentType>()
                .HasKey(pt => pt.Id);


            builder.Entity<Schedule>()
                .HasKey(s => new { s.ShipperId, s.OrderId });
            builder.Entity<Schedule>()
                .HasOne(s => s.Shipper)
                .WithMany(sh => sh.Schedules)
                .HasForeignKey(s => s.ShipperId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Schedule>()
                .HasOne(s => s.Order)
                .WithOne(o => o.Schedule)
                .HasForeignKey<Schedule>(s => s.OrderId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Schedule>()
                .HasOne(s => s.TimeSlot)
                .WithMany(t => t.Schedules)
                .HasForeignKey(s => s.TimeSlotId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<Shipper>()
                .HasKey(s => s.Id);
            builder.Entity<Shipper>()
                .HasOne(s => s.AppUser)
                .WithOne(au => au.Shipper)
                .HasForeignKey<Shipper>(s => s.AppUserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Shipper>()
                .HasOne(s => s.District)
                .WithMany(d => d.Shippers)
                .HasForeignKey(s => s.DistrictId)
                .OnDelete(DeleteBehavior.NoAction);



            builder.Entity<Supplier>()
                .HasKey(s => s.Id);


            builder.Entity<TimeSlot>()
                .HasKey(ts => ts.Id);


            builder.Entity<Transaction>()
                .HasKey(t => t.Id);
            builder.Entity<Transaction>()
                .HasOne(t => t.Order)
                .WithMany(o => o.Transactions)
                .HasForeignKey(t => t.OrderId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<Ward>()
                .HasKey(w => w.Id);
        }
    }
}
