using BusinessObject.Entities;

namespace Repository.Interface
{
	public interface ISupplierRepository
	{
		Task<List<Supplier>> GetAll();
		Task<Supplier> GetSingleSupplier(int id);
		Task<bool> CreateSupplier(Supplier supplier);
		Task<bool> UpdateSupplier(Supplier supplier);
		Task<bool> DeleteSupplier(int id);
	}
}
