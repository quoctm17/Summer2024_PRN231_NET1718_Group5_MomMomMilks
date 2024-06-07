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
    public class WardRepository : IWardRepository
    {
        public Ward GetWard(int id) => WardDAO.Instance.GetWard(id);

        public List<Ward> GetAllWards() => WardDAO.Instance.GetAllWards();

        public void AddWard(Ward ward) => WardDAO.Instance.AddWard(ward);

        public void UpdateWard(Ward ward) => WardDAO.Instance.UpdateWard(ward);

        public void DeleteWard(int id) => WardDAO.Instance.DeleteWard(id);
    }
}
