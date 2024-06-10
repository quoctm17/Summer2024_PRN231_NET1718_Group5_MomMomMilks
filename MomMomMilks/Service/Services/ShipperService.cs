using DataTransfer.Manager;
using Repository.Interface;
using Service.Interfaces;

namespace Service.Services
{
	public class ShipperService : IShipperService
	{
		private readonly IShipperRepository _shipperRepository;

		public ShipperService(IShipperRepository shipperRepository)
        {
			_shipperRepository = shipperRepository;
		}
        public async Task<List<ManagerShipperDTO>> GetAvailableShippers()
		{
			return await _shipperRepository.GetAvailableShippers();
		}
	}
}
