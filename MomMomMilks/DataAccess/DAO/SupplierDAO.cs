using AutoMapper;
using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
	public class SupplierDAO
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;

		private static SupplierDAO instance;

		public SupplierDAO()
		{
			_context = new AppDbContext();
			_mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile.AutoMapperProfile())).CreateMapper();
		}

		public static SupplierDAO Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new SupplierDAO();
				}
				return instance;
			}
		}
		public async Task<List<Supplier>> GetAllSuppliers()
		{
			try
			{
				var result = await _context
					.Suppliers
					.ToListAsync();
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<Supplier> GetSingleSupplier(int id)
		{
			try
			{
				var result = await _context
					.Suppliers
					.FirstOrDefaultAsync(x => x.Id == id);
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<bool> CreateSupplier(Supplier supplier)
		{
			try
			{
				await _context.Suppliers.AddAsync(supplier);
				return await _context.SaveChangesAsync() > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<bool> UpdateSupplier(Supplier supplier)
		{
			try
			{
				var existed = await _context.Suppliers.FindAsync(supplier.Id);
				if (existed != null)
				{
					existed.Name = supplier.Name;
				}
				return await _context.SaveChangesAsync() > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<bool> DeleteSupplier(int id)
		{
			try
			{
				var existed = await _context.Suppliers.FindAsync(id);
				if (existed != null)
				{
					_context.Suppliers.Remove(existed);
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
