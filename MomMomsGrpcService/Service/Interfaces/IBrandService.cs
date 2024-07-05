using BusinessObject.Entities;

namespace Service.Interfaces
{
	public interface IBrandService
	{
		Task<List<Brand>> GetAll();
		Task<Brand> GetSingleBrand(int id);
		Task<bool> CreateBrand(Brand brand);
		Task<bool> UpdateBrand(Brand brand);
		Task<bool> DeleteBrand(int id);
	}
}
