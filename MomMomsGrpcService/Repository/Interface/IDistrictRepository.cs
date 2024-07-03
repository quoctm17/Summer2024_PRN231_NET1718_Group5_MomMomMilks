using BusinessObject.Entities;

namespace Repository.Interface
{
    public interface IDistrictRepository
    {
        District GetDistrict(int id);
        List<District> GetAllDistricts();
        void AddDistrict(District district);
        void UpdateDistrict(District district);
        void DeleteDistrict(int id);
    }
}
