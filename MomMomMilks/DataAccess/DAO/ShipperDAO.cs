using AutoMapper;
using DataTransfer.Manager;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
	public class ShipperDAO
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;

		private static ShipperDAO instance;

		public ShipperDAO()
		{
			_context = new AppDbContext();
			_mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile.AutoMapperProfile())).CreateMapper();
		}

		public static ShipperDAO Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ShipperDAO();
				}
				return instance;
			}
		}

		public async Task<List<ManagerShipperDTO>> GetAvailableShippers()
		{
			try
			{
				var shippers = await _context.Shippers
					.Include(x => x.AppUser)
					.Include(x => x.District)
					.Where(x => x.Status == "Available")
					.ToListAsync();
				var result = new List<ManagerShipperDTO>();
				foreach (var shipper in shippers)
				{
					var monthlyDeliveries = _context.Orders
						.Where(x => x.ShipperId == shipper.Id && x.CreateAt.Month == DateTime.UtcNow.Month)
						.Count();
					var shipperDTO = _mapper.Map<ManagerShipperDTO>(shipper);
					shipperDTO.MonthlyDeliveries = monthlyDeliveries;
					shipperDTO.Id = shipper.AppUserId;
					result.Add(shipperDTO);
				}
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
