using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess.Seed
{
    public class Seed
    {
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
                new Supplier{Name="Danone, New Zealand"}
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
    }
}
