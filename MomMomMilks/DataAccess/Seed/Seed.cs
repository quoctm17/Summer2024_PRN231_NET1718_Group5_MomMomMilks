using BusinessObject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DataAccess.Seed
{
    public class Seed
    {
        public static async Task SeedDistrictsAndWards(AppDbContext _context)
        {
            if (!await _context.Districts.AnyAsync())
            {
                var districtData = await File.ReadAllTextAsync("../DataAccess/Seed/DistrictSeed.json");
                var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var districts = JsonSerializer.Deserialize<List<District>>(districtData, jsonOptions);

                foreach (var district in districts)
                {
                    await _context.Districts.AddAsync(district);
                }

                await _context.SaveChangesAsync();
            }

            if (!await _context.Wards.AnyAsync())
            {
                var wardData = await File.ReadAllTextAsync("../DataAccess/Seed/WardSeed.json");
                var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var wards = JsonSerializer.Deserialize<List<Ward>>(wardData, jsonOptions);

                foreach (var ward in wards)
                {
                    await _context.Wards.AddAsync(ward);
                }

                await _context.SaveChangesAsync();
            }
        }

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

            var shipperUsers = new List<AppUser>();

            for (int i = 1; i <= 24; i++)
            {
                shipperUsers.Add(new AppUser
                {
                    UserName = $"shipper{i}@gmail.com",
                    Email = $"shipper{i}@gmail.com",
                    EmailConfirmed = true,
                    Status = 1,
                    Point = 0,
                    Shipper = new Shipper
                    {
                        Status = i % 3 == 0 ? "Shipping" : i % 2 == 0 ? "Unavailable" : "Available",
                        DistrictId = i
                    }
                });
            }

            foreach (var shipper in shipperUsers)
            {
                await _userManager.CreateAsync(shipper, "Pa$$w0rd");
                await _userManager.AddToRolesAsync(shipper, new[] { "Shipper" });
            }

        }



        public static async Task SeedAddress(AppDbContext _context)
        {
            if (await _context.Addresses.AnyAsync()) return;

            var addressData = await File.ReadAllTextAsync("../DataAccess/Seed/AddressSeed.json");
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var addresses = JsonSerializer.Deserialize<List<Address>>(addressData, jsonOptions);

            foreach (var address in addresses)
            {
                await _context.Addresses.AddAsync(address);
            }

            await _context.SaveChangesAsync();
        }



        public static async Task SeedBranch(AppDbContext _context)
        {
            if (await _context.Brands.AnyAsync()) { return; }

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

            foreach (var i in list)
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
                new MilkAge {Min = 0, Max = 12, Unit="Tháng Tuổi"},
                new MilkAge {Min = 1, Max = 10, Unit= "Tuổi"},
                new MilkAge {Min = 6, Max = 12, Unit = "Tuổi"},
                new MilkAge {Min = 12, Max = 18, Unit = "Tuổi"},
                new MilkAge {Min = 18, Max = 30, Unit = "Tuổi"},
                new MilkAge {Min = 30, Max = 60, Unit = "Tuổi"}
            };

            foreach (var i in list)
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

            foreach (var i in list)
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
                new Category {Name="Chilren Milk", Description = "Powdered milk for infants or children over 12 months old"},
                new Category {Name="Baby Milk", Description = "Milk for children from 3 to 12 years old"},
                new Category {Name="Dairy Cow", Description = "Milk for children from 12 years old and adults"},
                new Category {Name="Pregnant Milk", Description="Powdered milk for pregnant mothers"}
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

            foreach (var i in m)
            {
                await _context.Milks.AddAsync(i);
                await _context.SaveChangesAsync();
            }
        }

        public static async Task MilkBatchSeed(AppDbContext _context)
        {
            if (await _context.Batches.AnyAsync()) { return; }

            var batches = await File.ReadAllTextAsync("../DataAccess/Seed/BatchSeed.json");
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var b = JsonSerializer.Deserialize<List<Batch>>(batches, jsonOptions);
            var random = new Random();
            foreach (var i in b)
            {
                var randomNumber = random.Next(1, 5);
                i.ImportDate = DateTime.Now.AddMonths(-randomNumber);
                i.ExpiredDate = DateTime.Now.AddMonths(-randomNumber + 6);
                await _context.Batches.AddAsync(i);
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

        public static async Task TimeSlotsSeed(AppDbContext _context)
        {
            if (await _context.TimeSlots.AnyAsync()) return;

            var list = new List<TimeSlot>
            {
                new TimeSlot { Name = "Morning", StartTime = new TimeSpan(7, 0, 0), EndTime = new TimeSpan(10, 0, 0) },
                new TimeSlot { Name = "Afternoon", StartTime = new TimeSpan(12, 0, 0), EndTime = new TimeSpan(15, 0, 0) },
                new TimeSlot { Name = "Evening", StartTime = new TimeSpan(17, 0, 0), EndTime = new TimeSpan(20, 0, 0) }
            };

            foreach (var timeSlot in list)
            {
                await _context.TimeSlots.AddAsync(timeSlot);
            }

            await _context.SaveChangesAsync();
        }

        public static async Task OrderStatusSeed(AppDbContext _context)
        {
            if (await _context.OrderStatuses.AnyAsync()) { return; }

            var list = new List<OrderStatus>
            {
                new OrderStatus{Name="Chờ thanh toán"},
                new OrderStatus{Name="Phân công giao"},
                new OrderStatus{Name="Đang vận chuyển"},
                new OrderStatus{Name="Hoàn thành"},
                new OrderStatus{Name="Hủy"},
                new OrderStatus{Name="Hoàn trả"}
            };

            foreach (var i in list)
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
        public static async Task OrderDetailSeed(AppDbContext _context)
        {
            if (await _context.OrderDetails.AnyAsync()) return;

            var orderDetailData = await File.ReadAllTextAsync("../DataAccess/Seed/OrderDetailSeed.json");
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var orderDetails = JsonSerializer.Deserialize<List<OrderDetail>>(orderDetailData, jsonOptions);

            foreach (var orderDetail in orderDetails)
            {
                await _context.OrderDetails.AddAsync(orderDetail);
            }

            await _context.SaveChangesAsync();
        }


        public static async Task ScheduleSeed(AppDbContext _context)
        {
            if (await _context.Schedules.AnyAsync()) return;

            var scheduleData = await File.ReadAllTextAsync("../DataAccess/Seed/ScheduleSeed.json");
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var schedules = JsonSerializer.Deserialize<List<Schedule>>(scheduleData, jsonOptions);

            foreach (var schedule in schedules)
            {
                await _context.Schedules.AddAsync(schedule);
            }

            await _context.SaveChangesAsync();
        }


        public static async Task TransactionSeed(AppDbContext _context)
        {
            if (await _context.Transactions.AnyAsync()) return;

            var transactionData = await File.ReadAllTextAsync("../DataAccess/Seed/TransactionSeed.json");
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var transactions = JsonSerializer.Deserialize<List<Transaction>>(transactionData, jsonOptions);

            foreach (var transaction in transactions)
            {
                await _context.Transactions.AddAsync(transaction);
            }

            await _context.SaveChangesAsync();
        }

        private class ShipperSeedData
        {
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Status { get; set; }
            public int DistrictId { get; set; }
        }
    }
}
