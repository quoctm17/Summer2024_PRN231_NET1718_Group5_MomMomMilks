using AutoMapper;
using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
	public class MilkAgeDAO
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;

		private static MilkAgeDAO instance;

		public MilkAgeDAO()
		{
			_context = new AppDbContext();
			_mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile.AutoMapperProfile())).CreateMapper();
		}

		public static MilkAgeDAO Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new MilkAgeDAO();
				}
				return instance;
			}
		}
		public async Task<List<MilkAge>> GetAllMilkAges()
		{
			try
			{
				var result = await _context
					.MilkAges
					.ToListAsync();
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<MilkAge> GetSingleMilkAge(int id)
		{
			try
			{
				var result = await _context
					.MilkAges
					.FirstOrDefaultAsync(x => x.Id == id);
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<bool> CreateMilkAge(MilkAge milkAge)
		{
			try
			{
				await _context.MilkAges.AddAsync(milkAge);
				return await _context.SaveChangesAsync() > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<bool> UpdateMilkAge(MilkAge milkAge)
		{
			try
			{
				var existed = await _context.MilkAges.FindAsync(milkAge.Id);
				if (existed != null)
				{
					existed.Min = milkAge.Min;
					existed.Max = milkAge.Max;
				}
				return await _context.SaveChangesAsync() > 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<bool> DeleteMilkAge(int id)
		{
			try
			{
				var existed = await _context.MilkAges.FindAsync(id);
				if (existed != null)
				{
					_context.MilkAges.Remove(existed);
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
