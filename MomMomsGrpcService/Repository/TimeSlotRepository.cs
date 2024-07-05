using BusinessObject.Entities;
using DataAccess.DAO;
using Repository.Interface;

namespace Repository
{
    public class TimeSlotRepository : ITimeSlotRepository
    {
        public async Task<IEnumerable<TimeSlot>> GetAllTimeSlotsAsync()
        {
            return await TimeSlotDAO.Instance.GetAllTimeSlotsAsync();
        }

        public async Task<TimeSlot> GetTimeSlotByIdAsync(int id)
        {
            return await TimeSlotDAO.Instance.GetTimeSlotByIdAsync(id);
        }
    }
}
