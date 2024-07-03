using BusinessObject.Entities;
using DataAccess.DAO;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class DistrictRepository : IDistrictRepository
    {
        public District GetDistrict(int id) => DistrictDAO.Instance.GetDistrict(id);

        public List<District> GetAllDistricts() => DistrictDAO.Instance.GetAllDistricts();

        public void AddDistrict(District district) => DistrictDAO.Instance.AddDistrict(district);

        public void UpdateDistrict(District district) => DistrictDAO.Instance.UpdateDistrict(district);

        public void DeleteDistrict(int id) => DistrictDAO.Instance.DeleteDistrict(id);
    }
}
