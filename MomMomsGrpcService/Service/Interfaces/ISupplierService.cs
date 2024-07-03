using BusinessObject.Entities;

namespace Service.Interfaces
{
	public interface ISupplierService
	{
		Task<List<Supplier>> GetAll();
		Task<Supplier> GetSingleSupplier(int id);
		Task<bool> CreateSupplier(Supplier supplier);
		Task<bool> UpdateSupplier(Supplier supplier);
		Task<bool> DeleteSupplier(int id);
	}
}
