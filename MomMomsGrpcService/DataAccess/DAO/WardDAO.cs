using BusinessObject.Entities;

namespace DataAccess.DAO
{
    public class WardDAO
    {
        private readonly AppDbContext _context;

        private static WardDAO instance;

        public WardDAO()
        {
            _context = new AppDbContext();
        }

        public static WardDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WardDAO();
                }
                return instance;
            }
        }

        public Ward GetWard(int id)
        {
            return _context.Wards.FirstOrDefault(w => w.Id == id);
        }

        public List<Ward> GetAllWards()
        {
            return _context.Wards.ToList();
        }

        public void AddWard(Ward ward)
        {
            _context.Wards.Add(ward);
            _context.SaveChanges();
        }

        public void UpdateWard(Ward ward)
        {
            _context.Wards.Update(ward);
            _context.SaveChanges();
        }

        public void DeleteWard(int id)
        {
            var ward = _context.Wards.FirstOrDefault(w => w.Id == id);
            if (ward != null)
            {
                _context.Wards.Remove(ward);
                _context.SaveChanges();
            }
        }
    }
}
