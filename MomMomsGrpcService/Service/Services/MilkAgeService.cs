using BusinessObject.Entities;
using Repository.Interface;
using Service.Interfaces;

namespace Service.Services
{
	public class MilkAgeService : IMilkAgeService
	{
		private readonly IMilkAgeRepository _milkAgeRepository;

		public MilkAgeService(IMilkAgeRepository milkAgeRepository)
		{
			_milkAgeRepository = milkAgeRepository;
		}
        public async Task<bool> CreateMilkAge(MilkAge milkAge)
		{
			return await _milkAgeRepository.CreateMilkAge(milkAge);
		}

		public async Task<bool> DeleteMilkAge(int id)
		{
			return await _milkAgeRepository.DeleteMilkAge(id);
		}

		public async Task<List<MilkAge>> GetAll()
		{
			return await _milkAgeRepository.GetAll();
		}

		public async Task<MilkAge> GetSingleMilkAge(int id)
		{
			return await _milkAgeRepository.GetSingleMilkAge(id);
		}

		public async Task<bool> UpdateMilkAge(MilkAge milkAge)
		{
			return await _milkAgeRepository.UpdateMilkAge(milkAge);
		}
	}
}
