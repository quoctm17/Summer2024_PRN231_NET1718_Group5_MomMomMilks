using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class TimeSlotDAO
    {
        private readonly AppDbContext _context;

        private static TimeSlotDAO instance;

        public TimeSlotDAO()
        {
            _context = new AppDbContext();
        }

        public static TimeSlotDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TimeSlotDAO();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<TimeSlot>> GetAllTimeSlotsAsync()
        {
            return await _context.TimeSlots.AsNoTracking().ToListAsync();
        }

        public async Task<TimeSlot> GetTimeSlotByIdAsync(int id)
        {
            return await _context.TimeSlots.AsNoTracking().FirstOrDefaultAsync(ts => ts.Id == id);
        }
    }
}
