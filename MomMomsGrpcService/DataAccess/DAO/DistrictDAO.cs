using BusinessObject.Entities;

namespace DataAccess.DAO
{
    public class DistrictDAO
    {
        private readonly AppDbContext _context;

        private static DistrictDAO instance;

        public DistrictDAO()
        {
            _context = new AppDbContext();
        }

        public static DistrictDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DistrictDAO();
                }
                return instance;
            }
        }

        public District GetDistrict(int id)
        {
            return _context.Districts.FirstOrDefault(d => d.Id == id);
        }

        public List<District> GetAllDistricts()
        {
            return _context.Districts.ToList();
        }

        public void AddDistrict(District district)
        {
            _context.Districts.Add(district);
            _context.SaveChanges();
        }

        public void UpdateDistrict(District district)
        {
            _context.Districts.Update(district);
            _context.SaveChanges();
        }

        public void DeleteDistrict(int id)
        {
            var district = _context.Districts.FirstOrDefault(d => d.Id == id);
            if (district != null)
            {
                _context.Districts.Remove(district);
                _context.SaveChanges();
            }
        }
    }
}
