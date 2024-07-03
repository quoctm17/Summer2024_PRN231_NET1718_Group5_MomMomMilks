using BusinessObject.Entities;
using DataAccess.DAO;
using Repository.Interface;

namespace Repository
{
	public class SupplierRepository : ISupplierRepository
	{

		public async Task<bool> CreateSupplier(Supplier supplier)
		{
			return await SupplierDAO.Instance.CreateSupplier(supplier);
		}

		public async Task<bool> DeleteSupplier(int id)
		{
			return await SupplierDAO.Instance.DeleteSupplier(id);
		}

		public async Task<List<Supplier>> GetAll()
		{
			return await SupplierDAO.Instance.GetAllSuppliers();
		}

		public async Task<Supplier> GetSingleSupplier(int id)
		{
			return await SupplierDAO.Instance.GetSingleSupplier(id);
		}

		public async Task<bool> UpdateSupplier(Supplier supplier)
		{
			return await SupplierDAO.Instance.UpdateSupplier(supplier);
		}
	}
}
