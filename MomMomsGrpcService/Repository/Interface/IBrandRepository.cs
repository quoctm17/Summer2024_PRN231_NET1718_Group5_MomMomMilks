using BusinessObject.Entities;

namespace Repository.Interface
{
	public interface IBrandRepository
	{
		Task<List<Brand>> GetAll();
		Task<Brand> GetSingleBrand(int id);
		Task<bool> CreateBrand(Brand brand);
		Task<bool> UpdateBrand(Brand brand);
		Task<bool> DeleteBrand(int id);
	}
}
