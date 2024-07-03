using BusinessObject.Entities;
using DataAccess.DAO;
using Repository.Interface;
using System.Drawing.Drawing2D;

namespace Repository
{
	public class BrandRepository : IBrandRepository
	{
		public async Task<bool> CreateBrand(Brand brand)
		{
			return await BrandDAO.Instance.CreateBrand(brand);
		}

		public async Task<bool> DeleteBrand(int id)
		{
			return await BrandDAO.Instance.DeleteBrand(id);
		}

		public async Task<List<Brand>> GetAll()
		{
			return await BrandDAO.Instance.GetAllBrands();
		}

		public async Task<Brand> GetSingleBrand(int id)
		{
			return await BrandDAO.Instance.GetSingleBrand(id);
		}

		public async Task<bool> UpdateBrand(Brand brand)
		{
			return await BrandDAO.Instance.UpdateBrand(brand);
		}
	}
}
