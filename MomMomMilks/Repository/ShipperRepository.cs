using DataAccess.DAO;
using DataTransfer.Manager;
using Repository.Interface;

namespace Repository
{
	public class ShipperRepository : IShipperRepository
	{
		public async Task<List<ManagerShipperDTO>> GetAvailableShippers()
		{
			return await ShipperDAO.Instance.GetAvailableShippers();
		}
	}
}
