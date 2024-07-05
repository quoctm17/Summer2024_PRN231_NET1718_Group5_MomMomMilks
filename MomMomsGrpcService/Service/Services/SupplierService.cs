using BusinessObject.Entities;
using Repository.Interface;
using Service.Interfaces;

namespace Service.Services
{
	public class SupplierService : ISupplierService
	{
		private readonly ISupplierRepository _supplierRepository;

		public SupplierService(ISupplierRepository supplierRepository)
        {
			_supplierRepository = supplierRepository;
		}
        public async Task<bool> CreateSupplier(Supplier supplier)
		{
			return await _supplierRepository.CreateSupplier(supplier);
		}

		public async Task<bool> DeleteSupplier(int id)
		{
			return await _supplierRepository.DeleteSupplier(id);
		}

		public async Task<List<Supplier>> GetAll()
		{
			return await _supplierRepository.GetAll();
		}

		public async Task<Supplier> GetSingleSupplier(int id)
		{
			return await _supplierRepository.GetSingleSupplier(id);
		}

		public async Task<bool> UpdateSupplier(Supplier supplier)
		{
			return await _supplierRepository.UpdateSupplier(supplier);
		}
	}
}
