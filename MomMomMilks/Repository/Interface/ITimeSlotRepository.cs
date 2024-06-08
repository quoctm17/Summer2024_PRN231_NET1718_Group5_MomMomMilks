using BusinessObject.Entities;

namespace Repository.Interface
{
    public interface ITimeSlotRepository
    {
        Task<IEnumerable<TimeSlot>> GetAllTimeSlotsAsync();
        Task<TimeSlot> GetTimeSlotByIdAsync(int id);
    }
}
