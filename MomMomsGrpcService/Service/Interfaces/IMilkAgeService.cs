using BusinessObject.Entities;

namespace Service.Interfaces
{
	public interface IMilkAgeService
	{
		Task<List<MilkAge>> GetAll();
		Task<MilkAge> GetSingleMilkAge(int id);
		Task<bool> CreateMilkAge(MilkAge milkAge);
		Task<bool> UpdateMilkAge(MilkAge milkAge);
		Task<bool> DeleteMilkAge(int id);
	}
}
