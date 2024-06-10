using DataTransfer.Manager;

namespace Service.Interfaces
{
	public interface IShipperService
	{
		Task<List<ManagerShipperDTO>> GetAvailableShippers();
	}
}
