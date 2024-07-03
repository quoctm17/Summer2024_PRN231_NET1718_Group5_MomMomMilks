using DataTransfer.Manager;

namespace Repository.Interface
{
	public interface IShipperRepository
	{
		Task<List<ManagerShipperDTO>> GetAvailableShippers();
	}
}
