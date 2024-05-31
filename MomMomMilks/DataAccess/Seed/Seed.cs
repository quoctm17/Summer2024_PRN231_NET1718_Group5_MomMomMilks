using BusinessObject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DataAccess.Seed
{
    public class Seed
    {
        public static async Task SeedUser(UserManager<AppUser> _userManager, RoleManager<AppRole> _roleManager)
        {
            if (await _userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("../DataAccess/Seed/UserSeed.json");
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, jsonOptions);

            var role = new List<AppRole>
            {
                new AppRole {Name = "Customer"},
                new AppRole {Name = "Admin"},
                new AppRole {Name = "Manager"},
                new AppRole {Name = "Shipper"}
            };

            foreach (var roles in role)
            {
                await _roleManager.CreateAsync(roles);
            }

            foreach (var user in users)
            {
                user.EmailConfirmed = true;
                var result = await _userManager.CreateAsync(user, "Pa$$w0rd");
                await _userManager.AddToRoleAsync(user, "Customer");
            }

            var admin = new AppUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Status = 1,
                Point = 0
            };

            var resultAdmin = await _userManager.CreateAsync(admin, "Pa$$w0rd");
            await _userManager.AddToRolesAsync(admin, new[] { "Admin" });

            var manager = new AppUser
            {
                UserName = "manager@gmail.com",
                Email = "manager@gmail.com",
                EmailConfirmed = true,
                Status = 1,
                Point = 0
            };

            await _userManager.CreateAsync(manager, "Pa$$w0rd");
            await _userManager.AddToRolesAsync(manager, new[] { "Manager" });

            var shipper = new AppUser
            {
                UserName = "shipper@gmail.com",
                Email = "shipper@gmail.com",
                EmailConfirmed = true,
                Status = 1,
                Point = 0,
                Shipper = new Shipper
                {
                    Status = "Available"
                }
            };

            await _userManager.CreateAsync(shipper, "Pa$$w0rd");
            await _userManager.AddToRolesAsync(shipper, new[] { "Shipper" });
            var shipper2 = new AppUser
            {
                UserName = "shipper2@gmail.com",
                Email = "shipper2@gmail.com",
                EmailConfirmed = true,
                Status = 1,
                Point = 0,
                Shipper = new Shipper
                {
                    Status = "Unavailable"
                }
            };

            await _userManager.CreateAsync(shipper2, "Pa$$w0rd");
            await _userManager.AddToRolesAsync(shipper2, new[] { "Shipper" });
            var shipper3 = new AppUser
            {
                UserName = "shipper3@gmail.com",
                Email = "shipper3@gmail.com",
                EmailConfirmed = true,
                Status = 1,
                Point = 0,
                Shipper = new Shipper
                {
                    Status = "Shipping"
                }
            };

            await _userManager.CreateAsync(shipper3, "Pa$$w0rd");
            await _userManager.AddToRolesAsync(shipper3, new[] { "Shipper" });
        }

        public static async Task SeedBranch(AppDbContext _context)
        {
            if(await _context.Brands.AnyAsync()) { return; }

            var list = new List<Brand>
            {
                new Brand {Name="Nestle"},
                new Brand {Name="Meiji"},
                new Brand {Name="Abbott"},
                new Brand {Name="Enfa"},
                new Brand {Name="Dutch Lady"},
                new Brand {Name="NutiFood"},
                new Brand {Name="TH True Milk"},
                new Brand {Name="Friso"}
            };

            foreach(var i in list)
            {
                await _context.Brands.AddAsync(i);
                await _context.SaveChangesAsync();
            }
        }

        public static async Task MilkAgeSeed(AppDbContext _context)
        {
            if (await _context.MilkAges.AnyAsync()) { return; }

            var list = new List<MilkAge>
            {
                new MilkAge {Min = 0, Max = 2},
                new MilkAge {Min = 1, Max = 10},
                new MilkAge {Min = 6, Max = 12},
                new MilkAge {Min = 12, Max = 18},
                new MilkAge {Min = 18, Max = 30},
                new MilkAge {Min = 30, Max = 60}
            };

            foreach(var i in list)
            {
                await _context.MilkAges.AddAsync(i);
                await _context.SaveChangesAsync();
            }
        }

        public static async Task SupplierSeed(AppDbContext _context)
        {
            if (await _context.Suppliers.AnyAsync()) { return; }

            var list = new List<Supplier>
            {
                new Supplier{Name="Nhà máy Konolfingen, Thụy Sĩ"},
                new Supplier{Name="Abbott Manufacturing Singapore Private Limited, Singapore"},
                new Supplier{Name="Danone Nutricia, Ireland"},
                new Supplier{Name="Công ty TNHH FrieslandCampina Hà Nam, Việt Nam"},
                new Supplier{Name="Nhà máy Meiji Saitama, Nhật Bản"},
                new Supplier{Name="Nutifood Việt Nam"},
                new Supplier{Name="Danone, New Zealand"},
                new Supplier{Name="Hà Lan"}
            };

            foreach(var i in list)
            {
                await _context.Suppliers.AddAsync(i);
                await _context.SaveChangesAsync();
            }
        }
        public static async Task CategorySeed(AppDbContext _context)
        {
            if (await _context.Categories.AnyAsync()) { return; }

            var list = new List<Category>
            {
                new Category {Name="Sữa bột cho trẻ", Description = "Sữa bột dành cho trẻ sơ sinh hoặc trẻ từ trên 12 tháng tuổi"},
                new Category {Name="Sữa hộp cho bé", Description = "Sữa hộp dành cho trẻ em từ 3 đến 12 tuổi"},
                new Category {Name="Sữa hộp bình thường", Description = "Sữa hộp dành cho trẻ từ 12 tuổi và người trưởng thành"},
                new Category {Name="Sữa dành cho bà bầu", Description="Sữa bột dành cho các bà mẹ đang mang thai"}
            };
            
            foreach (var i in list)
            {
                await _context.Categories.AddAsync(i);
                await _context.SaveChangesAsync();
            }
        }

        public static async Task MilkSeed(AppDbContext _context)
        {
            if (await _context.Milks.AnyAsync()) { return; }

            var milk = await File.ReadAllTextAsync("../DataAccess/Seed/MilkSeed.json");
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var m = JsonSerializer.Deserialize<List<Milk>>(milk, jsonOptions);

            foreach(var i in m)
            {
                await _context.Milks.AddAsync(i);
                await _context.SaveChangesAsync();
            }
        }
        public static async Task PaynmentTypeSeed(AppDbContext _context)
        {
            if (await _context.PaymentTypes.AnyAsync()) { return; }

            var list = new List<PaymentType>
            {
                new PaymentType {Name="OnlinePayment"},
                new PaymentType {Name="In cash"}
            };

            foreach (var i in list)
            {
                await _context.PaymentTypes.AddAsync(i);
                await _context.SaveChangesAsync();
            }
        }

        public static async Task OrderSeed(AppDbContext _context)
        {
            if (await _context.Orders.AnyAsync()) { return; }

            var order = await File.ReadAllTextAsync("../DataAccess/Seed/OrderSeed.json");
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var m = JsonSerializer.Deserialize<List<Order>>(order, jsonOptions);

            foreach (var i in m)
            {
                await _context.Orders.AddAsync(i);
                await _context.SaveChangesAsync();
            }
        }

        public static async Task OrderStatusSeed(AppDbContext _context)
        {
            if(await _context.OrderStatuses.AnyAsync()) { return; }

            var list = new List<OrderStatus>
            {
                new OrderStatus{Name="Chờ Giao"},
                new OrderStatus{Name="Đang Giao"},
                new OrderStatus{Name="Đã Giao"},
                new OrderStatus{Name="Đã Hủy"}
            };

            foreach(var i in list)
            {
                await _context.OrderStatuses.AddAsync(i);
                await _context.SaveChangesAsync();
            }

        }
        public static async Task CouponSeed(AppDbContext _context)
        {
            if (await _context.Coupons.AnyAsync()) { return; }

            var list = new List<Coupon>
            {
                new Coupon{Code="FPTCOUPON100", Name="Coupon 1", Discount=100, EpiryDate=DateTime.Now.AddDays(1), CreateAt=DateTime.Now, UpdateAt= DateTime.Now, NumberOfUse=10, Status=1},
                new Coupon{Code="FPTCOUPON200", Name="Coupon 2", Discount=200, EpiryDate=DateTime.Now.AddDays(30), CreateAt=DateTime.Now, UpdateAt= DateTime.Now, NumberOfUse=10, Status=1},
                new Coupon{Code="FPTCOUPON300", Name="Coupon 3", Discount=300, EpiryDate=DateTime.Now.AddDays(30), CreateAt=DateTime.Now, UpdateAt= DateTime.Now, NumberOfUse=10, Status=1},
                new Coupon{Code="FPTCOUPON400", Name="Coupon 4", Discount=400, EpiryDate=DateTime.Now.AddDays(60), CreateAt=DateTime.Now, UpdateAt= DateTime.Now, NumberOfUse=10, Status=1},
                new Coupon{Code="FPTCOUPON500", Name="Coupon 5", Discount=500, EpiryDate=DateTime.Now.AddDays(60), CreateAt=DateTime.Now, UpdateAt= DateTime.Now, NumberOfUse=10, Status=1},
                new Coupon{Code="FPTCOUPON600", Name="Coupon 6", Discount=600, EpiryDate=DateTime.Now.AddDays(5), CreateAt=DateTime.Now, UpdateAt= DateTime.Now, NumberOfUse=10, Status=1}
            };

            foreach (var i in list)
            {
                await _context.Coupons.AddAsync(i);
                await _context.SaveChangesAsync();
            }

        }
    }
}
