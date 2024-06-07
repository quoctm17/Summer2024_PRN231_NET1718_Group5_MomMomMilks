using BusinessObject.Entities;
using Repository.Interface;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class WardService : IWardService
    {
        private readonly IWardRepository _wardRepo;

        public WardService(IWardRepository wardRepo)
        {
            _wardRepo = wardRepo;
        }

        public Ward GetWard(int id) => _wardRepo.GetWard(id);

        public List<Ward> GetAllWards() => _wardRepo.GetAllWards();

        public void AddWard(Ward ward) => _wardRepo.AddWard(ward);

        public void UpdateWard(Ward ward) => _wardRepo.UpdateWard(ward);

        public void DeleteWard(int id) => _wardRepo.DeleteWard(id);
    }
}
