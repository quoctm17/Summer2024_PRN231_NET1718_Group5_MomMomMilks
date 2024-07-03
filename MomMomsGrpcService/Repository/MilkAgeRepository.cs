using BusinessObject.Entities;
using DataAccess.DAO;
using Repository.Interface;

namespace Repository
{
	public class MilkAgeRepository : IMilkAgeRepository
	{
		public async Task<bool> CreateMilkAge(MilkAge milkAge)
		{
			return await MilkAgeDAO.Instance.CreateMilkAge(milkAge);
		}

		public async Task<bool> DeleteMilkAge(int id)
		{
			return await MilkAgeDAO.Instance.DeleteMilkAge(id);
		}

		public async Task<List<MilkAge>> GetAll()
		{
			return await MilkAgeDAO.Instance.GetAllMilkAges();
		}

		public async Task<MilkAge> GetSingleMilkAge(int id)
		{
			return await MilkAgeDAO.Instance.GetSingleMilkAge(id);
		}

		public async Task<bool> UpdateMilkAge(MilkAge milkAge)
		{
			return await MilkAgeDAO.Instance.UpdateMilkAge(milkAge);
		}
	}
}
