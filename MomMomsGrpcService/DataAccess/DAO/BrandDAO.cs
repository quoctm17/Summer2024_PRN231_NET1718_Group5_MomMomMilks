using AutoMapper;
using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
	public class BrandDAO
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;

		private static BrandDAO instance;

		public BrandDAO()
		{
			_context = new AppDbContext();
			_mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile.AutoMapperProfile())).CreateMapper();
		}

		public static BrandDAO Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new BrandDAO();
				}
				return instance;
			}
		}
		public async Task<List<Brand>> GetAllBrands()
		{
			try
			{
				var result = await _context
					.Brands
					.ToListAsync();
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<Brand> GetSingleBrand(int id)
		{
			try
			{
				var result = await _context
					.Brands
					.FirstOrDefaultAsync(x => x.Id == id);
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<bool> CreateBrand(Brand brand)
		{
			try
			{
				await _context.Brands.AddAsync(brand);
				return await _context.SaveChangesAsync() > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<bool> UpdateBrand(Brand brand)
		{
			try
			{
				var existed = await _context.Brands.FindAsync(brand.Id);
				if (existed != null)
				{
					existed.Name = brand.Name;
				}
				return await _context.SaveChangesAsync() > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<bool> DeleteBrand(int id)
		{
			try
			{
				var existed = await _context.Brands.FindAsync(id);
				if (existed != null)
				{
					_context.Brands.Remove(existed);
				}
				return await _context.SaveChangesAsync() > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
