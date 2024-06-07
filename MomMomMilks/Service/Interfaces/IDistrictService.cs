using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IDistrictService
    {
        District GetDistrict(int id);
        List<District> GetAllDistricts();
        void AddDistrict(District district);
        void UpdateDistrict(District district);
        void DeleteDistrict(int id);
    }
}
