using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IWardService
    {
        Ward GetWard(int id);
        List<Ward> GetAllWards();
        void AddWard(Ward ward);
        void UpdateWard(Ward ward);
        void DeleteWard(int id);
    }
}
