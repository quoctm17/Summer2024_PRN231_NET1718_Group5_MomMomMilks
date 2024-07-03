using BusinessObject.Entities;
using Repository.Interface;
using Service.Interface;


namespace Service.Services
{
    public class DistrictService : IDistrictService
    {
        private readonly IDistrictRepository _districtRepo;

        public DistrictService(IDistrictRepository districtRepo)
        {
            _districtRepo = districtRepo;
        }

        public District GetDistrict(int id) => _districtRepo.GetDistrict(id);

        public List<District> GetAllDistricts() => _districtRepo.GetAllDistricts();

        public void AddDistrict(District district) => _districtRepo.AddDistrict(district);

        public void UpdateDistrict(District district) => _districtRepo.UpdateDistrict(district);

        public void DeleteDistrict(int id) => _districtRepo.DeleteDistrict(id);
    }
}
