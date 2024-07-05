using BusinessObject.Entities;
using Repository.Interface;
using Service.Interfaces;
using System.Drawing.Drawing2D;

namespace Service.Services
{
	public class BrandService : IBrandService
	{
		private readonly IBrandRepository _brandRepository;

		public BrandService(IBrandRepository brandRepository)
        {
			_brandRepository = brandRepository;
		}
        public async Task<bool> CreateBrand(Brand brand)
		{
			return await _brandRepository.CreateBrand(brand);
		}

		public async Task<bool> DeleteBrand(int id)
		{
			return await _brandRepository.DeleteBrand(id);
		}

		public async Task<List<Brand>> GetAll()
		{
			return await _brandRepository.GetAll();
		}

		public async Task<Brand> GetSingleBrand(int id)
		{
			return await _brandRepository.GetSingleBrand(id);
		}

		public async Task<bool> UpdateBrand(Brand brand)
		{
			return await _brandRepository.UpdateBrand(brand);
		}
	}
}
